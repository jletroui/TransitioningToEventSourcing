using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Commands;
using Infrastructure;

namespace Domain.CommandHandlers
{
    public class CorrectStudentNameCommandHandler : IHandleCommand<CorrectStudentNameCommand>
    {
        private IRepository<Student> studentRepository;

        public CorrectStudentNameCommandHandler(IRepository<Student> studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public void Handle(CorrectStudentNameCommand cmd)
        {
            var student = studentRepository.ById(cmd.StudentId);

            if (student != null)
            {
                student.CorrectName(cmd.FirstName, cmd.LastName);
            }
        }    
    }
}
