# 选用数据库

本项目选用 `PostgreSQL` 作为数据库，优势在于该 DB 即是关系型数据库，又能很好的支持文档存储。另，本着尝鲜（体验）的原则进行选择。

# 数据库结构

在 `PostgreSQL` 中，有 `Database, Schema, Table` 这样的概念。在创建 DB 时，默认会创建一个 `public` Schema，如果不带 Schema 创建对象（表，视图等），则默认会创建到 `public` 这个 Schema。

在本项目中，新建了 `pm` 这个 Schema，所以在创建数据库对象的时候，都应该选择该 Schema。

如下：

```sql
CREATE TABLE "pm"."user" (
  "id" serial PRIMARY KEY,
  "name" varchar(50) NOT NULL,
  "age" integer NOT NULL DEFAULT 0
);
```
