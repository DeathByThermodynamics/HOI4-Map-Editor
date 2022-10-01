import numpy as np
from PIL import Image

import mapReader
import province
import os

def getProvToCountry(stateDict):
    returnDict = {}
    for i in stateDict:
        for j in stateDict[i]["provinces"]:
            try:
                returnDict[int(j)] = stateDict[i]["owner"]
            except:
                continue
    #print(returnDict)
    return returnDict

def getProvToCountryColour(countrydict, statedict):
    returnDict = {}
    for i in statedict:
        for j in statedict[i]["provinces"]:
            try:
                returnDict[int(j)] = countrydict[statedict[i]["owner"]][1]
            except:
                continue
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
            #print(tuple(pixel_val))
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
            #if tuple(img[new_i - 1, new_j]) != faraday or \
            #        tuple(img[new_i + 1, new_j]) != faraday \
            #        or tuple(img[new_i, new_j - 1]) != faraday \
            #        or tuple(img[new_i, new_j + 1]) != faraday:

            new_img[i, j] = new_pixel

        percent = int(((i * width) / total) * 100)
        if percent > percent1:
            percent1 = percent
            print(percent)

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
    #provcountrydict = getProvToCountry(stateDict)
    print(height)
    print(width)
    print(img.shape)
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
            print(percent)
    accum = 0
    total = len(pixelmap.keys())
    provtocountrycolour = getProvToCountryColour(countryDict, stateDict)
    for i in pixelmap:
        accum += 1
        width, height = province.calcsize(pixelmap[i])
        #print(pixelmap[i])
        #os.mkdir(outdir + "/provinces")

        if i in provtocountrycolour:
            province.getindividualbmp(i, provtocountrycolour[i], pixelmap[i], outdir + "/provinces", width + 1, height + 1)

        else:
            province.getindividualbmp(i, revprovDict[i], pixelmap[i], outdir+"/provinces", width+1, height+1)
        print("Finished " + str(accum) + "/" + str(total))
    pil_img = Image.fromarray(new_img)
    pil_img.save(outdir+"/provinces/borders.png")
    return pixelmap


def getProvincePos(pixelmap):
    accum = str(len(pixelmap)) + ";"
    for i in pixelmap:
        xcord = min(p[0] for p in pixelmap[i])
        ycord = min(p[1] for p in pixelmap[i])
        accum += str(i) + ":" + str(xcord) + "," + str(ycord) + "?"

    return accum


def saveProvinceString(string):
    if os.path.isfile('provincepos.txt'):
        os.remove('provincepos.txt')
    with open("provincepos.txt", 'w') as f:
        f.write(string)


def redrawprovince(directory, id, colour):
    raw_img = Image.open(directory + "/provinces/" + str(id) + ".png")
    img = np.array(raw_img)
    width, height = raw_img.size
    for i in range(width):
        for j in range(height):
            if tuple(img[j, i])[3] != 0:
                img[j, i] = (colour[0], colour[1], colour[2], 255)
    pil_img = Image.fromarray(img)
    os.remove(directory + "/provinces/" + str(id) + ".png")
    pil_img.save(directory + "/provinces/" + str(id) + ".png")




