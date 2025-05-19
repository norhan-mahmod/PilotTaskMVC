using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pilot.DAL.Context;

namespace Pilot.PL.Helper
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseExists(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TicketsDB')
                BEGIN
                    CREATE DATABASE TicketsDB;
                END";
            cmd.ExecuteNonQuery();
        }

        public static void Initialize( TicketDbContext context)
        {
            var connection = context.Database.GetDbConnection();
            connection.Open();
            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = @"
                IF NOT EXISTS (
                    SELECT * 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'IssueTypes'
                )
                select 1
                else select 0
            ";
            int runScript = Convert.ToInt32(checkCommand.ExecuteScalar());
            if(runScript == 1)
            {
                string rootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../../"));
                string scriptPath = Path.Combine(rootPath, "Pilot.DAL", "SQLScript", "SQLQuery1.sql");
                var sqlScript = File.ReadAllText(scriptPath);
                var scriptsArr = sqlScript.Split("GO");
                foreach(var sql in scriptsArr)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                

            }
            connection.Close();
        }
    }
}
