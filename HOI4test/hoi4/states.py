import mapDrawer

def changestateprovince(state_id, province_id, statedict):
    for i in statedict:
        if province_id in statedict[i]["provinces"]:
            statedict[i]["provinces"].remove(province_id)

    statedict[state_id]["provinces"] += [province_id]
    return statedict

# Basic function
def changestateinfo(state_id, info, newentry, statedict)
    ''' Return the state dictionary with the changes in info consisting of newentry. 
    For example, to transfer the ownership of 1-Corsica to GER, 
    changestateinfo(1, "owner", "GER", statedict) can be used.
    
    :param state_id: the id of the state.
    :param info: the category of information.
    :param newentry: the new information
    :param statedict: the state dictionary; a temporary solution.
    :return: the changed statedict.
    '''
    try:
        statedict[state_id][info] = newentry
        return statedict
    except:
        return False


def changestateownership(state_id, newowner, directory, statedict, countrydict):
    returndict = changestateinfo(state_id, "owner", newowner, statedict)

    if not returndict:
        return False
    else:
        for k in statedict[state_id]["provinces"]:
            mapDrawer.redrawprovince(directory, k, countrydict[newowner][1])
        return statedict

def changebuilding(state_id, )
