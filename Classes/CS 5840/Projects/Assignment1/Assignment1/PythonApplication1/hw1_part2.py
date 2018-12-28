import numpy as np
import cv2
import matplotlib.pyplot as plt

#plt.ion()

#%%
# Step 1: Read in dog.bmp in grayscale using cv2.imread().

img = cv2.imread('C:/Users/Ryan/Desktop/Fall2017/5840/Projects/Assignment1/Assignment1/PythonApplication1/dog.bmp', 0)

#%%
#Step 2: Convert the image to 32-bit float, divide
# by 255 and show it using plt.imshow()

img = img.astype(np.float32) / 255
#plt.imshow(cv2.cvtColor(img, cv2.COLOR_GRAY2RGB))
plt.imshow(img)
plt.show()

#%%
# Step 4: Compute the mean and standard deviation of the image.

print 'Mean color'
mean = np.mean(img, axis=(0, 1))
print mean

print 'Standard deviation'
std = np.std(img, axis=(0, 1))
print std


#%%
# Step 5: Subtract the mean from the image and divide by the standard deviation.

img = np.subtract(img, mean)

img = np.divide(img, std)

#%%
# Step 6: Show the new image.

plt.imshow(img)
plt.show()