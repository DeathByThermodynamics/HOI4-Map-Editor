using HOI4test;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace hoi4test3.InfoWindows
{

    internal class PoliticalView
    {
        MainWindow main;

        public PoliticalView(MainWindow main)
        {
            this.main = main;
        }

        public Tuple<Dictionary<string, Object>, string> getWindowData(Dictionary<string, Object> stateinfo, int province)
        {
            // Save the state category
            stateinfo["category"] = main.category.SelectedItem.GetType().GetProperty("Name").GetValue(main.category.SelectedItem, null).ToString();
            string old_owner;
            if (!stateinfo.ContainsKey("owner"))
            {
                old_owner = "NONE";
            }
            else
            {
                old_owner = (string)stateinfo["owner"];
            }
            // Save the state owner
            stateinfo["owner"] = main.ProvOwner.Text;
            // Save state manpower
            stateinfo["manpower"] = main.Manpower.Text;
            // Save state cores
            var corelist = main.Cores.Text.Trim(' ').Split(',');
            for (var i = 0; i < corelist.Length; i++)
            {
                corelist[i].Trim(' ');
            }
            if (!stateinfo.ContainsKey("cores"))
            {
                stateinfo.Add("cores", corelist);
            }
            stateinfo["cores"] = corelist;
            // Save state claims
            var claimslist = main.Claims.Text.Trim(' ').Split(',');
            for (var i = 0; i < claimslist.Length; i++)
            {
                claimslist[i].Trim(' ');
            }
            if (!stateinfo.ContainsKey("claims"))
            {
                stateinfo.Add("claims", claimslist);
            }
            stateinfo["claims"] = claimslist;
            // Save province VPs
            if (stateinfo.ContainsKey("vp"))
            {
                var vpdict = (Dictionary<string, string>)stateinfo["vp"];

                if (vpdict.ContainsKey(province.ToString()))
                {
                    vpdict[province.ToString()] = main.VP.Text;
                }
                else
                {
                    vpdict.Add(province.ToString(), main.VP.Text);
                }

            }
            else
            {
                var vpdict = new Dictionary<string, string>();
                vpdict.Add(province.ToString(), main.VP.Text);
                stateinfo.Add("vp", vpdict);
            }
            // Save DMZ status
            if ((bool)main.DMZ.IsChecked)
            {
                if (stateinfo.ContainsKey("dmz"))
                {
                    stateinfo["dmz"] = "yes";
                } else
                {
                    stateinfo.Add("dmz", "yes");
                }
            } else
            {
                if (stateinfo.ContainsKey("dmz"))
                {
                    stateinfo["dmz"] = "no";
                }
                else
                {
                    stateinfo.Add("dmz", "no");
                }
            }
            // Save impassable status
            if ((bool)main.Impassable.IsChecked)
            {
                if (stateinfo.ContainsKey("impassable"))
                {
                    stateinfo["impassable"] = "yes";
                }
                else
                {
                    stateinfo.Add("impassable", "yes");
                }
            }
            else
            {
                if (stateinfo.ContainsKey("impassable"))
                {
                    stateinfo["impassable"] = "no";
                }
                else
                {
                    stateinfo.Add("impassable", "no");
                }
            }
            return Tuple.Create(stateinfo, old_owner);
        }

        public void updateWindow(Dictionary<string, Object> stateinfo, int provinceid)
        {
            if (stateinfo.ContainsKey("owner"))
            {
                main.updateUIContent(main.ProvOwner, stateinfo["owner"].ToString());
            }
            else
            {
                main.updateUIContent(main.ProvOwner, "");
            }

            if (stateinfo.ContainsKey("manpower"))
            {
                main.updateUIContent(main.Manpower, stateinfo["manpower"].ToString());
            }
            else
            {
                main.updateUIContent(main.Manpower, "0");
            }

            main.updateUIContent(main.Cores, "");
            if (stateinfo.ContainsKey("cores"))
            {
                var corelist = (string[])stateinfo["cores"];
                string core_display = corelist[0];
                for (var i = 1; i < corelist.Length; i++)
                {
                    core_display += "," + corelist[i];
                }
                main.updateUIContent(main.Cores, core_display);
            }

            main.updateUIContent(main.Claims, "");
            if (stateinfo.ContainsKey("claims"))
            {
                var claimslist = (string[])stateinfo["claims"];
                string claims_display = claimslist[0];
                for (var i = 1; i < claimslist.Length; i++)
                {
                    claims_display += "," + claimslist[i];
                }
                main.updateUIContent(main.Claims, claims_display);
            }

            if (stateinfo.ContainsKey("category"))
            {
                switch (stateinfo["category"].ToString())
                {
                    case "wasteland":
                        main.category.SelectedItem = main.wasteland;
                        break;
                    case "enclave":
                        main.category.SelectedItem = main.enclave;
                        break;
                    case "small_island":
                        main.category.SelectedItem = main.small_island;
                        break;
                    case "tiny_island":
                        main.category.SelectedItem = main.tiny_island;
                        break;
                    case "pastoral":
                        main.category.SelectedItem = main.pastoral;
                        break;
                    case "town":
                        main.category.SelectedItem = main.town;
                        break;
                    case "large_town":
                        main.category.SelectedItem = main.large_town;
                        break;
                    case "rural":
                        main.category.SelectedItem = main.rural;
                        break;
                    case "city":
                        main.category.SelectedItem = main.city;
                        break;
                    case "large_city":
                        main.category.SelectedItem = main.large_city;
                        break;
                    case "metropolis":
                        main.category.SelectedItem = main.metropolis;
                        break;
                    case "megalopolis":
                        main.category.SelectedItem = main.megalopolis;
                        break;
                    default:
                        main.category.SelectedItem = main.wasteland;
                        break;

                }
            }
            else
            {
                main.category.SelectedItem = main.wasteland;
            }

            if (stateinfo.ContainsKey("vp"))
            {
                try
                {
                    var next = (Dictionary<string, string>)stateinfo["vp"];
                    main.updateUIContent(main.VP, next[provinceid.ToString()]);
                }
                catch (Exception e)
                {
                    main.updateUIContent(main.VP, "0");
                }
            }
            else
            {
                main.updateUIContent(main.VP, "0");
            }

            if (stateinfo.ContainsKey("dmz") && stateinfo["dmz"].ToString() == "yes")
            {
                main.DMZ.IsChecked = true;
            } else
            {
                main.DMZ.IsChecked = false;
            }

            if (stateinfo.ContainsKey("impassable") && stateinfo["impassable"].ToString() == "yes")
            {
                main.Impassable.IsChecked = true;
            }
            else
            {
                main.Impassable.IsChecked = false;
            }
        }
    }
}
