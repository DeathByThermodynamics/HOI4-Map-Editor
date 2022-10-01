def getProvinceData(directory):
    '''Return a dictionary of provinces, where the keys are the province id and the values are the RGB values of
    the provinces on the map.

    :param directory: the directory to read the province definition.
    :return: the dictionary.

    0: provinceid
    1, 2, 3: rgb colours
    4: land / sea
    5: is_coastal
    6: terrain
    7: continent
    '''
    provdictionary = {}
    f = open(directory + "/map/definition.csv")
    line = f.readline()
    while line:
        parsedline = line.split(";")
        if len(parsedline) > 3:
            provdictionary[int(parsedline[0])] = parsedline
        line = f.readline()
    f.close()
    return provdictionary


def getProvinceDataReverse(directory):
    '''Return a dictionary of provinces, where the values are the province id and the keys are the RGB values of
    the provinces on the map.

    :param directory: the directory to read the province definition.
    :return: the dictionary.
    '''
    provdictionary = {}
    f = open(directory + "/map/definition.csv")
    line = f.readline()
    while line:
        parsedline = line.split(";")
        if len(parsedline) > 3:
            provdictionary[(int(parsedline[1]), int(parsedline[2]), int(parsedline[3]))] = parsedline[0]
        line = f.readline()
    f.close()
    #print(provdictionary)
    return provdictionary
