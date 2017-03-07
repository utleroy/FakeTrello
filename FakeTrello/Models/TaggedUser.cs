using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakeTrello.Models
{
    public class TaggedUser
    {
        public ICollection<Card> Cards { get; set; }
    }
}