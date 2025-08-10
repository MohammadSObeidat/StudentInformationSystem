using Microsoft.EntityFrameworkCore;
using SIS.Data;
using SIS.Models;

namespace SIS.Services
{
    public class SectionService : ISectionService
    {
        SisContext sisContext;
        public SectionService(SisContext sisContext)
        {
            this.sisContext = sisContext;
        }

        public void CreateSection(VmSection vmSection)
        {
            Section section = new Section();

            section.SectionNumber = vmSection.Section.SectionNumber;
            section.MaxCapacity = vmSection.Section.MaxCapacity;
            section.CourseId = vmSection.Section.CourseId;
            section.InstructorId = vmSection.Section.InstructorId;

            sisContext.Section.Add(section);

            sisContext.SaveChanges();
        }

        public PagedSectionResult GetAllSections(string courseName, int page)
        {
            //List<Section> sections = sisContext.Section.ToList();

            //List<SectionModel> sectionModels = new List<SectionModel>();

            //foreach (Section section in sections)
            //{
            //    SectionModel sectionModel = new SectionModel();
            //    sectionModel.SectionNumber = section.SectionNumber;
            //    sectionModel.MaxCapacity = section.MaxCapacity;
            //    sectionModel.CourseId = section.CourseId;
            //    sectionModel.InstructorId = section.InstructorId;

            //    sectionModels.Add(sectionModel);
            //}

            //List<SectionModel> sections = sisContext.Section.Select(s => new SectionModel
            //{
            //    Id = s.Id,
            //    SectionNumber = s.SectionNumber,
            //    MaxCapacity = s.MaxCapacity,
            //    CourseName = s.Course.Name,
            //    InstructorName = s.Instructor.FirstName + " " + s.Instructor.LastName

            //}).ToList();

            IQueryable<Section> query = sisContext.Section;

            // Search
            if (!string.IsNullOrEmpty(courseName))
            {
                query = query.Where(s => s.Course.Name.Contains(courseName));
            }

            // Pagination
            int pageSize = 10;
            int currentPage = page > 0 ? page : 1;
            int skip = (currentPage - 1) * pageSize;

            int totalStudents = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            query = query.Skip(skip).Take(pageSize);

            List<SectionModel> sections = query.Select(s => new SectionModel
            {
                Id = s.Id,
                SectionNumber = s.SectionNumber,
                MaxCapacity = s.MaxCapacity,
                CourseName = s.Course.Name,
                InstructorName = s.Instructor.FirstName + " " + s.Instructor.LastName

            }).ToList();

            return new PagedSectionResult
            {
                Sections = sections,
                TotalPages = totalPages,
                CurrentPage = currentPage,
            };
        }

        public SectionModel GetSectionById(int id)
        {
            Section section = sisContext.Section.Find(id);

            SectionModel sectionModel = new SectionModel();
            sectionModel.Id = section.Id;
            sectionModel.SectionNumber = section.SectionNumber;
            sectionModel.MaxCapacity = section.MaxCapacity;
            sectionModel.CourseId = section.CourseId;
            sectionModel.InstructorId= section.InstructorId;

            return sectionModel;
        }

        public void UpdateSection(VmSection vmSection)
        {
            Section section = sisContext.Section.Find(vmSection.Section.Id);

            section.SectionNumber = vmSection.Section.SectionNumber;
            section.MaxCapacity = vmSection.Section.MaxCapacity;
            section.CourseId = vmSection.Section.CourseId;
            section.InstructorId = vmSection.Section.InstructorId;

            sisContext.SaveChanges();
        }
        public void DeleteSection(int id)
        {
            Section section = sisContext.Section.Find(id);

            sisContext.Section.Remove(section);

            sisContext.SaveChanges();
        }

        public List<SectionModel> GetSectionsByCourseId(int id)
        {
            List<Section> sections = sisContext.Section.Include(s => s.Instructor).Where(s => s.CourseId == id).ToList();

            List<SectionModel> sectionModels = new List<SectionModel>();

            foreach (Section section in sections)
            {
                SectionModel sectionModel = new SectionModel();
                sectionModel.Id = section.Id;
                sectionModel.SectionNumber = section.SectionNumber;
                sectionModel.MaxCapacity = section.MaxCapacity;
                sectionModel.InstructorName = section.Instructor.FirstName + " " + section.Instructor.LastName;

                sectionModels.Add(sectionModel);
            }

            return sectionModels;
        }
    }
}
