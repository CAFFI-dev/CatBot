using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace CatBot.Database
{
    public class DBConnection
    {
        public static async Task InitializeComponent() 
        {

            //ничего не работает тут, перейду на - postgreSql в новых версиях ;)
            string connStr;
            StreamReader reader = new StreamReader("config.txt");
            //DB
            while (!reader.EndOfStream)
            {
                if (reader.ReadLine() == "databaseStr:") break;
            }
            connStr = reader.ReadLine();
            Console.WriteLine("Код для подключения к MySql: " + connStr);
            using (var conn = new MySqlConnection(connStr))
            {

                //await conn.OpenAsync(); //тут ошибка?
                //await conn.CloseAsync();
            }

            //Console.WriteLine("ебать подключился:" , connection.State);

        }

        //public Task<DataSet> GetDataSetAsync(string connStr)
        //{
        //    return Task.Run(() =>
        //    {
        //        using (var conn = new MySqlConnection)
        //    }
        //}
    }
}
