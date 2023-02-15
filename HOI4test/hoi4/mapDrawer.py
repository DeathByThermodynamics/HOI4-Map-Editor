import numpy as np
from PIL import Image

import mapReader
import province
import os
import sys
import main


def getProvToCountry(stateDict):
    returnDict = {}
    for i in stateDict:
        for j in stateDict[i]["provinces"]:
            try:
                returnDict[int(j)] = stateDict[i]["owner"]
            except:
                continue
    # print(returnDict)
    return returnDict


def getProvToCountryColour(countrydict, statedict, revprovDict):
    returnDict = {}
    #print(revprovDict)
    for i in statedict:
        for j in statedict[i]["provinces"]:
            try:
                returnDict[int(j)] = countrydict[statedict[i]["owner"]][1]
            except:
                if j != '':
                    returnDict[int(j)] = (revprovDict[int(j)][0], revprovDict[int(j)][1], revprovDict[int(j)][2], 125)

    for i in revprovDict:
        if i not in returnDict:
            returnDict[int(i)] = (6, 66, 115)
    # print(returnDict)
    return returnDict


def drawPolitical(indir, outdir, countryDict, stateDict, provDict):
    raw_img = Image.open(indir)

    width, height = raw_img.size;
    img = np.array(raw_img)
    provcountrydict = getProvToCountry(stateDict)
    print(height)
    print(width)
    print(img.shape)
    total = height * width
    for i in range(height):
        for j in range(width):
            pixel_val = img[i, j]
            # print(tuple(pixel_val))
            provid = int(provDict[tuple(pixel_val)])
            try:
                new_pixel = tuple(countryDict[provcountrydict[provid]][1])
            except:
                new_pixel = (1, 1, 25)
            img[i, j] = new_pixel
        percent = ((i * width) / total) * 100
        print(percent)

    pil_img = Image.fromarray(img)
    pil_img.save(outdir)


def drawPoliticalResized(indir, outdir, width, countryDict, stateDict, provDict):
    raw_img = Image.open(indir)
    img = np.array(raw_img)
    basewidth = width
    wpercent = (basewidth / float(raw_img.size[0]))
    hsize = int((float(raw_img.size[1]) * float(wpercent)))
    raw_img = raw_img.resize((basewidth, hsize), Image.ANTIALIAS)
    width, height = raw_img.size;
    new_img = np.array(raw_img)
    provcountrydict = getProvToCountry(stateDict)
    print(height)
    print(width)
    print(img.shape)
    total = height * width
    for i in range(height):
        for j in range(width):
            new_i = int(i / wpercent)
            new_j = int(j / wpercent)
            pixel_val = img[new_i, new_j]
            # print(tuple(pixel_val))
            provid = int(provDict[tuple(pixel_val)])
            try:
                new_pixel = tuple(countryDict[provcountrydict[provid]][1])
            except:
                new_pixel = (1, 1, 45)
            new_img[i, j] = new_pixel
        percent = ((i * width) / total) * 100
        print(percent)

    pil_img = Image.fromarray(new_img)
    pil_img.save(outdir)


def drawPoliticalProvinces(indir, outdir, width, countryDict, stateDict, provDict):
    raw_img = Image.open(indir)
    img = np.array(raw_img)
    basewidth = width
    wpercent = (basewidth / float(raw_img.size[0]))
    hsize = int((float(raw_img.size[1]) * float(wpercent)))
    raw_img = raw_img.resize((basewidth, hsize), Image.ANTIALIAS)
    width, height = raw_img.size;
    new_img = np.array(raw_img)
    provcountrydict = getProvToCountry(stateDict)
    print(height)
    print(width)
    print(img.shape)
    total = height * width
    percent1 = 0
    for i in range(height):
        for j in range(width):
            new_i = int(i / wpercent)
            new_j = int(j / wpercent)
            # print(tuple(pixel_val))
            if tuple(img[new_i - 1, new_j]) != tuple(img[new_i + 1, new_j]) or \
                    tuple(img[new_i, new_j - 1]) != tuple(img[new_i, new_j + 1]):
                # print(tuple(img[i - 1, j]))
                # print(tuple(img[i - 1, j - 1]))
                # print(tuple(img[i, j - 1]))
                # print("my tuple:" + str(tuple(img[i, j])))
                new_pixel = (1, 1, 1)
            else:
                pixel_val = img[new_i, new_j]
                provid = int(provDict[tuple(pixel_val)])
                if provid in provcountrydict.keys():
                    new_pixel = tuple(countryDict[provcountrydict[provid]][1])
                else:
                    new_pixel = (1, 1, 45)
            faraday = tuple(img[new_i, new_j])
            # if tuple(img[new_i - 1, new_j]) != faraday or \
            #        tuple(img[new_i + 1, new_j]) != faraday \
            #        or tuple(img[new_i, new_j - 1]) != faraday \
            #        or tuple(img[new_i, new_j + 1]) != faraday:

            new_img[i, j] = new_pixel

        percent = int(((i * width) / total) * 100)
        if percent > percent1:
            percent1 = percent
            main.communicator(str(percent) + "% done map drawing")
            sys.stdout.flush()

    pil_img = Image.fromarray(new_img)
    pil_img.save(outdir)


