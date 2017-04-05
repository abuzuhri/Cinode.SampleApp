using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoTech.Framework.Core
{
    public class BaseGoTechEntity
    {
        [Key]
        public long ID { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; }

    }
}
