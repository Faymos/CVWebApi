using AutoMapper;
using CVWebApi.Dtos;
using CVWebApi.Entities;
using CVWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CVWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CVController : ControllerBase
    {
        private readonly ICVServices _cvService;
       

        public CVController(ICVServices cvService)
        {
            _cvService = cvService;
           
        }

        [HttpGet("GetAllCVs")]
        public async Task<ActionResult<List<dynamic>>> GetCVs()
        {
            var cvs = await _cvService.GetCVsAsync();
            if(cvs == null || cvs.Count() <= 0)
            {
                ResponseDto responseDto = new()
                {
                    Message = $"No User was found",
                    Status = HttpStatusCode.NotFound,
                    Success = false
                };

                return NotFound(responseDto);
            }
            return Ok(cvs);
        }
        [HttpGet("GetCvByEmail")]
        public async Task<ActionResult<dynamic>> GetCVByEmail(string email)
        {
            var cv = await _cvService.GetCVByEmailAsync(email);
            if (cv == null)
            {
                ResponseDto responseDto = new()
                {
                    Message = $"No User with the email {email} was found",
                    Status = HttpStatusCode.NotFound,
                    Success = false
                };

                return NotFound(responseDto);
            }
            return Ok(cv);
        }

        [HttpPost("CreateCV")]
        public async Task<ActionResult<dynamic>> AddCV([FromBody] CVDto cv)
        {
            if (cv == null)
            {
                return BadRequest("CV is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _cvService.AddCVAsync(cv);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        
        [HttpGet("GetByExperience")]
        public ActionResult<List<dynamic>> GetByExperience(string JobTitle)
        {
            var cvs = _cvService.GetCVsByExperience(JobTitle);
            if (cvs == null || cvs.Result.Count() <= 0)
            {
                ResponseDto responseDto = new()
                {
                    Message = $"No User with the Experience  {JobTitle} was found",
                    Status = HttpStatusCode.NotFound,
                    Success = false
                };

                return NotFound(responseDto);
            }

            return Ok(cvs);
        }

        [HttpGet("GetBySkill")]
        public  ActionResult<List<dynamic>> GetBySkill(string skill)
        {
            var cvs =  _cvService.GetCVsBySkill(skill);

            if (cvs == null || cvs.Result.Count <= 0)
            {
                ResponseDto responseDto = new()
                {
                    Message = $"No User with the skill {skill} was found",
                    Status = HttpStatusCode.NotFound,
                    Success = false
                };

                return NotFound(responseDto);
            }

            return Ok(cvs);
        }

        [HttpGet("GetByQualification")]
        public ActionResult<List<dynamic>> GetByQualification(string qualification)
        {
            var cvs = _cvService.GetCVsByQualification(qualification);

            if (cvs == null || cvs.Result.Count <= 0)
            {
                ResponseDto responseDto = new()
                {
                    Message = $"No User with the qualification {qualification} was found",
                    Status = HttpStatusCode.NotFound,
                    Success = false
                };

                return NotFound(responseDto);
            }

            return Ok(cvs);
        }

        [HttpPut("UpdateCV")]
       public async Task<ActionResult<dynamic>> UpdateCV([FromBody] CVDto cv)
        {
            if (cv == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _cvService.UpdateCVAsync(cv);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetCvByYearsOfExperienceWithMinimumYears")]
        public async Task<ActionResult<dynamic>> GetCVByYearsOfExperienceMin(int minimumYears)
        {
            var cv = await _cvService.GetCVsByYearOfExperiences(minimumYears);
            if (cv == null || cv.Count() <= 0)
            {
                ResponseDto responseDto= new()
                {
                    Message = $"No Record with the Minimum number of {minimumYears} years was found",
                    Status = HttpStatusCode.NotFound,
                    Success  = false
                };

                return NotFound(responseDto);
            }
            return Ok(cv);
        }
       
        [HttpGet("GetCvByYearsOfExperienceWithMaximumYear")]
        public async Task<ActionResult<dynamic>> GetCVByYearsOfExperienceMax(int maximumYears)
        {
            var cv = await _cvService.GetCVsByYearOfExperiencesMax(maximumYears);
            if (cv == null || cv.Count() <= 0)
            {
                ResponseDto responseDto = new()
                {
                    Message = $"No Record with the Maximun number of {maximumYears} years was found",
                    Status = HttpStatusCode.NotFound,
                    Success = false
                };

                return NotFound(responseDto);
            }
            return Ok(cv);
        }
    }


}
