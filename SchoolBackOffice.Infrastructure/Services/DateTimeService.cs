using System;
using SchoolBackOffice.Application.Common.Interfaces;

namespace SchoolBackOffice.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}