using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.ComponentModel;
using Neurotec.Biometrics;

namespace FingerprintMatch.classes
{
    class dlDataHelper
    {
        Thread t;
        worker _worker;
        private bool stopped;

        public dlDataHelper(List<course> Courses)
        {
            stopped = false;
            _worker = new worker(Courses);
        }

        public void StartGetStudents()
        {
            
            t = new Thread(new ThreadStart(_worker.getStudentsForCourse));
            t.Start();
            while (!t.IsAlive) ;
        }

        public void StopGetStudents()
        {
            t.Abort();
            t.Join();
            stopped = true;
        }

        public int TotalNumberOfStudents
        {
            get
            {
                return _worker.TotalStudents;
            }
        }

        public int CurrentStudent
        {
            get
            {
                return _worker.CurrentStudent;
            }
        }

        public bool isCompleted
        {
            get
            {
                return _worker.Completed;
            }
        }

        public bool isStopped
        {
            get
            {
                return stopped;
            }
        }

        public BindingList<student> ListOfStudents
        {
            get
            {
                return _worker.ListOfStudents;
            }
        }

    }

    class worker
    {
        private static string conRegistrationDB = FingerprintMatch.Properties.Settings.Default.RegistrationDBconn;
        private string msg;
        private List<course> _courses;
        private int _totalStudents, _currentStudent;
        private BindingList<student> _listOfStudents;
        private bool _completed;

        public worker(List<course> Courses)
        {
            _totalStudents = 0;
            _currentStudent = 0;
            _courses = Courses;
            
            _listOfStudents = new BindingList<student>();
            _completed = false;
        }

        public void getStudentsForCourse()
        {

            BindingList<student> studentsInCourse = new BindingList<student>();
            DataTable dtStudents = _getStudentNumbersUsingCourse(_courses);
            _totalStudents = dtStudents.Rows.Count;

            foreach (DataRow dr in dtStudents.Rows)
            {
                _listOfStudents.Add(_getStudentFromStudentNumber(dr["Student_number"].ToString()));
                _currentStudent++;
            }

            _completed = true;
        }

        public BindingList<student> ListOfStudents
        {
            get
            {
                return _listOfStudents;
            }
        }

        public bool Completed
        {
            get
            {
                return _completed;
            }
        }

        public int TotalStudents
        {
            get
            {
                return _totalStudents;
            }
        }

        public int CurrentStudent
        {
            get
            {
                return _currentStudent;
            }
        }

        public string WorkerMsg
        {
            get
            {
                return msg;
            }
        }

