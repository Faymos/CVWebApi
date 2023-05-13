
using CVWebApi.Dtos;
using CVWebApi.Services;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(typeof(List<CVDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
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
        [ProducesResponseType(typeof(CVDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
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
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
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
        [ProducesResponseType(typeof(List<CVDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
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
        [ProducesResponseType(typeof(List<CVDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
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
        [ProducesResponseType(typeof(List<CVDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
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
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
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
        [ProducesResponseType(typeof(List<CVDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
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
        [ProducesResponseType(typeof(List<CVDto>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
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

        //[HttpDelete("DeleteRecord")]
        //public async Task<ActionResult<dynamic>> DeleteRecord(string EmailAddress)
        //{
        //    return _cvService.DeleteCVAsync(EmailAddress);
        //}
    }


}
