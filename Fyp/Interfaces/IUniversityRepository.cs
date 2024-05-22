using Fyp.Dto;
using Fyp.Models;

public interface IUniversityRepository
{
    
    Task<Faculty> AddFaculty(FacultyDto facultyDto);
    Task RemoveFaculty(int facultyId);
    Task<List<FacultyDto>> DisplayFaculties();

    
    Task<Major> AddMajorToFaculty(int facultyId, MajorDto majorDto);
    Task RemoveMajorFromFaculty(int majorId);
    Task<List<MajorDto>> DisplayMajorsOfFaculty(int facultyId);

    
    Task<Corse> AddCourseToMajor(int majorId, CorseDto courseDto);
    Task RemoveCourseFromMajor(int courseId);
    Task<List<CorseDto>> DisplayCoursesOfMajor(int majorId);
}
