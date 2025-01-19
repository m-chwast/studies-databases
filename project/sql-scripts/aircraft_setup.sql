CREATE OR REPLACE VIEW linia.samolot_dlugi_widok AS
SELECT s.samolot_id, ts.typ_samolotu_nazwa
FROM linia.samolot s JOIN linia.typ_samolotu ts 
ON s.typ_samolotu_id = ts.typ_samolotu_id;

CREATE OR REPLACE VIEW linia.samolot_krotki_widok AS
SELECT ts.typ_samolotu_nazwa, COUNT(*) ilosc
FROM linia.samolot s
JOIN linia.typ_samolotu ts
ON s.typ_samolotu_id = ts.typ_samolotu_id
GROUP BY ts.typ_samolotu_nazwa
ORDER BY ilosc DESC;


GRANT SELECT ON linia.samolot_krotki_widok TO db_user_1;
GRANT SELECT ON  linia.samolot_dlugi_widok TO db_user_1;

