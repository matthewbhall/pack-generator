using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WebRole.Managers;
using WebRole.Models;

namespace WebRole
{
    /// <summary>
    /// Gatherer is a singleton class in charge of managing the vast library of
    /// card sets. Gatherer houses a single record of information about each set
    /// allows controllers to use them. It also houses managers that controllers
    /// can use to perform specific tasks. 
    /// </summary>
    public class Gatherer
    {
        public List<String> SetNames { get; } = new List<string> { "akh" };
        public PackManager PackManager { get; } = new PackManager();

        private Dictionary<String, Set> Sets { get; } = new Dictionary<string, Set>();
        private static Gatherer _instance = null;

        /// <summary>
        /// Gets the Gatherer instance for this process.
        /// 
        /// The instance is lazily instantiated.
        /// </summary>
        /// <returns>A Gatherer object.</returns>
        public static Gatherer Instance()
        {
            if (_instance == null)
            {
                _instance = new Gatherer();
            }

            return _instance;
        }

        /// <summary>
        /// Gets a set associated with a particular set name (abbreviated).
        /// </summary>
        /// <param name="setName">The abbreviated name of the set.</param>
        /// <returns>The set of cards.</returns>
        public Set GetSet(string setName)
        {
            Set set = null;
            Sets.TryGetValue(setName, out set);
            return set;
        }

        private Gatherer()
        {
            foreach (string set in SetNames)
            {
                try
                {
                    Sets.Add(set, LoadSet(set));
                }
                catch (FileNotFoundException e)
                {
                    Trace.TraceError($"Could not find set file for {set}");
                }
            }
        }

        // Load a set from a json file
        private static Set LoadSet(string setName)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cards", $"{setName}.txt");
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                var cards = JsonConvert.DeserializeObject<List<Card>>(json);
                var set = new Set();

                if (cards.Count == 0)
                {
                    Trace.TraceError($"Loaded empty set {set}");
                }

                foreach (Card card in cards)
                {
                    switch (card.Rarity)
                    {
                        case "Mythic":
                            set.Mythics.Add(card);
                            break;
                        case "Rare":
                            set.Rares.Add(card);
                            break;
                        case "Uncommon":
                            set.Uncommons.Add(card);
                            break;
                        case "Common":
                            if (card.IsBasic)
                            {
                                set.BasicLands.Add(card);
                            }
                            else
                            {
                                set.Commons.Add(card);
                            }
                            break;
                        default:
                            Trace.TraceError($"Could not sort card: {card}");
                            break;
                    }
                }

                return set;
            }
        }
    }
}