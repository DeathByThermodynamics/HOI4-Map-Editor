using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace HOI4test
{
    public class StratRegion
    {
        public Dictionary<string, List<string>> stratProvinces = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> stratData = new Dictionary<string, List<string>>();
        List<string> nums = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public StratRegion()
        {
            //whatever
        }

        public void transferProvince(string province, string targetprovince)
        {
            // This does not stop the FRACTURED STRATEGIC ZONE warning we all love.
            foreach (var entry in stratProvinces.Keys)
            {
                if (stratProvinces[entry].Contains(province))
                {
                    stratProvinces[entry].Remove(province);

                }
                if (stratProvinces[entry].Contains(targetprovince))
                {
                    stratProvinces[entry].Add(province);
                }
            }
        }
        
        public void getStratRegions(string directory)
        {
            string folder;
            // directory should be input folder
            if (Directory.Exists(starter.outputfolder + "/strategicregions"))
            {
                folder = starter.outputfolder + "/strategicregions";
            } else
            {
                folder = directory + "/map/strategicregions";
            }
            DirectoryInfo d = new DirectoryInfo(folder);

            foreach (var file in d.GetFiles("*.txt"))
            {
                List<string> text = File.ReadLines(file.FullName).ToList();
                var provinces = new List<string>();
                var data = new List<string>();
                var regionid = "";
                foreach (string line in text)
                {
                    if (line == "") {
                        continue;
                    }
                    // This only works if
                    //  1. All provinces are on lines w/o other junk
                    //  2. There's no junk after the period definitions
                    //  3. The provinces are situated before the period definitions
                    // I have no idea why someone would manually change the STRATREGION files, but nudge would be compatible with this code.
                    // Look into improving this in the future.
                    
                    var newline = line.Trim();
                    if (nums.Contains(newline[0].ToString()))
                    {
                        provinces.AddRange(newline.Split(' '));
                    } else
                    {
                        data.Add(line);
                        if (newline.StartsWith("id"))
                        {
                            regionid = newline.Split('=')[1].Trim();

                        }
                    }

                }

                stratProvinces.Add(regionid, provinces);
                stratData.Add(regionid, data);
            }
        }

    }
}
