using SIS.Models;

namespace SIS.Services
{
    public interface ISectionService
    {
        void CreateSection(VmSection vmSection);
        PagedSectionResult GetAllSections(string courseName, int page);
        SectionModel GetSectionById(int id);
        void UpdateSection(VmSection vmSection);
        public void DeleteSection(int id);

        public List<SectionModel> GetSectionsByCourseId(int id);
    }
}