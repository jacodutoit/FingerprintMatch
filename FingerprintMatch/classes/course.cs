using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FingerprintMatch.classes
{
    class course
    {
        public string Description { get; set; }
        public string CourseCode { get; set; }
        public course(string _courseCode, string _description)
        {
            Description = _description;
            CourseCode = _courseCode;
        }
    }
}
