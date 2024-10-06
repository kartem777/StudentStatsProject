namespace StudentStatsProject.Models
{
    public class ScheduledLesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TeacherName { get; set; }
        public string Classroom { get; set; }
    }
}
