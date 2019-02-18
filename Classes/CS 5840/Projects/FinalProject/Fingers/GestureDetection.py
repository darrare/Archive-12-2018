import cv2
import numpy as np
import copy
import math
import time
import sys

#rectangle tuning stuff
smallWidth = 30
smallHeight = 30
bigWidth = 200
bigHeight = 150

curRectangleTopLeft = (150,150)
curSmall = True
curWidth = smallWidth
curHeight = smallHeight


oldTime = time.time()
startTime = time.time()

index = 0
fingerHistory = [0,0,0,0,0,0,0,0,0,0]

def Update(numFingers, positions, frame):
    global curRectangleTopLeft
    global curWidth
    global curHeight
    global curSmall
    if numFingers > 0:
        numFingers += 1
    span = GetSpan()
    UpdateFingerHistory(numFingers)
    curFingers = GetCurrentFingers()
    avgPosition = GetAverageFingerPosition(positions, numFingers)
    if numFingers > 0:
        cv2.circle(frame, (int(avgPosition[0]), int(avgPosition[1])), 8, [0, 0, 0], -1)
    cv2.rectangle(frame, (int(curRectangleTopLeft[0]), int(curRectangleTopLeft[1])),
                  (int(curRectangleTopLeft[0] + curWidth), int(curRectangleTopLeft[1] + curHeight)), (255, 255, 255), -1)
    cv2.imshow('original', frame)
    if curSmall:
        result = IsPointContainedInRectangle(avgPosition, curWidth, curHeight, curRectangleTopLeft)
        if result and curFingers == 2:
            curRectangleTopLeft = (int(avgPosition[0] - smallWidth / 2), int(avgPosition[1] - smallHeight / 2))
        if result and curFingers == 3:
            curSmall = False
        if curWidth > smallWidth:
            curWidth *= .9
            if curWidth < smallWidth:
                curWidth = smallWidth
        if curHeight > smallHeight:
            curHeight *= .9
            if curHeight < smallHeight:
                curHeight = smallHeight
    else:
        result = IsPointContainedInRectangle(avgPosition, curWidth, curHeight, curRectangleTopLeft)
        if result and curFingers == 4:
            curSmall = True
        if curWidth < bigWidth:
            curWidth *= 1.1
            if curWidth > bigWidth:
                curWidth = bigWidth
        if curHeight < bigHeight:
            curHeight *= 1.1
            if curHeight > bigHeight:
                curHeight = bigHeight


#Utilities
def GetSpan():
    global oldTime
    curTime = time.time()
    span = curTime - oldTime
    oldTime = curTime
    return span

def UpdateFingerHistory(numFingers):
    global index
    global fingerHistory
    fingerHistory[index] = numFingers
    index += 1
    if index == 10:
        index = 0

def GetCurrentFingers():
    global fingerHistory
    return GetMostCommonElement(fingerHistory)

from itertools import groupby as g
def GetMostCommonElement(L):
  return max(g(sorted(L)), key=lambda x, v:(len(list(v)),-L.index(x)))[0]

def GetAverageFingerPosition(positions, N):
    if N == 0:
        return (0, 0)
    xVal = 0.0;
    yVal = 0.0;
    for i in positions:
        xVal += i[0]
        yVal += i[1]
    return (xVal / N, yVal / N)

def IsPointContainedInRectangle(point, recWidth, recHeight, recTopLeft):
    result = True
    if point[0] < recTopLeft[0] or point[0] > recTopLeft[0] + recWidth:
        result = False
    if point[1] < recTopLeft[1] or point[1] > recTopLeft[1] + recHeight:
        result = False
    return result

def RemoveSimilarPositions(positions, distance):
    newPositions = list()
    if len(positions) == 0:
        return newPositions
    temp = positions[0]
    for i,j in zip(positions, positions[1:]):
        temp = j
        if math.sqrt((i[0] - j[0])**2 + (i[1] - j[1])**2) > distance:
            newPositions.append(i)
    newPositions.append(temp)
    return newPositions
            

def GetFingerTipPositions(positions):
    cleanedPositions = RemoveSimilarPositions(positions, 25)
    return cleanedPositions
    


