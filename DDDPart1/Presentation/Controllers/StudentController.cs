using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Repositories;
using Domain;
using Infrastructure.Web;
using Presentation.Models;

namespace Presentation.Controllers
{
    [HandleError]
    public class StudentController : Controller
    {
        private IStudentRepository studentRepository;
        private IClassRepository classRepository;

        public StudentController(IStudentRepository studentRepository, IClassRepository classRepository)
        {
            this.studentRepository = studentRepository;
            this.classRepository = classRepository;
        }

        public ViewResult Index(int Page = 1, string Name = null)
        {
            var model = new StudentSearchModel()
            {
                Name = Name,
                Students = studentRepository.ByNameLike(Name).AsPagination(Page)
            };

            return View(model);
        }

        public ViewResult Create()
        {
            return View();
        }

        public RedirectToRouteResult DoCreate(StudentModel model)
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
            var student = studentRepository.ById(studentId);

            var model = new StudentModel()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName
            };

            return View(model);
        }

        public RedirectToRouteResult DoCorrectName(StudentModel model)
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
            var student = studentRepository.ById(studentId);

            var model = new RegisterToClassModel()
            {
                StudentId = studentId,
                StudentName = string.Format("{0} {1}", student.FirstName, student.LastName),
                Classes = classRepository.All().ToEnumerable()
            };

            return View(model);
        }

        public RedirectToRouteResult DoRegisterToClass(RegisterToClassModel model)
        {
            var student = studentRepository.ById(model.StudentId);
            var @class = classRepository.ById(model.ClassId);

            student.RegisterTo(@class);

            return RedirectToRoute(new
            {
                controller = "Student",
                action = "Index"
            });
        }

        public ViewResult Details(Guid studentId)
        {
            var student = studentRepository.ById(studentId);

            return View(student);
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
