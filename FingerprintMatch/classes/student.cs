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
        public NFRecord LeftPinkie { get; set; }
        public NFRecord LeftRing { get; set; }
        public NFRecord LeftMiddle { get; set; }
        public NFRecord LeftIndex { get; set; }
        public NFRecord LeftThumb { get; set; }
        public NFRecord RightThumb { get; set; }
        public NFRecord RightIndex { get; set; }
        public NFRecord RightMiddle { get; set; }
        public NFRecord RightRing { get; set; }
        public NFRecord RightPinkie { get; set; }
        public byte[][] template {get; set;}
        public bool Enrolled { get; set; }


        public student()
        {
            byte[][] _template = new byte[10][];
            template = _template;
        }

        public student(string _firstName, string _lastname, string _idNumber, string _studentNumber, string _cardNumber,
            NFRecord _leftPinkie, NFRecord _leftRing, NFRecord _leftMiddle, NFRecord _leftIndex, NFRecord _leftThumb,
            NFRecord _rightThumb, NFRecord _rightIndex, NFRecord _rightMiddle, NFRecord _rightRing, NFRecord _rightPinkie, bool _enrolled)
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
