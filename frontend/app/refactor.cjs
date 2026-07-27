const fs = require('fs');
const path = require('path');

const targetDir = path.join(__dirname, 'pages');

function refactorFile(filePath) {
    let content = fs.readFileSync(filePath, 'utf-8');
    
    // Check if file contains confirm( or prompt(
    if (!content.includes('confirm(') && !content.includes('prompt(')) {
        return;
    }

    console.log(`Refactoring ${filePath}`);

    // Replace `confirm(` with `await confirm(` if not already awaited
    content = content.replace(/(?<!await\s)confirm\(/g, 'await confirm(');
    
    // Replace `prompt(` with `await prompt(` if not already awaited
    content = content.replace(/(?<!await\s)prompt\(/g, 'await prompt(');

    // Now we need to make sure the enclosing arrow function is async.
    // E.g., `const handleDelete = () => {` -> `const handleDelete = async () => {`
    // E.g., `const updateStatus = (id, status) => {` -> `const updateStatus = async (id, status) => {`
    // We will do a simple regex that finds `const XYZ = (...) => {` and adds `async ` if it contains `await`.
    // Wait, since we just added `await`, any function containing `await ` should be async.
    // Let's use a regex to find all arrow functions, and if their body contains `await`, make them async.
    
    // This regex tries to match `const foo = (args) => {` or `function foo() {`
    // It's tricky to parse JS with regex. 
    // Instead, I'll replace all common Vue 3 arrow functions: `const [a-zA-Z0-9_]+ = (?:\(.*?\)|[a-zA-Z0-9_]+) => {`
    content = content.replace(/(const\s+\w+\s*=\s*)(\(.*?\)\s*=>\s*\{|\w+\s*=>\s*\{)/g, (match, p1, p2) => {
        if (!p2.startsWith('async ')) {
            return p1 + 'async ' + p2;
        }
        return match;
    });

    fs.writeFileSync(filePath, content, 'utf-8');
}

function walkDir(dir) {
    const files = fs.readdirSync(dir);
    for (const file of files) {
        const fullPath = path.join(dir, file);
        if (fs.statSync(fullPath).isDirectory()) {
            walkDir(fullPath);
        } else if (fullPath.endsWith('.vue') || fullPath.endsWith('.js')) {
            refactorFile(fullPath);
        }
    }
}

walkDir(targetDir);
console.log('Refactoring complete.');
