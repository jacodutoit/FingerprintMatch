using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.ComponentModel;
using Neurotec.Biometrics;

namespace FingerprintMatch.classes
{
    class dlRegistration
    {
        
        private static string conRegistrationDB = FingerprintMatch.Properties.Settings.Default.RegistrationDBconn;
        
        public static BindingList<course> getClasses()
        {
            BindingList<course> courses = new BindingList<course>();
            DataTable dtCourses = _getDBCourses();

            foreach (DataRow dr in dtCourses.Rows)
            {
                course tempCourse = new course(dr["Module_code"].ToString(), dr["Module_name"].ToString());
                
                courses.Add(tempCourse);
            }
            return courses;
        }

        public static BindingList<student> getStudentsForCourse(string courseCode)
        {

            BindingList<student> studentsInCourse = new BindingList<student>();
            DataTable dtStudents = _getStudentNumbersUsingCourse(courseCode);
            
            foreach (DataRow dr in dtStudents.Rows)
            {
                studentsInCourse.Add(_getStudentFromStudentNumber(dr["Student_number"].ToString()));
            }
            return studentsInCourse;

        }

        public static student GetStudentFromStudentNumber(string StudentNumber)
        {
            return _getStudentFromStudentNumber(StudentNumber);
        }

        public static List<course> getCoursesForStudentNumber(string StudentNumber)
        {
            return _GetCoursesForStudentNumber(StudentNumber);
        }

