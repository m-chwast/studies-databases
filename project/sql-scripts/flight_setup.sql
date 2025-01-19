DROP VIEW airline.flights_view;

CREATE OR REPLACE VIEW airline.flights_view AS
SELECT f.flight_id, f.route_id, f.flight_date
FROM airline.flight f; 

GRANT SELECT ON airline.flights_view TO db_user_1;

INSERT INTO airline.flight (route_id, aircraft_id, flight_date) VALUES
(6, 1, '2025-1-20 11:00');




DROP FUNCTION airline.get_flight_details;

CREATE OR REPLACE FUNCTION airline.get_flight_details(selected_flight_id int)
  RETURNS TABLE (
  aircraft_id int,
  aircraft_type airline.aircraft_type.aircraft_type_name%TYPE,
  route text
  )
LANGUAGE plpgsql
AS
$$
BEGIN
  RETURN QUERY 
  SELECT f.aircraft_id, at.aircraft_type_name, dep_a.airport_designator || '-' || dest_a.airport_designator
  FROM airline.flight f
  
  JOIN airline.aircraft ac ON f.aircraft_id = ac.aircraft_id
  JOIN airline.aircraft_type at ON ac.aircraft_type_id = at.aircraft_type_id
  
  JOIN airline.route r ON f.route_id = r.route_id
  JOIN airline.airport dep_a ON r.departure_airport_id = dep_a.airport_id
  JOIN airline.airport dest_a ON r.arrival_airport_id = dest_a.airport_id
  
  WHERE f.flight_id = selected_flight_id;
  
  RETURN;
  END;
$$;

GRANT EXECUTE ON FUNCTION airline.get_flight_details TO db_user_1;


CREATE OR REPLACE PROCEDURE airline.insert_flight(
  route int, 
  flight_date timestamp,
  aircraft int
)
LANGUAGE plpgsql
AS
$$  
DECLARE
  tmp_id integer;
  
BEGIN
  SELECT FROM airline.route r WHERE r.route_id = route INTO tmp_id;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Route % not in database', route;
  END IF;

  SELECT FROM airline.aircraft a WHERE a.aircraft_id = aircraft INTO tmp_id;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Aircraft % not in database', aircraft;
  END IF;
   
  INSERT INTO airline.flight (route_id, flight_date, aircraft_id) 
  VALUES (route, flight_date, aircraft);
 END;
 $$;
 
 GRANT EXECUTE ON PROCEDURE airline.insert_flight TO db_user_1;
 GRANT INSERT ON airline.flight TO db_user_1;
