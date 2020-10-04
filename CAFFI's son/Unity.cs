using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace CatBot
{
    public static class Unity
    {
        private static UnityContainer container;

        private static UnityContainer Container
        {
            get
            {
                if (container == null)
                    RegisterTypes();
                return container;
            }
        }

        public static void RegisterTypes()
        {
            container = new UnityContainer();
            //container.RegisterType<IBot, Bot>
        }
    }
}
