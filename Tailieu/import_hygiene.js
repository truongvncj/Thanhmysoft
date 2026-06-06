const xlsx = require('xlsx');
const { Client } = require('pg');

const client = new Client({
  host: 'localhost',
  port: 5432,
  database: 'SaleManagerDB',
  user: 'postgres',
  password: 'postgres',
});

// Infer correct answer logic based on string matching
function isCoDat(chitiet) {
    const text = chitiet.toLowerCase();
    if (text.includes('có bị rò rỉ')) return false; // Không
    if (text.includes('ẩm mốc, bụi bẩn')) return false; // Không
    if (text.includes('công ty đối thủ')) return false; // Không
    return true; // Có
}

async function run() {
  await client.connect();

  const workbook = xlsx.readFile('kiểm tra vệ sinh xe tải.xlsx');
  const sheetName = workbook.SheetNames[0];
  const sheet = workbook.Sheets[sheetName];
  const data = xlsx.utils.sheet_to_json(sheet, { header: 1 });

  await client.query('DELETE FROM kiemtravesinhxeparam');
  await client.query('ALTER SEQUENCE kiemtravesinhxeparam_id_seq RESTART WITH 1');

  let currentNoiDung = '';

  for (let i = 1; i < data.length; i++) {
    const row = data[i];
    if (!row || row.length === 0) continue;

    let noidung = row[0];
    const stt = row[1];
    const chitiet = row[2];
    const ghichu = row[10] || null;

    if (!stt || isNaN(parseInt(stt))) continue;

    if (noidung) {
      currentNoiDung = String(noidung).trim();
    } else {
      noidung = currentNoiDung;
    }

    if (!chitiet) continue;
    
    const dapAnDat = isCoDat(String(chitiet));

    const query = `
      INSERT INTO kiemtravesinhxeparam (noidung, stt, chitiet, ghichu, dapandat, isactive)
      VALUES ($1, $2, $3, $4, $5, $6)
    `;
    const values = [
      currentNoiDung || 'Khác',
      parseInt(stt),
      String(chitiet).trim().substring(0, 1000),
      ghichu ? String(ghichu).trim().substring(0, 1000) : null,
      dapAnDat,
      true
    ];

    try {
      await client.query(query, values);
      console.log(`Inserted STT: ${stt} - Đáp án Đạt: ${dapAnDat ? 'Có' : 'Không'}`);
    } catch (err) {
      console.error(`Error inserting STT: ${stt}`, err);
    }
  }

  await client.end();
  console.log('Done importing data.');
}

run().catch(console.error);
