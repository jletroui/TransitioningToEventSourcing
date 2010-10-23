using System;
using System.Web.Mvc;
using Domain.Commands;
using Domain.ViewModel;
using Domain.ViewModel.Queries;
using Infrastructure;
using Infrastructure.Web;

namespace Presentation.Controllers
{
    public class ClassController : Controller
    {
        private IClassDTOQueries classQueries;
        private ICommandBus commandBus;

        public ClassController(IClassDTOQueries classQueries, ICommandBus commandBus)
        {
            this.classQueries = classQueries;
            this.commandBus = commandBus;
        }

        public ViewResult Index(int page=1)
        {
            return View(classQueries.All().AsPagination(page));
        }

        public ViewResult Create()
        {
            return View();
        }

        public RedirectToRouteResult DoCreate(ClassDTO model)
        {
            var cmd = new CreateClassCommand(Guid.NewGuid(), model.Name, model.Credits);
            commandBus.Send(cmd);

            return RedirectToRoute(new
            {
                action = "Index",
                controller = "Class"
            });
        }
    }
}
