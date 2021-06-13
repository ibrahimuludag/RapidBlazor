using RapidBlazor.Application.Common.Interfaces;
using System;

namespace RapidBlazor.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
