CREATE OR REPLACE PROCEDURE airline.insert_route(
  departure airline.airport.airport_designator%TYPE, 
  destination airline.airport.airport_designator%TYPE,
  flight_time airline.route.flight_time%TYPE
)
LANGUAGE plpgsql
AS
$$  
DECLARE
  departure_id integer;
  destination_id integer;
  
BEGIN
  SELECT INTO departure_id a.airport_id FROM airline.airport a WHERE a.airport_designator = departure;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Airport % not in database', departure;
  END IF;
   
  SELECT INTO destination_id a.airport_id FROM airline.airport a WHERE a.airport_designator = destination;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Airport % not in database', destination;
  END IF;
  
  INSERT INTO airline.route (flight_time, departure_airport_id, arrival_airport_id) 
  VALUES (flight_time, departure_id, destination_id);
 END;
 $$;



CREATE OR REPLACE FUNCTION airline.get_routes()
  RETURNS TABLE (
  route_id int,
  departure char(4),
  destination char(4),
  flight_time float
  )
LANGUAGE plpgsql
AS
$$
BEGIN
  RETURN QUERY SELECT r.route_id, dep_a.airport_designator departure, dest_a.airport_designator destination, r.flight_time
  FROM airline.route r
  JOIN airline.airport dest_a ON r.arrival_airport_id = dest_a.airport_id
  JOIN airline.airport dep_a ON r.departure_airport_id = dep_a.airport_id;
  RETURN;
  END;
$$;


CREATE OR REPLACE PROCEDURE airline.delete_route(
  route_to_delete_id int)
LANGUAGE plpgsql
AS
$$  
BEGIN
  DELETE FROM airline.route r WHERE r.route_id = route_to_delete_id;
END;
$$;

