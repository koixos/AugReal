import pandas as pd
import numpy as np

num_points = 2000
x = np.random.uniform(-10, 10, num_points)
y = np.random.uniform(-10, 10, num_points)
z = np.random.uniform(-10, 10, num_points)

df = pd.DataFrame({'x':x, 'y':y, 'z':z})
df.to_csv("radar_data.csv", index=False)
