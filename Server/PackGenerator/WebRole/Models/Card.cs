using System;

namespace WebRole.Models
{
    public class Card
    {
        public string Name { get; set; }
        public int ConvertedManaCost { get; set; }
        public string Color { get; set; }
        public string Rarity { get; set; }
        public Boolean IsCreature { get; set; }
        public string Image { get; set; }
    }
}