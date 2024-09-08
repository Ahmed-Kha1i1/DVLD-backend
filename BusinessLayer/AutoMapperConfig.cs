using AutoMapper;
using DVLDApi.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore
{
    internal class AutoMapperConfig
    {
        public static IMapper Mapper { get; private set; }

        static AutoMapperConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            Mapper = config.CreateMapper();
        }
    }
}
