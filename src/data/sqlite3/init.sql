CREATE TABLE IF NOT EXISTS inventory(
	id int primary key,
	description varchar(250) NOT NULL
);

DELETE FROM inventory;

INSERT INTO inventory (id, description) VALUES (1, 'Copo');
INSERT INTO inventory (id, description) VALUES (2, 'Batata');
