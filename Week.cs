using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG6212_POE
{
    public class Week
    {

        //variables used for properties of each "Week" object
public IDictionary<String, int> ModuleSelfStudy { get; set; }
        public static List<Week> Weeks { get; set; }
        
// empty constructor used to initialize static "Weeks" list
        public Week() 
        {
            Weeks = new List<Week>(); 

        }

        //overloaded constructor used to create "Week" objects
        public Week(IDictionary<String,int> moduleSelfStudy) 
        {
            ModuleSelfStudy = moduleSelfStudy;
        }
    }
}
