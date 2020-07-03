from pyspark.mllib.recommendation import MatrixFactorizationModel
from utils import create_spark

def get_recommendations(user_id):
    spark = create_spark()
    
    books = []

    model = MatrixFactorizationModel.load(spark.sparkContext, "./model")

    recommendations = model.recommendProducts(user_id, 10)
    for recommendation in recommendations:
        books.append(recommendation[1])
    
    return books