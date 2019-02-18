#import sys
#sys.path.append('f:/programmingtools/python/lib/site-packages')
import cv2
import numpy as np
import copy
import math
from GestureDetection import Update
from GestureDetection import GetAverageFingerPosition
from GestureDetection import GetFingerTipPositions
            
def Nothing(thr):
    return()

def calculateFingers(res,drawing):  # -> finished bool, cnt: finger count
    #  convexity defect
    hull = cv2.convexHull(res, returnPoints=False)
    if len(hull) > 3:
        defects = cv2.convexityDefects(res, hull)
        if type(defects) != type(None):  # avoid crashing.   (BUG not found)

            cnt = 0
            positions = list()
            for i in range(defects.shape[0]):  # calculate the angle
                s, e, f, d = defects[i][0]
                start = tuple(res[s][0])
                end = tuple(res[e][0])
                far = tuple(res[f][0])
                a = math.sqrt((end[0] - start[0]) ** 2 + (end[1] - start[1]) ** 2)
                b = math.sqrt((far[0] - start[0]) ** 2 + (far[1] - start[1]) ** 2)
                c = math.sqrt((end[0] - far[0]) ** 2 + (end[1] - far[1]) ** 2)
                angle = math.acos((b ** 2 + c ** 2 - a ** 2) / (2 * b * c))  # cosine theorem
                
                if angle <= math.pi / 2:  # angle less than 90 degree, treat as fingers
                    cnt += 1
                    #cv2.circle(drawing, far, 8, [211, 84, 0], -1)
                    #positions.append(far)
                    positions.append(start)
                    positions.append(end)
                    cv2.line(drawing, start, end, [0,255,255], 2)
            fingerTipPositions = GetFingerTipPositions(positions)
            for i in fingerTipPositions:
                cv2.circle(drawing, i, 8, [211, 84, 0], -1)
            avgPosition = GetAverageFingerPosition(fingerTipPositions, cnt + 1)
            if cnt > 0:
                cv2.circle(drawing, (int(avgPosition[0]), int(avgPosition[1])), 8, [255, 255, 255], -1)

            return True, cnt, fingerTipPositions
    return False, 0, list()


# parameters
cap_region_x_begin=0.5  # start point/total width
cap_region_y_end=0.8  # start point/total width
ORI = 60 #  BINARY threshold
BLUR = 20  # GaussianBlur parameter

rangeVal = 1

cnt = 0
positions = list()
drawing = None

camera = cv2.VideoCapture(0)
camera.set(10,200)
camera.set(cv2.CAP_PROP_AUTOFOCUS, 0)
cv2.namedWindow('Settings')
cv2.createTrackbar('ORI', 'Settings', ORI, 100, Nothing)
cv2.createTrackbar('BLUR', 'Settings', BLUR, 100, Nothing)
_, bgModel = camera.read()
bgModel = cv2.flip(bgModel, 1)  # flip the frame horizontally
#bgGray = cv2.cvtColor(bgModel, cv2.COLOR_BGR2GRAY)

while 1:
    BLUR = cv2.getTrackbarPos('BLUR', 'Settings')
    if BLUR % 2 == 0:
        BLUR = BLUR + 1
    ORI = cv2.getTrackbarPos('ORI', 'Settings')
    
    ret, frame = camera.read()
    frame = cv2.flip(frame, 1)  # flip the frame horizontally
    
    #gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    img = cv2.subtract(frame, bgModel)

    ret, img = cv2.threshold(img,15, 255, cv2.THRESH_BINARY)
    img = cv2.erode(img, np.ones((5, 5), np.uint8), iterations = 2)
    



    img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    img[img < 25] = 0
    img[img > 25] = 255
    #ret, img = cv2.threshold(img,15, 255, cv2.THRESH_BINARY)


    for i in range(rangeVal):
        img = cv2.dilate(img, np.ones((5, 5), np.uint8), iterations=1)
        img = cv2.GaussianBlur(img, (5, 5), 0)
        img = cv2.erode(img, np.ones((3, 3), np.uint8), iterations=1)
        
    #img = cv2.erode(img, np.ones((5, 5), np.uint8), iterations=3)
    #ret, img = cv2.threshold(img,25, 255, cv2.THRESH_BINARY)

    
    #cv2.imshow('img', img)

    #ret, thresh = cv2.threshold(img, ORI, 255, cv2.THRESH_BINARY)
    #cv2.imshow('ori', thresh)



    # get the coutours
    img1 = copy.deepcopy(img)
    _, contours, hierarchy = cv2.findContours(img1, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
    length = len(contours)
    maxArea = -1


    drawing = np.zeros(frame.shape, np.uint8)
    #drawing = frame
    for i in range(length):
        res = contours[i]
        hull = cv2.convexHull(res)
        area = cv2.contourArea(res)
        if area > 3000:
            cv2.drawContours(drawing, [res], 0, (0, 255, 0), 2)
            #cv2.drawContours(drawing, [hull], 0, (0, 0, 255), 3)

        isFinishCal,cnt,positions = calculateFingers(res,drawing)
    cv2.imshow('output', drawing)                                              
    Update(cnt, positions, drawing)

    
    # Keyboard OP
    k = cv2.waitKey(10)
    if k == 27:  # press ESC to exit
        cv2.destroyAllWindows()
        break
    if k == 32:
        _, bgModel = camera.read()
        bgModel = cv2.flip(bgModel, 1)  # flip the frame horizontally
        #bgGray = cv2.cvtColor(bgModel, cv2.COLOR_BGR2GRAY)
    if k == ord('a'):
        rangeVal -= 1
        print(rangeVal)
    if k == ord('d'):
        rangeVal += 1
        print(rangeVal)



