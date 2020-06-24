import csv
import numpy as np
import pandas as pd
from utils import * 
from faker import Faker
from random import randint

DB_CONFIG = read_db_config()

def table_is_not_populated(table_name, seeder):
    sql = f"SELECT COUNT(Id) FROM {table_name}"
    seeder.execute(sql)
    result = seeder.fetchall()[0][0]

    if result == 0: return True 

    return False


def prepare_df_col(col_name):
    """Reads one column from the initial
    dataset and returns the unique values"""
    data = pd.read_csv('books_reduces.csv', sep=';', error_bad_lines=False, encoding='ISO-8859-1', usecols=[col_name])
    data.drop_duplicates(inplace=True)
    data.replace(np.nan, 'Unknown', regex=True, inplace=True)

    return data[col_name].values



def link_book_to_author(seeder, isbn, author):
    book_id = get_entity_id_by_field(isbn, 'Book', seeder)
    author_id = get_entity_id_by_field(author, 'Author', seeder)

    sql = f"INSERT INTO BookAuthors (BookId, AuthorId) VALUES ({book_id}, {author_id});"

    try:
        seeder.execute(sql)
    except mysql.connector.errors.ProgrammingError:
        print('ERROR LINKING BOOK TO AUTHOR AT SQL COMMAND', sql)


def insert_author_data(seeder):
    """Seeds the author table"""
    if table_is_not_populated('Author', seeder):
        authors = prepare_df_col('Book-Author')
        for author in authors:
            author = sanitize_name(author)
            sql = f"INSERT INTO Author (Name) VALUES ('{author}');"
            
            try:
                seeder.execute(sql)
                print('Inserted into Author table:', author)
            except mysql.connector.errors.ProgrammingError:
                print('ERROR AT SQL COMMAND:', sql)
            except mysql.connector.errors.IntegrityError:
                continue


def insert_book_data(seeder):
    """Go through the main dataset CSV and insert Books"""
    if table_is_not_populated('Book', seeder):
        with open('books_reduces.csv', 'r', encoding="latin-1") as csv_file:
            reader = csv.reader(csv_file, delimiter=";")

            # skip header
            next(reader)

            for row in reader:
                isbn = row[0]
                title = sanitize_name(row[1])
                author = sanitize_name(row[2])
                year = row[3]
                publisher = sanitize_name(row[4])
                thumbnail_url = row[6]
                cover_big_url = row[7]

                # patches
                if publisher.isnumeric() and is_not_year(year):
                    publisher, year = year, publisher

                try:
                    sql = f"INSERT INTO Book (Title, ISBN, ThumbnailURL, CoverBigURL, Publisher, Year) \
                        VALUES ('{title}', '{isbn}', '{thumbnail_url}', '{cover_big_url}', '{publisher}', {year});"
                    seeder.execute(sql)

                    link_book_to_author(seeder, isbn, author)
                    print('Inserted into Book table:', title)
                except mysql.connector.errors.ProgrammingError:
                    continue
                except mysql.connector.errors.IntegrityError:
                    continue
                except IndexError:
                    break


def insert_user_data(seeder):
    fake = Faker()

    for _ in range(100):
        name = fake.name()
        email = fake.email()
        username = fake.profile()['username']
        password = '5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8'

        sql = f"INSERT INTO User (Name, Username, Password, Email, CreationDate, UserRole) VALUES \
            ('{name}', '{username}', '{password}', '{email}', NOW(), 'User');"
        try:
            seeder.execute(sql)
        except (mysql.connector.errors.ProgrammingError, mysql.connector.errors.IntegrityError):
            continue
        print('INSERTED', _, 'USERS')
    

def insert_ratings_data(seeder):
    books_sql = "SELECT Id FROM Book;"
    seeder.execute(books_sql)
    results = seeder.fetchall()

    for result in results:
        book_id = result[0]

        for _ in range(5):
            user_id = randint(1, 100)
            rating_value = randint(1, 5)

            sql = f"INSERT INTO UserRatings (UserId, BookId, RatingValue) VALUES \
                ({user_id}, {book_id}, {rating_value});"
            seeder.execute(sql)

        print('INSERTED RATINGS FOR BOOK', book_id)

if __name__ == "__main__":

    with SQLSeeder(DB_CONFIG) as seeder:
        insert_author_data(seeder)
        insert_book_data(seeder)
        insert_user_data(seeder)
        insert_ratings_data(seeder)
