SET search_path to airline;

-- inserting aircraft types
INSERT INTO airline.aircraft_type (aircraft_type_id, aircraft_type_name) VALUES
(1, 'B737-800'), (2, 'A320-200'), (3, 'A320neo'), (4, 'A319-100');

-- inserting initial fleet
INSERT INTO airline.aircraft VALUES
  (1, 2), (2, 2), (3, 2), 
  (4, 3), 
  (5, 1), (6, 1), (7, 1), (8, 1),
  (9, 4),
  (10, 3), (11, 3);

-- inserting available roles
INSERT INTO airline.role VALUES 
(1, 'Flight Attendant'), (2, 'Captain'), (3, 'First Officer');

-- adding permissions to use airline schema for the user
GRANT USAGE ON SCHEMA airline TO db_user_1;
-- adding permissions to read all tables for the user
GRANT SELECT ON ALL TABLES IN SCHEMA airline TO db_user_1;
GRANT INSERT ON person TO db_user_1;

-- getting all users in db - helper
-- SELECT * FROM pg_catalog.pg_user;

