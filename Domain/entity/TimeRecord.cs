using System;
using System.Collections.Generic;
using System.Text;

namespace RSaitov.SoftwareDevelop.Domain
{
    public class TimeRecord
    {
        public DateTime Date { get; }
        public string Name { get; }
        public byte Hours { get; }
        public string Description { get; }
        public TimeRecord(DateTime date, string name, byte hours, string description)
        {
            Date = date;
            Name = name;
            Hours = hours;
            Description = description;
        }
    }
}
