const { Client, Connection, Pool, Query, types } = require('pg');
const PostgreSQLClient = require('../src/db-providers/PostgreSQLClient');

describe('PostgreSQLClient', () => {
  let client = new PostgreSQLClient(null);
  beforeEach(async () => {
    const pool = new Pool({
      host: '192.168.1.200',
      user: 'root',
      password: 'humin',
      database: 'pm_db',
      max: 20,
      idleTimeoutMillis: 30000,
      connectionTimeoutMillis: 2000
    });
    client = new PostgreSQLClient(pool);
  });

  it('should print types', () => {
    console.log(types);
  });

  it('create table', async () => {
    client.executeQuery('CREATE TABLE user(id int, name varchar(50), age int)');
  });
});
