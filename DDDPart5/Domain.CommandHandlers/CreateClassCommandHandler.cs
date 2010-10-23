using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Commands;
using Infrastructure;

namespace Domain.CommandHandlers
{
    public class CreateClassCommandHandler : IHandleCommand<CreateClassCommand>
    {
        private IRepository<Class> classRepository;

        public CreateClassCommandHandler(IRepository<Class> classRepository)
        {
            this.classRepository = classRepository;
        }

        public void Handle(CreateClassCommand cmd)
        {
            var newClass = new Class(cmd.ClassId, cmd.Name, cmd.Credits);
            classRepository.Add(newClass);
        }
    }
}
