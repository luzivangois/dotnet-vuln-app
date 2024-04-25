using Dapper;
using brokenaccesscontrol.Models;
using brokenaccesscontrol.Services;

namespace brokenaccesscontrol.Repositories;

public static class UserRepository
{

    public static async Task<UserResponse> Insert(UserRequest userRequest){

        var userResponse = new UserResponse {
                User = new Models.User{
                Id = Guid.NewGuid().ToString(),
                DateInsert = DateTime.UtcNow,
                IsAdmin = userRequest.IsAdmin.HasValue ? userRequest.IsAdmin.Value : false,
                Login = userRequest.Login,
                Name = userRequest.Name,
                Password = UtilService.ReturnMD5(userRequest.Password)             
            }
        };          

        var conn = SqliteConfigConnection.GetSQLiteConnection(); 
        var query = "insert into users (id,name,login, password,isAdmin,dateInsert) values(@id,@name,@login,@password,@isAdmin,@dateInsert)";
        var table = await conn.ExecuteAsync(query, new{
            id = userResponse.User.Id,
            name = userResponse.User.Name,
            login = userResponse.User.Login,
            password = userResponse.User.Password,
            isAdmin = userResponse.User.IsAdmin,
            dateInsert = userResponse.User.DateInsert.ToString("yyyy-MM-dd HH:mm:ss")
        });        


        return userResponse;        
    }

    public static async Task RecoveryPassword(PasswordRecovery recovery){
        var conn = SqliteConfigConnection.GetSQLiteConnection(); 
        var query = "update users set inativo=@inativo, dateChangePassword=@dateChangePassword where login=@login";
        var table = await conn.ExecuteAsync(query, new{
            inativo = 1,
            dateChangePassword = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            login = recovery.Login
        });          
    }    

    public static async Task<IEnumerable<User>> GetAllUsers()
    {
            var conn = SqliteConfigConnection.GetSQLiteConnection();
            string query = "Select id, name, login, isAdmin, inativo, dateChangePassword from users";
            var lstUsers = await conn.QueryAsync<User>(query);
            return lstUsers;
    }  

    public static async Task<User> GetUserById(String id)
    {
        var conn = SqliteConfigConnection.GetSQLiteConnection();
        string query = "Select id, name, login, password, dateInsert, dateUpdate, isAdmin, inativo, dateChangePassword from users where id = @id";
        var user = await conn.QueryAsync<User>(query, new{
            id = id
        });
        return user.FirstOrDefault();
    }   

        public static async Task<bool> Delete(string id){

        var conn = SqliteConfigConnection.GetSQLiteConnection(); 
        var query = "delete from users where id = @id";
        var table = await conn.ExecuteAsync(query, new{
            id = id
        });        

        return table > 0 ? true : false;        
    }     


    public static async Task<User> Login(LoginRequest login)
    {
        var conn = SqliteConfigConnection.GetSQLiteConnection();
        string query = "Select id, name, login, password, dateInsert, dateUpdate, isAdmin, inativo, dateChangePassword from users where login = @login and inativo = 0";
        var user = await conn.QueryAsync<User>(query, new{
            @login = login.Login
        });
        return user.FirstOrDefault();
    }  

    public static async Task<User> LoginSQL(LoginRequest login)
    {
        var conn = SqliteConfigConnection.GetSQLiteConnection();
        string query = "Select id, name, login, password, dateInsert, dateUpdate, isAdmin, inativo, dateChangePassword from users "  + 
            "where login = '"+login.Login+"' and password = '"+UtilService.ReturnMD5(login.Password)+"' and inativo = 0";
        var user = await conn.QueryAsync<User>(query);
        return user.FirstOrDefault();
    }  


    public static async Task<bool> LoginExist(string login)
    {
        var conn = SqliteConfigConnection.GetSQLiteConnection();
        string query = "Select count(*) from users where login = @login";
        var user = await conn.QueryAsync<int>(query, new{
            @login = login
        });
        return user.FirstOrDefault() > 0 ? true : false;
    }  
}