def saveAllProvinces(indir, outdir, width, countryDict, stateDict, provDict, revprovDict):
    raw_img = Image.open(indir)
    img = np.array(raw_img)
    basewidth = width
    wpercent = (basewidth / float(raw_img.size[0]))
    hsize = int((float(raw_img.size[1]) * float(wpercent)))
    raw_img = raw_img.resize((basewidth, hsize), Image.ANTIALIAS)
    width, height = raw_img.size
    new_img = np.array(Image.new('RGBA', (width, height), (255, 255, 255, 0)))
    # provcountrydict = getProvToCountry(stateDict)
    # print(height)
    # print(width)
    # print(img.shape)
    total = height * width
    percent1 = 0

    pixelmap = {}
    for i in range(height):
        for j in range(width):
            new_i = int(i / wpercent)
            new_j = int(j / wpercent)
            # print(tuple(pixel_val))
            pixel_val = img[new_i, new_j]
            provid = int(provDict[tuple(pixel_val)])
            if tuple(img[new_i - 1, new_j]) != tuple(img[new_i + 1, new_j]) or \
                    tuple(img[new_i, new_j - 1]) != tuple(img[new_i, new_j + 1]):
                # print(tuple(img[i - 1, j]))
                # print(tuple(img[i - 1, j - 1]))
                # print(tuple(img[i, j - 1]))
                # print("my tuple:" + str(tuple(img[i, j])))
                new_pixel = (1, 1, 1)
                new_img[i, j] = (new_pixel[0], new_pixel[1], new_pixel[2], 255)
            if provid not in pixelmap:
                pixelmap[provid] = [(i, j)]
            else:
                pixelmap[provid].append((i, j))
        percent = int(((i * width) / total) * 100)
        if percent > percent1:
            percent1 = percent
            main.communicator(str(percent) + "% done map drawing")
    accum = 0
    main.communicator("Done.")
    total = len(pixelmap.keys())
    provtocountrycolour = getProvToCountryColour(countryDict, stateDict)
    if not os.path.isdir(outdir + "/provinces"):
        os.mkdir(outdir + "/provinces")
    for i in pixelmap:
        accum += 1
        width, height = province.calcsize(pixelmap[i])
        # print(pixelmap[i])

        if i in provtocountrycolour:
            province.getindividualbmp(i, provtocountrycolour[i], pixelmap[i], outdir + "/provinces", width + 1,
                                      height + 1)

        else:
            province.getindividualbmp(i, revprovDict[i], pixelmap[i], outdir + "/provinces", width + 1, height + 1)
        main.communicator("Finished " + str(accum) + "/" + str(total))
    pil_img = Image.fromarray(new_img)
    pil_img.save(outdir + "/provinces/borders.bmp")
    return pixelmap


def getProvincePos(pixelmap):
    accum = str(len(pixelmap)) + ";"
    for i in pixelmap:
        xcord = min(p[0] for p in pixelmap[i])
        ycord = min(p[1] for p in pixelmap[i])
        accum += str(i) + ":" + str(xcord) + "," + str(ycord) + "?"

    return accum


