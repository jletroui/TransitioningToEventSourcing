using System;
using System.Linq;
using System.Text;
using Infrastructure;
using nVentive.Umbrella.Extensions;
using nVentive.Umbrella.Validation;
using Iesi.Collections;
using Iesi.Collections.Generic;

namespace Domain
{
    public class Student : AggregateRoot
    {
        private ISet<Registration> registrations;
        private ISet<Guid> passedClasses;
        private ISet<Guid> failedClasses;
        private IdSequence registrationSequence;
        private int credits;
        private string firstName;
        private string lastName;
        private bool hasGraduated;

        /// <summary>
        /// NHibernate constructor.
        /// </summary>
        protected Student() { }

        public Student(Guid id, string firstName, string lastName)
            : base(id)
        {
            CorrectName(firstName, lastName);

            registrations = new HashedSet<Registration>();
            passedClasses = new HashedSet<Guid>();
            failedClasses = new HashedSet<Guid>();
            registrationSequence = new IdSequence(0);
            credits = 0;
            hasGraduated = false;
        }

        public virtual void CorrectName(string firstName, string lastName)
        {
            // Business rules
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > 255)
            {
                throw new ArgumentException("firstName must be a text that have between 1 and 255 characters");
            }
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > 255)
            {
                throw new ArgumentException("firstName must be a text that have between 1 and 255 characters");
            }

            // State changes
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public virtual void RegisterTo(Class @class)
        {
            // Business rules
            @class.Validation().NotNull("class");
            if (registrations.Where(x => x._class.Id == @class.Id).Count() > 0)
            {
                throw new InvalidOperationException("You can not register a student to a class he already registered");
            }
            if (passedClasses.Where(x => x == @class.Id).Count() > 0)
            {
                throw new InvalidOperationException("You can not register a student to a class he already passed");
            }

            // State changes
            registrationSequence = registrationSequence.Next();
            registrations.Add(new Registration(this, registrationSequence.ToId(), @class));
        }

        public virtual void MakePass(int registrationId)
        {
            // Business rules
            Registration registration = registrations.Where(x => x.Id == registrationId).FirstOrDefault();
            if (registration == null)
            {
                throw new InvalidOperationException("Registration not found.");
            }

            // State changes
            registrations.Remove(registration);
            passedClasses.Add(registration._class.Id);
            credits += registration._class.credits;
            if (credits >= 120)
            {
                hasGraduated = true;
            }
        }

        public virtual void MakeFail(int registrationId)
        {
            // Business rules
            Registration registration = registrations.Where(x => x.Id == registrationId).FirstOrDefault();
            if (registration == null)
            {
                throw new InvalidOperationException("The student is not registered in this class");
            }

            // State changes
            registrations.Remove(registration);
            failedClasses.Add(registration._class.Id);
        }
    }
}
