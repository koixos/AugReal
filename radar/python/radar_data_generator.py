import os
import pandas as pd
import numpy as np

download_path = './sample-dataset'
os.makedirs(download_path, exist_ok=True)
os.system(f"kaggle datasets download -d ahtshamzafar/tiawr6843aop-gesture-language-data -p {download_path} --unzip")


'''num_points = 2000
x = np.random.uniform(-10, 10, num_points)
y = np.random.uniform(-10, 10, num_points)
z = np.random.uniform(-10, 10, num_points)

df = pd.DataFrame({'x':x, 'y':y, 'z':z})
df.to_csv("radar_data.csv", index=False)
'''