-- inserting aircraft types
INSERT INTO linia.typ_samolotu (typ_samolotu_id, typ_samolotu_nazwa) VALUES
(1, 'B737-800'), (2, 'A320-200'), (3, 'A320neo'), (4, 'A319-100');

-- inserting initial fleet
INSERT INTO linia.samolot VALUES
  (1, 2), (2, 2), (3, 2), 
  (4, 3), 
  (5, 1), (6, 1), (7, 1), (8, 1),
  (9, 4),
  (10, 3), (11, 3);

-- inserting available roles
INSERT INTO linia.rola VALUES 
(1, 'Steward'), (2, 'Kapitan'), (3, 'Pierwszy Oficer');

-- adding permissions to use linia schema for the user
GRANT USAGE ON SCHEMA linia TO db_user_1;
-- adding permissions to read all tables for the user
GRANT SELECT ON ALL TABLES IN SCHEMA linia TO db_user_1;
-- adding other permissions
GRANT INSERT ON linia.osoba TO db_user_1;
GRANT DELETE ON linia.osoba TO db_user_1;

-- grant permissions for route procedures/functions
GRANT EXECUTE ON PROCEDURE linia.dodaj_trase TO db_user_1;
GRANT EXECUTE ON FUNCTION linia.czytaj_trasy TO db_user_1;
GRANT EXECUTE ON PROCEDURE linia.usun_trase TO db_user_1;


-- getting all users in db - helper
-- SELECT * FROM pg_catalog.pg_user;

