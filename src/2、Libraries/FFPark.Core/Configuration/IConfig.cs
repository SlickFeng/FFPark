using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FFPark.Core.Configuration
{
    public partial interface IConfig
    {
        [JsonIgnore]
        string Name => GetType().Name;
    }
}
