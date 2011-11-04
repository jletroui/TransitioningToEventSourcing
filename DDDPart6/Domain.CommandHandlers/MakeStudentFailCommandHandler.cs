using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Commands;
using Infrastructure;

namespace Domain.CommandHandlers
{
    public class MakeStudentFailCommandHandler : IHandleCommand<MakeStudentFailCommand>
    {
        private IRepository<Student> studentRepository;

        public MakeStudentFailCommandHandler(IRepository<Student> studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public void Handle(MakeStudentFailCommand cmd)
        {
            var student = studentRepository.ById(cmd.StudentId);

            if (student != null)
            {
                student.MakeFail(cmd.ClassRegistrationId);
            }
        }    
    }
}
