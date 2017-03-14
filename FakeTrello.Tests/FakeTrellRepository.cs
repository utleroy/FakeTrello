using FakeTrello.DAL;

namespace FakeTrello.Tests
{
    public class FakeTrelloRepository : IRepository
    {
        public FakeTrelloContext Context { get; set; }

        public FakeTrelloRepository()
        {

        }
    }
}