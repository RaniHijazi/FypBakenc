using Fyp.Dto;
using Fyp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fyp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public UniversityController(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        [HttpPost("faculties")]
        public async Task<ActionResult<Faculty>> AddFaculty([FromForm] SaveUrlRequest2 request3)
        {
            var faculty = await _universityRepository.AddFaculty(request3.Description, request3.Image);
            return Ok(faculty);
        }

        [HttpDelete("faculties/{facultyId}")]
        public async Task<IActionResult> RemoveFaculty(int facultyId)
        {
            await _universityRepository.RemoveFaculty(facultyId);
            return NoContent();
        }

        [HttpGet("faculties")]
        public async Task<ActionResult<List<FacultyDto>>> DisplayFaculties()
        {
            var faculties = await _universityRepository.DisplayFaculties();
            return Ok(faculties);
        }

        [HttpPost("faculties/{facultyId}/majors")]
        public async Task<ActionResult<Major>> AddMajorToFaculty(int facultyId, [FromBody] MajorDto majorDto)
        {
            var major = await _universityRepository.AddMajorToFaculty(facultyId, majorDto);
            return Ok(major);
        }

        [HttpDelete("majors/{majorId}")]
        public async Task<IActionResult> RemoveMajorFromFaculty(int majorId)
        {
            await _universityRepository.RemoveMajorFromFaculty(majorId);
            return NoContent();
        }

        [HttpGet("faculties/{facultyId}/majors")]
        public async Task<ActionResult<List<MajorDto>>> DisplayMajorsOfFaculty(int facultyId)
        {
            var majors = await _universityRepository.DisplayMajorsOfFaculty(facultyId);
            return Ok(majors);
        }

        [HttpPost("majors/{majorId}/courses")]
        public async Task<ActionResult<Corse>> AddCourseToMajor(int majorId, [FromBody] CorseDto courseDto)
        {
            var course = await _universityRepository.AddCourseToMajor(majorId, courseDto);
            return Ok(course);
        }

        [HttpDelete("courses/{courseId}")]
        public async Task<IActionResult> RemoveCourseFromMajor(int courseId)
        {
            await _universityRepository.RemoveCourseFromMajor(courseId);
            return NoContent();
        }

        [HttpGet("majors/{majorId}/courses")]
        public async Task<ActionResult<List<CorseDto>>> DisplayCoursesOfMajor(int majorId)
        {
            var courses = await _universityRepository.DisplayCoursesOfMajor(majorId);
            return Ok(courses);
        }
    }
}
public class SaveUrlRequest3
{
    public IFormFile Image { get; set; }
    
    public string Description { get; set; }

}