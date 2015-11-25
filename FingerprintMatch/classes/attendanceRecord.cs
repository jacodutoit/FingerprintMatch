using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FingerprintMatch.classes
{
    class attendanceRecord
    {
        public string AuditNumber { get; set; }
        public DateTime ClassTime { get; set; }
        public string CourseCode { get; set; }
        public string StudentNumber { get; set; }
        public string CardNumber { get; set; }
        public int FingerAID { get; set; }
        public int FingerBID { get; set; }
        public byte[] FingerAImage { get; set; }
        public byte[] FingerBImage { get; set; }
        public DateTime TimeStamp { get; set; }
        public int score1 { get; set; }
        public int score2 { get; set; }
        public string ScanResult { get; set; }
        public byte[] FingerATemplate { get; set; }
        public byte[] FingerBTemplate { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }


    }
}
