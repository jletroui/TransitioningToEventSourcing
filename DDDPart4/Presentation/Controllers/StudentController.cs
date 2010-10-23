using System;
using System.Web.Mvc;
using Infrastructure;
using Infrastructure.Web;
using Domain.ViewModel.Queries;
using Domain.ViewModel;
using Domain.Commands;

namespace Presentation.Controllers
{
    [HandleError]
    public class StudentController : Controller
    {
        private ICommandBus commandBus;
        private IStudentDTOQueries studentQueries;

        public StudentController(ICommandBus commandBus, 
            IStudentDTOQueries studentQueries)
        {
            this.commandBus = commandBus;
            this.studentQueries = studentQueries;
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
            var cmd = new CreateStudentCommand(Guid.NewGuid(), model.FirstName, model.LastName);
            commandBus.Send(cmd);

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
            var cmd = new CorrectStudentNameCommand(model.Id, model.FirstName, model.LastName);
            commandBus.Send(cmd);

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
            var cmd = new RegisterStudentToClassCommand(model.Id, model.ClassToRegister);
            commandBus.Send(cmd);

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
            var cmd = new MakeStudentPassCommand(studentId, registrationId);
            commandBus.Send(cmd);

            return RedirectToRoute(new
            {
                controller = "Student",
                action = "Details",
                studentId = studentId
            });
        }

        public RedirectToRouteResult MakeFail(Guid studentId, int registrationId)
        {
            var cmd = new MakeStudentFailCommand(studentId, registrationId);
            commandBus.Send(cmd);

            return RedirectToRoute(new
            {
                controller = "Student",
                action = "Details",
                studentId = studentId
            });
        }
    }
}
