using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Trine.Mobile.Bootstrapper.Builders
{
    internal class MapperBuilder
    {
        public MapperBuilder()
        {
        }

        public IMapper CreateMapper()
        {
            var config = new MapperConfiguration((cfg) => { });
            config.AssertConfigurationIsValid();

            return config.CreateMapper();
        }
    }
}
