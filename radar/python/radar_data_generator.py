import pandas as pd
import numpy as np
import kagglehub

kagglehub.dataset_download("ahtshamzafar/tiawr6843aop-gesture-language-data", path="./sample-dataset")

print("Path to dataset files:")

'''num_points = 2000
x = np.random.uniform(-10, 10, num_points)
y = np.random.uniform(-10, 10, num_points)
z = np.random.uniform(-10, 10, num_points)

df = pd.DataFrame({'x':x, 'y':y, 'z':z})
df.to_csv("radar_data.csv", index=False)'''