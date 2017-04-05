﻿using Cinode.RateApp.Business.UnitOfWork;
using Cinode.RateApp.Data.Entity;
using Cinode.RateApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace Cinode.RateApp.Business.Service
{
    public class SkillService : AppBaseUnitOfWork
    {
        public void DeleteUserSkill(long Id)
        {
            UserSkillRepository.Delete(Id);
        }

        public void AddEditUserSkill(AddEditUserSkill skillModel)
        {
            if (skillModel.SkillId.HasValue)
            {
                var userSkill = UserSkillRepository.Where(a => a.UserId == skillModel.UserId && a.SkillId == skillModel.SkillId.Value).FirstOrDefault();
                if (userSkill != null)
                {
                    userSkill.RateId = skillModel.RateId;
                    UserSkillRepository.Update(userSkill);
                }
                else
                {
                    var newUserSkill = new UserSkill();
                    newUserSkill.RateId = skillModel.RateId;
                    newUserSkill.SkillId = skillModel.SkillId.Value;
                    newUserSkill.UserId = skillModel.UserId;
                    UserSkillRepository.Insert(newUserSkill);
                }
            }
            else
            {
                var qry = SkillRepository.Query(a => a.Name == skillModel.SkillName);
                var skill = qry.FirstOrDefault();
                if (skill == null)
                    skill = AddNewSkill(skillModel.SkillName);

                var newUserSkill = new UserSkill();
                newUserSkill.RateId = skillModel.RateId;
                newUserSkill.Skill = skill;
                newUserSkill.UserId = skillModel.UserId;
                UserSkillRepository.Insert(newUserSkill);
            }
        }

        private Skill AddNewSkill(string skillName)
        {
            var skill = new Skill();
            skill.Name = skillName;
            SkillRepository.Insert(skill);
            return skill;
        }
    }
}
