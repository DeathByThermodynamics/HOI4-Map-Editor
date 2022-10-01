# This is the main loop for the HOI4 map extractor.

import mapDrawer
import mapReader
import countryColourGetter
import definitionReader
import fileExtractor

pather = 'C:/Users/alexh/templetondevelopment'
backuppath = 'C:/Users/alexh/terralis'

# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    print(pather)
    stateDict = mapReader.getStates(backuppath + "/history/states")
    countryDict = countryColourGetter.getCountries(backuppath + "/common")
    print(countryDict)
    provDict = definitionReader.getProvinceDataReverse(backuppath)
    revprovDict = definitionReader.getProvinceData(backuppath)
    print(stateDict)
    #stateDict = mapReader.getStates(pather + "/history/states", stateDict)
    #provDict = definitionReader.getProvinceDataReverse(pather)
    #countryDict = countryColourGetter.getCountries(pather + "/common", countryDict)

    #mapDrawer.drawPoliticalProvinces(backuppath + "/map/provinces.bmp", backuppath + "/polmap.bmp", 5631, countryDict, stateDict, provDict)
    saki = mapReader.getStateData(stateDict)
    ichie = mapReader.getStateDataFullList(stateDict)
    hona = countryColourGetter.countrydicttostring(countryDict)
    mapReader.saveAsLocalFile("statedata.txt", saki)
    mapReader.saveAsLocalFileList("statedatafull.txt", ichie)
    mapReader.saveAsLocalFile("countrycolours.txt", hona)

    ichika = mapDrawer.getProvincePos(
        mapDrawer.saveAllProvinces(backuppath + "/map/provinces.bmp", backuppath, 5631, countryDict,
                                   stateDict, provDict, revprovDict))

    mapDrawer.saveProvinceString(ichika)

    print("Done.")
# See PyCharm help at https://www.jetbrains.com/help/pycharm/
