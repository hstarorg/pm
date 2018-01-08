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

# PostgreSQL 使用规范

1、【强制】对象名（表名、列名、函数名、视图名、序列名、等对象名称）规范，对象名务必只使用小写字母，下划线，数字。不要以pg开头，不要以数字开头，不要使用保留字。 

2、【强制】query中的别名不要使用 "小写字母，下划线，数字" 以外的字符，例如中文。**提示：写成大写再查询结果中，也会被转换为小写。**

3、【推荐】主键索引应以 pk_ 开头， 唯一索引要以 uk_ 开头，普通索引要以 idx_ 打头。

4、【推荐】不建议使用public schema(不同业务共享的对象可以使用public schema)，应该为每个应用分配对应的schema，schema_name最好与user name一致。**提示：未使用该规则**

更多，请参考 [https://yq.aliyun.com/articles/60899](https://yq.aliyun.com/articles/60899)
