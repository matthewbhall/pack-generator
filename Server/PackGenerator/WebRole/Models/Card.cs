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
        public Boolean IsBasic { get; set; }
        public string Image { get; set; }
        public Boolean IsFoil { get; set; } = false;

        public Card Foil()
        {
            return new Card
            {
                Name = this.Name,
                ConvertedManaCost = this.ConvertedManaCost,
                Color = this.Color,
                Rarity = this.Rarity,
                IsCreature = this.IsCreature,
                IsBasic = this.IsBasic,
                Image = this.Image,
                IsFoil = true
            };
        }
    }
}