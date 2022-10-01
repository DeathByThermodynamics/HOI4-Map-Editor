import mapDrawer

class State:
    '''

    '''

    def __init__(self, stateid, stateinfo):
        self.id = stateid
        self.stateinfo = stateinfo

    def getState(self):
        return self.stateinfo

    def changestateinfo(self, info, newentry)
        ''' Return the state dictionary with the changes in info consisting of newentry. 
        For example, to transfer the ownership of 1-Corsica to GER, 
        changestateinfo(1, "owner", "GER", statedict) can be used.

        :param info: the category of information.
        :param newentry: the new information
        :return: the changed statedict.
        '''
        try:
            self.stateinfo[info] = newentry
            return True
        except:
            return False

    def changestateownership(self, newowner, directory, countrydict):

        if not self.changestateinfo("owner", newowner):
            return False
        else:
            for k in self.stateinfo["provinces"]:
                mapDrawer.redrawprovince(directory, k, countrydict[newowner][1])
            return True


    def changebuilding(self, building, amount):
        if building not in self.stateinfo["buildings"].keys() and amount > 0:
            self.stateinfo["buildings"][building] = amount
            return True
        else:
            self.stateinfo["buildings"][building] += amount
            if self.stateinfo["buildings"][building] < 0:
                self.stateinfo["buildings"][building] = 0
                return "zero"
            return True

    def changeprovincebuilding(self, building, province, amount):
        if (str(province) not in self.stateinfo["buildings"].keys() or str(building) not in self.stateinfo["buildings"][province]) and amount > 0:
            self.stateinfo["buildings"][province][building] = amount
            return True
        else:
            self.stateinfo["buildings"][province][building] += amount
            if self.stateinfo["buildings"][province][building] < 0:
                self.stateinfo["buildings"][province][building] = 0
                return "zero"
            return True



