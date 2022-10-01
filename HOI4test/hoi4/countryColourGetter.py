import os



def getCountries(directory, countryDict=None):
    if countryDict is None:
        countryDict = {}
    newlist = os.listdir(path=directory + "/country_tags")
    for i in newlist:
        with open(directory + "/country_tags" + "/" + i, "r") as f:
            for line in f:
                line = line.strip()
                path = line.split("\"")
                if len(path) > 1:
                    countryDict[line[:3]] = [path[1]]
    for i in countryDict:
        if os.path.isfile(directory + "/" + countryDict[i][0]):
            with open(directory + "/" + countryDict[i][0], "r") as f:
                for line in f:
                    if line.strip().startswith("color"):
                        temptuplet = line.split("{")[1].split("}")[0].strip().split(" ")
                        for j in temptuplet:
                            if j == "":
                                temptuplet.remove(j)
                        countryDict[i].append((int(temptuplet[0]), int(temptuplet[1]), int(temptuplet[2])))

        else:
            print("Did not find any such file for tag " + i + "!")
        if len(countryDict[i]) == 1:
            countryDict[i].append((250, 250, 250))

    return countryDict

def countrydicttostring(countrydict):
    returnstring = str(len(countrydict)) + ":"
    for i in countrydict:
        returnstring += str(i) + ";"
        for j in countrydict[i][1]:
            returnstring += str(j) + ","
        returnstring += "?"
    return returnstring



pather = 'C:/Users/alexh/templetondevelopment/common'
