import json
import re
import csv
import mysql.connector


def read_db_config():
    with open('local_config.json') as config_file:
        return json.load(config_file)


def sanitize_name(name):
    illegal_chars = ['\\', '\'', '"']
    for char in illegal_chars:
        name = name.replace(char, '')
    return name


def sanitize_description(text):
    illegal_chars = ['\\', '\'', '"']

    for char in illegal_chars:
        text = text.replace(char, ' ')
    
    html_pattern = re.compile(r'<\w.+>')
    matches = re.findall(html_pattern, text)

    if matches:
        for match in matches:
            text = text.replace(match, '')
    
    return text
    


def is_not_year(string):
    pattern = r'^\d{4}$'

    if re.match(pattern, string):
        return False
    return True


def get_entity_id_by_field(field, table, seeder):
    if table == 'Book':
        col = 'ISBN'
    elif table == 'Author':
        col = 'Name'

    sql = f"SELECT Id FROM {table} WHERE {col} = '{field}';"
    
    seeder.execute(sql)
    try:
        return seeder.fetchall()[0][0]
    except IndexError:
        raise Exception('NOT FOUND ERROR at SQL COMMAND ', sql)


def get_subject_id(subject_name, seeder):
    sql = f"SELECT Id FROM Category WHERE CategoryDescription = '{subject_name}';"
    seeder.execute(sql)
    result = seeder.fetchall()

    if not result:
        return False
    
    return result[0][0]
    

class SQLSeeder():

    def __init__(self, db_config):
        self.db_config = db_config
    
    def __enter__(self):
        self.connection = mysql.connector.connect(
            host = self.db_config['db_host'],
            user = self.db_config['db_user'],
            password = self.db_config['db_password'],
            database = self.db_config['database'],
            auth_plugin='mysql_native_password'
        )
        return self.connection.cursor(buffered=True)
    
    def __exit__(self, exc_type, exc_value, esc_traceback):
        self.connection.commit()
        self.connection.close()
    