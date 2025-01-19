DROP VIEW airline.flights_view;

CREATE OR REPLACE VIEW airline.flights_view AS
SELECT f.flight_id, f.route_id, f.flight_date
FROM airline.flight f; 

GRANT SELECT ON airline.flights_view TO db_user_1;

INSERT INTO airline.flight (route_id, aircraft_id, flight_date) VALUES
(6, 1, '2025-1-20 11:00');

