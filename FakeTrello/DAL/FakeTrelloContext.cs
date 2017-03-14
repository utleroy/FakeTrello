using FakeTrello.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FakeTrello.DAL
{
    public class FakeTrelloContext : ApplicationDbContext
    {
        public virtual DbSet<Board> Board { get; set; }
        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<Card> Card { get; set; }
    }
}