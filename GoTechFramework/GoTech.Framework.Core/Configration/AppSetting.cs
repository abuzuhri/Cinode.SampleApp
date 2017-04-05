using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoTech.Framework.Core.Configration
{
    public class AppSetting
    {
        public GoTech GoTech { get; set; }
        public AppSetting()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            var config = builder.Build();
            GoTech = config.GetSection("GoTech").Get<GoTech>();


        }
    }
}
