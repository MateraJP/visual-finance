using Dapper;
using System;
using System.IO;
using System.Threading.Tasks;

namespace VisualFinanceiro.Auth.Implementations
{
    internal class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory connectionFactory;
        public UserRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IUser> GetUserById(long id)
        {
            var conn = await connectionFactory.getConnection();
            var user = await conn.QueryFirstOrDefaultAsync<User>(selectUserById, new { id });

            if (user == null)
                throw new ArgumentException("User not found", "Id");

            return user;
        }

        public async Task<IUser> GetUserByUsername(string username)
        {
            var conn = await connectionFactory.getConnection();
            var user = await conn.QueryFirstOrDefaultAsync<User>(selectUserByUsername, new { username });

            if (user == null)
                throw new ArgumentException("User not found", "Username");

            return user;
        }

        public async Task<IUser> InsertUser(string username, string email, string password, bool isEmailValid)
        {
            var conn = await connectionFactory.getConnection();
            var user = await conn.QueryFirstOrDefaultAsync<User>(selectUserByUsername, new { username });
            if (user == null)
                return await conn.QueryFirstOrDefaultAsync<User>(insertUser, new { username, email, password, isEmailValid });
            else
                throw new ArgumentException("Username already been used", "Username");
        }

        public async Task<IUser> SetProfilePic(string username, string path)
        {
            var conn = await connectionFactory.getConnection();
            var user = await conn.QueryFirstOrDefaultAsync<User>(updateProfilePic, new { username, path });

            if (user == null)
                throw new ArgumentException("User not found", "Id");

            return user;
        }

        public async Task<IUser> SetEmailValid(long id, bool isEmailValid)
        {
            var conn = await connectionFactory.getConnection();
            var user = await conn.QueryFirstOrDefaultAsync<User>(updateEmailValid, new { id, isEmailValid });

            if (user == null)
                throw new ArgumentException("User not found", "Id");

            return user;
        }

        public async Task<bool> IsAuth(string username, string password)
        {
            var conn = await connectionFactory.getConnection();
            return await conn.QueryFirstOrDefaultAsync<bool>(selectAuthByUsernamePass, new { username, password });
        }

        private string selectUserById => $"Select Id, Username, Email, IsEmailValid, ProfilePic From AuthUser Where Id = @id";
        private string selectUserByUsername => $"Select Id, Username, Email, IsEmailValid, ProfilePic From AuthUser Where Username = @username";
        private string insertUser => $"Insert Into AuthUser (Username, Email, Password, IsEmailValid) Values (@username, @email, @password, @isEmailValid); {selectUserByUsername}";
        private string updateProfilePic => $"Update AuthUser Set ProfilePic = @path Where username = @username; {selectUserByUsername}";
        private string updateEmailValid => $"Update AuthUser Set IsEmailValid = @isEmailValid Where Id = @id; {selectUserById}";
        private string selectAuthByUsernamePass => $"Select Count(1) From AuthUser Where Username = @username And Password = @password";
    }
}
