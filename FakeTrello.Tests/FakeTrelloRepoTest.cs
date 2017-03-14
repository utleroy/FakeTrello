using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeTrello.DAL;
using FakeTrello.Tests;
using Moq;
using FakeTrello.Models;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace FakeTrello.Tests
{
    [TestClass]
    public class FakeTrelloRepoTest
    {
        public Mock<FakeTrelloContext> fake_context { get; set; }
        public FakeTrelloRepository repo { get; set; }
        public Mock<DbSet<Board>> mock_boards_set { get; set; }
        public IQueryable<Board> query_boards { get; set; }
        public List<Board> fake_board_table { get; set; }

        [TestInitialize]
        public void Setup()
        {
            fake_board_table = new List<Board>();
            fake_context = new Mock<FakeTrelloContext>();
            mock_boards_set = new Mock<DbSet<Board>>();
            repo = new FakeTrelloRepository(fake_context.Object);

        }

        public void CreateFakeDb()
        {
            var query_boards = fake_board_table.AsQueryable();

            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.Provider).Returns(query_boards.Provider);
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.Expression).Returns(query_boards.Expression);
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.ElementType).Returns(query_boards.ElementType);
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.GetEnumerator()).Returns(() => query_boards.GetEnumerator());

            mock_boards_set.Setup(b => b.Add(It.IsAny<Board>())).Callback((Board board) => fake_board_table.Add(board));
            fake_context.Setup(c => c.Board).Returns(mock_boards_set.Object);
        }

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

        [TestMethod]
        public void EnsureICanGetBoard()
        {
            CreateFakeDb();

            ApplicationUser a_user = new ApplicationUser {
                Id = "my_id_user",
                UserName = "Lee",
                Email = "lee@lee.com"
            };
            //act
            repo.AddBoard("My board", a_user);
            //assert

            Assert.AreEqual(1, repo.Context.Board.Count());


        }
        [TestMethod]
        public void EnsureCanReturnBoards()
        {
            //Arrange
            fake_board_table.Add(new Board { Name = "My Board" });
           
            CreateFakeDb();

            int expected_board_count = 1;
            int actual_board_count = repo.Context.Board.Count();

            Assert.AreEqual(expected_board_count, actual_board_count); ;
        }

        [TestMethod]
        public void EnsureICanFindBoard()
        {
            fake_board_table.Add(new Board { BoardId = 1, Name = "My Board" });
            CreateFakeDb();

            string expected_board_name = "My Board";
            Board actual_board = repo.GetBoard(1);
            string actual_board_name = repo.GetBoard(1).Name;

            Assert.IsNotNull(actual_board);
            Assert.AreEqual(expected_board_name, actual_board_name);
        }
    }
}
