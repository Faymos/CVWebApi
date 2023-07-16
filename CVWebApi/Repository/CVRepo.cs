using AutoMapper;
using CVWebApi.Dtos;
using CVWebApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Mail;

namespace CVWebApi.Repository
{
    public class CVRepo : ICVRepo
    {
        private readonly CVContext _cVContext;
        private readonly IMapper _mapper;
        public CVRepo(CVContext cVContext, IMapper mapper)
        {
            _cVContext = cVContext;
            _mapper = mapper;
        }

        public async Task<ResponseDto> AddCVAsync(CVDto cv)
        {
            ResponseDto response = new();

            List<WorkExperience> workExperiences = new();
            List<Skills> skills = new();
            List<Qualifications> qualifications = new();

            try
            {
                var executionStrategy = _cVContext.Database.CreateExecutionStrategy();

                await executionStrategy.ExecuteAsync(async () =>
                {
                    using (var dbContextTransaction = _cVContext.Database.BeginTransaction())
                    {
                        Users users = new()
                        {
                            EmailAddress = cv.EmailAddress,
                            YearExperience = cv.YearExperience,
                            Firstname = cv.Firstname,
                            Middlename = cv.Middlename,
                            PhoneNumber = cv.PhoneNumber,
                            Surname = cv.Surname,
                            DateCreated = DateTime.Now
                        };

                        try
                        {
                            _cVContext.Users.Add(users);
                            await _cVContext.SaveChangesAsync();
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }

                        foreach (var item in cv.WorkExperience)
                        {
                            WorkExperience workExperience = new()
                            {
                                Organization = item.Organization,
                                JobTitle = item.JobTitle,
                                EndDate = item.EndDate,
                                UsersId = users.Id,
                                DateCreated = DateTime.Now,
                                StartDate = item.StartDate,
                            };

                            workExperiences.Add(workExperience);
                        }

                        foreach (var item in cv.Skills)
                        {
                            Skills skill = new()
                            {
                                Skill = item.Skill,
                                UsersId = users.Id,
                                DateCreated = DateTime.Now
                            };
                            skills.Add(skill);
                        }

                        foreach (var item in cv.Qualifications)
                        {
                            Qualifications qualification = new()
                            {
                                Qualification = item.Qualification,
                                TypeOfQualifiction = item.TypeOfQualifiction,
                                YearObtain = item.YearObtain,
                                UsersId = users.Id,
                                DateCreated = DateTime.Now
                            };
                            qualifications.Add(qualification);
                        }

                        await _cVContext.Qualifications.AddRangeAsync(qualifications);
                        await _cVContext.Skills.AddRangeAsync(skills);
                        await _cVContext.WorkExperience.AddRangeAsync(workExperiences);

                        if (await _cVContext.SaveChangesAsync() > 0)
                        {
                            dbContextTransaction.Commit();
                            
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            
                        }
                    }
                });
                return response = new()
                {
                    Message = "CV Created Successfully",
                    Status = HttpStatusCode.OK,
                    Success = true
                };

            }
            catch (Exception ex)
            {

                if (ex.InnerException != null && ex.InnerException.Message.Contains("Violation of UNIQUE KEY constraint 'UC_EmailAddress'"))
                {
                    return response = new()
                    {
                        Message = $"Cv with the email Address {cv.EmailAddress} already exist",
                        Status = HttpStatusCode.BadRequest,
                        Success = false
                    }; 
                }
                else
                {
                    return response = new()
                    {
                        Message = ex.Message,
                        Status = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
            }
        }
        public async Task<Users> GetCVByEmailAsync(string emailAddress)
        {
            try
            {
                var user = await _cVContext.Users.Include(u => u.WorkExperience)
                                                  .Include(u => u.Skills)
                                                  .Include(u => u.Qualifications)
                                                  .SingleOrDefaultAsync(u => u.EmailAddress == emailAddress);

                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Users>> GetCVsByExperience(string jobTitle)
        {
            try
            {
                _cVContext.Database.SetCommandTimeout(120);

                var cvList = (from s in _cVContext.WorkExperience
                              join u in _cVContext.Users on s.UsersId equals u.Id
                              where s.JobTitle == jobTitle.Trim()
                              select new Users
                              {
                                  Surname = u.Surname,
                                  Firstname = u.Firstname,
                                  YearExperience = u.YearExperience,
                                  EmailAddress = u.EmailAddress,
                                  Middlename = u.Middlename,
                                  PhoneNumber = u.PhoneNumber,
                                  Qualifications = _cVContext.Qualifications.Where(x => x.UsersId == u.Id).ToList(),
                                  Skills = _cVContext.Skills.Where(x => x.UsersId == u.Id).ToList(),
                                  WorkExperience = _cVContext.WorkExperience.Where(x => x.UsersId == u.Id).ToList(),
                              }).ToList();
                return cvList;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<List<Users>> GetCVsByQualification(string qualification)
        {
            try
            {
                _cVContext.Database.SetCommandTimeout(120);

                var cvList = (from s in _cVContext.Qualifications
                              join u in _cVContext.Users on s.UsersId equals u.Id
                              where s.Qualification == qualification.Trim()
                              select new Users
                              {
                                  Surname = u.Surname,
                                  Firstname = u.Firstname,
                                  YearExperience = u.YearExperience,
                                  EmailAddress = u.EmailAddress,
                                  Middlename = u.Middlename,
                                  PhoneNumber = u.PhoneNumber,
                                  Qualifications = _cVContext.Qualifications.Where(x => x.UsersId == u.Id).ToList(),
                                  Skills = _cVContext.Skills.Where(x => x.UsersId == u.Id).ToList(),
                                  WorkExperience = _cVContext.WorkExperience.Where(x => x.UsersId == u.Id).ToList(),
                              }).ToList();
                return cvList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Users>> GetCVsBySkill(string skill)
        {
            try
            {
                _cVContext.Database.SetCommandTimeout(120);

                var cvList =   (from s in _cVContext.Skills
                             join u in _cVContext.Users on s.UsersId equals u.Id
                             where s.Skill == skill.Trim()
                             select new Users
                             {
                                 Surname = u.Surname,
                                 Firstname = u.Firstname,
                                 YearExperience = u.YearExperience,
                                 EmailAddress = u.EmailAddress,
                                 Middlename = u.Middlename,
                                 PhoneNumber = u.PhoneNumber,
                                 Qualifications = _cVContext.Qualifications.Where(x => x.UsersId == u.Id).ToList(),
                                 Skills = _cVContext.Skills.Where(x => x.UsersId == u.Id).ToList(),
                                 WorkExperience = _cVContext.WorkExperience.Where(x => x.UsersId == u.Id).ToList(),
                             }).ToList();
                return cvList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ResponseDto> UpdateCVAsync(CVDto cv)
        {
            ResponseDto response = new();

            List<WorkExperience> workExperiences = new();
            List<Skills> skills = new();
            List<Qualifications> qualifications = new();

            try
            {
                var executionStrategy = _cVContext.Database.CreateExecutionStrategy();
           var res =     await executionStrategy.ExecuteAsync(async () =>
                {
                    using var dbContextTransaction = await _cVContext.Database.BeginTransactionAsync();
                    try
                    {
                        var user = await _cVContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == cv.EmailAddress);

                        if (user != null)
                        {
                            user.Firstname = cv.Firstname;
                            user.Middlename = cv.Middlename;
                            user.Surname = cv.Surname;
                            user.PhoneNumber = cv.PhoneNumber;
                            user.YearExperience = cv.YearExperience;
                            user.DateUpdated = DateTime.Now;
                            
                            _cVContext.Users.Update(user);

                            var existingWorkExperiences = await _cVContext.WorkExperience.Where(we => we.UsersId == user.Id).ToListAsync();
                            
                            _cVContext.WorkExperience.RemoveRange(existingWorkExperiences);

                            foreach (var item in cv.WorkExperience)
                            {
                                WorkExperience workExperience = new()
                                {
                                    Organization = item.Organization,
                                    JobTitle = item.JobTitle,
                                    EndDate = item.EndDate,
                                    UsersId = user.Id,
                                    DateCreated = DateTime.Now,
                                    DateModified = DateTime.Now,
                                    StartDate = item.StartDate,
                                };
                                workExperiences.Add(workExperience);
                            }
                            await _cVContext.WorkExperience.AddRangeAsync(workExperiences);

                            var existingSkills = await _cVContext.Skills.Where(s => s.UsersId == user.Id).ToListAsync();
                            _cVContext.Skills.RemoveRange(existingSkills);

                            foreach (var item in cv.Skills)
                            {
                                Skills skill = new()
                                {
                                    Skill = item.Skill,
                                    UsersId = user.Id,
                                    DateCreated = DateTime.Now,
                                    DateModified = DateTime.Now
                                };
                                skills.Add(skill);
                            }
                            await _cVContext.Skills.AddRangeAsync(skills);

                            var existingQualifications = await _cVContext.Qualifications.Where(q => q.UsersId == user.Id).ToListAsync();
                            _cVContext.Qualifications.RemoveRange(existingQualifications);

                            foreach (var item in cv.Qualifications)
                            {
                                Qualifications qualification = new()
                                {
                                    Qualification = item.Qualification,
                                    TypeOfQualifiction = item.TypeOfQualifiction,
                                    YearObtain = item.YearObtain,
                                    UsersId = user.Id,
                                    DateCreated = DateTime.Now,
                                    DateModified = DateTime.Now
                                };
                                qualifications.Add(qualification);
                            }
                            await _cVContext.Qualifications.AddRangeAsync(qualifications);
                            await _cVContext.SaveChangesAsync();
                            await dbContextTransaction.CommitAsync();

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        await dbContextTransaction.RollbackAsync();
                        throw ex;
                    }
                });
                if (!res)
                {
                    return response = new()
                    {
                        Message = "Record not found, No Record was Updated",
                        Status = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
                return response = new()
                {
                    Message = "Record Updated Successfully",
                    Status = HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (Exception ex)
            {
               if(ex.InnerException != null)
                {
                    return response = new()
                    {
                        Message = ex.InnerException.Message,
                        Status = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
                else
                {
                    return response = new()
                    {
                        Message = ex.Message,
                        Status = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
            }
        }
        public async Task<List<Users>> GetCVsAsync()
        {
            try
            {
                var cvList = await _cVContext.Users.Include(u => u.WorkExperience)
                                                     .Include(u => u.Skills)
                                                     .Include(u => u.Qualifications)
                                                     .Select(u => new Users
                                                     {
                                                         Id = u.Id,
                                                         EmailAddress = u.EmailAddress,
                                                         YearExperience = u.YearExperience,
                                                         Firstname = u.Firstname,
                                                         Middlename = u.Middlename,
                                                         PhoneNumber = u.PhoneNumber,
                                                         Surname = u.Surname,
                                                         DateCreated = u.DateCreated,
                                                         WorkExperience = u.WorkExperience.Select(w => new WorkExperience
                                                         {
                                                             Id = w.Id,
                                                             Organization = w.Organization,
                                                             JobTitle = w.JobTitle,
                                                             StartDate = w.StartDate,
                                                             EndDate = w.EndDate,
                                                             UsersId = w.UsersId,
                                                             DateCreated = w.DateCreated
                                                         }).ToList(),
                                                         Skills = u.Skills.Select(s => new Skills
                                                         {
                                                             Id = s.Id,
                                                             Skill = s.Skill,
                                                             UsersId = s.UsersId,
                                                             DateCreated = s.DateCreated
                                                         }).ToList(),
                                                         Qualifications = u.Qualifications.Select(q => new Qualifications
                                                         {
                                                             Id = q.Id,
                                                             Qualification = q.Qualification,
                                                             TypeOfQualifiction = q.TypeOfQualifiction,
                                                             YearObtain =q.YearObtain,
                                                             UsersId = q.UsersId,
                                                             DateCreated = q.DateCreated
                                                         }).ToList()
                                                     })
                                                     .ToListAsync();
                var cvs = cvList;
                return cvs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Users>> GetCVsByYearsOfExperience(int years)
        {
            try
            {
                var cvList = await _cVContext.Users.Include(u => u.WorkExperience)
                                                    .Include(u => u.Skills)
                                                    .Include(u => u.Qualifications)
                                                    .Where(u => u.YearExperience >= years)
                                                    .Select(u => new Users
                                                    {
                                                        Id = u.Id,
                                                        EmailAddress = u.EmailAddress,
                                                        YearExperience = u.YearExperience,
                                                        Firstname = u.Firstname,
                                                        Middlename = u.Middlename,
                                                        PhoneNumber = u.PhoneNumber,
                                                        Surname = u.Surname,
                                                        DateCreated = u.DateCreated,
                                                        WorkExperience = u.WorkExperience.Select(w => new WorkExperience
                                                        {
                                                            Id = w.Id,
                                                            Organization = w.Organization,
                                                            JobTitle = w.JobTitle,
                                                            StartDate = w.StartDate,
                                                            EndDate = w.EndDate,
                                                            UsersId = w.UsersId,
                                                            DateCreated = w.DateCreated
                                                        }).ToList(),
                                                        Skills = u.Skills.Select(s => new Skills
                                                        {
                                                            Id = s.Id,
                                                            Skill = s.Skill,
                                                            UsersId = s.UsersId,
                                                            DateCreated = s.DateCreated
                                                        }).ToList(),
                                                        Qualifications = u.Qualifications.Select(q => new Qualifications
                                                        {
                                                            Id = q.Id,
                                                            Qualification = q.Qualification,
                                                            TypeOfQualifiction = q.TypeOfQualifiction,
                                                            UsersId = q.UsersId,
                                                            DateCreated = q.DateCreated
                                                        }).ToList()
                                                    })
                                                    .ToListAsync();

                return cvList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Users>> GetCVsByYearsOfExperienceMax(int years)
        {
            try
            {
                var cvList = await _cVContext.Users.Include(u => u.WorkExperience)
                                                    .Include(u => u.Skills)
                                                    .Include(u => u.Qualifications)
                                                    .Where(u => u.YearExperience <= years)
                                                    .Select(u => new Users
                                                    {
                                                        Id = u.Id,
                                                        EmailAddress = u.EmailAddress,
                                                        YearExperience = u.YearExperience,
                                                        Firstname = u.Firstname,
                                                        Middlename = u.Middlename,
                                                        PhoneNumber = u.PhoneNumber,
                                                        Surname = u.Surname,
                                                        DateCreated = u.DateCreated,
                                                        WorkExperience = u.WorkExperience.Select(w => new WorkExperience
                                                        {
                                                            Id = w.Id,
                                                            Organization = w.Organization,
                                                            JobTitle = w.JobTitle,
                                                            StartDate = w.StartDate,
                                                            EndDate = w.EndDate,
                                                            UsersId = w.UsersId,
                                                            DateCreated = w.DateCreated
                                                        }).ToList(),
                                                        Skills = u.Skills.Select(s => new Skills
                                                        {
                                                            Id = s.Id,
                                                            Skill = s.Skill,
                                                            UsersId = s.UsersId,
                                                            DateCreated = s.DateCreated
                                                        }).ToList(),
                                                        Qualifications = u.Qualifications.Select(q => new Qualifications
                                                        {
                                                            Id = q.Id,
                                                            Qualification = q.Qualification,
                                                            TypeOfQualifiction = q.TypeOfQualifiction,
                                                            UsersId = q.UsersId,
                                                            DateCreated = q.DateCreated
                                                        }).ToList()
                                                    })
                                                    .ToListAsync();

                return cvList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ResponseDto> DeleteCVAsync(string email)
        {
            ResponseDto response = new();

            try
            {
                var executionStrategy = _cVContext.Database.CreateExecutionStrategy();
                var res = await executionStrategy.ExecuteAsync(async () =>
                {
                    using var dbContextTransaction = await _cVContext.Database.BeginTransactionAsync();
                    try
                    {
                        var user = await _cVContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == email.Trim());

                        if (user != null)
                        {
                            _cVContext.Users.Remove(user);
                            await _cVContext.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        await dbContextTransaction.RollbackAsync();
                        throw ex;
                    }
                });
                if (!res)
                {
                    return response = new()
                    {
                        Message = "Record not found, No Record was Deleted",
                        Status = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
                return response = new()
                {
                    Message = "Record Deleted Successfully",
                    Status = HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return response = new()
                    {
                        Message = ex.InnerException.Message,
                        Status = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
                else
                {
                    return response = new()
                    {
                        Message = ex.Message,
                        Status = HttpStatusCode.BadRequest,
                        Success = false
                    };
                }
            }
        }
    }
}
