using HOI4test;
using hoi4test3.InfoWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace hoi4test3
{
    internal class NewProvinceView
    {
        public Dictionary<string, Object> state;
        int province;
        MainWindow main;
        PoliticalView politicalView;
        ResourcesView resourcesView;
        BuildingsView buildingsView;
        List<string> ints;

        public NewProvinceView(MainWindow main)
        {
            this.main = main;
            state = new Dictionary<string, Object>();
            province = 0;
            politicalView = new PoliticalView(main);
            resourcesView = new ResourcesView(main);
            buildingsView = new BuildingsView(main);
            ints = new List<string>();
            ints.Add("1"); // Github copilot generated this and I'm too lazy to change
            ints.Add("2");
            ints.Add("3");
            ints.Add("4");
            ints.Add("5");
            ints.Add("6");
            ints.Add("7");
            ints.Add("8");
            ints.Add("9");
        }

        public void updateWindow(Dictionary<string, Object> stateinfo, int provinceid)
        {
            state = stateinfo;
            province = provinceid;
            main.updateUIContent((Label) main.ProvinceID, provinceid.ToString());
            main.updateUIContent(main.StateID, stateinfo["id"].ToString());
            politicalView.updateWindow(stateinfo, provinceid);
            resourcesView.updateWindow(stateinfo);
            var spbuildingtuple = buildingsView.defineBuildings(provinceid);
            buildingsView.drawBuildings(spbuildingtuple.Item1, spbuildingtuple.Item2);
            buildingsView.updateWindow(stateinfo, provinceid);
        }

        public void saveWindows()
        {
            Dictionary<string, object> resourceWindowData = resourcesView.getWindowData();
            var spbuildingtuple = buildingsView.defineBuildings(province);
            Dictionary<string, object> buildingWindowData = buildingsView.getWindowData(spbuildingtuple.Item1, spbuildingtuple.Item2, province);
            var newInfoandOwner = politicalView.getWindowData(state, province);
            Dictionary<string, object> stateInfo = newInfoandOwner.Item1;
            string old_owner = newInfoandOwner.Item2;

            if (!stateInfo.ContainsKey("buildings"))
            {
                //stateInfo.Remove("buildings");
                stateInfo.Add("buildings", buildingWindowData);
            } else
            {
                var tempstate = (Dictionary<string, object>)stateInfo["buildings"];
                foreach (var building in tempstate.Keys)
                {
                    if (ints.Contains(building.ToCharArray()[0].ToString()))
                    {
                        if (!buildingWindowData.ContainsKey(building))
                        {
                            buildingWindowData.Add(building, tempstate[building]);
                        }
                        
                    }
                }
                stateInfo["buildings"] = buildingWindowData;
            }
            if (stateInfo.ContainsKey("resources"))
            {
                stateInfo.Remove("resources");
            }
            
            stateInfo.Add("resources", resourceWindowData);

            sentToMain(old_owner, stateInfo);
        }

        public void sentToMain(string old_owner, Dictionary<string, object> stateInfo)
        {
            int newstateprov = 0;
            if (main.states.states.ContainsKey(int.Parse(stateInfo["id"].ToString())))
            {
                main.states.changeState(stateInfo, old_owner, main);
            }
            else
            {
                main.states.addState(stateInfo);
                newstateprov = province;
            }


            var bruh = (string[])state["provinces"];
            main.UpdateProvinces(bruh.ToList(), newstateprov);

            main.states.saveStates();
        }

        public void CreateNewState()
        {
            var stateinfo = new Dictionary<string, object>();
            stateinfo.Add("id", main.states.getStates().Count + 1);
            stateinfo.Add("provinces", new string[] { province.ToString() });
            stateinfo.Add("owner", main.ProvOwner.Text);
            stateinfo.Add("manpower", "0");
            state = stateinfo;
            updateWindow(stateinfo, province);
            sentToMain("NONE", stateinfo);
        }


    }
}
