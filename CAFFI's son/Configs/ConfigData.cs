using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CatBot.Models;
using Newtonsoft.Json;
namespace CatBot.Configs
{
    public static class ConfigData
    {
        //Директории папок - указать
        private const string ConfigFolder = "Configs";
        private const string ConfigFile = "ConfigData.json";

        public static ConfigDataModel Data { get; }
        //Ищет необходимые файлы чтобы запустить бота
        static ConfigData()
        {
            if (!Directory.Exists(ConfigFolder)) Directory.CreateDirectory(ConfigFolder);

            if(!File.Exists(ConfigFolder + "/" + ConfigFile))
            {
                Data = new ConfigDataModel();
                var json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
            }
            else
            {
                var json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                Data = JsonConvert.DeserializeObject<ConfigDataModel>(json);
            }
        }
    }
}
