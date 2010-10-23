using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Domain.Commands;

namespace Domain.CommandHandlers
{
    public class CreateStudentCommandHandler : IHandleCommand<CreateStudentCommand>
    {
        private IRepository<Student> studentRepository;

        public CreateStudentCommandHandler(IRepository<Student> studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public void Handle(CreateStudentCommand cmd)
        {
            var newStudent = new Student(cmd.StudentId, cmd.FirstName, cmd.LastName);
            studentRepository.Add(newStudent);
        }
    }
}
