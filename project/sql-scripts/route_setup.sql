CREATE OR REPLACE PROCEDURE insert_route(
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

CREATE OR REPLACE PROCEDURE get_routes()
LANGUAGE plpgsql
AS
$$
BEGIN

END;
$$;
