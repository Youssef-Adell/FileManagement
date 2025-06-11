import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const envFiles = {
  '.env': 'VITE_API_BASE_URL=http://localhost:5000/api',
  '.env.development': 'VITE_API_BASE_URL=http://localhost:5000/api',
  '.env.production': 'VITE_API_BASE_URL=/api'
};

Object.entries(envFiles).forEach(([filename, content]) => {
  const filePath = path.join(__dirname, filename);
  fs.writeFileSync(filePath, content);
  console.log(`Created ${filename}`);
}); 