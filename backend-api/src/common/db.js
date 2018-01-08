const config = require('../config');
const { Pool } = require('pg');
const { PostgreSQLClient } = require('../db-providers');

const pool = new Pool(config.dbConfig);

module.exports = new PostgreSQLClient(pool);