def saveProvinceString(dir, string):
    if os.path.isfile(dir + 'provincepos.txt'):
        os.remove(dir + 'provincepos.txt')
    with open(dir + "provincepos.txt", 'w') as f:
        f.write(string)


def redrawProvinceInMap(directory, ids, colourlist, positionlist):
    map_img = Image.open(directory + "/mapEditor/provinces/borders.png")
    map_bitmap = np.array(map_img)
    for index in range(len(ids)):

        id = ids[index]
        colour = colourlist[index]
        if id == "":
            continue
        x = int(positionlist[id][0])
        y = int(positionlist[id][1])

        raw_img = Image.open(directory + "/provinces/" + str(id) + ".png")
        img = np.array(raw_img)
        width, height = raw_img.size
        for i in range(width):
            for j in range(height):
                if tuple(img[j, i])[3]:
                    bitval = map_bitmap[x + j, y + i]
                    if int(bitval[0]) + int(bitval[1]) + int(bitval[2]) != 0:
                        map_bitmap[x + j, y + i] = (colour[0], colour[1], colour[2], 255)
    pil_img = Image.fromarray(map_bitmap)
    os.remove(directory + "/mapEditor/provinces/borders.png")
    pil_img.save(directory + "/mapEditor/provinces/borders.png")


def redrawprovince(directory, id, colour):
    raw_img = Image.open(directory + "/provinces/" + str(id) + ".png")
    img = np.array(raw_img)
    width, height = raw_img.size
    for i in range(width):
        for j in range(height):
            if tuple(img[j, i])[3] != 0:
                img[j, i] = (colour[0], colour[1], colour[2], 255)
    pil_img = Image.fromarray(img)
    os.remove(directory + "/mapEditor/provinces/" + str(id) + ".png")
    pil_img.save(directory + "/mapEditor/provinces/" + str(id) + ".png")


def redrawprovinceincache(directory, id, colour):
    raw_img = Image.open(directory + "/mapEditor/provinces/" + str(id) + ".png")
    img = np.array(raw_img)
    width, height = raw_img.size
    for i in range(width):
        for j in range(height):
            if tuple(img[j, i])[3] != 0:
                img[j, i] = (colour[0], colour[1], colour[2], 255)
    pil_img = Image.fromarray(img)
    if not os.path.isdir(directory + "/mapEditor/temp"):
        os.mkdir(directory + "/mapEditor/temp")
    if os.path.isfile(directory + "/mapEditor/temp/" + str(id) + ".png"):
        os.remove(directory + "/mapEditor/temp/" + str(id) + ".png")
    pil_img.save(directory + "/mapEditor/temp/" + str(id) + ".png")


