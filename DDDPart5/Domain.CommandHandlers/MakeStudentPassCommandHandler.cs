using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Commands;
using Infrastructure;

namespace Domain.CommandHandlers
{
    public class MakeStudentPassCommandHandler : IHandleCommand<MakeStudentPassCommand>
    {
        private IRepository<Student> studentRepository;

        public MakeStudentPassCommandHandler(IRepository<Student> studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public void Handle(MakeStudentPassCommand cmd)
        {
            var student = studentRepository.ById(cmd.StudentId);

            if (student != null)
            {
                student.MakePass(cmd.ClassRegistrationId);
            }
        }    
    }
}
