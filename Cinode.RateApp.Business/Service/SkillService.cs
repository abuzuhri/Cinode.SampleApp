using Cinode.RateApp.Business.UnitOfWork;
using Cinode.RateApp.Data.Entity;
using Cinode.RateApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Cinode.RateApp.Business.Service
{
    public class SkillService : AppBaseUnitOfWork
    {

        public IList<UserSkill> GetUserSkills(long Id)
        {
            return UserSkillRepository.Query(a => a.UserId == Id).Include(a=>a.Skill).ToList();
        }
        public IList<SkillModel> GetSkill(string name)
        {
            var list= SkillRepository.Query(a => a.Name.Contains(name)).Take(10).ToList();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Skill, SkillModel>();
            });
            var model = Mapper.Map<IList<SkillModel>>(list);
            return model;
        }

        public void DeleteUserSkill(long Id)
        {
            UserSkillRepository.Delete(Id);
        }

        public void AddEditUserSkill(AddEditUserSkill skillModel)
        {
            if (skillModel.SkillId.HasValue)
            {
                var userSkill = UserSkillRepository.Where(a => a.UserId == skillModel.UserId && a.SkillId == skillModel.SkillId.Value).FirstOrDefault();
                if (userSkill != null && userSkill.RateId != skillModel.RateId)
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
                    newUserSkill.DateCreated = DateTime.Now;
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
                newUserSkill.DateCreated = DateTime.Now;
                UserSkillRepository.Insert(newUserSkill);
            }

            Save();
        }

        private Skill AddNewSkill(string skillName)
        {
            var skill = new Skill();
            skill.Name = skillName;
            skill.DateCreated = DateTime.Now;
            SkillRepository.Insert(skill);
            return skill;
        }
    }
}
