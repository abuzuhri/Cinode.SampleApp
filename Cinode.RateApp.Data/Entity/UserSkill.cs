using GoTech.Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cinode.RateApp.Data.Entity
{
    public class UserSkill : BaseGoTechEntity
    {
        public long UserId { get; set; }
        public long SkillId { get; set; }
        public long RateId { get; set; }

        [ForeignKey("SkillId")]
        public Skill Skill { get; set; }

        [ForeignKey("RateId")]
        public Rate Rate { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
