const config = require('../config');
const { MysqlClient } = require('../db-providers');

// const pool = mysql.createPool(config.dbConfig);

module.exports = new MysqlClient({});
