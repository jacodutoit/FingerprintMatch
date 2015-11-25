using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FingerprintMatch.classes
{
    class improEntity
    {
        private string pvtFirstName;
        private string pvtLastName;
        private string pvtNumber;
        private byte[] pvtTemplate1;
        private byte[] pvtTemplate2;
        private string pvtCardNumber;

        public string FirstName
        {
            get
            {
                return pvtFirstName;
            }

            set
            {
                pvtFirstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return pvtLastName;
            }

            set
            {
                pvtLastName = value;
            }
        }

        public string Number
        {
            get
            {
                return pvtNumber;
            }

            set
            {
                pvtNumber = value;
            }
        }

        public byte[] Template1
        {
            get
            {
                return pvtTemplate1;
            }

            set
            {
                pvtTemplate1 = value;
            }
        }

        public byte[] Template2
        {
            get
            {
                return pvtTemplate2;
            }

            set
            {
                pvtTemplate2 = value;
            }
        }

        public string CardNumber
        {
            get
            {
                return pvtCardNumber;
            }

            set
            {
                pvtCardNumber = value;
            }
        }

        public byte[][] template { get; set; }

        public improEntity()
        {
            byte[][] _template = new byte[2][];
            template = _template;
        }
    }
}
