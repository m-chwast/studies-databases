CREATE OR REPLACE VIEW airline.aircraft_long_view AS
SELECT a.aircraft_id, at.aircraft_type_name
FROM airline.aircraft a JOIN airline.aircraft_type at 
ON a.aircraft_type_id = at.aircraft_type_id;

CREATE OR REPLACE VIEW airline.aircraft_short_view AS
SELECT at.aircraft_type_name, COUNT(*) cnt
FROM airline.aircraft a
JOIN airline.aircraft_type at
ON a.aircraft_type_id = at.aircraft_type_id
GROUP BY at.aircraft_type_name
ORDER BY cnt DESC;


GRANT SELECT ON airline.aircraft_short_view TO db_user_1;
GRANT SELECT ON  airline.aircraft_long_view TO db_user_1;

