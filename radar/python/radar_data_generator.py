import os
import pandas as pd
import numpy as np
import kagglehub

<<<<<<< HEAD
kagglehub.dataset_download("ahtshamzafar/tiawr6843aop-gesture-language-data", path="./sample-dataset")

print("Path to dataset files:")
=======
download_path = './sample-dataset'
os.makedirs(download_path, exist_ok=True)
os.system(f"kaggle datasets download -d ahtshamzafar/tiawr6843aop-gesture-language-data -p {download_path} --unzip")

>>>>>>> de55a084e5310c8393f746b7ca799b6573c79276

'''num_points = 2000
x = np.random.uniform(-10, 10, num_points)
y = np.random.uniform(-10, 10, num_points)
z = np.random.uniform(-10, 10, num_points)

df = pd.DataFrame({'x':x, 'y':y, 'z':z})
<<<<<<< HEAD
df.to_csv("radar_data.csv", index=False)'''
=======
df.to_csv("radar_data.csv", index=False)
'''
>>>>>>> de55a084e5310c8393f746b7ca799b6573c79276
