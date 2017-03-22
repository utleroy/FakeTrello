using FakeTrello.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTrello.Controllers.Contracts
{
    interface IListRepository
    {
        // create
        void AddList(string name, Board board);
        void AddList(string name, int boardId);

        // read
        List GetList(int listId);
        List<List> GetListsFromBoard(int boardId);

        //delete
        bool RemoveList(int listId);
    }
}
