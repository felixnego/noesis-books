from pyspark.sql import SparkSession
import mysql.connector
import json

def read_db_config():
    with open('local_config.json') as config_file:
        return json.load(config_file)


def create_spark():
    appName = "Recommendetion Engine"
    master = "local"

    # Create Spark session
    spark = SparkSession.builder \
        .appName(appName) \
        .master(master) \
        .getOrCreate()
    
    return spark

class SQLHandler():

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
    