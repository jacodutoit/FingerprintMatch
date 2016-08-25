using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neurotec;
using Neurotec.Biometrics;

namespace FingerprintMatch.classes
{
    class student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public string StudentNumber { get; set; }
        public string CardNumber { get; set; }
        public string CardHistory { get; set; }
        public NTemplate LeftPinkie { get; set; }
        public NTemplate LeftRing { get; set; }
        public NTemplate LeftMiddle { get; set; }
        public NTemplate LeftIndex { get; set; }
        public NTemplate LeftThumb { get; set; }
        public NTemplate RightThumb { get; set; }
        public NTemplate RightIndex { get; set; }
        public NTemplate RightMiddle { get; set; }
        public NTemplate RightRing { get; set; }
        public NTemplate RightPinkie { get; set; }
        public byte[][] template {get; set;}
        public bool Enrolled { get; set; }


        public student()
        {
            byte[][] _template = new byte[10][];
            template = _template;
        }

        public student(string _firstName, string _lastname, string _idNumber, string _studentNumber, string _cardNumber,
            NTemplate _leftPinkie, NTemplate _leftRing, NTemplate _leftMiddle, NTemplate _leftIndex, NTemplate _leftThumb,
            NTemplate _rightThumb, NTemplate _rightIndex, NTemplate _rightMiddle, NTemplate _rightRing, NTemplate _rightPinkie, bool _enrolled)
        {
            FirstName = _firstName;
            LastName = _lastname;
            IDNumber = _idNumber;
            StudentNumber = _studentNumber;
            CardNumber = _cardNumber;
            LeftPinkie = _leftPinkie;
            LeftRing = _leftRing;
            LeftMiddle = _leftMiddle;
            LeftIndex = _leftIndex;
            LeftThumb = _leftThumb;
            RightThumb = _rightThumb;
            RightIndex = _rightIndex;
            RightMiddle = _rightIndex;
            RightRing = _rightRing;
            RightPinkie = _rightPinkie;
            Enrolled = _enrolled;
        }


    }
}
