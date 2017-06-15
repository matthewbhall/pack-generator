using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WebRole.Managers;
using WebRole.Models;

namespace WebRole
{
    public class Gatherer
    {
        public List<String> SetNames { get; }
        public Dictionary<String, SetManager> SetManagers { get; }

        private static Gatherer _instance = null;

        public static Gatherer Instance()
        {
            if (_instance == null)
            {
                _instance = new Gatherer();
            }

            return _instance;
        }

        private Gatherer()
        {
            SetNames = new List<string> { "akh" };
            SetManagers = new Dictionary<string, SetManager>();

            foreach (string set in SetNames)
            {
                SetManagers.Add(set, LoadSet(set));
            }
        }

        private static SetManager LoadSet(string set)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cards", $"{set}.txt");
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                var cards = JsonConvert.DeserializeObject<List<Card>>(json);
                var manager = new SetManager(cards);
                return manager;
            }
        }
    }
}