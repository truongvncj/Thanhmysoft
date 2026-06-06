const xlsx = require('xlsx');
const { Client } = require('pg');

const client = new Client({
  host: 'localhost',
  port: 5432,
  database: 'SaleManagerDB',
  user: 'postgres',
  password: 'postgres',
});

async function run() {
  await client.connect();

  const workbook = xlsx.readFile('Pre_trip checklist.xlsx');
  const sheetName = workbook.SheetNames[0];
  const sheet = workbook.Sheets[sheetName];
  const data = xlsx.utils.sheet_to_json(sheet, { header: 1 });

  // Delete all existing records
  await client.query('DELETE FROM pretripchecklistparam');
  // Reset sequence
  await client.query('ALTER SEQUENCE pretripchecklistparam_id_seq RESTART WITH 1');

  let currentNoiDung = '';

  for (let i = 1; i < data.length; i++) { // Skip header row 0
    const row = data[i];
    if (!row || row.length === 0) continue;

    let noidung = row[0];
    const stt = row[1];
    const dieukien = row[2];

    if (!stt) continue; // Skip rows without STT

    if (noidung) {
      currentNoiDung = String(noidung).trim();
    } else {
      noidung = currentNoiDung; // Fill forward
    }

    if (!dieukien) continue;

    const query = `
      INSERT INTO pretripchecklistparam (noidung, stt, dieukien, isactive)
      VALUES ($1, $2, $3, $4)
    `;
    const values = [
      currentNoiDung || 'Khác',
      parseInt(stt),
      String(dieukien).trim().substring(0, 1000),
      true
    ];

    try {
      await client.query(query, values);
      console.log(`Inserted STT: ${stt}`);
    } catch (err) {
      console.error(`Error inserting STT: ${stt}`, err);
    }
  }

  await client.end();
  console.log('Done importing data.');
}

run().catch(console.error);
