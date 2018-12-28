
import numpy as np
import cv2

#load a color imagfe in grayscale
img = cv2.imread('C:/Users/Ryan/Desktop/Fall2017/5840/Projects/Assignment1/Assignment1/PythonApplication1/dog.bmp')

cv2.imshow('image', img)
cv2.waitKey(0)
cv2.destroyAllWindows()