using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentStatsProject.Models;
using System.Security.Claims;
namespace StudentStatsProject.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        public HomeController(ApplicationContext db)
        {
            this.db = db;
            if (!db.Students.Any())
            {
                List<Mark> s1 = new List<Mark>
                {
                    new Mark() { Grade = 100, Date = DateTime.Now, Lesson = "C#" },
                    new Mark() { Grade = 70, Date = DateTime.Now, Lesson = "C++" },
                    new Mark() { Grade = 50, Date = DateTime.Now, Lesson = "C++" }
                };

                List<Mark> s2 = new List<Mark>
                {
                    new Mark() { Grade = 70, Date = DateTime.Now, Lesson = "C++" },
                    new Mark() { Grade = 50, Date = DateTime.Now, Lesson = "C++" },
                    new Mark() { Grade = 20, Date = DateTime.Now, Lesson = "C#" }
                };

                List<Mark> s3 = new List<Mark>
                {
                    new Mark() { Grade = 50, Date = DateTime.Now, Lesson = "C++" },
                    new Mark() { Grade = 20, Date = DateTime.Now, Lesson = "C#" },
                    new Mark() { Grade = 5, Date = DateTime.Now, Lesson = "C#" }
                };

                List<ScheduledLesson> l1 = new List<ScheduledLesson>
                { 
                    new ScheduledLesson() {Name = "C#", StartTime=DateTime.Now.AddDays(1), EndTime=DateTime.Now.AddDays(1).AddMinutes(90), TeacherName = "Taras", Classroom = "online"},
                    new ScheduledLesson() {Name = "C++", StartTime=DateTime.Now.AddDays(2), EndTime=DateTime.Now.AddDays(2).AddMinutes(90), TeacherName = "xxx", Classroom = "online"},
                    new ScheduledLesson() {Name = "Java", StartTime=DateTime.Now.AddDays(3), EndTime=DateTime.Now.AddDays(3).AddMinutes(90), TeacherName = "yyy", Classroom = "online"},
                    new ScheduledLesson() {Name = "C++", StartTime=DateTime.Now.AddDays(4), EndTime=DateTime.Now.AddDays(4).AddMinutes(90), TeacherName = "xxx", Classroom = "online"},
                    new ScheduledLesson() {Name = "C#", StartTime=DateTime.Now.AddDays(5), EndTime=DateTime.Now.AddDays(5).AddMinutes(90), TeacherName = "Taras", Classroom = "online"},
                    new ScheduledLesson() {Name = "SQL", StartTime=DateTime.Now.AddDays(6), EndTime=DateTime.Now.AddDays(6).AddMinutes(90), TeacherName = "Taras", Classroom = "online"}
                };

                List<ScheduledLesson> l2 = new List<ScheduledLesson>
                {
                    new ScheduledLesson() {Name = "C#", StartTime=DateTime.Now.AddDays(1), EndTime=DateTime.Now.AddDays(1).AddMinutes(90), TeacherName = "Taras", Classroom = "online"},
                    new ScheduledLesson() {Name = "C++", StartTime=DateTime.Now.AddDays(3), EndTime=DateTime.Now.AddDays(3).AddMinutes(90), TeacherName = "xxx", Classroom = "online"}
                };
                Student student = new Student() { Name = "Artem", Age = 17, Login = "kartem7", Marks = s1, ScheduledLessons = l1, Password = "12345" };
                Student student2 = new Student() { Name = "Someone", Age = 17, Login = "some_one", Marks = s2, ScheduledLessons = l2, Password = "1234" };
                Student student3 = new Student() { Name = "Noname", Age = 17, Login = "noName", Marks = s3, ScheduledLessons = l2, Password = "78901" };
                db.Students.AddRange(student, student2, student3);
                db.SaveChanges();
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var student = db.Students.FirstOrDefault(s => s.Login == login && s.Password == password);

            if (student != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, student.Name),
                new Claim("StudentId", student.Id.ToString())
            };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Schedule", "Home", new { studentId = student.Id });
            }

            ModelState.AddModelError("", "Invalid login or password");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index");
        }
        public IActionResult Schedule(int studentId)
        {
            var student = db.Students.Include(s => s.ScheduledLessons).FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public IActionResult LessonInfo(int studentId, int lessonId)
        {
            var student = db.Students
                .Include(s => s.ScheduledLessons)
                .FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound();
            }

            var lesson = student.ScheduledLessons.FirstOrDefault(l => l.Id == lessonId);
            if (lesson == null)
            {
                return NotFound();
            }

            ViewBag.Lesson = lesson;
            return View(student);
        }

        public IActionResult Gradebook(int studentId)
        {
            var student = db.Students.Include(s => s.Marks).FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
    }
}
