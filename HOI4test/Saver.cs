using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HOI4test
{
    public class Saver
    {
        
        public static List<string> saveData(Dictionary<int, Dictionary<string, Object>> stateDict)
        {
            List<string> nums = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            List<string> returnlist = new List<string>();
            returnlist.Add(stateDict.Keys.Count.ToString() + ":");
            foreach (var entry in stateDict.Keys)
            {
                
                string returnstring = "";
                
                returnstring += entry.ToString() + ";"; 
                foreach (var entry1 in stateDict[entry].Keys)
                {
                    //MessageBox.Show(entry1.ToString());
                    returnstring += entry1.ToString() + ">";
                    var doorn = stateDict[entry][entry1];
                    
                    if (entry1.ToString().Equals("buildings") || entry1.ToString().Equals("resources"))
                    {

                        var nextDict = (Dictionary<string, object>)stateDict[entry][entry1];
                        foreach (var entry2 in nextDict.Keys)
                        {
                            if (nums.Contains(entry2[0].ToString()))
                            {

                                returnstring += "@" + entry2.ToString() + "#";
                                var tempDict = (Dictionary<string, int>)nextDict[entry2];

                                foreach (var entry3 in tempDict.Keys)
                                {
                                    returnstring += entry3.ToString() + "<" + tempDict[entry3].ToString() + "+";
                                }
                                returnstring += "!";
                            }
                            else
                            {
                                returnstring += entry2.ToString() + "#" + nextDict[entry2].ToString() + "!";
                            }
                        }
                    } else if (entry1.ToString().Equals("vp")) {
                        //MessageBox.Show(stateDict[entry][entry1].ToString());
                        var nextDict = (Dictionary<string, string>)stateDict[entry][entry1];
                        foreach (var entry2 in nextDict.Keys)
                        {
                            //MessageBox.Show(entry2.ToString());
                            returnstring += entry2.ToString() + "#" + nextDict[entry2].ToString() + "!";
                        }
                            
                    } else if (doorn is string || doorn is int)
                    {
                        returnstring += stateDict[entry][entry1].ToString();
                    } else if (doorn is string[])
                    {
                        foreach (var entry2 in (string[]) stateDict[entry][entry1])
                        {
                            //if (entry2[0] != ' ' && entry2.ToString().Length != 0 && !entry2.EndsWith("}")) {
                            if (entry2.Length != 0)
                            {
                                returnstring += entry2.ToString() + "!";
                            } 
                            
                            //}
                        }
                    }

                    returnstring += "$";
                }
                returnstring += "?";

                returnlist.Add(returnstring);
            }
            //MessageBox.Show("bruh");

            return returnlist;
            
        }
    }
}
