CREATE TABLE inventory IF NOT EXISTS(
	id int primary key,
	description varchar(250) NOT NULL
);

INSERT INTO inventory (id, description) VALUES (1, 'Copo');
INSERT INTO inventory (id, description) VALUES (2, 'Batata');
