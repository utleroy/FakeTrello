﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FakeTrello.Models;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using FakeTrello.Controllers.Contracts;
using System.Configuration;
using System.Data.Common;

namespace FakeTrello.DAL.Repository
{
    public class BoardRepository : IBoardManager, IBoardQuery
    {

        IDbConnection _trelloConnection;

        public BoardRepository(IDbConnection trelloConnection)
        {
            _trelloConnection = trelloConnection;
            _trelloConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
        public void AddBoard(string name, ApplicationUser owner)
        {
            Board board = new Board { Name = name, Owner = owner };
            //Context.Boards.Add(board);
            //Context.SaveChanges();

            _trelloConnection.Open();
            try
            {
                var addBoardCommand = _trelloConnection.CreateCommand();
                addBoardCommand.CommandText = $" Insert into Boards(Name, Owner_Id) values(@name, @ownerId)";

                var nameParameter = new SqlParameter("name", SqlDbType.VarChar);
                nameParameter.Value = name;
                addBoardCommand.Parameters.Add(nameParameter);

                var ownerParameter = new SqlParameter("name", SqlDbType.Int);
                ownerParameter.Value = name;
                addBoardCommand.Parameters.Add(ownerParameter);

                addBoardCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _trelloConnection.Close();
            }
        }

        public Board GetBoard(int boardId)
        {
            // SELECT * FROM Boards WHERE BoardId == boardId 

            _trelloConnection.Open();

            try
            {
                var getBoardCommand = _trelloConnection.CreateCommand();
                getBoardCommand.CommandText = @"
                SELECT boardId,Name<Url,Owner_Id 
                FROM Boards 
                WHERE BoardId == @boardId";
                var boardIdParameter = new SqlParameter("boardId", SqlDbType.Int);
                boardIdParameter.Value = boardId;
                getBoardCommand.Parameters.Add(boardIdParameter);

                var reader = getBoardCommand.ExecuteReader();

                if (reader.Read())
                {
                    var board = new Board
                    {
                        BoardId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        URL = reader.GetString(2),
                        Owner = new ApplicationUser { Id = reader.GetString(3) }
                    };
                    return board;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _trelloConnection.Close();
            }

            return null;

        }

        public List<Board> GetBoardsFromUser(string userId)
        {
            _trelloConnection.Open();

            try
            {
                var getBoardCommand = _trelloConnection.CreateCommand();
                getBoardCommand.CommandText = @"
                SELECT boardId,Name<Url,Owner_Id 
                FROM Boards 
                WHERE BoardId == @userId";
                var boardIdParameter = new SqlParameter("userId", SqlDbType.VarChar);
                boardIdParameter.Value = userId;
                getBoardCommand.Parameters.Add(boardIdParameter);

                var reader = getBoardCommand.ExecuteReader();


                var boards = new List<Board>();
                while (reader.Read())
                {
                    var board = new Board
                    {
                        BoardId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        URL = reader.GetString(2),
                        Owner = new ApplicationUser { Id = reader.GetString(3) }
                    };
                    boards.Add(board);
                }
                return boards;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _trelloConnection.Close();
            }

            return new List<Board>();
        }

        public bool RemoveBoard(int boardId)
        {
            _trelloConnection.Open();

            try
            {
                var removeBoardCommand = _trelloConnection.CreateCommand();
                removeBoardCommand.CommandText = @"
                    Delete
                    From Boards
                    Where boardId = @boardId";

                var boardIdParameter = new SqlParameter("boardId", SqlDbType.Int);
                boardIdParameter.Value = boardId;
                removeBoardCommand.Parameters.Add(boardIdParameter);

                var reader = removeBoardCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _trelloConnection.Close();
            }

            return false;
        }

        public void EditBoardName(int boardId, string newname)
        {
            _trelloConnection.Open();
            try
            {
                var updateBoardCommand = _trelloConnection.CreateCommand();
                updateBoardCommand.CommandText = @"
                    Update Boards
                    Set Name = @NewName
                    Where boardId = @boardId";

                var nameParameter = new SqlParameter("NewName", SqlDbType.VarChar);
                nameParameter.Value = newname;
                updateBoardCommand.Parameters.Add(nameParameter);

                var boardIdParameter = new SqlParameter("name", SqlDbType.Int);
                boardIdParameter.Value = boardId;
                updateBoardCommand.Parameters.Add(boardIdParameter);

                updateBoardCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _trelloConnection.Close();
            }
        }
    }
}