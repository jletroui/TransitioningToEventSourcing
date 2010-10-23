using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Web;
using Domain.Repositories;
using Presentation.Models;
using Domain;

namespace Presentation.Controllers
{
    public class ClassController : Controller
    {
        private IClassRepository classRepository;

        public ClassController(IClassRepository classRepository)
        {
            this.classRepository = classRepository;
        }

        public ViewResult Index(int page=1)
        {
            return View(classRepository.All().AsPagination(page));
        }

        public ViewResult Create()
        {
            return View();
        }

        public RedirectToRouteResult DoCreate(ClassModel model)
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
