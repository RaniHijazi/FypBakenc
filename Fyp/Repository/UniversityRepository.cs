using Fyp.Dto;
using Fyp.Models;
using Microsoft.EntityFrameworkCore;

namespace Fyp.Repository
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly DataContext _context;
        private readonly BlobStorageService _blobStorageService;

        public UniversityRepository(DataContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        
        public async Task<Faculty> AddFaculty(string name, IFormFile image)
        {
            string imageUrl;
            

                imageUrl = await _blobStorageService.UploadImageAsync(image);
            
            var faculty = new Faculty
            {
                Name = name,
                ImgUrl = imageUrl,
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
                    id=f.Id,
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
                Description = majorDto.Details,
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

        public async Task<List<GetMajorDto>> DisplayMajorsOfFaculty(int facultyId)
        {
            return await _context.majors
                .Where(m => m.FacultyId == facultyId)
                .Select(m => new GetMajorDto
                {
                    id=m.Id,
                    Name = m.Name,
                    Department = m.Department,
                    Details = m.Description,
                    ImgUrl = m.ImgUrl
                })
                .ToListAsync();
        }

        
        public async Task<Corse> AddCourseToMajor(int majorId, CorseDto courseDto)
        {
            var course = new Corse
            {
                Name = courseDto.Name,
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

        public async Task<List<GetCorseDto>> DisplayCoursesOfMajor(int majorId)
        {
            return await _context.corses
                .Where(c => c.MajorId == majorId)
                .Select(c => new GetCorseDto
                {
                    Name = c.Name,
                    id= c.Id,
                    Credits = c.Credits
                })
                .ToListAsync();
        }
    }

}
