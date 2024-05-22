using Fyp.Dto;
using Fyp.Models;
using Microsoft.EntityFrameworkCore;

namespace Fyp.Repository
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly DataContext _context;

        public UniversityRepository(DataContext context)
        {
            _context = context;
        }

        
        public async Task<Faculty> AddFaculty(FacultyDto facultyDto)
        {
            var faculty = new Faculty
            {
                Name = facultyDto.Name,
                ImgUrl = facultyDto.ImgUrl
            };
            _context.faculties.Add(faculty);
            await _context.SaveChangesAsync();
            return faculty;
        }

        public async Task RemoveFaculty(int facultyId)
        {
            var faculty = await _context.faculties.FindAsync(facultyId);
            if (faculty != null)
            {
                _context.faculties.Remove(faculty);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<FacultyDto>> DisplayFaculties()
        {
            return await _context.faculties
                .Select(f => new FacultyDto
                {
                    Name = f.Name,
                    ImgUrl = f.ImgUrl
                })
                .ToListAsync();
        }

        
        public async Task<Major> AddMajorToFaculty(int facultyId, MajorDto majorDto)
        {
            var major = new Major
            {
                Name = majorDto.Name,
                Department = majorDto.Department,
                Description = majorDto.Description,
                ImgUrl = majorDto.ImgUrl,
                FacultyId = facultyId
            };
            _context.majors.Add(major);
            await _context.SaveChangesAsync();
            return major;
        }

        public async Task RemoveMajorFromFaculty(int majorId)
        {
            var major = await _context.majors.FindAsync(majorId);
            if (major != null)
            {
                _context.majors.Remove(major);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MajorDto>> DisplayMajorsOfFaculty(int facultyId)
        {
            return await _context.majors
                .Where(m => m.FacultyId == facultyId)
                .Select(m => new MajorDto
                {
                    Name = m.Name,
                    Department = m.Department,
                    Description = m.Description,
                    ImgUrl = m.ImgUrl
                })
                .ToListAsync();
        }

        
        public async Task<Corse> AddCourseToMajor(int majorId, CorseDto courseDto)
        {
            var course = new Corse
            {
                Name = courseDto.Name,
                Description = courseDto.Description,
                Credits = courseDto.Credits,
                MajorId = majorId
            };
            _context.corses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task RemoveCourseFromMajor(int courseId)
        {
            var course = await _context.corses.FindAsync(courseId);
            if (course != null)
            {
                _context.corses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CorseDto>> DisplayCoursesOfMajor(int majorId)
        {
            return await _context.corses
                .Where(c => c.MajorId == majorId)
                .Select(c => new CorseDto
                {
                    Name = c.Name,
                    Description = c.Description,
                    Credits = c.Credits
                })
                .ToListAsync();
        }
    }

}
