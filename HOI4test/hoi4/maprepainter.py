import mapDrawer
import sys
import os

import mapReader

# TWO 4TH ARGUMENTS: 'temp' and 'full'

provinces = "2583,"
colour = "106,100,95;"
directory = "C:/users/alexh/terralis"
param = "full"
if len(sys.argv) > 3:

    provinces = sys.argv[1]
    colour = sys.argv[2]
    directory = sys.argv[3]
    param = sys.argv[4]
print("done")

def repaintProvinces(provincelist, colour):
    for i in provincelist:
        if i != "" and colour != ",":
            provinceid = int(i)

            mapDrawer.redrawprovinceincache(directory, provinceid, colour)

def radicalMapRepaint(provincelist, colour):
    provPosDict = getProvincePositions(directory)
    mapDrawer.redrawProvinceInMap(directory, provincelist, colour, provPosDict)

def getProvincePositions(directory):
    provdictionary = {}
    f = open(directory + "/mapEditor/provincepos.txt")
    line = f.readline()
    line = line.split(";")
    entries = line[1].split("?")
    for i in entries:
        if i == "":
            continue
        ip = i.split(":")
        #print(ip)
        coords = ip[1].split(",")
        provdictionary[ip[0]] = (coords[0], coords[1])
    f.close()
    #print(provdictionary)
    return provdictionary

def tripletize(string):
    # Assumes it is tripletizable
    if "," in string:
        string = string.split(",")
        return (string[0], string[1], string[2])

#saveAsLocalFile("debugdraw1.txt", provinces + " " + colour)

if param == "temp":
    # this is the old method of taking each province, blitting a new version, and then putting it back into the program
    provinces1 = provinces.strip(",").split(",")
    colourlist = colour.split(",")
    colourfinal = (colourlist[0], colourlist[1], colourlist[2])
    repaintProvinces(provinces1, colourfinal)

elif param == "full":
    # this is the new method which draws directly on the background
    # assumes colour is a giant string for each province, in the form "0,0,0;0,0,0;0,0,0" etc
    provinces1 = provinces.strip(",").split(",")
    colourfinal = list(map(tripletize, colour.split(";")))
    radicalMapRepaint(provinces1, colourfinal)
#saveAsLocalFile("debugdraw.txt", str(provinces) + str(colour))
#print(str(provinces) + str(colour))
#print("bruh moment")
sys.stdout.flush()
