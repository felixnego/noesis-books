from flask import Flask, jsonify
from flask_cors import CORS
from train import train_model
from recommend import get_recommendations

app = Flask(__name__)
CORS(app)

@app.route('/train')
def train():
    train_model()
    return 'Model trained successfully'

@app.route('/recommend/<user_id>')
def recommend(user_id):
    return jsonify(get_recommendations(int(user_id)))

if __name__ == '__main__':
    app.run(debug=True)