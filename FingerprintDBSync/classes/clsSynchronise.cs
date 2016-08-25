using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBRepClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace FingerprintDBSync.classes
{
    class clsSynchronise
    {
        private bool pvtSyncClassBusy;
        private int pvtSyncClassTotalRecords;
        private int pvtSyncClassRecordsLeft;
        private int pvtSyncExamIDTotalRecords;
        private bool pvtSyncExamIDBusy;
        private bool pvtSyncPullBusy;
        private int pvtSyncPullTotalRecords;
        private int pvtSyncPullRecordsLeft;
        private bool pvtSyncPushBusy;
        private int pvtSyncExamIDRecordsLeft;  
   
        public clsSynchronise()
        {
            pvtSyncClassBusy = false;
            pvtSyncClassRecordsLeft = 0;
            pvtSyncClassTotalRecords = 0;
            pvtSyncExamIDBusy = false;
            pvtSyncExamIDRecordsLeft = 0;
            pvtSyncExamIDTotalRecords = 0;
            pvtSyncPullBusy = false;
            pvtSyncPullRecordsLeft = 0;
            pvtSyncPullTotalRecords = 0;
            pvtSyncPushBusy = false;
            

            syncDB();
        }

        public DateTime LastSyncRegis
        {
            get
            {
                return Properties.Settings.Default.SyncLastRegis;
                
            }
            set
            {
                Properties.Settings.Default.SyncLastRegis = value;
                Properties.Settings.Default.Save();
            }
        }

        public DateTime LastSyncExamID
        {
            get
            {
                return Properties.Settings.Default.SyncLastExamID;
            }
            set
            {
                Properties.Settings.Default.SyncLastExamID = value;
                Properties.Settings.Default.Save();
            }
        }

        public DateTime LastSyncClass
        {
            get
            {
                return Properties.Settings.Default.SyncLastClass;
            }
            set
            {
                Properties.Settings.Default.SyncLastClass = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool ComplSyncClass
        {
            get
            {
                return Properties.Settings.Default.SyncComplClass;
            }
            set
            {
                Properties.Settings.Default.SyncComplClass = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool ComplSyncRegis
        {
            get
            {
                return Properties.Settings.Default.SyncComplRegis;
            }
            set
            {
                Properties.Settings.Default.SyncComplRegis = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool ComplSyncExamID
        {
            get
            {
                return Properties.Settings.Default.SyncComplExamID;

            }
            set
            {
                Properties.Settings.Default.SyncComplExamID = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool ComplSyncPush
        {
            get
            {
                return Properties.Settings.Default.SyncComplPush;
            }
            set
            {
                Properties.Settings.Default.SyncComplPush = value;
                Properties.Settings.Default.Save();
            }
        }

        public  bool ComplSyncPull
        {
            get
            {
                return Properties.Settings.Default.SyncComplPull;
            }
            set
            {
                Properties.Settings.Default.SyncComplPull = value;
                Properties.Settings.Default.Save();
            }
        }

        public int SyncPullTotalRecords
        {
            get
            {
                return Properties.Settings.Default.SyncPullTotalRecords;
            }
            set
            {
                Properties.Settings.Default.SyncPullTotalRecords = value;
                Properties.Settings.Default.Save();
            }
        }

        public int SyncPullRecordsLeft
        {
            get
            {
                return Properties.Settings.Default.SyncPullRecordsLeft;
            }
            set
            {
                Properties.Settings.Default.SyncPullRecordsLeft = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool SyncPullBusy
        {
            get
            {
                return !this.ComplSyncPull;
            }
            set
            {
                this.ComplSyncPull = !value;
            }
        }

        public bool SyncPushBusy
        {
            get
            {
                return !this.ComplSyncPush;
            }
            set
            {
                this.ComplSyncPush = !value;
            }
        }

        public int SyncExamIDTotalRecords
        {
            get
            {
                return Properties.Settings.Default.SyncExamIDTotalRecords;
            }
            set
            {
                Properties.Settings.Default.SyncExamIDTotalRecords = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool SyncExamIDBusy
        {
            get
            {
                return !this.ComplSyncExamID;
            }
            set
            {
                this.ComplSyncExamID = !value;
            }
        }

        public bool SyncClassBusy
        {
            get
            {
                return !this.ComplSyncClass;
            }
            set
            {
                this.ComplSyncClass = !value;
            }
        }

        public int SyncClassTotalRecords
        {
            get
            {
                return Properties.Settings.Default.SyncClassTotalRecords;
            }
            set
            {
                Properties.Settings.Default.SyncClassTotalRecords = value;
                Properties.Settings.Default.Save();
            }
        }

        public int SyncClassRecordsLeft
        {
            get
            {
                return Properties.Settings.Default.SyncClassRecordsLeft;
            }
            set
            {
                Properties.Settings.Default.SyncClassRecordsLeft = value;
                Properties.Settings.Default.Save();
            }
        }

        public int SyncExamIDRecordsLeft
        {
            get
            {
                return pvtSyncExamIDRecordsLeft;
            }
            set
            {
                pvtSyncExamIDRecordsLeft = value;
            }
        }
    
        public bool connected()
        {
            string syncUserName = Properties.Settings.Default.SyncUserName;
            string syncPassword = Properties.Settings.Default.SyncPassword;
            string syncDomain = Properties.Settings.Default.SyncDomain;
            string syncURL = Properties.Settings.Default.SyncReplicationURL;
            string syncRepPartner = Properties.Settings.Default.SyncRepPartner;
            bool boolAnonymous = Properties.Settings.Default.SyncAnonymous;
            bool boolBasic = Properties.Settings.Default.SyncBasic;


            string sSource;
            string sLog;
            string sEvent;

            sSource = "FingerprintDBSync";
            sLog = "Application";
            sEvent = "Sync Started";
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            bool blConnected = main.Connected(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain, syncRepPartner);

            return blConnected;
        }

        private void syncDB()
        {
            string syncUserName = Properties.Settings.Default.SyncUserName;
            string syncPassword = Properties.Settings.Default.SyncPassword;
            string syncDomain = Properties.Settings.Default.SyncDomain;
            string syncURL = Properties.Settings.Default.SyncReplicationURL;
            string syncRepPartner = Properties.Settings.Default.SyncRepPartner;
            string syncMessage = "";
            bool boolAnonymous = Properties.Settings.Default.SyncAnonymous;
            bool boolBasic = Properties.Settings.Default.SyncBasic;
            bool boolProblem = false;
            Int64 intTotalRecords = 0;
            Int64 intRecordsLeft = 0;
            Int64 intRecordsCount = 0;

            if (Properties.Settings.Default.SyncLastClass == null) Properties.Settings.Default.SyncLastClass = DateTime.Now.AddDays(-1);
            if (Properties.Settings.Default.SyncLastExamID == null) Properties.Settings.Default.SyncLastExamID = DateTime.Now.AddDays(-1);
            if (Properties.Settings.Default.SyncLastRegis == null) Properties.Settings.Default.SyncLastRegis = DateTime.Now.AddDays(-1);

            if (!connected()) return;
            
            main.procSetupReplication(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain);
            
            
        }

        public bool syncRegis()
        {
            int syncMinutes = Properties.Settings.Default.SyncMinutes;
            DateTime LastSync = this.LastSyncRegis;

            if (DateTime.Now > LastSync.AddMinutes(syncMinutes))
            {
                if (syncPullRegis() && syncPushRegis())
                {
                    this.LastSyncRegis = DateTime.Now;
                    this.ComplSyncRegis = true;
                }
                else
                {
                    this.ComplSyncRegis = false;
                }

            }
            else
            {
                this.ComplSyncRegis = true;
            }
            

            return this.ComplSyncRegis;
        }

        public bool syncPullRegis()
        {
            string sSource;
            string sLog;
            string sEvent;

            sSource = "FingerMatch";
            sLog = "Application";
            sEvent = "Sync Started";
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            string syncUserName = Properties.Settings.Default.SyncUserName;
            string syncPassword = Properties.Settings.Default.SyncPassword;
            string syncDomain = Properties.Settings.Default.SyncDomain;
            string syncURL = Properties.Settings.Default.SyncReplicationURL;
            string syncRepPartner = Properties.Settings.Default.SyncRepPartner;
            string syncMessage = "";
            bool boolAnonymous = Properties.Settings.Default.SyncAnonymous;
            bool boolBasic = Properties.Settings.Default.SyncBasic;
            Int64 intTotalRecords = 0;
            Int64 intRecordsLeft = 0;
            

            if (!connected()) return false;
            //if (this.SyncPullBusy) return true;

            main.procSetupReplication(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain);
            bool completed = false;
            this.SyncPullBusy = true;
            EventLog.WriteEntry(sSource, "SyncPull: Before completed and connected");
            while (!completed && connected())
            {
                EventLog.WriteEntry(sSource, "SyncPull: SyncPull: Before ReplicationPull");
                completed = main.ReplicationPull(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain, syncRepPartner, out syncMessage, out intTotalRecords, out intRecordsLeft);
                EventLog.WriteEntry(sSource, "SyncPull: SyncPull: RecordsLeft:" + intRecordsLeft);
                this.SyncPullTotalRecords = (int)intTotalRecords;
                this.SyncPullRecordsLeft = (int)intRecordsLeft;
            }
            if (completed)
            {
                this.pvtSyncPullBusy = false;
                this.ComplSyncPull = completed;
            }
            
            return completed;
        }

        public bool syncPushRegis()
        {
            string syncUserName = Properties.Settings.Default.SyncUserName;
            string syncPassword = Properties.Settings.Default.SyncPassword;
            string syncDomain = Properties.Settings.Default.SyncDomain;
            string syncURL = Properties.Settings.Default.SyncReplicationURL;
            string syncRepPartner = Properties.Settings.Default.SyncRepPartner;
            string syncMessage = "";
            bool boolAnonymous = Properties.Settings.Default.SyncAnonymous;
            bool boolBasic = Properties.Settings.Default.SyncBasic;
            bool boolProblem = false;
            Int64 intTotalRecords = 0;
            Int64 intRecordsLeft = 0;
            Int64 intRecordsCount = 0;

            string sSource;
            string sLog;
            string sEvent;

            sSource = "FingerMatch";
            sLog = "Application";
            sEvent = "Sync Started";
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            if (!connected()) return false;
            //if (this.SyncPushBusy) return true;

            main.procSetupReplication(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain);
            bool completed = false;
                        
            completed = false;
            this.SyncPushBusy = true;
            EventLog.WriteEntry(sSource, "SyncPush: Before completed and connected");
            while (!completed && connected())
            {
                completed = main.ReplicationPush(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain, syncRepPartner);
                EventLog.WriteEntry(sSource, "SyncPush: Pushing: " + completed);
            }

            if (completed)
            {
                this.ComplSyncPush = completed;
                this.SyncPushBusy = false;
            }
            return completed;
        }

        public bool syncExamID()
        {

            string sSource;
            string sLog;
            string sEvent;

            sSource = "FingerMatch";
            sLog = "Application";
            sEvent = "Sync Started";
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            string syncUserName = Properties.Settings.Default.SyncUserName;
            string syncPassword = Properties.Settings.Default.SyncPassword;
            string syncDomain = Properties.Settings.Default.SyncDomain;
            string syncURL = Properties.Settings.Default.SyncReplicationURL;
            string syncRepPartner = Properties.Settings.Default.SyncRepPartner;
            string syncMessage = "";
            bool boolAnonymous = Properties.Settings.Default.SyncAnonymous;
            bool boolBasic = Properties.Settings.Default.SyncBasic;
            bool boolProblem = false;
            Int64 intTotalRecords = 0;
            Int64 intRecordsLeft = 0;
            Int64 intRecordsCount = 0;
            bool blnContinueSync = false;
            int syncMinutes = Properties.Settings.Default.SyncMinutes;

            //EventLog.WriteEntry(sSource, "ExamIDSync: Started Sync: SyncMinutes:" + syncMinutes);
            if (!connected()) return false;
            //EventLog.WriteEntry(sSource, "ExamIDSync: Connected Succesfully");
            //if (this.SyncExamIDBusy) return true;
            main.procSetupReplication(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain);
            //EventLog.WriteEntry(sSource, "ExamIDSync: SetupReplication Successfully");
            bool completed = this.ComplSyncExamID;
            DateTime LastSync = this.LastSyncExamID;
            this.SyncExamIDBusy = true;

            //EventLog.WriteEntry(sSource, "ExamIDSync: " + DateTime.Now.ToString() + " -- " + LastSync.AddMinutes(syncMinutes));
            if (DateTime.Now > LastSync.AddMinutes(syncMinutes))
            {
                completed = false;
                this.ComplSyncExamID = false;
                while (!completed && connected())
                {
                    //EventLog.WriteEntry(sSource, "ExamIDSync: Before completed");
                    completed = main.ReplicationUpdateExamID(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain, syncRepPartner, out syncMessage, out intTotalRecords,
                        out intRecordsCount, out boolProblem);
                    //EventLog.WriteEntry(sSource, "ExamIDSync: completed: " + completed);
                    EventLog.WriteEntry(sSource, "ExamIDSync: TotalRecords: " + intTotalRecords);
                    this.SyncExamIDTotalRecords = (int)intTotalRecords;

                }
                this.ComplSyncExamID = completed;
                if (completed)
                {
                    EventLog.WriteEntry(sSource, "ExamIDSync: Completed Sync");
                    this.LastSyncExamID = DateTime.Now;
                    this.SyncExamIDBusy = false;
                    this.ComplSyncExamID = true;
                }
            }
            else
            {
                this.SyncExamIDBusy = false;
                this.ComplSyncExamID = true;
            }

            
            return completed;
        }

        public bool syncClass()
        {
            string syncUserName = Properties.Settings.Default.SyncUserName;
            string syncPassword = Properties.Settings.Default.SyncPassword;
            string syncDomain = Properties.Settings.Default.SyncDomain;
            string syncURL = Properties.Settings.Default.SyncReplicationURL;
            string syncRepPartner = Properties.Settings.Default.SyncRepPartner;
            string syncMessage = "";
            bool boolAnonymous = Properties.Settings.Default.SyncAnonymous;
            bool boolBasic = Properties.Settings.Default.SyncBasic;
            bool boolProblem = false;
            Int64 intTotalRecords = 0;
            Int64 intRecordsLeft = 0;
            Int64 intRecordsCount = 0;
            
            string sSource;
            string sLog;
            string sEvent;

            sSource = "FingerMatch";
            sLog = "Application";
            sEvent = "Class Sync Started";
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            if (!connected()) return false;
            //if (this.SyncClassBusy) return true;
            main.procSetupReplication(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain);
            bool completed = this.ComplSyncClass;
            int syncDays = Properties.Settings.Default.SyncDays;
            DateTime LastSync = this.LastSyncClass;
            this.SyncClassBusy = true;
            //EventLog.WriteEntry(sSource, "ClassSync: LastSync: " + LastSync.DayOfYear);
            //EventLog.WriteEntry(sSource, "ClassSync: Today: " + DateTime.Now.DayOfYear);
            if (DateTime.Now.DayOfYear > LastSync.DayOfYear || DateTime.Now.Year > LastSync.Year )
            {
                EventLog.WriteEntry(sSource, "ClassSync: Started Sync");
                if (connected())
                {
                    if (main.ReplicateCoursesIncr(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain, syncRepPartner))
                    {
                        completed = true;
                        this.LastSyncClass = DateTime.Now;
                    }
                    else
                    {

                        EventLog.WriteEntry(sSource, "ClassSync: Failure in replication");
                        completed = false;
                    }
                }

                while (!completed && connected())
                {
                    //EventLog.WriteEntry(sSource, "ClassSync: Before ReplicateCourses");
                    completed = main.ReplicateCourses(syncURL, boolBasic, boolAnonymous, syncUserName, syncPassword, syncDomain, syncRepPartner, out syncMessage, out intTotalRecords, out intRecordsCount);
                    this.SyncClassRecordsLeft = (int)(intTotalRecords - intRecordsCount);
                    this.SyncClassTotalRecords = (int)intTotalRecords;
                    
                    //EventLog.WriteEntry(sSource, "ClassSync: RecordsLeft: " + this.SyncClassRecordsLeft);
                    //EventLog.WriteEntry(sSource, "ClassSync: RecordsLeft Completed: " + completed);
                }
                this.ComplSyncClass = completed;

                if (completed)
                {
                    EventLog.WriteEntry(sSource, "ClassSync: Completed Sync");
                    this.LastSyncClass = DateTime.Now;
                    this.SyncClassBusy = false;
                    this.ComplSyncClass = true;
                }
            }
            else
            {
                EventLog.WriteEntry(sSource, "ClassSync: Completed Sync. No Sync Necessary");
                completed = true;
                this.SyncClassBusy = false;
                this.ComplSyncClass = true;
            }

            
            return completed;
        }

    }
}
