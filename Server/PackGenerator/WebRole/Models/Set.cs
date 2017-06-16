using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class Set
    {
        public List<Card> Mythics { get; set; } = new List<Card>();
        public List<Card> Rares { get; set; } = new List<Card>();
        public List<Card> Uncommons { get; set; } = new List<Card>();
        public List<Card> Commons { get; set; } = new List<Card>();
        public List<Card> BasicLands { get; set; } = new List<Card>();
    }
}