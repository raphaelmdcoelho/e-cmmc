CREATE TABLE IF NOT EXISTS "order"(
	id int primary key,
	description varchar(250) NOT NULL,
	username varchar(250) NOT NULL
);

DELETE FROM "order";

INSERT INTO "order" (id, description, username) VALUES (1, 'Copo', 'Test');
INSERT INTO "order" (id, description, username) VALUES (2, 'Batata', 'Test');
