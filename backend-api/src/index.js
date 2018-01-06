const path = require('path');
const Koa = require('koa');
const helmet = require('koa-helmet');
const cors = require('koa-cors');
const responseTime = require('koa-response-time');
const body = require('koa-body');
const logger = require('koa-logger');
const session = require('koa-session');

const { errorHandler } = require('./middlewares');
const { util } = require('./common');
const config = require('./config');

const app = new Koa();
app.keys = [config.sessionSecret];
app.use(logger());
app.use(errorHandler({ env: config.debug ? 'development' : 'production' }));
app.use(responseTime());
app.use(helmet());
app.use(session(config.sessionConfig, app));
app.use(cors());
app.use(body({ multipart: true }));
// Load routes
util.loadRoutes(app, config.routesPath);

process.on('uncaughtException', err => {
  console.error('uncaughtException', err);
});

const server = app.listen(config.port, err => {
  let addr = server.address();
  console.log(`Server started at ${addr.address}:${addr.port}...`);
});
