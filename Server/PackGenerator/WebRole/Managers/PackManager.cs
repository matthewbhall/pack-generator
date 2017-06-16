using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRole.Models;

namespace WebRole.Managers
{
    /// <summary>
    /// PackManager is responsible for generating randomized packs.
    /// </summary>
    public class PackManager
    {
        private Random rng = new Random();

        /// <summary>
        /// Generates a random booster pack of cards from a given set.
        /// 
        /// Uses data mined rules and probabilities for what a booster pack
        /// consists of.
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public List<Card> GeneratePack(Set set)
        {
            var pack = new List<Card>();

            // Generate the rare
            // 1 rare per pack
            // There is about a 1/8 chance of a pack having a mythic rare
            if (rng.Next(8) == 7)
            {
                pack.Add(set.Mythics[rng.Next(set.Mythics.Count)]);
            }
            else
            {
                pack.Add(set.Rares[rng.Next(set.Rares.Count)]);
            }

            // Generate the uncommons
            // 3 uncommons per pack
            pack.AddRange(PickRandomCards(3, set.Uncommons));

            // Generate the commons
            // 10 commons per pack
            // If there is a foil, it replace a common
            // There is about a 1/6 chance of a pack having a foil
            // Check for a foil
            if (rng.Next(6) == 5)
            {
                pack.AddRange(PickRandomCards(9, set.Commons));
                pack.Add(GenerateFoil(set));
            }
            else
            {
                pack.AddRange(PickRandomCards(10, set.Commons));
            }

            return pack;
        }

        /// <summary>
        /// Generate a random foil card. Foils can be of any rarity, but are
        /// skewed towards common.
        /// </summary>
        /// <param name="set">The set to generate a foil from.</param>
        /// <returns>A random foil card.</returns>
        private Card GenerateFoil(Set set)
        {
            // Foil cards can be mythic rare, rare, uncommon, common, or land
            // The foil card probabilities:
            // 88 / 128 common
            // 24 / 128 uncommon
            // 8 / 128 land
            // 7 / 128 rare
            // 1 / 128 mythic

            var random = rng.Next(128);
            if (random < 88)
            {
                // Common
                return set.Commons[rng.Next(set.Commons.Count)].Foil();
            }

            if (random < 112)
            {
                // Uncommon
                return set.Uncommons[rng.Next(set.Uncommons.Count)].Foil();
            }

            if (random < 120)
            {
                // Land
                return set.BasicLands[rng.Next(set.BasicLands.Count)].Foil();
            }

            if (random < 127)
            {
                // Rare
                return set.Rares[rng.Next(set.Rares.Count)].Foil();
            }

            // Mythic Rare
            return set.Mythics[rng.Next(set.Mythics.Count)].Foil();
        }

        /// <summary>
        /// Pick a specified number of cards randomly out of a given card pool.
        /// Duplicates are not allowed.
        /// </summary>
        /// <param name="numCards">The number of cards to pick.</param>
        /// <param name="cardPool">The card pool to pick from.</param>
        /// <returns>A list of randomly selected cards.</returns>
        private List<Card> PickRandomCards(int numCards, List<Card> cardPool)
        {
            List<int> indexes = new List<int>();
            List<Card> randomCards = new List<Card>();
            for (int i = 0; i < numCards; i++)
            {
                var newIndex = rng.Next(cardPool.Count);
                while (indexes.Contains(newIndex))
                {
                    newIndex = rng.Next(cardPool.Count);
                }
                indexes.Add(newIndex);
                randomCards.Add(cardPool[newIndex]);
            }

            return randomCards;
        }
    }
}