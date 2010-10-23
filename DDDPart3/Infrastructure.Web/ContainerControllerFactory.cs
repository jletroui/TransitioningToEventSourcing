using System;
using System.Web.Mvc;

using System.Web.Routing;

namespace Infrastructure.Web
{
    /// <summary>
    /// Builds controllers using the current container.
    /// </summary>
    public class ContainerControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            IController resVal = null;

            if (controllerType != null)
            {
                resVal = CurrentContainer.Container.Build<IController>(controllerType);
            }
            else
            {
                resVal = base.GetControllerInstance(requestContext, controllerType);
            }

            return resVal;
        }

    }
}
