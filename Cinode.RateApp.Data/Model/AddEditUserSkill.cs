using System;
using System.Collections.Generic;
using System.Text;

namespace Cinode.RateApp.Data.Model
{
    public class AddEditUserSkill
    {
        public long UserId { get; set; }
        public long? SkillId { get; set; }
        public string SkillName { get; set; }
        public long RateId { get; set; }
    }
}
