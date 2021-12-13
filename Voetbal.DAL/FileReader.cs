using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Voetbal.Models;

namespace Voetbal.DAL
{
    public class FileReader
    {
        // if file exists, return all lines in file as array
        // if not, return null
        private string[] ReadArr(string fName) => File.Exists(fName) ?
            File.ReadAllLines(fName) : null;

        // if file exists, return list object of players
        // if not, return null
        public List<Speler> ReadSpelersToList(string fName)
        {
            var spelers = new List<Speler>();
            try
            {
                var lines = ReadArr(fName);



                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var split = line.Split(';');

                        string vnaam = split[0];
                        string fnaam = split[1];
                        string ploeg = split[2];

                        var baller = new Speler(vnaam, fnaam, ploeg);
                        spelers.Add(baller);
                    }
                }
            }
            catch (Exception genEx)
            {
                string msg = genEx.Message;
                return null;
            }

            return spelers.Count > 0 ?
                spelers : null;
        }
    }
}
