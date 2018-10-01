using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LootIncreaser
{
    class Program
    {
        static void Main(string[] args)
        {
            int increaseLootBy = 5;

            try
            {
                if (increaseLootBy <= 1)
                {
                    Console.WriteLine("1 means original vanilla loot, please pick a different number!");
                }
                else
                {
                    DoIncreaseLoot("types.xml", increaseLootBy);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.Read();
        }

        private static void DoIncreaseLoot(string fileName, int increaseLootBy)
        {
            XDocument doc = XDocument.Load(fileName);
            var types = doc.Descendants("type");

            foreach (var type in types)
            {
                var element = type.Element("nominal");

                if (element == null)
                    continue;

                int nomimal = int.Parse(element.Value);
                int newNominal = (nomimal * increaseLootBy);

                type.SetElementValue(element.Name, newNominal);
            }

            doc.Save("types_new.xml");
        }
    }
}