        public static BindingList<string> getRegisteredStudents()
        {
            BindingList<string> validStudents = new BindingList<string>();
            SqlConnection conn = new SqlConnection(conRegistrationDB);
            conn.Open();
            string query = "SELECT DISTINCT [Student_number] FROM [RegistrationDB].[dbo].[module_registrations] ORDER BY Student_number";
            SqlCommand cmd = new SqlCommand(query, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            foreach (DataRow dr in dt.Rows)
            {
                validStudents.Add(dr["Student_number"].ToString());
            }
            conn.Close();
            return validStudents;
        }

        private static List<course> _GetCoursesForStudentNumber(string StudentNumber)
        {
            List<course> courses = new List<course>();
            SqlConnection conn = new SqlConnection(conRegistrationDB);
            conn.Open();
            string query = "SELECT [Module_code],[Module_name] FROM [RegistrationDB].[dbo].[module_registrations] WHERE Student_number = '" + StudentNumber + "' ORDER BY Module_code ";
            SqlCommand cmd = new SqlCommand(query, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow dr in dt.Rows)
            {
                course newCourse = new course((string)dr["Module_code"], (string)dr["Module_name"]);
                courses.Add(newCourse);
            }
            conn.Close();

            return courses;
        }

        private static DataTable _getDBCourses()
        {
            SqlConnection conn = new SqlConnection(conRegistrationDB);
            conn.Open();
            string query = "SELECT distinct [Module_code],[Module_name] FROM [RegistrationDB].[dbo].[module_registrations] ORDER BY Module_code";
            //string query = "uspGetUniqueCourseList";
            SqlCommand cmd = new SqlCommand(query, conn);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 60;
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }

        private static DataTable _getStudentNumbersUsingCourse(string CourseCode)
        {
            SqlConnection conn = new SqlConnection(conRegistrationDB);
            conn.Open();
            string query = "SELECT Distinct [Student_number] FROM [RegistrationDB].[dbo].[module_registrations] WHERE Module_code = '" + CourseCode + "'";
            //string query = "SELECT DISTINCT [Student_number] FROM [RegistrationDB].[dbo].[module_registrations]";
            SqlCommand cmd = new SqlCommand(query, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }

        private static BindingList<student> _getStudentsFromListOfStudentNumbers(List<string> lstStudentNumbers)
        {
            BindingList<student> lstStudents = new BindingList<student>();
            foreach (string studentNumber in lstStudentNumbers)
            {
                lstStudents.Add(_getStudentFromStudentNumber(studentNumber));
            }
            return lstStudents;
        }

        public static universityStudent GetUniversityStudentFromStudentNumber(string StudentNumber)
        {
            return _getUniversityStudentFromStudentNumber(StudentNumber);
        }

        private static universityStudent _getUniversityStudentFromStudentNumber(string StudentNumber)
        {
            SqlConnection conn = new SqlConnection(conRegistrationDB);
            conn.Open();
            string query = "SELECT [student_number],[firstname],[lastname],[id_Number]";
            query = query + " FROM [RegistrationDB].[dbo].[registered_students] WHERE [student_number] = '" + StudentNumber + "' "; 
            SqlCommand cmd = new SqlCommand(query, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            universityStudent _student = new universityStudent();
            _student.StudentNumber = StudentNumber;
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                if (dr.IsNull("firstname")) { _student.FirstName = null; } else { _student.FirstName = (string)dr["firstname"]; };
                if (dr.IsNull("lastname")) { _student.LastName = null; } else { _student.LastName = (string)dr["lastname"]; };
                if (dr.IsNull("id_number")) { _student.IDNumber = null; } else { _student.IDNumber = (string)dr["id_Number"]; };
            }
            conn.Close();
            return _student;
        }

        private static student _getStudentFromStudentNumber(string StudentNumber)
        {
            SqlConnection conn = new SqlConnection(conRegistrationDB);
            conn.Open();
            string query = "SELECT [student_number],[card_number],[OldCards],[firstname],[lastname],[IDNumber]";
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
                if (dr.IsNull("OldCards")) { _student.CardHistory = null; } else { _student.CardHistory = (string)dr["OldCards"]; };
                if (dr.IsNull("student_number")) { _student.StudentNumber = null; } else { _student.StudentNumber = (string)dr["student_number"];};
                if (dr.IsNull("LeftPinkieTemplate")) { _student.LeftPinkie = null; } else { _student.LeftPinkie = new NFRecord((byte[])dr["LeftPinkieTemplate"]); };
                if (dr.IsNull("LeftRingTemplate")) { _student.LeftRing = null; } else { _student.LeftRing = new NFRecord((byte[])dr["LeftRingTemplate"]);};
                if (dr.IsNull("LeftMiddleTemplate")) { _student.LeftMiddle = null; } else {  _student.LeftMiddle = new NFRecord((byte[])dr["LeftMiddleTemplate"]);};
                if (dr.IsNull("LeftIndexTemplate")) { _student.LeftIndex = null; } else { _student.LeftIndex = new NFRecord((byte[])dr["LeftIndexTemplate"]);};
                if (dr.IsNull("LeftThumbTemplate")) {_student.LeftThumb = null;} else {_student.LeftThumb = new NFRecord((byte[])dr["LeftThumbTemplate"]); };
                if (dr.IsNull("RightThumbTemplate")) { _student.RightThumb = null; } else { _student.RightThumb = new NFRecord((byte[])dr["RightThumbTemplate"]);};
                if (dr.IsNull("RightIndexTemplate")) { _student.RightIndex = null; } else { _student.RightIndex = new NFRecord((byte[])dr["RightIndexTemplate"]);};
                if (dr.IsNull("RightMiddleTemplate")) { _student.RightMiddle = null; } else { _student.RightMiddle = new NFRecord((byte[])dr["RightMiddleTemplate"]);};
                if (dr.IsNull("RightRingTemplate")) { _student.RightRing = null; } else { _student.RightRing = new NFRecord((byte[])dr["RightRingTemplate"]);};
                if (dr.IsNull("RightPinkieTemplate")) { _student.RightPinkie = null; } else { _student.RightPinkie = new NFRecord((byte[])dr["RightPinkieTemplate"]);};
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

        public static bool enrolNewStudent(student NewStudent)
        {

            return false;
        }

        //private static bool _enrolNewStudent(student NewStudent)
        //{
        //    bool returnedValue;
        //    int usn = _getUsn(System.Environment.MachineName.ToLower() );
        //    SqlConnection conn = new SqlConnection(conRegistrationDB);
        //    conn.Open();
        //    string query = "INSERT INTO [dbo].[registration] ";
        //    query = query + "([registration_id]";
        //    query = query + ",[student_number]";
        //    query = query + ",[card_number]";
        //    query = query + ",[firstname]";
        //    query = query + ",[lastname]";
        //    query = query + ",[IDNumber]";
        //    query = query + ",[LeftPinkieTemplate]";
        //    query = query + ",[LeftRingTemplate]";
        //    query = query + ",[LeftMiddleTemplate]";
        //    query = query + ",[LeftIndexTemplate]";
        //    query = query + ",[LeftThumbTemplate]";
        //    query = query + ",[RightThumbTemplate]";
        //    query = query + ",[RightIndexTemplate]";
        //    query = query + ",[RightMiddleTemplate]";
        //    query = query + ",[RightRingTemplate]";
        //    query = query + ",[RightPinkieTemplate]";
        //    query = query + ",[OldCards]";
        //    query = query + ",[TimeStamp]";
        //    query = query + ",[usn]";
        //    query = query + ",[orig_usn]";
        //    query = query + ",[orig_identifier]) ";
        //    query = query + " VALUES";
        //    query = query + "(@examidunit_id";
        //    query = query + ",@auditnumber";
        //    query = query + ",@examdate";
        //    query = query + ",@examnumber";
        //    query = query + ",@mode";
        //    query = query + ",@studentnumber";
        //    query = query + ",@cardnumber";
        //    query = query + ",@creator";
        //    query = query + ",@fingerA_id";
        //    query = query + ",@fingerB_id";
        //    query = query + ",@fingerA_image";
        //    query = query + ",@fingerB_image";
        //    query = query + ",@uploadtime";
        //    query = query + ",@TimeStamp";
        //    query = query + ",@Deleted";
        //    query = query + ",@score1";
        //    query = query + ",@score2";
        //    query = query + ",@category";
        //    query = query + ",@fingerA_template";
        //    query = query + ",@fingerB_template";
        //    query = query + ",@lat";
        //    query = query + ",@lon)";

        //    SqlCommand cmd = new SqlCommand(query, conn);
        //    cmd.Parameters.Add("@examidunit_id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
        //    cmd.Parameters.Add("@auditnumber", SqlDbType.NVarChar).Value = record.AuditNumber;
        //    cmd.Parameters.Add("@examdate", SqlDbType.DateTime).Value = record.ClassTime;
        //    cmd.Parameters.Add("@examnumber", SqlDbType.NVarChar).Value = record.CourseCode;
        //    cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "1";
        //    cmd.Parameters.Add("@studentnumber", SqlDbType.NVarChar).Value = record.StudentNumber;
        //    cmd.Parameters.Add("@cardnumber", SqlDbType.NVarChar).Value = record.CardNumber;
        //    cmd.Parameters.Add("@creator", SqlDbType.NVarChar).Value = "";
        //    cmd.Parameters.Add("@fingerA_id", SqlDbType.Int).Value = record.FingerAID;
        //    cmd.Parameters.Add("@fingerB_id", SqlDbType.Int).Value = record.FingerBID;
        //    cmd.Parameters.Add("@fingerA_image", SqlDbType.Image).Value = record.FingerAImage;
        //    cmd.Parameters.Add("@fingerB_image", SqlDbType.Image).Value = record.FingerBImage;
        //    cmd.Parameters.Add("@uploadtime", SqlDbType.DateTime).Value = DateTime.Now;
        //    cmd.Parameters.Add("@TimeStamp", SqlDbType.DateTime).Value = record.TimeStamp;
        //    cmd.Parameters.Add("@Deleted", SqlDbType.NVarChar).Value = "";
        //    cmd.Parameters.Add("@score1", SqlDbType.Int).Value = record.score1;
        //    cmd.Parameters.Add("@score2", SqlDbType.Int).Value = record.score2;
        //    cmd.Parameters.Add("@category", SqlDbType.NVarChar).Value = record.ScanResult;
        //    cmd.Parameters.Add("@fingerA_template", SqlDbType.Image).Value = record.FingerATemplate;
        //    cmd.Parameters.Add("@fingerB_template", SqlDbType.Image).Value = record.FingerBTemplate;
        //    cmd.Parameters.Add("@lat", SqlDbType.NVarChar).Value = record.lat;
        //    cmd.Parameters.Add("@lon", SqlDbType.NVarChar).Value = record.lon;

        //    cmd.CommandType = CommandType.Text;

        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        returnedValue = true;

        //    }
        //    catch (Exception)
        //    {
        //        returnedValue = false;

        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //    return returnedValue;
        //}

        private static int _getUsn(string server)
        {
            SqlConnection conn = new SqlConnection(conRegistrationDB);
            conn.Open();
            string query = "SELECT [usn] FROM [RegistrationDB].[dbo].[Status] WHERE identifier = '" + server.ToLower() + "'"; 
            SqlCommand cmd = new SqlCommand(query, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            int usn = 0;

            foreach (DataRow dr in dt.Rows)
            {
                usn = (int)dr["usn"];
                
            }
            conn.Close();
            return usn;
        }

        public static bool updateStudent(student ThisStudent)
        {
            return true;
        }

    }
}
