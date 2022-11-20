import mapDrawer
import sys
import os

import mapReader


def saveAsLocalFile(name, string):
    if os.path.isfile("C:/Users/alexh/electron/backend/hoi4/" + name):
        os.remove("C:/Users/alexh/electron/backend/hoi4/" + name)
    with open("C:/Users/alexh/electron/backend/hoi4/" + name, 'w') as f:
        f.write(string)



provinces = sys.argv[1]
colour = sys.argv[2]

print("done")

def repaintProvinces(provincelist, colour):
    directory = 'C:/Users/alexh/terralis'
    for i in provincelist:
        if i != "":
            provinceid = int(i)

            mapDrawer.redrawprovince(directory, provinceid, colour)



provinces1 = provinces.strip(",").split(",")
colourlist = colour.split(",")
colourfinal = (colourlist[0], colourlist[1], colourlist[2])
saveAsLocalFile("debugdraw1.txt", provinces + " " + colour)
repaintProvinces(provinces1, colourfinal)
saveAsLocalFile("debugdraw.txt", str(provinces) + str(colour))
#print(str(provinces) + str(colour))
#print("bruh moment")
sys.stdout.flush()
