const xlsx = require('xlsx');

// Load the workbook
const workbook = xlsx.readFile('Pre_trip checklist.xlsx');
// Get the first sheet
const sheetName = workbook.SheetNames[0];
const sheet = workbook.Sheets[sheetName];

// Convert to JSON
const data = xlsx.utils.sheet_to_json(sheet, { header: 1 });

console.log(JSON.stringify(data, null, 2));
