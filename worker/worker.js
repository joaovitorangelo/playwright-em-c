const amqp = require('amqplib');
const { chromium } = require('playwright');
const axios = require('axios');

async function runScraping(postId) {
  const browser = await chromium.launch();
  const page = await browser.newPage();

  await page.goto('https://www.kabum.com.br/');
  await page.waitForSelector('#inputBusca');
  await page.fill('#inputBusca', postId.toString());
  await page.press('#inputBusca', 'Enter');
  await page.waitForTimeout(3000);

  const resultText = `Scraping concluído com sucesso para PostId ${postId}`;

  await browser.close();
  return resultText;
}

async function updateJobInAPI(jobId, result) {
  try {
    const url = `http://localhost:5033/api/job/${jobId}/complete`; // Altere se a porta for diferente
    await axios.put(url, { result });
    console.log(`✅ Job ${jobId} atualizado na API.`);
  } catch (error) {
    console.error(`❌ Erro ao atualizar job ${jobId}:`, error.message);
  }
}

async function start() {
  try {
    const connection = await amqp.connect('amqp://127.0.0.1');
    const channel = await connection.createChannel();
    const queue = 'scraping';

    await channel.assertQueue(queue, { durable: false });

    console.log("Aguardando mensagens na fila 'scraping'...");

    channel.consume(queue, async (msg) => {
      if (msg !== null) {
        const content = JSON.parse(msg.content.toString());
        console.log('Mensagem recebida:', content);

        try {
          const result = await runScraping(content.PostId);
          await updateJobInAPI(content.Id, result);

          channel.ack(msg);
        } catch (err) {
          console.error('Erro no scraping:', err);
          // channel.nack(msg);
        }
      }
    });
  } catch (error) {
    console.error('Erro ao conectar RabbitMQ:', error);
  }
}

start();
