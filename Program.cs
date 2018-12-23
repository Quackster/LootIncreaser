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
            int increaseLootBy = 2;

            if (!Directory.Exists("output"))
            {
                Directory.CreateDirectory("output");
            }

            try
            {
                if (increaseLootBy <= 1)
                {
                    Console.WriteLine("1 means original vanilla loot, please pick a different number!");
                }
                else
                {
                    DoIncreaseLoot(increaseLootBy);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.Read();
        }

        private static void DoIncreaseLoot(int increaseLootBy)
        {
            XDocument doc;
            
            doc = XDocument.Load("types.xml");
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

            doc.Save("output/types.xml");

            doc = XDocument.Load("mapgroupproto.xml");
            types = doc.Descendants("container");

            foreach (var type in types)
            {
                var element = type.Attribute("lootmax");

                if (element != null) {
                    int nomimal = int.Parse(element.Value);
                    int newNominal = (nomimal * increaseLootBy);

                    type.SetAttributeValue(element.Name, newNominal);
                }
            }

            types = doc.Descendants("group");

            foreach (var type in types)
            {
                var element = type.Attribute("lootmax");

                if (element != null)
                {
                    int nomimal = int.Parse(element.Value);
                    int newNominal = (nomimal * increaseLootBy);

                    type.SetAttributeValue(element.Name, newNominal);
                }


                bool hasMilitary = false;

                var subElements = type.Elements();

                foreach (var subElement in subElements)
                {
                    if (subElement.Name == "usage" && subElement.Attribute("name").Value == "Military")
                    {
                        hasMilitary = true;
                    }
                }

                if (!hasMilitary)
                {
                    var militaryElement = new XElement("usage");
                    militaryElement.Add(new XAttribute("name", "Military"));

                    type.AddFirst(militaryElement);
                }
            }

            doc.Save("output/mapgroupproto.xml");
        }
    }
}
