﻿namespace Timesheets.Entities
{
    public sealed class Employee : Entity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
