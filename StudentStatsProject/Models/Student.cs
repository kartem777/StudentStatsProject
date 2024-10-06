namespace StudentStatsProject.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Mark> Marks { get; set; }
        public List<ScheduledLesson> ScheduledLessons { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
