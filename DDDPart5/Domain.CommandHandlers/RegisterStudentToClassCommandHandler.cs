using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Commands;
using Infrastructure;

namespace Domain.CommandHandlers
{
    public class RegisterStudentToClassCommandHandler : IHandleCommand<RegisterStudentToClassCommand>
    {
        private IRepository<Student> studentRepository;
        private IRepository<Class> classRepository;

        public RegisterStudentToClassCommandHandler(IRepository<Student> studentRepository, IRepository<Class> classRepository)
        {
            this.studentRepository = studentRepository;
            this.classRepository = classRepository;
        }

        public void Handle(RegisterStudentToClassCommand cmd)
        {
            var student = studentRepository.ById(cmd.StudentId);
            var @class = classRepository.ById(cmd.ClassId);

            if (student != null && @class != null)
            {
                student.RegisterTo(@class);
            }
        }    
    }
}
