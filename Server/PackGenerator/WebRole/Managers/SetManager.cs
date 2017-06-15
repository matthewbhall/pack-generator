using System.Collections.Generic;
using System.Diagnostics;
using WebRole.Models;

namespace WebRole.Managers
{
    public class SetManager
    {
        private readonly Set _set = new Set();
        private class Set
        {
            public List<Card> Mythics { get; set; } = new List<Card>();
            public List<Card> Rares { get; set; } = new List<Card>();
            public List<Card> Uncommons { get; set; } = new List<Card>();
            public List<Card> Commons { get; set; } = new List<Card>();
        }

        public SetManager(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                switch (card.Rarity)
                {
                    case "Mythic":
                        _set.Mythics.Add(card);
                        break;
                    case "Rare":
                        _set.Rares.Add(card);
                        break;
                    case "Uncommon":
                        _set.Uncommons.Add(card);
                        break;
                    case "Common":
                        _set.Commons.Add(card);
                        break;
                    default:
                        Trace.TraceError($"Could not sort card: {card}");
                        break;
                }
            }
        }

        public List<Card> GetMythics()
        {
            return _set.Mythics;
        }
    }
}