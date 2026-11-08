// Runs the WebGL build in headless Chromium (software rendering) and takes screenshots.
// The drone is flown with a virtual Xbox 360 pad steering towards the yellow orbs.
// Usage: node captures.mjs <build folder> <output folder>
import { chromium } from 'playwright';
import { createServer } from 'node:http';
import { readFile, mkdir } from 'node:fs/promises';
import { join, extname } from 'node:path';

const [root, out] = process.argv.slice(2);
const types = { '.html': 'text/html', '.js': 'application/javascript', '.json': 'application/json', '.wasm': 'application/wasm', '.png': 'image/png', '.css': 'text/css' };

const server = createServer(async (req, res) => {
  let path = decodeURIComponent(new URL(req.url, 'http://x').pathname);
  if (path.endsWith('/')) path += 'index.html';
  try {
    const body = await readFile(join(root, path));
    res.writeHead(200, { 'Content-Type': types[extname(path)] ?? 'application/octet-stream' });
    res.end(body);
  } catch {
    res.writeHead(404).end();
  }
}).listen(8080);

// Virtual gamepad (standard mapping): axes = [left x, left y, right x, right y], button 7 = right trigger.
// window.pad.set({ axes, buttons }) updates it; window.autopilot(true) steers towards yellow pixels.
const padScript = () => {
  const pad = {
    id: 'Xbox 360 Controller (XInput STANDARD GAMEPAD)', index: 0, connected: true, mapping: 'standard',
    axes: [0, 0, 0, 0], buttons: Array.from({ length: 17 }, () => ({ pressed: false, touched: false, value: 0 })),
    timestamp: performance.now(),
  };
  window.pad = {
    set({ axes, buttons = {} }) {
      if (axes) pad.axes = axes.slice();
      pad.buttons = pad.buttons.map((b, i) => (i in buttons ? { pressed: buttons[i] > 0.1, touched: buttons[i] > 0, value: buttons[i] } : b));
      pad.timestamp = performance.now();
    },
  };
  // Count the polls to check in the log that the game reads the pad.
  window.padPolls = 0;
  navigator.getGamepads = () => { window.padPolls++; return [pad, null, null, null]; };
  // Keep the WebGL frame readable after presentation, so the autopilot can sample it.
  const getContext = HTMLCanvasElement.prototype.getContext;
  HTMLCanvasElement.prototype.getContext = function (type, attributes) {
    if (type === 'webgl' || type === 'webgl2') attributes = { ...attributes, preserveDrawingBuffer: true };
    return getContext.call(this, type, attributes);
  };
  // The game registers its listeners late (after the wasm start), so announce the pad again later.
  window.announcePad = () => {
    const event = new Event('gamepadconnected');
    event.gamepad = pad;
    window.dispatchEvent(event);
  };
  window.addEventListener('load', window.announcePad);

  // Reads the last frame at low resolution and steers towards the centroid of the yellow pixels.
  let timer = null;
  window.autopilotStats = { frames: 0, seen: 0 };
  window.autopilot = (on) => {
    clearInterval(timer);
    if (!on) return;
    const probe = document.createElement('canvas');
    probe.width = 96;
    probe.height = 60;
    const ctx = probe.getContext('2d', { willReadFrequently: true });
    timer = setInterval(() => {
      const source = document.querySelector('canvas');
      ctx.drawImage(source, 0, 0, probe.width, probe.height);
      const { data } = ctx.getImageData(0, 0, probe.width, probe.height);
      let sx = 0, sy = 0, n = 0;
      // Only the upper half below the HUD: the drone and its yellow exhaust fill the lower half.
      for (let y = 6; y < probe.height / 2; y++) {
        for (let x = 0; x < probe.width; x++) {
          const i = (y * probe.width + x) * 4;
          const [r, g, b] = [data[i], data[i + 1], data[i + 2]];
          // Saturated yellow orbs, not the hazy yellowish cliffs nor the orange ground.
          if (r > 200 && g > 180 && b < 110 && r - b > 120 && g - b > 100) { sx += x; sy += y; n++; }
        }
      }
      window.autopilotStats.frames++;
      window.autopilotStats.brightness = Math.round(data.reduce((sum, v, i) => (i % 4 === 3 ? sum : sum + v), 0) / (data.length * 0.75));
      // Without a target, weave and climb a little (the orbs float above the ground).
      let steerX = Math.sin(performance.now() / 1500) * 0.4, steerY = 0.3;
      if (n > 0) {
        window.autopilotStats.seen++;
        steerX = Math.max(-1, Math.min(1, (sx / n / probe.width - 0.5) * 3));
        steerY = Math.max(-1, Math.min(1, (0.42 - sy / n / probe.height) * 4));
      }
      // Browsers report stick up as -1 (Unity inverts it back), hence -steerY.
      window.pad.set({ axes: [steerX, 0, 0, -steerY], buttons: { 7: 1 } });
    }, 100);
  };
};

await mkdir(out, { recursive: true });
const browser = await chromium.launch({ args: ['--use-angle=swiftshader', '--enable-unsafe-swiftshader', '--ignore-gpu-blocklist'] });
const page = await browser.newPage({ viewport: { width: 960, height: 600 } });
page.on('console', (m) => console.log('[game]', m.text()));
await page.addInitScript(padScript);
await page.goto('http://localhost:8080/');

const canvas = page.locator('canvas').first();
const shot = async (name, wait) => {
  await page.waitForTimeout(wait);
  await canvas.screenshot({ path: join(out, `${name}.png`) });
  console.log('screenshot', name);
};

await shot('01-startup', 20000);
await shot('02-menu', 15000);
// Start the game: click the Play button area, then Enter and the pad A button.
await canvas.click();
await page.keyboard.press('Enter');
await page.evaluate(() => { window.announcePad(); window.pad.set({ buttons: { 0: 1 } }); });
await page.waitForTimeout(300);
await page.evaluate(() => window.pad.set({ buttons: { 0: 0 } }));
await shot('03-game', 8000);

await page.evaluate(() => { window.announcePad(); window.autopilot(true); });
for (let i = 4; i <= 10; i++) await shot(`${String(i).padStart(2, '0')}-flight`, 5000);
console.log('autopilot', JSON.stringify(await page.evaluate(() => ({ ...window.autopilotStats, padPolls: window.padPolls }))));

await browser.close();
server.close();
