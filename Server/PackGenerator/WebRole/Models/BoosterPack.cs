using System;
using System.Collections.Generic;

namespace WebRole.Models
{
    /// <summary>
    /// A booster pack consists of a random list of cards.
    /// </summary>
    public class BoosterPack
    {
        public List<Card> Cards { get; set; }
    }
}