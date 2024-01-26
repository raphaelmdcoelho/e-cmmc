-- init-db.sql
CREATE TABLE IF NOT EXISTS "inventory" (
    id SERIAL PRIMARY KEY,
    product_name VARCHAR(255)
);
