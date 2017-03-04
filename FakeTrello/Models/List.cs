using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FakeTrello.Models
{
    public class List
    {
        [Key]
        public int ListId {get;set;}

        public string Name { get; set; }

        public List<Card> Cards { get; set; }
    }
}