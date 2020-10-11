using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;
namespace CatBot.Database
{
    class DBConnection
    {
        private readonly MySqlConnection connection = new MySqlConnection("server=");
    }
}
