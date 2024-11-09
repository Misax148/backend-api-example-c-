CREATE EXTENSION IF NOT EXISTS pgcrypto;

INSERT INTO movies (title, release_date, description, genre, rating) VALUES
    ('Inception', '2010-07-16', 'A thief who steals corporate secrets...', 'Sci-Fi', 8.8),
    ('The Dark Knight', '2008-07-18', 'When the menace known as the Joker...', 'Action', 9.0),
    ('Forrest Gump', '1994-07-06', 'The story of a man who witnesses and influences...', 'Drama', 8.8),
    ('The Shawshank Redemption', '1994-09-23', 'Two imprisoned men bond over a number of years...', 'Drama', 9.3),
    ('The Godfather', '1972-03-24', 'The aging patriarch of an organized crime dynasty...', 'Crime', 9.2);

INSERT INTO actors (name, biography, birthdate) VALUES
    ('Leonardo DiCaprio', 'American actor and producer...', '1974-11-11'),
    ('Christian Bale', 'English actor...', '1974-01-30'),
    ('Tom Hanks', 'American actor and filmmaker...', '1956-07-09'),
    ('Morgan Freeman', 'American actor and narrator...', '1937-06-01'),
    ('Marlon Brando', 'American actor, film director...', '1924-04-03');

INSERT INTO movie_actors (movie_id, actor_id) VALUES
    ((SELECT id FROM movies WHERE title = 'Inception'), (SELECT id FROM actors WHERE name = 'Leonardo DiCaprio')),
    ((SELECT id FROM movies WHERE title = 'The Dark Knight'), (SELECT id FROM actors WHERE name = 'Christian Bale')),
    ((SELECT id FROM movies WHERE title = 'Forrest Gump'), (SELECT id FROM actors WHERE name = 'Tom Hanks')),
    ((SELECT id FROM movies WHERE title = 'The Shawshank Redemption'), (SELECT id FROM actors WHERE name = 'Morgan Freeman')),
    ((SELECT id FROM movies WHERE title = 'The Godfather'), (SELECT id FROM actors WHERE name = 'Marlon Brando'));

INSERT INTO users (username, email, password_hash, role) VALUES
    ('admin', 'admin@movies.com', crypt('Pass@word1', gen_salt('bf')), 'Admin'),
    ('user1', 'user1@example.com', crypt('Pass@word2', gen_salt('bf')), 'User'),
    ('user2', 'user2@example.com', crypt('Pass@word3', gen_salt('bf')), 'User');