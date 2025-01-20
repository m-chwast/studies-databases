DROP VIEW linia.loty_widok;

CREATE OR REPLACE VIEW linia.loty_widok AS
SELECT lot.lot_id, lot.trasa_id, lot.lot_data
FROM linia.lot lot; 

GRANT SELECT ON linia.loty_widok TO db_user_1;

INSERT INTO linia.lot (trasa_id, samolot_id, lot_data) VALUES
(6, 1, '2025-1-20 11:00');




DROP FUNCTION linia.czytaj_detale_lotu;

CREATE OR REPLACE FUNCTION linia.czytaj_detale_lotu(wybrany_lot_id int)
  RETURNS TABLE (
  samolot_id int,
  samolot_typ linia.typ_samolotu.typ_samolotu_nazwa%TYPE,
  route text
  )
LANGUAGE plpgsql
AS
$$
BEGIN
  RETURN QUERY 
  SELECT lot.samolot_id, ts.typ_samolotu_nazwa, odlot_l.lotnisko_kod || '-' || przylot_l.lotnisko_kod
  FROM linia.lot lot
  
  JOIN linia.samolot s ON lot.samolot_id = s.samolot_id
  JOIN linia.typ_samolotu ts ON s.typ_samolotu_id = ts.typ_samolotu_id
  
  JOIN linia.trasa t ON lot.trasa_id = t.trasa_id
  JOIN linia.lotnisko odlot_l ON t.odlot_id = odlot_l.lotnisko_id
  JOIN linia.lotnisko przylot_l ON t.przylot_id = przylot_l.lotnisko_id
  
  WHERE lot.lot_id = wybrany_lot_id;
  
  RETURN;
  END;
$$;

GRANT EXECUTE ON FUNCTION linia.czytaj_detale_lotu TO db_user_1;


CREATE OR REPLACE PROCEDURE linia.dodaj_lot(
  trasa int, 
  lot_data timestamp,
  samolot int
)
LANGUAGE plpgsql
AS
$$  
DECLARE
  tmp_id integer;
  
BEGIN
  SELECT FROM linia.trasa t WHERE t.trasa_id = trasa INTO tmp_id;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Trasa % nie jest w bazie danych', trasa;
  END IF;

  SELECT FROM linia.samolot s WHERE s.samolot_id = samolot INTO tmp_id;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Samolot % nie jest w bazie danych', samolot;
  END IF;
   
  INSERT INTO linia.lot (trasa_id, lot_data, samolot_id) 
  VALUES (trasa, lot_data, samolot);
 END;
 $$;
 
 GRANT EXECUTE ON PROCEDURE linia.dodaj_lot TO db_user_1;
 GRANT INSERT ON linia.lot TO db_user_1;
 
 
 
 GRANT EXECUTE ON PROCEDURE linia.dodaj_osobe_do_lotu TO db_user_1;
 GRANT INSERT ON linia.lot_zaloga TO db_user_1;

CREATE OR REPLACE PROCEDURE linia.dodaj_osobe_do_lotu(
  lot int, 
  osoba int
)
LANGUAGE plpgsql
AS
$$  
DECLARE
  tmp_id integer;
  osoba_rola integer;
  tmp_data RECORD;
  
BEGIN
  SELECT FROM linia.lot l WHERE l.lot_id = lot INTO tmp_id;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Lot % nie jest w bazie danych', lot;
  END IF;

  SELECT FROM linia.osoba o WHERE o.osoba_id = osoba INTO tmp_id;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Osoba % nie jest w bazie danych', osoba;
  END IF;

  SELECT r.rola_id
  FROM linia.rola r
  JOIN linia.osoba o ON o.osoba_rola_id = r.rola_id
  WHERE o.osoba_id = osoba
  INTO osoba_rola;

  IF osoba_rola = 2 OR osoba_rola = 3 THEN
        SELECT r.rola_id
        FROM linia.lot l
        JOIN linia.lot_zaloga lz ON l.lot_id = lz.lot_id
        JOIN linia.osoba o ON o.osoba_id = lz.osoba_id
        JOIN linia.rola r ON r.rola_id = o.osoba_rola_id
        WHERE l.lot_id = lot AND o.osoba_rola_id = osoba_rola 
        INTO tmp_data;
        IF FOUND THEN
          RAISE EXCEPTION 'Podwójna rola zabroniona (%)', osoba_rola;
        END IF;
  END IF;

  SELECT o.osoba_id
  FROM linia.lot l
  JOIN linia.lot_zaloga lz ON l.lot_id = lz.lot_id
  JOIN linia.osoba o ON o.osoba_id = lz.osoba_id
  JOIN linia.rola r ON r.rola_id = o.osoba_rola_id
  WHERE l.lot_id = lot AND o.osoba_id = osoba 
  INTO tmp_data;
  IF FOUND THEN
    RAISE EXCEPTION 'Ta sama osoba zabroniona (%)', osoba;
  END IF;
   
  
  INSERT INTO linia.lot_zaloga (lot_id, osoba_id) 
  VALUES (lot, osoba);
 END;
 $$;
 
 

DROP PROCEDURE linia.usun_osobe_z_lotu;
CREATE OR REPLACE PROCEDURE linia.usun_osobe_z_lotu(
  lot_id_do_usuniecia int, 
  osoba_id_do_usuniecia int
)
LANGUAGE plpgsql
AS
$$ 
BEGIN
DELETE FROM linia.lot_zaloga lz WHERE lz.lot_id =  lot_id_do_usuniecia AND lz.osoba_id = osoba_id_do_usuniecia;
END;
$$;

GRANT EXECUTE ON PROCEDURE linia.usun_osobe_z_lotu TO db_user_1;
GRANT DELETE ON linia.lot_zaloga TO db_user_1;



GRANT DELETE ON linia.lot TO db_user_1;

