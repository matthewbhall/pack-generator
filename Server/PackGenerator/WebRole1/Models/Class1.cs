using System;
using Newtonsoft.Json.Linq;

namespace WebRole.Models
{
    public class Card
    {
        public string Name { get; set; }
        public int ConvertedManaCost { get; set; }
        public string Rarity { get; set; }
        public Boolean IsCreature { get; set; }
        public string Image { get; set; }

        public virtual void DeserializeJson(JToken inputObject)
        {
            if (inputObject != null && inputObject.Type != JTokenType.Null)
            {
                JToken descriptionValue = inputObject["Description"];
                if (descriptionValue != null && descriptionValue.Type != JTokenType.Null)
                {
                    this.Description = ((string)descriptionValue);
                }
                JToken idValue = inputObject["ID"];
                if (idValue != null && idValue.Type != JTokenType.Null)
                {
                    this.ID = ((int)idValue);
                }
                JToken ownerValue = inputObject["Owner"];
                if (ownerValue != null && ownerValue.Type != JTokenType.Null)
                {
                    this.Owner = ((string)ownerValue);
                }
            }
        }
    }
}