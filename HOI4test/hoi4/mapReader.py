import os
import pprint


nums = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0']


# {'s THAT SHOW UP IN THE LINE  A F T E R  A EQUAL SIGN NEED TO BE FIXED TO BE PARSABLE
def getStratRegions(directory, regiondictionary=None):
    if regiondictionary is None:
        regiondictionary = {}

    newlist = os.listdir(path=directory)

    for i in newlist:
        regionInfo = {}
        with open(directory + "/" + i, "r") as f:
            temp = 'id=0'
            for line in f:
                if line.startswith("id"):
                    temp = line
                if line.startswith("provinces"):
                    if not provinceparser(line):
                        line = f.readline().strip()
                    firstpart = line.split("{")[1].split(" ")
                    if firstpart == ['']:
                        regionInfo["provinces"] = f.readline().strip().split(" ")
                    else:
                        regionInfo["provinces"] = firstpart
                        regionInfo["provinces"] += f.readline().strip().split(" ")
                    if '\\t}' in regionInfo["provinces"]:
                        regionInfo["provinces"].remove('\\t}')
            regiondictionary[int(parseequalsafter(temp))] = regionInfo

    return regiondictionary




def getStates(directory, stateDictionary=None):
    if stateDictionary is None:
        stateDictionary = {}
    newlist = os.listdir(path=directory)

    for i in newlist:
        stateInfo = {}
        with open(directory + "/" + i, "r") as f:
            # line = f.readline().strip()
            temp = "id=0"
            print(i)
            stateInfo["buildings"] = {}
            for line in f:
                line = line.strip()

                if line.startswith("provinces"):
                    if not provinceparser(line):
                        line = f.readline().strip()
                    firstpart = line.split("{")[1].split(" ")
                    if firstpart == ['']:
                        stateInfo["provinces"] = f.readline().strip().strip('\\t}').strip('\t').split(" ")
                    else:
                        stateInfo["provinces"] = firstpart
                        stateInfo["provinces"] += f.readline().strip().strip('\\t}').strip('\t').split(" ")
                    if '\\t}' in stateInfo["provinces"]:
                        stateInfo["provinces"].remove('\\t}')
                elif line.startswith("manpower"):
                    stateInfo["manpower"] = parseequalsafter(line)
                elif line.startswith("history"):
                    while not line.startswith("}"):
                        if line.startswith("buildings") and stateInfo["buildings"] == {}:
                            buildingdict = {}
                            line = f.readline().strip()
                            while not line.startswith("}"):
                                if line != "" and not line.startswith("#"):
                                    provdict = {}
                                    if line[0] in nums:
                                        line1 = f.readline().strip()
                                        while not line1.startswith("}"):
                                            if line1 != "" and line1 != "{":
                                                provdict[line1.split("=")[0].strip()] = int(line1.split("=")[1].split("#")[0])
                                            line1 = f.readline().strip()
                                        buildingdict[parseequalsbefore(line)] = provdict
                                        line = line1
                                    else:
                                        buildingdict[line.split("=")[0].strip()] = int(line.split("=")[1].split("#")[0])
                                line = f.readline().strip()
                            stateInfo["buildings"] = buildingdict
                        if line.startswith("owner") and "owner" not in stateInfo.keys():
                            stateInfo["owner"] = parseequalsafter(line);
                        if line.startswith("victory_points"):
                            if parseequalsafter(line) != '{':
                                if "vp" not in stateInfo.keys():
                                    stateInfo["vp"] = [(parseequalsafter(line).split(" ")[1],
                                                       parseequalsafter(line).split(" ")[2])]
                                else:
                                    stateInfo["vp"] += [(parseequalsafter(line).split(" ")[1],
                                                         parseequalsafter(line).split(" ")[2])]
                            else:
                                line = f.readline().strip()
                                if "vp" not in stateInfo.keys():
                                    stateInfo["vp"] = [(line.split(" ")[0],
                                                        line.split(" ")[1])]
                                else:
                                    stateInfo["vp"] += [(line.split(" ")[0],
                                                         line.split(" ")[1])]
                                if '}' not in line:
                                    f.readline()
                        if line.startswith("add_core_of"):
                            if "cores" not in stateInfo.keys():
                                stateInfo["cores"] = [parseequalsafter(line)]
                            else:
                                stateInfo["cores"] += [parseequalsafter(line)]
                        line = f.readline().strip()

                elif line.startswith("state_category"):
                    stateInfo["category"] = line.split("=")[1].strip()
                elif line.startswith("id"):
                    temp = line
                elif line.startswith("resources"):
                    stateInfo["resources"] = {}
                    line = f.readline().strip()
                    while not line.startswith("}") and not line == "":
                        if not line.startswith("#"):
                            adder = line.split("=")
                            stateInfo["resources"][adder[0].strip()] = adder[1].strip().split(" ")[0].strip()
                        line = f.readline().strip()

            stateDictionary[int(parseequalsafter(temp))] = stateInfo
            f.seek(0)
    #print(stateDictionary)
    return stateDictionary

