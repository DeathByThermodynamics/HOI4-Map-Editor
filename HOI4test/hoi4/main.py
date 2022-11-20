# This is the main loop for the HOI4 map extractor.

import mapDrawer
import mapReader
import countryColourGetter
import definitionReader
import fileExtractor
import mapDrawer
import sys
import os

backuppath = 'C:/Users/alexh/terralis'

if len(sys.argv) > 1:
    backuppath = sys.argv[1]


def communicator(msg):
    print(msg)
    sys.stdout.flush();


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    print("Initiating Map Load")
    sys.stdout.flush()
    stateDict = mapReader.getStates(backuppath + "/history/states")
    countryDict = countryColourGetter.getCountries(backuppath + "/common")
    print("Country Dict Complete")
    sys.stdout.flush()
    provDict = definitionReader.getProvinceDataReverse(backuppath)
    revprovDict = definitionReader.getProvinceData(backuppath)
    sys.stdout.flush()
    print("State Dict Complete")
    #stateDict = mapReader.getStates(pather + "/history/states", stateDict)
    #provDict = definitionReader.getProvinceDataReverse(pather)
    #countryDict = countryColourGetter.getCountries(pather + "/common", countryDict)

    if not os.path.isdir(backuppath + "/mapEditor"):
        os.mkdir(backuppath + "/mapEditor")
    #mapDrawer.drawPoliticalProvinces(backuppath + "/map/provinces.bmp", backuppath + "/polmap.bmp", 5631, countryDict, stateDict, provDict)
    saki = mapReader.getStateData(stateDict)
    ichie = mapReader.getStateDataFullList(stateDict)
    hona = countryColourGetter.countrydicttostring(countryDict)
    mapReader.saveAsLocalFile(backuppath + "/mapEditor/statedata.txt", saki)
    mapReader.saveAsLocalFileList(backuppath + "/mapEditor/statedatafull.txt", ichie)
    mapReader.saveAsLocalFile(backuppath + "/mapEditor/countrycolours.txt", hona)

    ichika = mapDrawer.getProvincePos(
        mapDrawer.saveAllProvinces2(backuppath + "/map/provinces.bmp", backuppath + "/mapEditor", 5631, countryDict,
                                   stateDict, provDict, revprovDict))

    mapDrawer.saveProvinceString(backuppath + "/mapEditor/", ichika)

    print("Done!")
    sys.stdout.flush()
# See PyCharm help at https://www.jetbrains.com/help/pycharm/
