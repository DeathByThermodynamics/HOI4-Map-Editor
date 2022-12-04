using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hoi4test3
{
    class DataManager
    {
        public static Dictionary<int, List<string>> stateData;
        public static Dictionary<string, List<string>> definitionData;
        public static Dictionary<int, Dictionary<string, Object>> fullStateData;
        public static Dictionary<string, List<int>> countryColourData;

        public DataManager()
        {
            Retriever retriever = new Retriever();
            definitionData = retriever.GetProvinceData();
            stateData = retriever.GetStatetoProvinceData();
            fullStateData = retriever.GetFullStateData();
            countryColourData = retriever.GetColourData();
        }
    }
}
