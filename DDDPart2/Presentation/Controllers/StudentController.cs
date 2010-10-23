using System;
using System.Web.Mvc;
using Domain;
using Infrastructure.Web;
using Domain.ViewModel.Queries;
using Infrastructure;
using Domain.ViewModel;

namespace Presentation.Controllers
{
    [HandleError]
    public class StudentController : Controller
    {
        private IRepository<Student> studentRepository;
        private IRepository<Class> classRepository;
        private IStudentDTOQueries studentQueries;
        private IClassDTOQueries classQueries;

        public StudentController(IRepository<Student> studentRepository, 
            IClassDTOQueries classQueries, 
            IStudentDTOQueries studentQueries,
            IRepository<Class> classRepository)
        {
            this.studentRepository = studentRepository;
            this.classQueries = classQueries;
            this.studentQueries = studentQueries;
            this.classRepository = classRepository;
        }

        public ViewResult Index(int page = 1, string name = null)
        {
            return View(studentQueries.ByNameLike(name).AsPagination(page));
        }

        public ViewResult Create()
        {
            return View();
        }

        public RedirectToRouteResult DoCreate(StudentDTO model)
        {
            studentRepository.Add(new Student(model.FirstName, model.LastName));

            return RedirectToRoute(new
            {
                controller="Student",
                action="Index"
            });
        }

        public ViewResult CorrectName(Guid studentId)
        {
            return View(studentQueries.ById(studentId));
        }

        public RedirectToRouteResult DoCorrectName(StudentDTO model)
        {
            var student = studentRepository.ById(model.Id);

            student.CorrectName(model.FirstName, model.LastName);

            return RedirectToRoute(new
            {
                controller = "Student",
                action = "Index"
            });
        }

        public ViewResult RegisterToClass(Guid studentId)
        {
            var model = studentQueries.ByIdForRegistration(studentId);
            return View(model);
        }

        public RedirectToRouteResult DoRegisterToClass(StudentDTO model)
        {
            var student = studentRepository.ById(model.Id);
            var @class = classRepository.ById(model.ClassToRegister);

            student.RegisterTo(@class);

            return RedirectToRoute(new
            {
                controller = "Student",
                action = "Index"
            });
        }

        public ViewResult Details(Guid studentId)
        {
            return View(studentQueries.ById(studentId));    
        }

        public RedirectToRouteResult MakePass(Guid studentId, int registrationId)
        {
            var student = studentRepository.ById(studentId);

            student.MakePass(registrationId);

            return RedirectToRoute(new
            {
                controller = "Student",
                action = "Details",
                studentId = studentId
            });
        }

        public RedirectToRouteResult MakeFail(Guid studentId, int registrationId)
        {
            var student = studentRepository.ById(studentId);

            student.MakeFail(registrationId);

            return RedirectToRoute(new
            {
                controller = "Student",
                action = "Details",
                studentId = studentId
            });
        }
    }
}
