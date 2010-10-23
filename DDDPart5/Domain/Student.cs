using System;
using System.Linq;
using System.Text;
using Infrastructure;
using nVentive.Umbrella.Extensions;
using nVentive.Umbrella.Validation;
using Iesi.Collections;
using Iesi.Collections.Generic;
using Domain.Events;

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

            registrations = new HashedSet<Registration>();
            passedClasses = new HashedSet<Guid>();
            failedClasses = new HashedSet<Guid>();

            ApplyEvent(new StudentCreatedEvent(Id));
            CorrectName(firstName, lastName);
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
            ApplyEvent(new StudentNameCorrectedEvent(Id, firstName, lastName));
        }

        public virtual void RegisterTo(Class @class)
        {
            // Business rules
            @class.Validation().NotNull("class");
            if (registrations.Where(x => x.ClassId == @class.Id).Count() > 0)
            {
                throw new InvalidOperationException("You can not register a student to a class he already registered");
            }
            if (passedClasses.Where(x => x == @class.Id).Count() > 0)
            {
                throw new InvalidOperationException("You can not register a student to a class he already passed");
            }

            // State changes
            ApplyEvent(new StudentRegisteredToClassEvent(Id, @class.Id, registrationSequence.Next().ToId(), @class.credits));
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
            ApplyEvent(new StudentPassedEvent(Id, registrationId));
            ApplyEvent(new StudentCreditsChangedEvent(Id, credits += registration.ClassCredits));
            if (credits >= 120)
            {
                ApplyEvent(new StudentGraduatedEvent(Id));
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
            ApplyEvent(new StudentFailedEvent(Id, registrationId));
        }

        private void Apply(StudentCreatedEvent evt)
        {
            // Only set defaults here.
            registrationSequence = new IdSequence(0);
            hasGraduated = false;
            credits = 0;
        }

        private void Apply(StudentNameCorrectedEvent evt)
        {
            this.firstName = evt.FirstName;
            this.lastName = evt.LastName;
        }

        private void Apply(StudentRegisteredToClassEvent evt)
        {
            registrationSequence = registrationSequence.Next();
            registrations.Add(new Registration(this, evt.RegistrationId, evt.ClassId, evt.Credits));
        }

        private void Apply(StudentCreditsChangedEvent evt)
        {
            credits = evt.Credits;
        }

        private void Apply(StudentPassedEvent evt)
        {
            Registration registration = registrations.Where(x => x.Id == evt.ClassRegistrationId).FirstOrDefault();
            registrations.Remove(registration);
            passedClasses.Add(registration.ClassId);
        }

        private void Apply(StudentFailedEvent evt)
        {
            Registration registration = registrations.Where(x => x.Id == evt.ClassRegistrationId).FirstOrDefault();
            registrations.Remove(registration);
            failedClasses.Add(registration.ClassId);
        }

        private void Apply(StudentGraduatedEvent evt)
        {
            hasGraduated = true;
        }
    }
}
