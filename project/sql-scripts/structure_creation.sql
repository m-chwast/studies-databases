-- use this to completely delete the schema
-- DROP SCHEMA linia CASCADE;

CREATE SCHEMA linia;
SET SEARCH_PATH to linia;

CREATE TABLE linia.typ_samolotu(typ_samolotu_id int, typ_samolotu_nazwa char(20),
                           PRIMARY KEY (typ_samolotu_id));
CREATE TABLE linia.samolot(samolot_id int, typ_samolotu_id int,
                      PRIMARY KEY (samolot_id),
                      FOREIGN KEY (typ_samolotu_id) REFERENCES linia.typ_samolotu(typ_samolotu_id));

CREATE TABLE linia.rola(rola_id int, rola_nazwa char(30),
                  PRIMARY KEY (rola_id));
CREATE TABLE linia.osoba(osoba_id int GENERATED ALWAYS AS IDENTITY, osoba_imie varchar(30), osoba_nazwisko varchar(30),
                    osoba_rola_id int,
                    PRIMARY KEY (osoba_id),
                    FOREIGN KEY (osoba_rola_id) REFERENCES linia.rola(rola_id));

CREATE TABLE linia.lotnisko(lotnisko_id int GENERATED ALWAYS AS IDENTITY, lotnisko_kod char(4), lotnisko_nazwa varchar(100),
                      PRIMARY KEY (lotnisko_id));

CREATE TABLE linia.trasa(trasa_id int GENERATED ALWAYS AS IDENTITY, czas_lotu float, odlot_id int, przylot_id int,
                   PRIMARY KEY (trasa_id),
                   FOREIGN KEY (odlot_id) REFERENCES linia.lotnisko(lotnisko_id),
                   FOREIGN KEY (przylot_id) REFERENCES linia.lotnisko(lotnisko_id));

CREATE TABLE linia.lot(lot_id int GENERATED ALWAYS AS IDENTITY, lot_data date,
                    trasa_id int, samolot_id int,
                    PRIMARY KEY (lot_id),
                    FOREIGN KEY (trasa_id) REFERENCES linia.trasa(trasa_id),
                    FOREIGN KEY (samolot_id) REFERENCES linia.samolot(samolot_id));

CREATE TABLE linia.lot_zaloga(lot_zaloga_id int GENERATED ALWAYS AS IDENTITY, lot_id int, osoba_id int,
                         PRIMARY KEY (lot_zaloga_id),
                         FOREIGN KEY (lot_id) REFERENCES linia.lot(lot_id),
                         FOREIGN KEY (osoba_id) REFERENCES linia.osoba(osoba_id));
