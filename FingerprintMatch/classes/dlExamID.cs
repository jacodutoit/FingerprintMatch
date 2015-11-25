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
    class dlExamID
    {
        private static string conExamIDDB = FingerprintMatch.Properties.Settings.Default.ExamIDDBConn;

        private static bool _insertRecord(attendanceRecord record)
        {
            bool returnedValue;
            SqlConnection conn = new SqlConnection(conExamIDDB);
            conn.Open();
            string query = "INSERT INTO [dbo].[ExamIDUnit] ";
            query = query + "([examidunit_id]";
            query = query + ",[auditnumber]";
            query = query + ",[examdate]";
            query = query + ",[examnumber]";
            query = query + ",[mode]";
            query = query + ",[studentnumber]";
            query = query + ",[cardnumber]";
            query = query + ",[creator]";
            query = query + ",[fingerA_id]";
            query = query + ",[fingerB_id]";
            query = query + ",[fingerA_image]";
            query = query + ",[fingerB_image]";
            query = query + ",[uploadtime]";
            query = query + ",[TimeStamp]";
            query = query + ",[Deleted]";
            query = query + ",[score1]";
            query = query + ",[score2]";
            query = query + ",[category]";
            query = query + ",[fingerA_template]";
            query = query + ",[fingerB_template]";
            query = query + ",[lat]";
            query = query + ",[lon])";
            query = query + " VALUES";
            query = query + "(@examidunit_id";
            query = query + ",@auditnumber";
            query = query + ",@examdate";
            query = query + ",@examnumber";
            query = query + ",@mode";
            query = query + ",@studentnumber";
            query = query + ",@cardnumber";
            query = query + ",@creator";
            query = query + ",@fingerA_id";
            query = query + ",@fingerB_id";
            query = query + ",@fingerA_image";
            query = query + ",@fingerB_image";
            query = query + ",@uploadtime";
            query = query + ",@TimeStamp";
            query = query + ",@Deleted";
            query = query + ",@score1";
            query = query + ",@score2";
            query = query + ",@category";
            query = query + ",@fingerA_template";
            query = query + ",@fingerB_template";
            query = query + ",@lat";
            query = query + ",@lon)";
            
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add("@examidunit_id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
            cmd.Parameters.Add("@auditnumber", SqlDbType.NVarChar).Value = record.AuditNumber;
            cmd.Parameters.Add("@examdate", SqlDbType.DateTime).Value = record.ClassTime;
            cmd.Parameters.Add("@examnumber", SqlDbType.NVarChar).Value = record.CourseCode;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "1";
            cmd.Parameters.Add("@studentnumber",SqlDbType.NVarChar).Value = record.StudentNumber;
            cmd.Parameters.Add("@cardnumber",SqlDbType.NVarChar).Value = record.CardNumber;
            cmd.Parameters.Add("@creator",SqlDbType.NVarChar).Value = "";
            cmd.Parameters.Add("@fingerA_id",SqlDbType.Int).Value = record.FingerAID;
            cmd.Parameters.Add("@fingerB_id",SqlDbType.Int).Value = record.FingerBID;
            cmd.Parameters.Add("@fingerA_image",SqlDbType.Image).Value = record.FingerAImage;
            cmd.Parameters.Add("@fingerB_image",SqlDbType.Image).Value = record.FingerBImage;
            cmd.Parameters.Add("@uploadtime",SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@TimeStamp",SqlDbType.DateTime).Value = record.TimeStamp;
            cmd.Parameters.Add("@Deleted",SqlDbType.NVarChar).Value = "";
            cmd.Parameters.Add("@score1",SqlDbType.Int).Value = record.score1;
            cmd.Parameters.Add("@score2",SqlDbType.Int).Value = record.score2;
            cmd.Parameters.Add("@category",SqlDbType.NVarChar).Value = record.ScanResult;
            cmd.Parameters.Add("@fingerA_template",SqlDbType.Image).Value = record.FingerATemplate;
            cmd.Parameters.Add("@fingerB_template",SqlDbType.Image).Value = record.FingerBTemplate;
            cmd.Parameters.Add("@lat",SqlDbType.NVarChar).Value = record.lat;
            cmd.Parameters.Add("@lon", SqlDbType.NVarChar).Value = record.lon;

            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.ExecuteNonQuery();
                returnedValue = true;

            }
            catch (Exception)
            {
                returnedValue = false;
                
            }
            finally
            {
                conn.Close();
            }
            return returnedValue;
        }

        public static bool InsertAttendanceRecord(attendanceRecord record)
        {
            return _insertRecord(record);
        }
    }
}
