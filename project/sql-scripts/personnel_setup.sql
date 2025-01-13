/*
Chat query:

You have SQL table: 
CREATE TABLE person(person_id int, person_name varchar(30), person_surname varchar(30),
                    person_role_id int,
                    PRIMARY KEY (person_id),
                    FOREIGN KEY (person_role_id) REFERENCES role(role_id));
Insert random persons with Polish name/surname, with random and unique IDs in range 1-50, where:
30 persons have person_role_id 1, 8 persons have person_role_id 2, and 12 person have person_role_id 3.  

*/


-- Insert 30 persons with person_role_id 1
INSERT INTO person (person_id, person_name, person_surname, person_role_id) VALUES
(1, 'Kamil', 'Nowak', 1),
(2, 'Marek', 'Kowalski', 1),
(3, 'Piotr', 'Wiśniewski', 1),
(4, 'Agnieszka', 'Wójcik', 1),
(5, 'Anna', 'Kamińska', 1),
(6, 'Paweł', 'Lewandowski', 1),
(7, 'Katarzyna', 'Zielińska', 1),
(8, 'Jakub', 'Szymański', 1),
(9, 'Daria', 'Jankowska', 1),
(10, 'Michał', 'Mazur', 1),
(11, 'Zuzanna', 'Kaczmarek', 1),
(12, 'Tomasz', 'Krawczyk', 1),
(13, 'Weronika', 'Markiewicz', 1),
(14, 'Maciej', 'Chmielowski', 1),
(15, 'Alicja', 'Jaworska', 1),
(16, 'Karol', 'Kubiak', 1),
(17, 'Ewa', 'Piotrowska', 1),
(18, 'Damian', 'Grabowski', 1),
(19, 'Monika', 'Dąbrowska', 1),
(20, 'Grzegorz', 'Zawisza', 1),
(21, 'Olga', 'Wróbel', 1),
(22, 'Bartłomiej', 'Ślusarczyk', 1),
(23, 'Wojciech', 'Sikora', 1),
(24, 'Natalia', 'Pawlak', 1),
(25, 'Szymon', 'Bąk', 1),
(26, 'Łukasz', 'Tomaszewski', 1),
(27, 'Monika', 'Sobolewska', 1),
(28, 'Grzegorz', 'Jasiński', 1),
(29, 'Elżbieta', 'Bielska', 1),
(30, 'Dawid', 'Wielki', 1);

-- Insert 8 persons with person_role_id 2
INSERT INTO person (person_id, person_name, person_surname, person_role_id) VALUES
(31, 'Zofia', 'Nowakowska', 2),
(32, 'Michał', 'Borkowski', 2),
(33, 'Wiktoria', 'Lis', 2),
(34, 'Maciej', 'Stolarz', 2),
(35, 'Jacek', 'Sikorski', 2),
(36, 'Eliza', 'Piętka', 2),
(37, 'Tomasz', 'Szulc', 2),
(38, 'Olga', 'Górska', 2);

-- Insert 12 persons with person_role_id 3
INSERT INTO person (person_id, person_name, person_surname, person_role_id) VALUES
(39, 'Julia', 'Zawisza', 3),
(40, 'Krzysztof', 'Michałowski', 3),
(41, 'Karolina', 'Turska', 3),
(42, 'Igor', 'Majewski', 3),
(43, 'Natalia', 'Ziółkowska', 3),
(44, 'Marek', 'Jasińska', 3),
(45, 'Krzysztof', 'Puchalski', 3),
(46, 'Ewa', 'Wilk', 3),
(47, 'Jakub', 'Bąk', 3),
(48, 'Aleksandra', 'Mroczek', 3),
(49, 'Marcin', 'Kowalczyk', 3),
(50, 'Barbara', 'Baran', 3);
