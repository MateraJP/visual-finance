using Dapper;
using System;
using System.Threading.Tasks;

namespace VisualFinanceiro.Auth.Implementations
{
    internal class SettupRepository : ISettupRepository
    {
        private readonly IConnectionFactory connectionFactory;
        public SettupRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
            try
            {
                CheckConfig().Wait();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to configure database for CustomAuth. Try overriding ISettupRepository and IUserRepository for custom table use.", ex);
            }
        }

        public async Task CheckConfig()
        {
            var conn = await connectionFactory.getConnection();

            var check = await conn.QueryFirstOrDefaultAsync<bool>(checkCommand);
            if (!check)
                await conn.ExecuteAsync(setupComand);
        }

        private string checkCommand = "Select 1 From sys.objects As o Where o.Name = 'AuthUser'";
        private string setupComand => @"
            Create Table AuthUser (
               Id bigint Not Null Identity,
               Password      Varchar(120)   Null,
               Username      Varchar(120)   Not Null,
               Email         Varchar(120)   Not Null,
               IsEmailValid  Bit            Null,
               ProfilePic    Varchar(240)   Null
            );

            Alter Table AuthUser
                Add Constraint [PK_AuthUser_Id] Primary Key Clustered ([Id] Asc);

            Alter Table AuthUser
                ADD CONSTRAINT Unique_Login UNIQUE (Email);
            ";
    }
}
