import numpy as np
from PIL import Image
def __init__(self, colour, id):
    self.colour = colour
    self.id = id
    self.pixels = []
    self.width = 0
    self.height = 0
    self.directory = "None"

def addpixel(self, pos):
    self.pixels.append(pos)

def calcsize(pixels):
    width = max(pixel[1] for pixel in pixels) - min(pixel[1] for pixel in pixels)
    height = max(pixel[0] for pixel in pixels) - min(pixel[0] for pixel in pixels)
    return width, height

def getindividualbmp(id, colour, pixels, directory, width, height):
    img = Image.new('RGBA', (width, height), (255, 255, 255, 0))
    new_img = np.array(img)
    min_y = min(pixel[1] for pixel in pixels)
    min_x = min(pixel[0] for pixel in pixels)
    new_array1 = [pos[0] - min_x for pos in pixels]
    new_array2 = [pos[1] - min_y for pos in pixels]
    for i in range(len(new_array1)):
        new_img[new_array1[i], new_array2[i]] = (0, 0, 0, 255)
    pil_img = Image.fromarray(new_img)

    pil_img.save(directory + "/" + str(id) + ".png")

def getindividualbmpnormal(id, colour, pixels, directory, width, height):
    img = Image.new('RGBA', (width, height), (255, 255, 255, 0))
    new_img = np.array(img)
    min_y = min(pixel[1] for pixel in pixels)
    min_x = min(pixel[0] for pixel in pixels)
    new_array1 = [pos[0] - min_x for pos in pixels]
    new_array2 = [pos[1] - min_y for pos in pixels]
    for i in range(len(new_array1)):
        new_img[new_array1[i], new_array2[i]] = (colour[0], colour[1], colour[2], 255)
    pil_img = Image.fromarray(new_img)

    pil_img.save(directory + "/" + str(id) + ".png")



def calculatepos(pixels, width, height):
    return (min(pixel[0] for pixel in pixels) + width/2,
            min(pixel[1] for pixel in pixels) + height/2)
