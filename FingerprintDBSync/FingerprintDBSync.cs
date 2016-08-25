using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using FingerprintDBSync.classes;

namespace FingerprintDBSync
{

    public partial class FingerprintDBSync : ServiceBase
    {
        private static System.Timers.Timer tmrSyncDb;
        private bool busySync;
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
        private DateTime pvtSyncLastClass;
        private DateTime pvtSyncLastExamID;
        private DateTime pvtSyncLastRegis;
        private System.Diagnostics.EventLog eventLog1;

        public FingerprintDBSync()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("FingerprintDBSync"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "FingerprintDBSync","Application");
            }
            eventLog1.Source = "FingerprintDBSync";
            //eventLog1.Log = "MyNewLog";
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();
            busySync = false;
            eventLog1.WriteEntry("FingerprintDBSync Starting");
            SetupTimer();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("FingerprintDBSync Stopping");
        }

        private void SetupTimer()
        {

            tmrSyncDb = new System.Timers.Timer(120000);

            ElapsedEventHandler handler = new ElapsedEventHandler(tmrSyncDBEvent);
            tmrSyncDb.Elapsed += handler;
            tmrSyncDb.Enabled = true;

            //Manually execute the event handler on a threadpool thread.
            handler.BeginInvoke(this, null, new AsyncCallback(Timer_ElapsedCallback), handler);
        }

        private void Timer_ElapsedCallback(IAsyncResult result)
        {
            ElapsedEventHandler handler = result.AsyncState as ElapsedEventHandler;
            if (handler != null)
            {
                handler.EndInvoke(result);
            }
        }

        private void GetSyncStatus()
        {
            pvtSyncClassBusy = !Properties.Settings.Default.SyncComplClass;
            pvtSyncClassRecordsLeft = Properties.Settings.Default.SyncClassRecordsLeft;
            pvtSyncClassTotalRecords = Properties.Settings.Default.SyncClassTotalRecords;
            pvtSyncExamIDBusy = !Properties.Settings.Default.SyncComplExamID;
            pvtSyncExamIDRecordsLeft = Properties.Settings.Default.SyncExamIDRecordsLeft;
            pvtSyncExamIDTotalRecords = Properties.Settings.Default.SyncExamIDRecordsLeft;
            pvtSyncPullBusy = !Properties.Settings.Default.SyncComplPull;
            pvtSyncPullRecordsLeft = Properties.Settings.Default.SyncPullRecordsLeft;
            pvtSyncPullTotalRecords = Properties.Settings.Default.SyncPullTotalRecords;
            pvtSyncPushBusy = !Properties.Settings.Default.SyncComplPush;
            pvtSyncLastClass = Properties.Settings.Default.SyncLastClass;
            pvtSyncLastExamID = Properties.Settings.Default.SyncLastExamID;
            pvtSyncLastRegis = Properties.Settings.Default.SyncLastRegis;
        }

        private void tmrSyncDBEvent(Object source, ElapsedEventArgs e)
        {
            eventLog1.WriteEntry("FingerprintDBSync Polling", EventLogEntryType.Information,1000);
            if (!busySync)
            {
                string sSource;
                string sLog;
                string sEvent;

                sSource = "FingerprintDBSync";
                sLog = "Application";
                sEvent = "Sync Started";
                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent);
                busySync = true;

                clsSynchronise mySync = new clsSynchronise();
                GetSyncStatus();
                EventLog.WriteEntry(sSource, "Created mySync Object");

                mySync.syncRegis();
                GetSyncStatus();
                //EventLog.WriteEntry(sSource, "Completed Registration Sync");

                mySync.syncExamID();
                GetSyncStatus();
                //EventLog.WriteEntry(sSource, "Completed ExamID Sync");

                mySync.syncClass();
                GetSyncStatus();
                //EventLog.WriteEntry(sSource, "Completed Class Sync");
                busySync = false;
            }
        }
    }
}
