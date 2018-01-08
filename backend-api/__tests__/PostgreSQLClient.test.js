const { Client, Connection, Pool, Query, types } = require('pg');
const PostgreSQLClient = require('../src/db-providers/PostgreSQLClient');

describe('PostgreSQLClient', () => {
  let client = new PostgreSQLClient(null);

  // Create the database
  beforeAll(async done => {
    const pool = new Pool({
      host: '192.168.1.200',
      user: 'root',
      password: 'humin',
      database: 'postgres',
      max: 20,
      idleTimeoutMillis: 30000,
      connectionTimeoutMillis: 2000
    });
    client = new PostgreSQLClient(pool);
    const createTableSql = `
CREATE TABLE "pm_db"."user" (
  "id" serial PRIMARY KEY,
  "name" varchar(50) NOT NULL,
  "age" integer NOT NULL DEFAULT 0
);
    `;
    await client.executeNonQuery(createTableSql);
    done();
  });

  // Clear the database
  afterAll(async done => {
    const deleteTableSql = `DROP TABLE "pm_db"."user"`;
    await client.executeNonQuery(deleteTableSql);
    done();
  });

  it('query data', async () => {
    const sql = `
SELECT COUNT(0)::integer FROM "pm_db"."user"
    `;
    const data = await client.executeScalar(sql);
    expect(data.count).toEqual(0);
  });

  it('insert data', async () => {
    const sql = `
INSERT INTO "pm_db"."user"(name, age)
VALUES(@name, @age) RETURNING id;
    `;
    const data = await client.executeScalar(sql, { name: 'humin', age: 15 });
    expect(data.id).toEqual(1);
  });

  it('update data', async () => {
    let sql = `
UPDATE "pm_db"."user"
SET name = 'jay';
    `;
    const count = await client.executeNonQuery(sql);
    expect(count).toEqual(1);
    sql = `
SELECT * FROM "pm_db"."user"
    `;
    const rows = await client.executeQuery(sql);
    expect(rows.length).toEqual(1);
    expect(rows[0].name).toEqual('jay');
  });

  it('delete data', async () => {
    let sql = `
DELETE FROM "pm_db"."user"
WHERE name = @name;
    `;
    const count = await client.executeNonQuery(sql, { name: 'jay' });
    expect(count).toEqual(1);
    sql = `
SELECT * FROM "pm_db"."user"
    `;
    const rows = await client.executeQuery(sql);
    expect(rows.length).toEqual(0);
  });

  it('tran commit', async () => {
    const tran = await client.beginTransaction();
    let sql = `
    INSERT INTO "pm_db"."user"(name, age)
VALUES(@name, @age) RETURNING id;`;
    const changes = await client.executeNonQuery(sql, { name: 'name2', age: 222 }, tran);
    expect(changes).toEqual(1);
    await client.commitTransaction(tran);
    sql = `SELECT COUNT(0)::integer FROM "pm_db"."user"`;
    const data = await client.executeScalar(sql);
    expect(data.count).toEqual(1);
  });

  it('tran rollback', async () => {
    const tran = await client.beginTransaction();
    let sql = `
    INSERT INTO "pm_db"."user"(name, age)
VALUES(@name, @age) RETURNING id;`;
    const changes = await client.executeNonQuery(sql, { name: 'name2', age: 222 }, tran);
    expect(changes).toEqual(1);
    await client.rollbackTransaction(tran);
    sql = `SELECT COUNT(0)::integer FROM "pm_db"."user"`;
    const data = await client.executeScalar(sql);
    expect(data.count).toEqual(1);
  });

  it('test params', async () => {
    let sql = `
    INSERT INTO "pm_db"."user"(name, age) VALUES($1, $2)
    `;
    const data = await client.executeNonQuery(sql, ['jay2', 999]);
    sql = `SELECT * FROM "pm_db"."user" WHERE name=$1`;
    const user = await client.executeScalar(sql, ['jay2']);
    expect(user.age).toEqual(999);
  });
});