        private static student _getStudentFromStudentNumber(string StudentNumber)
        {
            SqlConnection conn = new SqlConnection(conRegistrationDB);
            conn.Open();
            string query = "SELECT [student_number],[card_number],[firstname],[lastname],[IDNumber]";
            query = query + ",[LeftPinkieTemplate],[LeftRingTemplate],[LeftMiddleTemplate],[LeftMiddleImage],[LeftIndexTemplate] ,[LeftThumbTemplate]";
            query = query + ",[RightThumbTemplate],[RightIndexTemplate],[RightMiddleTemplate],[RightRingTemplate],[RightPinkieTemplate]";
            query = query + " FROM [RegistrationDB].[dbo].[registration] WHERE (Deleted is null OR Deleted != 'True') AND ([student_number] = '" + StudentNumber + "') ";
            SqlCommand cmd = new SqlCommand(query, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            student _student = new student();
            _student.StudentNumber = StudentNumber;
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                if (dr.IsNull("firstname")) { _student.FirstName = null; } else { _student.FirstName = (string)dr["firstname"]; };
                if (dr.IsNull("lastname")) { _student.LastName = null; } else { _student.LastName = (string)dr["lastname"]; };
                if (dr.IsNull("IDNumber")) { _student.IDNumber = null; } else { _student.IDNumber = (string)dr["IDNumber"]; };
                if (dr.IsNull("card_number")) { _student.CardNumber = null; } else { _student.CardNumber = (string)dr["card_number"]; };
                if (dr.IsNull("student_number")) { _student.StudentNumber = null; } else { _student.StudentNumber = (string)dr["student_number"]; };
                if (dr.IsNull("LeftPinkieTemplate")) { _student.LeftPinkie = null; } else { _student.LeftPinkie = new NFRecord((byte[])dr["LeftPinkieTemplate"]); };
                if (dr.IsNull("LeftRingTemplate")) { _student.LeftRing = null; } else { _student.LeftRing = new NFRecord((byte[])dr["LeftRingTemplate"]); };
                if (dr.IsNull("LeftMiddleTemplate")) { _student.LeftMiddle = null; } else { _student.LeftMiddle = new NFRecord((byte[])dr["LeftMiddleTemplate"]); };
                if (dr.IsNull("LeftIndexTemplate")) { _student.LeftIndex = null; } else { _student.LeftIndex = new NFRecord((byte[])dr["LeftIndexTemplate"]); };
                if (dr.IsNull("LeftThumbTemplate")) { _student.LeftThumb = null; } else { _student.LeftThumb = new NFRecord((byte[])dr["LeftThumbTemplate"]); };
                if (dr.IsNull("RightThumbTemplate")) { _student.RightThumb = null; } else { _student.RightThumb = new NFRecord((byte[])dr["RightThumbTemplate"]); };
                if (dr.IsNull("RightIndexTemplate")) { _student.RightIndex = null; } else { _student.RightIndex = new NFRecord((byte[])dr["RightIndexTemplate"]); };
                if (dr.IsNull("RightMiddleTemplate")) { _student.RightMiddle = null; } else { _student.RightMiddle = new NFRecord((byte[])dr["RightMiddleTemplate"]); };
                if (dr.IsNull("RightRingTemplate")) { _student.RightRing = null; } else { _student.RightRing = new NFRecord((byte[])dr["RightRingTemplate"]); };
                if (dr.IsNull("RightPinkieTemplate")) { _student.RightPinkie = null; } else { _student.RightPinkie = new NFRecord((byte[])dr["RightPinkieTemplate"]); };
                if (dr.IsNull("LeftPinkieTemplate")) { _student.LeftPinkie = null; } else { _student.template[0] = (byte[])dr["LeftPinkieTemplate"]; };
                if (dr.IsNull("LeftRingTemplate")) { _student.LeftRing = null; } else { _student.template[1] = (byte[])dr["LeftRingTemplate"]; };
                if (dr.IsNull("LeftMiddleTemplate")) { _student.LeftMiddle = null; } else { _student.template[2] = (byte[])dr["LeftMiddleTemplate"]; };
                if (dr.IsNull("LeftIndexTemplate")) { _student.LeftIndex = null; } else { _student.template[3] = (byte[])dr["LeftIndexTemplate"]; };
                if (dr.IsNull("LeftThumbTemplate")) { _student.LeftThumb = null; } else { _student.template[4] = (byte[])dr["LeftThumbTemplate"]; };
                if (dr.IsNull("RightThumbTemplate")) { _student.RightThumb = null; } else { _student.template[5] = (byte[])dr["RightThumbTemplate"]; };
                if (dr.IsNull("RightIndexTemplate")) { _student.RightIndex = null; } else { _student.template[6] = (byte[])dr["RightIndexTemplate"]; };
                if (dr.IsNull("RightMiddleTemplate")) { _student.RightMiddle = null; } else { _student.template[7] = (byte[])dr["RightMiddleTemplate"]; };
                if (dr.IsNull("RightRingTemplate")) { _student.RightRing = null; } else { _student.template[8] = (byte[])dr["RightRingTemplate"]; };
                if (dr.IsNull("RightPinkieTemplate")) { _student.RightPinkie = null; } else { _student.template[9] = (byte[])dr["RightPinkieTemplate"]; };


                _student.Enrolled = true;
            }
            else
            {
                _student.StudentNumber = StudentNumber;
                _student.Enrolled = false;
            }
            conn.Close();
            return _student;
        }

        private static DataTable _getStudentNumbersUsingCourse(List<course> Courses)
        {
            SqlConnection conn = new SqlConnection(conRegistrationDB);
            conn.Open();
            string strCourses = "";
            foreach (course thisCourse in Courses)
            {
                strCourses = strCourses + "'" + thisCourse.CourseCode + "',";
            }
            
            strCourses = strCourses.TrimEnd(',');
            

            string query = "SELECT Distinct [Student_number] FROM [RegistrationDB].[dbo].[module_registrations] WHERE Module_code in (" + strCourses + ")";
            //string query = "SELECT DISTINCT [Student_number] FROM [RegistrationDB].[dbo].[module_registrations]";
            SqlCommand cmd = new SqlCommand(query, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }

    }
}
