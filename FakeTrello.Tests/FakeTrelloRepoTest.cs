using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeTrello.DAL;
using FakeTrello.Tests;

namespace FakeTrello.Tests
{
    [TestClass]
    public class FakeTrelloRepoTest
    {
        [TestMethod]
        public void EnsureCanCreate()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void EnsureCanInject()
        {
            FakeTrelloContext context = new FakeTrelloContext();
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo.Context);

        }
    }
}
