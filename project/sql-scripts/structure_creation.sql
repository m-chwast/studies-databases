-- use this to completely delete the schema
-- DROP SCHEMA airline CASCADE;

CREATE SCHEMA airline;
SET SEARCH_PATH to airline;

CREATE TABLE aircraft_type(aircraft_type_id int, aircraft_type_name char(20),
                           PRIMARY KEY (aircraft_type_id));
CREATE TABLE aircraft(aircraft_id int, aircraft_type_id int,
                      PRIMARY KEY (aircraft_id),
                      FOREIGN KEY (aircraft_type_id) REFERENCES aircraft_type(aircraft_type_id));

CREATE TABLE role(role_id int, role_name char(30),
                  PRIMARY KEY (role_id));
CREATE TABLE person(person_id int GENERATED ALWAYS AS IDENTITY, person_name varchar(30), person_surname varchar(30),
                    person_role_id int,
                    PRIMARY KEY (person_id),
                    FOREIGN KEY (person_role_id) REFERENCES role(role_id));

CREATE TABLE airport (airport_id int, airport_designator char(4), airport_name varchar(100),
                      PRIMARY KEY (airport_id));

CREATE TABLE route(route_id int, flight_time float, departure_airport_id int, arrival_airport_id int,
                   PRIMARY KEY (route_id),
                   FOREIGN KEY (departure_airport_id) REFERENCES airport(airport_id),
                   FOREIGN KEY (arrival_airport_id) REFERENCES airport(airport_id));

CREATE TABLE flight(flight_id int, flight_date date, flight_start_time time,
                    route_id int, aircraft_id int, captain_id int,
                    first_officer_id int, cabin_crew_id int[6],
                    PRIMARY KEY (flight_id),
                    FOREIGN KEY (route_id) REFERENCES route(route_id),
                    FOREIGN KEY (aircraft_id) REFERENCES aircraft(aircraft_id),
                    FOREIGN KEY (captain_id) REFERENCES person(person_id),
                    FOREIGN KEY (first_officer_id) REFERENCES person(person_id));

CREATE TABLE flight_crew(flight_crew_id int, flight_id int, person_id int,
                         PRIMARY KEY (flight_crew_id),
                         FOREIGN KEY (flight_id) REFERENCES flight(flight_id),
                         FOREIGN KEY (person_id) REFERENCES person(person_id));
