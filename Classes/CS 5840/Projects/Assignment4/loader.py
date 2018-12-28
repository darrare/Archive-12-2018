import numpy as np
from scipy.misc import imread, imresize

def load_att_faces():
    x_train = np.zeros((320,28,28),dtype='float32')+48
    y_train = np.zeros((320,40),dtype='float32')
    n_train = 0

    x_val = np.zeros((40,28,28),dtype='float32')+48
    y_val = np.zeros((40,40),dtype='float32')
    n_val = 0

    x_test = np.zeros((40,28,28),dtype='float32')+48
    y_test = np.zeros((40,40),dtype='float32')
    n_test =0

    for i in range(40):
        for j in range(10):
            path = 'att_faces/s%d/%d.pgm'%(i+1,j+1)
            img = imread(path)
            img = imresize(img,25)
            
            if j<8:
                x_train[n_train,:,2:25] = img
                y_train[n_train,i] = 1
                n_train = n_train+1
            elif j<9:
                x_val[n_val,:,2:25] = img
                y_val[n_val,i] = 1
                n_val = n_val+1
            else:
                x_test[n_test,:,2:25] = img
                y_test[n_test,i] = 1
                n_test = n_test+1
    x_train = x_train/255.
    x_val = x_val/255.
    x_test = x_test/255.

    return (x_train, y_train), (x_val, y_val), (x_test, y_test)
