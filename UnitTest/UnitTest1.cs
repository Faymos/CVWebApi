
using Xunit;
using Moq;
using System.Threading.Tasks;
using CVWebApi.Repository;
using Microsoft.EntityFrameworkCore;
using CVWebApi.Entities;
using AutoMapper;
using CVWebApi.Mapper;
using CVWebApi.Dtos;
using System.Net;
using CVWebApi.Services;

namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task AddCVAsync_Should_Create_CV_Successfully()
        {
            // Arrange
            var cvContextMock = new Mock<CVContext>();
            var usersDbSetMock = new Mock<DbSet<Users>>();
            var qualificationsDbSetMock = new Mock<DbSet<Qualifications>>();
            var skillsDbSetMock = new Mock<DbSet<Skills>>();
            var workExperienceDbSetMock = new Mock<DbSet<WorkExperience>>();

            cvContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);
            cvContextMock.Setup(x => x.Qualifications).Returns(qualificationsDbSetMock.Object);
            cvContextMock.Setup(x => x.Skills).Returns(skillsDbSetMock.Object);
            cvContextMock.Setup(x => x.WorkExperience).Returns(workExperienceDbSetMock.Object);

            var cvService = new CVRepo(cvContextMock.Object, null);

            var cv = new CVDto
            {
                EmailAddress = "test@test.com",
                YearExperience = 2,
                Firstname = "John",
                Middlename = "Doe",
                PhoneNumber = "1234567890",
                Surname = "Doe",
                WorkExperience = new List<WorkExperienceDto>
            {
                new WorkExperienceDto
                {
                    Organization = "Test Organization",
                    JobTitle = "Test Job",
                    StartDate = new DateTime(2021, 01, 01),
                    EndDate = new DateTime(2023, 04, 26)
                }
            },
                Skills = new List<SkillsDto>
            {
                new SkillsDto
                {
                    Skill = "Test Skill"
                }
            },
                Qualifications = new List<QualificationsDto>
            {
                new QualificationsDto
                {
                    Qualification = "Test Qualification",
                    TypeOfQualifiction = "Test Type"
                }
            }
            };

            // Act
            var result = await cvService.AddCVAsync(cv);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(HttpStatusCode.OK, result.Status);
            Assert.Equal("CV Created Successfully", result.Message);
        }
    }

}