using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trine.Mobile.UWP
{
    public sealed class SingletonA
    {
        private static SingletonA instance = null;
        private static readonly object padlock = new object();

        public static SingletonA Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance is null)
                        instance = new SingletonA();

                    return instance;
                }
            }
        }
    }
}
