import shutil
from utils import * 
from pyspark.sql import SparkSession
from pyspark.sql.types import StructType, StructField, IntegerType
from pyspark.mllib.recommendation import ALS, MatrixFactorizationModel, Rating

# RUN: export PYSPARK_PYTHON=/usr/local/bin/python3
# export JAVA_HOME=$(/usr/libexec/java_home -v 1.8)

def train_model():
    spark = create_spark()

    DB_CONFIG = read_db_config()

    with SQLHandler(DB_CONFIG) as db_handler:
        ratings_sql = "SELECT UserId, BookId, RatingValue FROM UserRatings;"
        ratings = db_handler.execute(ratings_sql)
        next(db_handler)  # eliminate the header
        results = db_handler.fetchall()
        


    # Convert list to RDD
    rdd = spark.sparkContext.parallelize(results)
    rdd = rdd.map(lambda row: Rating(int(row[0]), int(row[1]), int(row[2]))).cache()

    print("Training recommendation model...")
    rank = 10
    # Lowered numIterations to ensure it works on lower-end systems
    numIterations = 6
    model = ALS.train(rdd, rank, numIterations)

    shutil.rmtree("./model")
    
    model.save(spark.sparkContext, "./model")
    print("Model trained and saved successfully")



