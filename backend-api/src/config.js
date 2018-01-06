const path = require('path');

module.exports = {
  debug: true,
  port: 7000,
  apiPrefix: '/api/v1',
  routesPath: path.join(__dirname, 'routes'),
  printSql: true,
  sessionSecret: 'h5ds_session_secret_key',
  hashSecret: 'h5ds', // 千万不要中途修改，危险
  sessionConfig: {
    key: 'sid', // cookie名称
    maxAge: 1000 * 60 * 60 * 24, // 24小时
    overwrite: true, /** (boolean) can overwrite or not (default true) */
    httpOnly: true, /** (boolean) httpOnly or not (default true) */
    signed: true, /** (boolean) signed or not (default true) */
    rolling: false, // 强制为每个用户设置session
  },
  dbConfig: {
    host: '192.168.1.200',
    port: '3308',
    database: 'h5ds',
    user: 'root',
    password: 'humin',
    connectionLimit: 10
  },
  fileUploadFolder: path.join(__dirname, 'upload')
};
