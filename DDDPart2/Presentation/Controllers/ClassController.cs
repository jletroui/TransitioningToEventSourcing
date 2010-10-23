using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Web;
using Domain;
using Domain.ViewModel.Queries;
using Infrastructure;
using Domain.ViewModel;

namespace Presentation.Controllers
{
    public class ClassController : Controller
    {
        private IClassDTOQueries classQueries;
        private IRepository<Class> classRepository;

        public ClassController(IClassDTOQueries classQueries, IRepository<Class> classRepository)
        {
            this.classQueries = classQueries;
            this.classRepository = classRepository;
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
            classRepository.Add(new Class(model.Name, model.Credits));

            return RedirectToRoute(new
            {
                action = "Index",
                controller = "Class"
            });
        }
    }
}
