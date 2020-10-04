using System;
using System.Collections.Generic;
using System.Text;

namespace CatBot.Models
{
    public struct ConfigDataModel
    {
        /// <summary>
        /// Токен, который бот использует чтобы подключиться
        /// </summary>
        public string Token;

        /// <summary>
        /// Время, отвечающее за время рестарта бота
        /// </summary>
        public int RestartTime;
    }
}