def saveAllProvinces2(indir, outdir, width, countryDict, stateDict, provDict, revprovDict):
    raw_img = Image.open(indir)
    img = np.array(raw_img)
    basewidth = width
    wpercent = (basewidth / float(raw_img.size[0]))
    hsize = int((float(raw_img.size[1]) * float(wpercent)))
    raw_img = raw_img.resize((basewidth, hsize), Image.ANTIALIAS)
    width, height = raw_img.size
    new_img = np.array(Image.new('RGBA', (width, height), (255, 255, 255, 0)))
    overlayborders = new_img.copy()
    # provcountrydict = getProvToCountry(stateDict)
    # print(height)
    # print(width)
    # print(img.shape)
    total = height * width
    percent1 = 0
    provtocountrycolour = getProvToCountryColour(countryDict, stateDict, revprovDict)
    pixelmap = {}
    for i in range(height):
        for j in range(width):
            new_i = int(i / wpercent)
            new_j = int(j / wpercent)
            # print(tuple(pixel_val))
            pixel_val = img[new_i, new_j]
            provid = int(provDict[tuple(pixel_val)])
            if tuple(img[new_i - 1, new_j]) != tuple(img[new_i + 1, new_j]) or \
                    tuple(img[new_i, new_j - 1]) != tuple(img[new_i, new_j + 1]):
                # print(tuple(img[i - 1, j]))
                # print(tuple(img[i - 1, j - 1]))
                # print(tuple(img[i, j - 1]))
                # print("my tuple:" + str(tuple(img[i, j])))
                #new_pixel = (1, 1, 1)
                new_img[i, j] = (1, 1, 1, 255)
                overlayborders[i, j] = (1, 1, 1, 255)
            else:
                #try:
                new_img[i, j] = (provtocountrycolour[provid][0], provtocountrycolour[provid][1],
                                     provtocountrycolour[provid][2], 255)
                #except:
                    #new_img[i, j] = (revprovDict[provid][0], revprovDict[provid][1], revprovDict[provid][2], 125)
            if provid not in pixelmap:
                pixelmap[provid] = [(i, j)]
            else:
                pixelmap[provid].append((i, j))

        percent = int(((i * width) / total) * 100)
        if percent > percent1:
            percent1 = percent
            main.communicator(str(percent) + "% done map drawing")
    accum = 0
    main.communicator("Done.")
    total = len(pixelmap.keys())

    if not os.path.isdir(outdir + "/provinces"):
        os.mkdir(outdir + "/provinces")
    for i in pixelmap:
        accum += 1
        width, height = province.calcsize(pixelmap[i])
        # print(pixelmap[i])

        if i in provtocountrycolour:
            province.getindividualbmp(i, provtocountrycolour[i], pixelmap[i], outdir + "/provinces", width + 1,
                                      height + 1)

        else:
            province.getindividualbmp(i, revprovDict[i], pixelmap[i], outdir + "/provinces", width + 1, height + 1)
        main.communicator("Finished " + str(accum) + "/" + str(total))
    pil_img = Image.fromarray(new_img)
    pil_img.save(outdir + "/provinces/borders.bmp")
    pil_img.save(outdir + "/provinces/borders.png")
    border_img = Image.fromarray(overlayborders)
    border_img.save(outdir + "/provinces/borders2.png")
    return pixelmap

def saveAllProvinces3(indir, outdir, width, countryDict, stateDict, provDict, revprovDict):
    raw_img = Image.open(indir)
    img = np.array(raw_img)
    basewidth = width
    wpercent = (basewidth / float(raw_img.size[0]))
    hsize = int((float(raw_img.size[1]) * float(wpercent)))
    raw_img = raw_img.resize((basewidth, hsize), Image.ANTIALIAS)
    width, height = raw_img.size
    new_img = np.array(Image.new('RGBA', (width, height), (255, 255, 255, 0)))
    overlayborders = new_img.copy()
    provtocountrycolour = getProvToCountryColour(countryDict, stateDict, revprovDict)
    pixelmap = {}
    for i in range(height):
        for j in range(width):
            new_i = int(i / wpercent)
            new_j = int(j / wpercent)
            pixel_val = img[new_i, new_j]
            provid = int(provDict[tuple(pixel_val)])
            if tuple(img[new_i - 1, new_j]) != tuple(img[new_i + 1, new_j]) or \
                    tuple(img[new_i, new_j - 1]) != tuple(img[new_i, new_j + 1]):
                new_img[i, j] = (1, 1, 1, 255)
                overlayborders[i, j] = (1, 1, 1, 255)
            else:
                new_img[i, j] = (provtocountrycolour[provid][0], provtocountrycolour[provid][1],
                                     provtocountrycolour[provid][2], 255)
            if provid not in pixelmap:
                pixelmap[provid] = [(i, j)]
            else:
                pixelmap[provid].append((i, j))

        percent = int(((i * width) / total) * 100)
        if percent > percent1:
            percent1 = percent
            main.communicator(str(percent) + "% done map drawing")
    accum = 0
    main.communicator("Done.")
    total = len(pixelmap.keys())

    if not os.path.isdir(outdir + "/provinces"):
        os.mkdir(outdir + "/provinces")
    for i in pixelmap:
        accum += 1
        width, height = province.calcsize(pixelmap[i])

        if i in provtocountrycolour:
            province.getindividualbmp(i, provtocountrycolour[i], pixelmap[i], outdir + "/provinces", width + 1,
                                      height + 1)

        else:
            province.getindividualbmp(i, revprovDict[i], pixelmap[i], outdir + "/provinces", width + 1, height + 1)
        main.communicator("Finished " + str(accum) + "/" + str(total))
    pil_img = Image.fromarray(new_img)
    pil_img.save(outdir + "/provinces/borders.bmp")
    return new_img
