namespace SIS.Models
{
    public class VmSection
    {
        public SectionModel Section { get; set; }
        public PagedCourseResult Course { get; set; }
        public PagedInstructorResult Instructor { get; set; }

        public VmSection()
        {
            Section = new SectionModel();

            Course = new PagedCourseResult();

            Instructor = new PagedInstructorResult();
        }
    }
}