def parseequalsafter(string):
    return string.split("=")[1].split("#")[0].strip()


def parseequalsbefore(string):
    return string.split("=")[0].split("#")[0].strip()


def provinceparser(line):
    if line.endswith("{"):
        return True
    elif any(num in nums for num in line):
        return True
    else:
        return False


# pather = 'C:/Users/alexh/templetondevelopment/history/states'
# getStates(pather)
# pprint.pprint(stateDictionary)

def getStateData(statedict):
    returnstring = str(len(statedict)) + ":"
    for i in statedict:
        returnstring += str(i) + ";"
        for j in statedict[i]["provinces"]:
            je = str(j).strip()
            if je != "":
                if je[0] in nums:
                    returnstring += je + ","
        #returnstring -= ","
        returnstring += "?"
    return returnstring

def getStateDataFull(statedict):
    returnstring = str(len(statedict)) + ":"
    for i in statedict:
        returnstring += str(i) + ";"
        for j in statedict[i]:
            returnstring += str(j) + ">"
            if j == "buildings" or j == "resources":
                for k in statedict[i][j]:
                    if k[0] in nums:
                        returnstring += "@" + str(k) + "#"
                        for l in statedict[i][j][k]:
                             returnstring += str(l) + "<" + str(statedict[i][j][k][l]) + "+"
                        returnstring += "!"
                    else:

                        returnstring += k + "#" + str(statedict[i][j][k]) + "!"
            elif j == "vp":
                for k in statedict[i][j]:
                    returnstring += k[0] + "#" + k[1] + "!"
            elif isinstance(statedict[i][j], str) or isinstance(statedict[i][j], int):
                returnstring += str(statedict[i][j])
            elif isinstance(statedict[i][j], list):
                for k in statedict[i][j]:
                    if len(k) > 0 and k[0] != " " and not str(k).endswith("}"):
                        returnstring += str(k) + "!"

            returnstring += "$"
        returnstring += "?"
    return returnstring


def getStateDataFullList(statedict):
    returnlist = [str(len(statedict)) + ":"]
    for i in statedict:
        returnstring = ""
        returnstring += str(i) + ";"
        for j in statedict[i]:
            returnstring += str(j) + ">"
            if j == "buildings" or j == "resources":
                for k in statedict[i][j]:
                    if k[0] in nums:
                        returnstring += "@" + str(k) + "#"
                        for l in statedict[i][j][k]:
                            returnstring += str(l) + "<" + str(statedict[i][j][k][l]) + "+"
                        returnstring += "!"
                    else:

                        returnstring += k + "#" + str(statedict[i][j][k]) + "!"
            elif j == "vp":
                for k in statedict[i][j]:
                    returnstring += k[0] + "#" + k[1] + "!"
            elif isinstance(statedict[i][j], str) or isinstance(statedict[i][j], int):
                returnstring += str(statedict[i][j])
            elif isinstance(statedict[i][j], list):
                for k in statedict[i][j]:
                    if len(k) > 0 and k[0] != " " and not str(k).endswith("}"):
                        returnstring += str(k) + "!"

            returnstring += "$"
        returnstring += "?\n"
        returnlist.append(returnstring)
    return returnlist


def saveAsLocalFile(name, string):
    if os.path.isfile(name):
        os.remove(name)
    with open(name, 'w') as f:
        f.write(string)


def saveAsLocalFileList(name, nextlist):
    if os.path.isfile(name):
        os.remove(name)
    with open(name, 'w') as f:
        for i in nextlist:
            f.write(i)
