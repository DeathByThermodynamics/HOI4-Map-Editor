import state
import mapDrawer


def transferprovince(directory, statedict, provinceid, newstate, countrydict):
    for state in statedict:
        if provinceid in (int(num) for num in state["provinces"]):
            state["provinces"].remove(str(provinceid))
            break
    statedict[newstate]["provinces"] += [str(provinceid)]
    newowner = statedict[newstate]["owner"]
    mapDrawer.redrawprovince(directory, str(provinceid), countrydict[newowner][1])
