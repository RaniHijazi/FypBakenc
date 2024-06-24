using Fyp.Dto;
using Fyp.Models;

public interface IUniversityRepository
{
    
    Task<Faculty> AddFaculty(string? Description, IFormFile? image);
    Task RemoveFaculty(int facultyId);
    Task<List<FacultyDto>> DisplayFaculties();

    
    Task<Major> AddMajorToFaculty(int facultyId, MajorDto majorDto);
    Task RemoveMajorFromFaculty(int majorId);
    Task<List<GetMajorDto>> DisplayMajorsOfFaculty(int facultyId);

    
    Task<Corse> AddCourseToMajor(int majorId, CorseDto courseDto);
    Task RemoveCourseFromMajor(int courseId);
    Task<List<GetCorseDto>> DisplayCoursesOfMajor(int majorId);
}
