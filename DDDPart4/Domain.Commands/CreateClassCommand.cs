using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Commands
{
    public class CreateClassCommand : ICommand
    {
        public readonly Guid ClassId;
        public readonly string Name;
        public readonly int Credits;

        public CreateClassCommand(Guid classId, string name, int credits)
        {
            ClassId = classId;
            Name = name;
            Credits = credits;
        }
    }
}
