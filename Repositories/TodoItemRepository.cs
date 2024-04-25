using Dapper;
using brokenaccesscontrol.Models;
using System.Security.Cryptography;
using System.Text;

namespace brokenaccesscontrol.Repositories;

public static class TodoItemRepository
{

    public static async Task<IEnumerable<TodoItem>> GetToDoItems()
    {
        var conn = SqliteConfigConnection.GetSQLiteConnection();
        string query = "Select id, name, description, userId from todoitems";
        var lstTodoItens = await conn.QueryAsync<TodoItem>(query);
        return lstTodoItens;
    }

    public static async Task<TodoItem> GetToDoItem(int id)
    {
        var conn = SqliteConfigConnection.GetSQLiteConnection();
        string query = "Select id, name, description, userId from todoitems where id = @id";
        var todoItem = await conn.QueryAsync<TodoItem>(query, new{
            id = id
        });
        return todoItem.FirstOrDefault();
    }    


    public static async Task<bool> Delete(int id){

        var conn = SqliteConfigConnection.GetSQLiteConnection(); 
        var query = "delete from todoitems where id = @id";
        var table = await conn.ExecuteAsync(query, new{
            id = id
        });        

        return table > 0 ? true : false;        
    }    

    public static async Task<bool> Insert(TodoItemNewRequest todoItem, string userId){

        var conn = SqliteConfigConnection.GetSQLiteConnection(); 
        var query = "insert into todoitems(name, description, userId) values (@name, @description, @userId)";
        var table = await conn.ExecuteAsync(query, new{
            name = todoItem.Name,
            description = todoItem.Description,
            userId = userId
        });        

        return table > 0 ? true : false;        
    }       
}