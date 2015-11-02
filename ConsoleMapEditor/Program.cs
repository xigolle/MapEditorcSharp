using _2_ViewMapEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMapEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Maken nieuwe kaart
            MapModel mijnMap = new MapModel(5, 3);
            //Element met coordinaten 2,2 op 6 zetten
            mijnMap.SetElement(2, 2, 6);
            //Kaart wegschrijven
            mijnMap.SaveMap("testje.map");


            //Kaart opnieuw inladen
            MapModel mijnandereMap = new MapModel("testje.map");
            //Waarde van element 2,2 uitlezen
            int waarde = mijnandereMap.GetElement(2, 2);
            //Waarde op scherm zetten, hopelijk komt er 6
            Console.WriteLine(waarde.ToString());
        }
    }
}
