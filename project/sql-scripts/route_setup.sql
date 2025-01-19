CREATE OR REPLACE PROCEDURE linia.dodaj_trase(
  odlot linia.lotnisko.lotnisko_kod%TYPE, 
  przylot linia.lotnisko.lotnisko_kod%TYPE,
  czas_lotu linia.trasa.czas_lotu%TYPE
)
LANGUAGE plpgsql
AS
$$  
DECLARE
  departure_id integer;
  destination_id integer;
  
BEGIN
  SELECT INTO departure_id l.lotnisko_id FROM linia.lotnisko l WHERE l.lotnisko_kod = odlot;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Lotnisko % nie jest w bazie danych', odlot;
  END IF;
   
  SELECT INTO destination_id l.lotnisko_id FROM linia.lotnisko l WHERE l.lotnisko_kod = przylot;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Lotnisko % nie jest w bazie danych', przylot;
  END IF;
  
  INSERT INTO linia.trasa (czas_lotu, odlot_id, przylot_id) 
  VALUES (czas_lotu, departure_id, destination_id);
 END;
 $$;



CREATE OR REPLACE FUNCTION linia.czytaj_trasy()
  RETURNS TABLE (
  trasa_id int,
  odlot char(4),
  przylot char(4),
  czas_lotu float
  )
LANGUAGE plpgsql
AS
$$
BEGIN
  RETURN QUERY SELECT t.trasa_id, odlot_l.lotnisko_kod odlot, przylot_l.lotnisko_kod przylot, t.czas_lotu
  FROM linia.trasa t
  JOIN linia.lotnisko przylot_l ON t.przylot_id = przylot_l.lotnisko_id
  JOIN linia.lotnisko odlot_l ON t.odlot_id = odlot_l.lotnisko_id;
  RETURN;
  END;
$$;


CREATE OR REPLACE PROCEDURE linia.usun_trase(
  trasa_do_usuniecia_id int)
LANGUAGE plpgsql
AS
$$  
BEGIN
  DELETE FROM linia.trasa t WHERE t.trasa_id = trasa_do_usuniecia_id;
END;
$$;

