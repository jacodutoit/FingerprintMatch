using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using FingerprintMatch.classes;
using Neurotec.Licensing;
using Neurotec.Devices;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Images;
using Neurotec.IO;
using System.Diagnostics;
using KnightsWarriorAutoupdater;
using System.Net;
using System.Xml;
//using Sagem.MorphoKit;
using Microsoft.Win32;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace FingerprintMatch
{
    public partial class Main : Form
    {

        private static string licServer = FingerprintMatch.Properties.Settings.Default.LicenseServer;
        private BindingList<FingerprintMatch.classes.course> lstCourses;
        private List<string> lstCourseCodes;
        private List<course> lstCoursesList;
        private BindingList<student> lstStudents;
        private BindingList<string> validStudents;
        private dlDataHelper _getStudents;
        private course _selectedCourse;
        
        private bool cancelMatchScan = false;
        private NImage fingerprintImage;
        private NTemplate fingerprintTemplate;
        private NFinger studentFinger;
        private NSubject studentSubject;

        private string matchedStudentNumber;
        private int matchedFingerID;
        private int matchedScore;
        private bool identified;
        private string strConfirmMessage;
        private bool keyHandled = false;

        private NRgb resultImageMinColor;
        private NRgb resultImageMaxColor;
        private string breakoutPin;
        private enum ScanStatus
        {
            TemplateCreated,
            NoScanner,
            NoTemplateCreated,
            NoLicenseObtained,
            Cancelled
        }
        private string ScanError;
        private ScanStatus GlobalScanStatus;
        private bool ScannedTemplated;
        private bool busySync;
        private static System.Timers.Timer tmrSyncDb;

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

        public Main()
        {
            
            InitializeComponent();
            ChangeWindowSize();
            
            ////Uncomment below line to see Russian version

            ////AutoUpdater.CurrentCulture = CultureInfo.CreateSpecificCulture("ru-RU");

            ////If you want to open download page when user click on download button uncomment below line.

            //AutoUpdater.OpenDownloadPage = true;

            ////Don't want user to select remind later time in AutoUpdater notification window then uncomment 3 lines below so default remind later time will be set to 2 days.

            ////AutoUpdater.LetUserSelectRemindLater = false;
            ////AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
            ////AutoUpdater.RemindLaterAt = 2;
            //string strUpdatePath = FingerprintMatch.Properties.Settings.Default.AutoUpdateSource;

            //AutoUpdater.Start(strUpdatePath);


            UpdateRegistry();
            AutoUpdate();
            UpdateFiles();

            InitSyncStatusInfo();
            busySync = false;
            //tmrSync.Interval = FingerprintMatch.Properties.Settings.Default.SyncMinutes * 60000;
            
            busySync = false;
            string sSource;
            string sLog;
            string sEvent;

            sSource = "FingerMatch";
            sLog = "Application";
            sEvent = "Sync Started";
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            EventLog.WriteEntry(sSource, "Started App");

            SetupTimer();
            //resultImageMinColor = new NRgb(0,230,0);
            //resultImageMaxColor = new NRgb(255, 255, 255);
            breakoutPin = "";
        }

        private void UpdateRegistry()
        {
            const string InternetBrowserAutoStartKeyName = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\NlaSvc\\Parameters\\Internet";
            const string DisableHelpStickerKeyName ="HKEY_CURRENT_USER\\Software\\Policies\\Microsoft\\Windows\\EdgeUI";

            
         //   try
	        //{
         //       Registry.SetValue(InternetBrowserAutoStartKeyName,"EnableActiveProbing",0,RegistryValueKind.DWord);
         //       Registry.SetValue(DisableHelpStickerKeyName, "DisableHelpSticker", 1, RegistryValueKind.DWord);
	        //}
	        //catch (Exception)
	        //{
		       // throw;
	        //}
        }

        //Use this process to update certain core files to new versions
        private void UpdateFiles()
        {
            InstallNewRoot();
        }

        //Install a new root certificate to ensure the connections can occur over https
        private void InstallNewRoot()
        {
            const string fileName = "\\ROOT.CER";
            string file; // Contains name of certificate file

            //get the file in the current working directory
            try
            {
                file = Directory.GetCurrentDirectory();
                file += fileName;
                if (File.Exists(file))
                {
                    X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadWrite);
                    X509Certificate rootCA = new X509Certificate(file);
                    if (!store.Certificates.Contains(rootCA))
                    {
                        store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(file)));
                    }
                    
                    store.Close();
                }
                
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error in InstallNewRoot: " + ex.Message);
            }
            
        }

        private void AutoUpdate()
        {
            #region check and download new version program
            bool bHasError = false;
            IAutoUpdater autoUpdater = new KnightsWarriorAutoupdater.AutoUpdater();
            
            try
            {
                autoUpdater.Update();
            }
            catch (WebException exp)
            {
                MessageBox.Show("Can not find the specified resource");
                bHasError = true;
            }
            catch (XmlException exp)
            {
                bHasError = true;
                MessageBox.Show("Download the upgrade file error");
            }
            catch (NotSupportedException exp)
            {
                bHasError = true;
                MessageBox.Show("Upgrade address configuration error");
            }
            catch (ArgumentException exp)
            {
                bHasError = true;
                MessageBox.Show("Download the upgrade file error");
            }
            catch (Exception exp)
            {
                bHasError = true;
                MessageBox.Show("An error occurred during the upgrade process");
            }
            finally
            {
                if (bHasError == true)
                {
                    try
                    {
                        autoUpdater.RollBack();
                    }
                    catch (Exception)
                    {
                        //Log the message to your file or database
                    }
                }
            }
            #endregion
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



        private void Main_Load(object sender, EventArgs e)
        {
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged +=
                new System.EventHandler(displaySettingsChanged);
            
            ChangeWindowSize();
            PrepareMainScreen();
            
            lstCourses = dlRegistration.getClasses();
            lstCoursesList = lstCourses.ToList();
            lstCourseCodes = new List<string>();
            foreach (course crsCourses in lstCoursesList)
            {
                lstCourseCodes.Add(crsCourses.CourseCode);
            }
            validStudents = dlRegistration.getRegisteredStudents();
        }

        private void displaySettingsChanged(object sender, EventArgs e)
        {
            ChangeWindowSize();
        }

        private void ChangeWindowSize()
        {
            int Width, Height;
            this.WindowState = FormWindowState.Maximized;
            Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;           
            this.Width = Width;
            this.WindowState = FormWindowState.Normal;
            this.Height = Height-100;
            this.Location = new Point(0, 0);
        }

        private void PrepareMainScreen()
        {
            LoadPanelMain();
        }

        #region Main MainPanel
        private void LoadPanelMain()
        {
            pnlIdentification.Visible = false;
            pnlIdentifyAction.Visible = false;
            pnlVerification.Visible = false;
            pnlConfirmMessage.Visible = false;
            pnlConfirmCancel.Visible = false;
            pnlStatusInfo.Visible = false;
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Visible = true;


            pnlConfirmMessage.Dock = DockStyle.Fill;
            MainTitle();
            MainButton1("Identification", 76);
            MainButton2("Verification", 151);
            SyncButton("Synchronise DB", btnMainButton2.Bottom + 10);
            
        }

        private void MainTitle()
        {
            lblTitle.Text = "Integrita Application";
            lblTitle.Font = new Font("Arial", 25);
            lblTitle.FlatStyle = FlatStyle.Flat;
            //int Width;
            //Width = (pnlMain.Width / 5) * 4;
            //lblTitle.Width = Width;
            lblTitle.Height = 75;
            lblTitle.Location = new Point((pnlMain.Width - lblTitle.Width) / 2, 0);
            
            lblTitle.FlatAppearance.BorderSize = 0;
        }

        private void MainButton1(string Title, int Top)
        {
            int Width;
            Width = (pnlMain.Width / 5) * 4;
            btnMainButton1.Width = Width;
            btnMainButton1.Height = 75;
            btnMainButton1.Location = new Point((pnlMain.Width - Width) /2, Top);
            btnMainButton1.Text = Title;
            btnMainButton1.Font = new Font("Arial", 25);
            
        }

        private void MainButton2(string Title, int Top)
        {
            int Width;
            Width = (pnlMain.Width / 5) * 4;
            btnMainButton2.Width = Width;
            btnMainButton2.Height = 75;
            btnMainButton2.Location = new Point((pnlMain.Width - Width) /2, Top);
            btnMainButton2.Text = Title;
            btnMainButton2.Font = new Font("Arial", 25);
            btnMainButton2.Enabled = false;
        }

        private void SyncButton(string Title, int Top)
        {
            int Width;
            Width = (pnlMain.Width / 5) * 4;

            btnSync.Width = Width;
            btnSync.Height = 75;
            btnSync.Location = new Point((pnlMain.Width - Width) / 2, Top);
            btnSync.Text = Title;
            btnSync.Font = new Font("Arial", 25);
            btnSync.Enabled = true;
        }

        private void btnMainButton1_Click(object sender, EventArgs e)
        {
            LoadPanelIdentification();
        }

        private void btnMainbutton2_click(object sender, EventArgs e)
        {
            LoadPanelVerification();
        }

        private void btnSync_click(object sender, EventArgs e)
        {
            StatusPanel();
        }


        #endregion

        #region Main Identification
        private void LoadPanelIdentification()
        {
            pnlMain.Visible = false;

            int Width, Height;
            Width = this.Width;
            Height = this.Height;
            //Width = Main.ActiveForm.Width;
            //Height = Main.ActiveForm.Height;
            pnlIdentification.Dock = DockStyle.Fill;
            

            IdentificationTitle();
            IdentificationCancel();
            var source = new AutoCompleteStringCollection();
            var sortedlist = (from d in lstCourses orderby d.CourseCode select d.CourseCode).ToList();
            source.AddRange(sortedlist.ToArray());
            cmbCourseList.AutoCompleteCustomSource = source;
            cmbCourseList.AutoCompleteMode = AutoCompleteMode.Append;
            cmbCourseList.AutoCompleteSource = AutoCompleteSource.CustomSource;

            //cmbCourseList.DataSource = lstCourses;
            //cmbCourseList.DisplayMember = "CourseCode";
            //cmbCourseList.ValueMember = "CourseCode";
            //cmbCourseList.AutoCompleteSource = AutoCompleteSource.ListItems;
            //cmbCourseList.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCourseList.Font = new Font("Arial", 25);

            IdentificationCourseControls();
            IdentificationBreakOutPin();
            TextBreakOutPin();
            IdentificationStartButton();

            pnlIdentification.Visible = true;
        }

        private void IdentificationCancel()
        {
            int Width, Height;
            Width = (pnlMain.Width / 5) * 1;

            btnIdentificationCancel.Width = Width;
            btnIdentificationCancel.Height = 75;
            Height = pnlIdentification.Height - btnIdentificationCancel.Height;
            btnIdentificationCancel.Location = new Point(0, Height);
            btnIdentificationCancel.Font = new Font("Arial", 25);

        }

        private void IdentificationCourseControls()
        {
            int Width, Height;
            Width = 280;
            cmbCourseList.Width = Width;
            cmbCourseList.Height = 65;
            cmbCourseList.Font = new Font("Arial", 25);
            cmbCourseList.Location = new Point((pnlMain.Width - Width) / 2, lblIdentification.Bottom + 5);

            lblSelectedValue.Text = "";
            lblSelectedValue.MaximumSize = new Size(pnlMain.Width, 250);
            lblSelectedValue.AutoSize = true;
            lblSelectedValue.Font = new Font("Arial", 15);
            if (pnlMain.Width > lblSelectedValue.Width)
                lblSelectedValue.Location = new Point((pnlMain.Width - lblSelectedValue.Width) / 2, cmbCourseList.Bottom + 5);
            else
                lblSelectedValue.Location = new Point(0, cmbCourseList.Bottom + 5);

            btnAddCourse.Width = 175;
            btnAddCourse.Height = 50;
            btnAddCourse.Font = new Font("Arial", 25);
            btnAddCourse.Location = new Point((pnlMain.Width - btnAddCourse.Width) / 2, lblSelectedValue.Bottom + 5);
            btnAddCourse.Enabled = false;

            cmbCourses.DisplayMember = "CourseCode";
            cmbCourses.Items.Clear();
            cmbCourses.Width = Width;
            cmbCourses.Height = 118;
            cmbCourses.Font = new Font("Arial", 25);
            cmbCourses.Location = new Point((pnlMain.Width - Width) / 2, btnAddCourse.Bottom + 5);

            
        }

        private void IdentificationSelectedValue()
        {
            //lblSelectedValue.Font = new Font("Arial", 25);
            if (pnlMain.Width > lblSelectedValue.Width)
                lblSelectedValue.Location = new Point((pnlMain.Width - lblSelectedValue.Width) / 2, cmbCourseList.Bottom + 5);
            else
                lblSelectedValue.Location = new Point(0, cmbCourseList.Bottom + 5);
        }

        private void IdentificationTitle()
        {
            //int Width;
            //Width = (pnlMain.Width / 5) * 4;
            //lblIdentification.Width = Width;
            lblIdentification.Text = "Identification";
            lblIdentification.Font = new Font("Arial", 25);
            lblIdentification.FlatStyle = FlatStyle.Flat;
            lblIdentification.Height = 75;
            lblIdentification.Location = new Point((pnlMain.Width - lblIdentification.Width) / 2, 0);
            
            lblIdentification.FlatAppearance.BorderSize = 0;
        }

        private void IdentificationBreakOutPin()
        {
            lblBreakOutPin.Visible = true;
            lblBreakOutPin.MaximumSize = new Size(pnlIdentification.Width, 0);
            lblBreakOutPin.AutoSize = true;
            lblBreakOutPin.Font = new Font("Arial", 20);
            if (pnlMain.Width > lblBreakOutPin.Width)
                lblBreakOutPin.Location = new Point((pnlMain.Width - lblBreakOutPin.Width) / 2, cmbCourses.Bottom +5);
            else
                lblBreakOutPin.Location = new Point(0, cmbCourses.Bottom +5);
        }

        private void TextBreakOutPin()
        {
            txtBreakoutPin.Visible = true;
            txtBreakoutPin.MaximumSize = new Size(pnlIdentification.Width, 0);
            txtBreakoutPin.Font = new Font("Arial", 20);
            if (pnlMain.Width > txtBreakoutPin.Width)
                txtBreakoutPin.Location = new Point((pnlMain.Width - txtBreakoutPin.Width) / 2, lblBreakOutPin.Bottom + 5);
            else
                txtBreakoutPin.Location = new Point(0, lblBreakOutPin.Bottom + 5);
        }

        private void IdentificationStartButton()
        {
            int Width, Height;
            Width = (pnlMain.Width / 5) * 4;

            btnStartId.Width = Width;
            btnStartId.Height = 75;
            btnStartId.Font = new Font("Arial", 25);
            Height = pnlIdentification.Height - btnStartId.Height;
            btnStartId.Location = new Point((pnlMain.Width - Width) / 2, txtBreakoutPin.Bottom + 5);
            btnStartId.Font = new Font("Arial", 25);
            btnStartId.Enabled = false;
        }
        
        //private void cmbCourseList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    course tempCourse = (course)cmbCourseList.SelectedItem;
        //    lblSelectedValue.Text = tempCourse.CourseCode;
        //    lblSelectedValue.Text = lblSelectedValue.Text + " - " + tempCourse.Description;
        //    IdentificationSelectedValue();
        //}

        //private void cmbCourseList_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    course tempCourse = (course)cmbCourseList.SelectedItem;
        //    lblSelectedValue.Text = tempCourse.CourseCode;
        //    lblSelectedValue.Text = lblSelectedValue.Text + " - " + tempCourse.Description;
        //    IdentificationSelectedValue();

        //}

        private void cmbCourseList_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode >= Keys.A) && (e.KeyCode <= Keys.Z))
            {
                cmbCourseList.SelectedText = e.KeyCode.ToString().ToUpper();
                keyHandled = true;
            }
            else keyHandled = false;
        }

        private void cmbCourseList_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = keyHandled;
        }

        private void cmbCourseList_TextChanged(object sender, EventArgs e)
        {
            if (lstCourseCodes.Contains(cmbCourseList.Text))
            {
                _selectedCourse = lstCoursesList.Find(c => c.CourseCode == cmbCourseList.Text);
                lblSelectedValue.Text = _selectedCourse.CourseCode;
                lblSelectedValue.Text = lblSelectedValue.Text + " - " + _selectedCourse.Description;
                IdentificationSelectedValue();
                btnAddCourse.Enabled = true;
                
            }
            else
            {
                btnAddCourse.Enabled = false;
            }
        }

        private void btnAddCourse_Click(object sender, EventArgs e)
        {
            cmbCourses.Items.Add(_selectedCourse);
            cmbCourseList.Text = "";
            btnStartId.Enabled = true;
        }


        private void btnIdentificationCancel_Click(object sender, EventArgs e)
        {
            pnlIdentification.Visible = false;
            pnlMain.Visible = true;
        }

        private void btnStartId_Click(object sender, EventArgs e)
        {
            breakoutPin = txtBreakoutPin.Text;
            txtBreakoutPin.Text = "";
            LoadIdentificationAction();
        }


        #endregion

        #region Main IdentificationAction

        private void LoadIdentificationAction()
        {
            pnlMain.Visible = false;
            pnlIdentification.Visible = false;

            btnMatchConfirm.Visible = false;
            btnMatchRetry.Visible = false;
            txtMatchStudentNumber.Visible = false;
            
            lblMatchResult.Visible = false;

            pnlIdentifyAction.Dock = DockStyle.Fill;
            pnlIdentifyAction.Visible = true;

            lblStatus.Location = new Point(0, 0);
            lblStatus.Text = "Before getting students for courses";

            txtMatchStudentNumber.Text = "";
            nFView1.Finger = null;
            studentSubject = null;
            studentFinger = null;
            //nfView1.ResultImage = null;
            //nfView1.Template = null;

            int Width;
            lblIdentifyActionMessage.Font = new Font("Arial", 25);
            Width = lblIdentifyActionMessage.Width;
            lblIdentifyActionMessage.Width = Width;
            lblIdentifyActionMessage.Height = 75;
            lblIdentifyActionMessage.Location = new Point((pnlIdentifyAction.Width - Width) / 2, 0);
            lblIdentifyActionMessage.Text = "Loading fingerprint templates";
            Application.DoEvents();

            bckLoadTemplates.WorkerReportsProgress = true;
            bckLoadTemplates.WorkerSupportsCancellation = true;

            bckScanFinger.WorkerReportsProgress = true;
            bckScanFinger.WorkerSupportsCancellation = true;

            pgbLoadTemplates.Visible = true;
            pgbLoadTemplates.Width = (pnlIdentifyAction.Width / 2) / 5 * 4;
            pgbLoadTemplates.Height = 75;
            pgbLoadTemplates.Location = new Point(pnlIdentifyAction.Width / 10 * 3, 100);
            pgbLoadTemplates.Maximum = 100;
            pgbLoadTemplates.Visible = true;
            pnlIdentifyAction.Refresh();
            Application.DoEvents();


            //_selectedCourse = (course)cmbCourseList.SelectedItem;
            //_selectedCourse = lstCoursesList.Find(c => c.CourseCode == cmbCourseList.Text);

            bckLoadTemplates.RunWorkerAsync();
            while (bckLoadTemplates.IsBusy)
            {
                Application.DoEvents();
            }

            pgbLoadTemplates.Visible = false;
            Application.DoEvents();
            IdentifyActionScreen();
            pnlIdentifyAction.Refresh();
            ScannedTemplated = false;
            
            //See if a license can be obtained
            const string Components = "Biometrics.FingerExtraction,Devices.FingerScanners";
            if (!NLicense.ObtainComponents(licServer, 5000, Components))
            {
                //Set the scanstatus to cancelled
                GlobalScanStatus = ScanStatus.NoLicenseObtained;
            }

            if (!bckScanFinger.IsBusy)
            {
                bckScanFinger.RunWorkerAsync();
            }
        }

        private void IdentifyActionScreen()
        {
            
            lblIdentifyActionMessage.Text = "Please scan right index finger";

            //nFView1.BackColor = Color.Azure;
            nFView1.Visible = true;
            nFView1.Location = new Point((pnlIdentifyAction.Width - nFView1.Width) / 2, 75);
            
            lblMatchResult.Text = "";
            MatchingButtons();
            MatchingCancel();
        }

        private void MatchingButtons()
        {
            int Width;

            pgbLoadTemplates.Visible = false;
            pgbLoadTemplates.Location = new Point(1, 1);
            
            lblMatchResult.Font = new Font("Arial", 25);
            Width = (pnlIdentifyAction.Width / 2) / 5 * 4;
            lblMatchResult.Width = Width;
            lblMatchResult.Height = 75;
            lblMatchResult.Location = new Point(pnlIdentifyAction.Width / 10 * 3, nFView1.Bottom + 10);
            lblMatchResult.Visible = true;

            Width = (pnlIdentifyAction.Width / 2) / 5 * 4;

            
            txtMatchStudentNumber.Visible = true;
            txtMatchStudentNumber.Font = new Font("Arial", 25);
            txtMatchStudentNumber.Width = Width;
            txtMatchStudentNumber.Height = 75;
            txtMatchStudentNumber.Location = new Point(pnlIdentifyAction.Width / 10 * 3 , lblMatchResult.Location.Y+76);


            Width = pnlIdentifyAction.Width / 5 * 3 - 20;
            btnMatchConfirm.Visible = true;
            btnMatchConfirm.Font = new Font("Arial", 25);
            btnMatchConfirm.Width = Width;
            btnMatchConfirm.Height = 75;
            //btnMatchConfirm.Location = new Point(((pnlIdentifyAction.Width / 2) / 5 * 1), txtMatchStudentNumber.Location.Y + 76);
            btnMatchConfirm.Location = new Point((pnlIdentifyAction.Width / 5 * 1 + 20), txtMatchStudentNumber.Bottom + 10);
            btnMatchConfirm.Enabled = false;
            btnMatchConfirm.Text = "Yes";

            //btnMatchNo.Visible = true;
            //btnMatchNo.Font = new Font("Arial", 25);
            //btnMatchNo.Width = Width;
            //btnMatchNo.Height = 75;
            //btnMatchNo.Location = new Point(pnlIdentifyAction.Width /2,txtMatchStudentNumber.Location.Y+    76);
            //btnMatchNo.Enabled = false;
            //btnMatchNo.Text = "No";

            Width = (pnlMain.Width / 5) * 1;
            btnMatchRetry.Width = Width;
            btnMatchRetry.Height = 75;
            btnMatchRetry.Location = new Point(pnlIdentifyAction.Width - Width, pnlIdentification.Height - btnMatchRetry.Height);
            btnMatchRetry.Font = new Font("Arial", 25);
            btnMatchRetry.Text = "Retry";
            btnMatchRetry.Visible = false;
            btnMatchRetry.Enabled = true;
        }

        private void MatchingCancel()
        {
            int Width, Height;
            Width = (pnlMain.Width / 5) * 1;

            btnCancelScan.Width = Width;
            btnCancelScan.Height = 75;
            Height = pnlIdentification.Height - btnCancelScan.Height;
            btnCancelScan.Location = new Point(0, Height);
            btnCancelScan.Font = new Font("Arial", 25);

        }

        private void btnCancelScan_Click(object sender, EventArgs e)
        {
            bckScanFinger.CancelAsync();
            LoadIdentificationCancel();
        }       

        private string MatchStudents(NTemplate template, BindingList<student> StudentsInCourse)
        {
            const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching";

            matchedFingerID = 0;
            matchedScore = 0;
            string highestStudent = "";
            int highestMatch = 0;

            
            
            //NMatcher matcher = null;
            try
            {
                lblStatus.Text = "Getting License!";
                Application.DoEvents();
                // Obtain license.
                if (!NLicense.ObtainComponents(licServer, 5000, Components))
                {
                    Console.WriteLine(@"Could not obtain licenses for components: {0}", Components);
                    //return -1;
                }
                lblStatus.Text = "Got License!";
                Application.DoEvents();
                // create an extractor
                //extractor = new NFExtractor();

                // extract probe template

                var biometricClient = new NBiometricClient();
                var probeSubject = new NSubject();
                NTemplate probeTemplate = template;
                probeSubject.SetTemplate(probeTemplate);
                
                NBiometricStatus status= biometricClient.CreateTemplate(probeSubject);
                biometricClient.MatchingFirstResultOnly = false;
                if (status != NBiometricStatus.Ok)
                {
                    Console.WriteLine("Failed to create probe template. Status: {0}.", status);
                }
                lblStatus.Text = "Exctracting templates";
                // extract gallery templates
                int args = StudentsInCourse.Count * 10;
                NTemplate[] galleryTemplates = new NTemplate[args - 1];

                // Create gallery templates and enroll them
                NBiometricTask enrollTask = biometricClient.CreateTask(NBiometricOperations.Enroll, null);
                
                string[] students = new string[args - 1];
                int n = 0;
                foreach (student currentStudent in StudentsInCourse)
                {
                    for (int count = 0; count < 10; count++)
                    {
                        if(currentStudent.template[count]!=null)
                        {
                            try
                            {
                                NTemplate galleryTemplate = new NTemplate(currentStudent.template[count]);

                                enrollTask.Subjects.Add(CreateSubject(galleryTemplate, (n + count).ToString()));
                                //galleryTemplates[n+count] = new NBuffer(currentStudent.template[count]);
                                students[n + count] = currentStudent.StudentNumber;

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Could not load template for student" + currentStudent.StudentNumber + " - " + ex.Message);
                            }
                        } 
                    }
                    
                    n = n + 10;
                }


                biometricClient.PerformTask(enrollTask);
                status = enrollTask.Status;
                if (status != NBiometricStatus.Ok)
                {
                    Console.WriteLine("Enrollment was unsuccessful. Status: {0}.", status);
                    if (enrollTask.Error != null) throw enrollTask.Error;
                }
                // Set matching threshold
                biometricClient.MatchingThreshold = 48;

                // Set matching speed
                biometricClient.FingersMatchingSpeed = NMatchingSpeed.Low;

                // Identify probe subject
                status = biometricClient.Identify(probeSubject);

                lblStatus.Text = "Identifying start";

                if (status == NBiometricStatus.Ok)
                {
                    foreach (var matchingResult in probeSubject.MatchingResults)
                    {
                        //Console.WriteLine("Matched with ID: '{0}' with score {1}", matchingResult.Id, matchingResult.Score);
                        int score = matchingResult.Score;
                        if (score > highestMatch)
                        {
                            Console.WriteLine(@"template[{0}] scored {1} {2}", matchingResult.Id, score, score > 0 ? "(Matched)" : "");
                            int intIndex = 0;
                            Int32.TryParse(matchingResult.Id, out intIndex);
                            Console.WriteLine("student number matched: " + students[intIndex - 1]);
                            highestMatch = score;
                            highestStudent = students[intIndex];
                            matchedFingerID = intIndex % 10;
                            matchedScore = score;
                        }
                    }
                }
                else if (status == NBiometricStatus.MatchNotFound)
                {
                    Console.WriteLine("Match not found!");
                }
                else
                {
                    Console.WriteLine("Matching failed! Status: {0}", status);
                    
                }
                
                Console.WriteLine(@"=== identification finished ===");

                //return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                
            }
            finally
            {
                lblStatus.Text = "Releasing License";
                Application.DoEvents();
                NLicense.ReleaseComponents(Components);

                
                lblStatus.Text = "License Released";
                Application.DoEvents();
            }

            return highestStudent;
        }

        private static NSubject CreateSubject(NTemplate template, string subjectId)
        {
            var subject = new NSubject();
            subject.SetTemplate(template);
            subject.Id = subjectId;
            return subject;
        }

        private void DisplayMatchResult(string MatchedStudent)
        {
            if (MatchedStudent != "")
            {
                lblMatchResult.Text = "Matched Student: " + MatchedStudent + ". If this is not correct. Please enter student number then press Continue";
                lblMatchResult.AutoSize = true;
                lblMatchResult.MaximumSize = new Size(pnlIdentifyAction.Width / 5 * 3, 250);
                lblMatchResult.Location = new Point((pnlIdentifyAction.Width / 2) - (lblMatchResult.Width / 2), nFView1.Bottom + 10);
                txtMatchStudentNumber.Location = new Point(pnlIdentifyAction.Width / 10 * 3, lblMatchResult.Bottom + 10);
                txtMatchStudentNumber.Enabled = true;
                txtMatchStudentNumber.Text = MatchedStudent;
                txtMatchStudentNumber.Focus();
                txtMatchStudentNumber.SelectAll();

                matchedStudentNumber = MatchedStudent;
                btnMatchConfirm.Location = new Point((pnlIdentifyAction.Width / 5 * 1 + 20), txtMatchStudentNumber.Bottom + 10);
                btnMatchConfirm.Enabled = true;
                btnMatchRetry.Enabled = true;
                pnlIdentifyAction.Refresh();
                bckScanFinger.CancelAsync();
                //cancelMatchScan = true;

                MatchedStudent = "";
            }
            else
            {
                matchedStudentNumber = "";
                lblMatchResult.Text = "Could not identify the student.  Please type your student number and press Continue";
                lblMatchResult.AutoSize = true;
                lblMatchResult.MaximumSize = new Size(pnlIdentifyAction.Width / 5 * 3, 250);
                lblMatchResult.Location = new Point((pnlIdentifyAction.Width / 2) - (lblMatchResult.Width / 2), nFView1.Bottom + 10);
                txtMatchStudentNumber.Location = new Point(pnlIdentifyAction.Width / 10 * 3, lblMatchResult.Bottom + 10);
                txtMatchStudentNumber.Enabled = true;
                txtMatchStudentNumber.Text = MatchedStudent;
                txtMatchStudentNumber.Focus();
                txtMatchStudentNumber.ForeColor = Color.Red;
                txtMatchStudentNumber.SelectAll();

                btnMatchConfirm.Location = new Point((pnlIdentifyAction.Width / 5 * 1 + 20), txtMatchStudentNumber.Bottom + 10);
                btnMatchConfirm.Enabled = false;
                btnMatchRetry.Enabled = true;
                btnMatchRetry.Visible = true;
                pnlIdentifyAction.Refresh();
                bckScanFinger.CancelAsync();
            }

        }

        private void txtMatchStudentNumber_InputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            txtMatchStudentNumber.ForeColor = Color.Red;
            btnMatchConfirm.Enabled = false;
        }

        private void txtMatchStudentNumber_TextChanged(object sender, EventArgs e)
        {
            if (validStudents.Contains(txtMatchStudentNumber.Text) && txtMatchStudentNumber.Text.Length != 0)
            {
                txtMatchStudentNumber.ForeColor = Color.Black;
                btnMatchConfirm.Enabled = true;
                btnMatchConfirm.Focus();
                btnMatchConfirm.Select();
            }
            else
            {
                txtMatchStudentNumber.ForeColor = Color.Red;
                btnMatchConfirm.Enabled = false;
            }
        }

        private void MisMatch(NFRecord template)
        {
            lblMatchResult.Text = "Please type studentnumber";
            txtStudentNumber.Visible = true;
            btnMatchConfirm.Visible = true;
            btnMatchConfirm.Text = "Continue";
            btnMatchRetry.Visible = true;
        }

        private void btnMatchConfirm_Click(object sender, EventArgs e)
        {
            //cancelMatchScan = true;
            bool enrolledForCourse = true;
            bckScanFinger.CancelAsync();
            Application.DoEvents();
            attendanceRecord NewAttendance = new attendanceRecord();
            student ThisMatchedStudent = dlRegistration.GetStudentFromStudentNumber(txtMatchStudentNumber.Text);
            course EnrolledCourse = new course("a","a");

            //See if there were multiple course codes selected.  If more than one selected see for which one the student is registered
            if (cmbCourses.Items.Count >= 1)
            {
                List<course> courses = dlRegistration.getCoursesForStudentNumber(ThisMatchedStudent.StudentNumber);
                List<course> _selectedCourses = new List<course>();
                foreach (course courseItem in cmbCourses.Items)
                {
                    course _tempCourse = new course(courseItem.CourseCode, courseItem.Description);
                    _selectedCourses.Add(_tempCourse);
                }

                foreach (course courseItem in _selectedCourses)
                {
                    foreach (course matchCourse in courses)
                    {
                        if (matchCourse.CourseCode == courseItem.CourseCode)
                            EnrolledCourse = new course(matchCourse.CourseCode, matchCourse.Description);
                    }
              
                }

                //Lets make sure the student is enrolled in this course
                if (EnrolledCourse.CourseCode == "a")
                {
                    enrolledForCourse = false;
                    course tempCourse = (course)cmbCourses.Items[0];
                    EnrolledCourse = new course(tempCourse.CourseCode, tempCourse.Description);
                }
            }
            else
            {

                course tempCourse = (course)cmbCourses.Items[0];
                EnrolledCourse = new course(tempCourse.CourseCode,tempCourse.Description);
            }

            NBuffer BufferTemplate = fingerprintTemplate.Save();

            byte[] templateBuffer = BufferTemplate.ToArray();
            if (ThisMatchedStudent.Enrolled)
            {
                //If the student has a registered fingerprint then check the grace setting.
                //We use the score2 pvtComplSyncClass as the grace setting
                //Before we confirm the student as attended we have to check the student number typed in the text box
                //if the textbox student number is the same as the matchedStudentNumber then
                //we can use the ThisMatchedStudent otherwise
                //we have match the student again using the given template
                //and use that as the category and score result
                if (txtMatchStudentNumber.Text == matchedStudentNumber)
                {
                    NewAttendance.FingerAID = matchedFingerID - 1;
                    NewAttendance.FingerAImage = new byte[0];
                    NewAttendance.FingerATemplate = templateBuffer;
                    NewAttendance.FingerBID = 0;
                    NewAttendance.FingerBImage = new byte[0];
                    NewAttendance.FingerBTemplate = new byte[0];
                    NewAttendance.ScanResult = "A";
                    NewAttendance.score1 = matchedScore;
                    NewAttendance.score2 = 0;
                    strConfirmMessage = "Identified: " + ThisMatchedStudent.FirstName + " " + ThisMatchedStudent.LastName + ". Student number: " + ThisMatchedStudent.StudentNumber;
                    lblConfirmMessage.ForeColor = Color.Green;
                }
                else
                {
                    //The student changed the student number.  This means that we now have to first see if the template scanned
                    //belong to the new studentnumber
                    if (VerifyFingerprint(ThisMatchedStudent, fingerprintTemplate))
                    {
                        NewAttendance.FingerAID = matchedFingerID - 1;
                        NewAttendance.FingerAImage = new byte[0];
                        NewAttendance.FingerATemplate = templateBuffer;
                        NewAttendance.FingerBID = 0;
                        NewAttendance.FingerBImage = new byte[0];
                        NewAttendance.FingerBTemplate = new byte[0];
                        if (enrolledForCourse)
                        {
                            NewAttendance.ScanResult = "A";
                        }
                        else
                        {
                            NewAttendance.ScanResult = "B";
                        }
                        
                        NewAttendance.score1 = matchedScore;
                        NewAttendance.score2 = 0;
                        strConfirmMessage = "Identified: " + ThisMatchedStudent.FirstName + " " + ThisMatchedStudent.LastName + ". Student number: " + ThisMatchedStudent.StudentNumber;
                        lblConfirmMessage.ForeColor = Color.Green;
                    }
                    else
                    {
                        //If the template does not belong to the new student then we need to look at the grace
                        //If grace is anything less than 3 then we add the matched FingedID and template, but with a score
                        //of 0 and a ScanResult of "C"

                        int grace = 0;
                        bool converted = Int32.TryParse(ThisMatchedStudent.CardHistory, out grace);
                        if (converted)
                        {
                            if (grace < 3)
                            {
                                NewAttendance.FingerAID = 6;
                                NewAttendance.FingerAImage = new byte[0];
                                NewAttendance.FingerATemplate = templateBuffer;
                                NewAttendance.FingerBID = 0;
                                NewAttendance.FingerBImage = new byte[0];
                                NewAttendance.FingerBTemplate = new byte[0];
                                NewAttendance.ScanResult = "C";
                                NewAttendance.score1 = 0;
                                NewAttendance.score2 = 0;

                                strConfirmMessage = "No Match: " + ThisMatchedStudent.FirstName + " " + ThisMatchedStudent.LastName + ". Student number: " + ThisMatchedStudent.StudentNumber;
                                lblConfirmMessage.ForeColor = Color.Red;
                                //We have to update the registration system with the grace number
                            }
                            else
                            {
                                //Fingerprints didn't match give it a "D" score
                                NewAttendance.FingerAID = 0;
                                NewAttendance.FingerAImage = new byte[0];
                                NewAttendance.FingerATemplate = new byte[0];
                                NewAttendance.FingerBID = 0;
                                NewAttendance.FingerBImage = new byte[0];
                                NewAttendance.FingerBTemplate = new byte[0];
                                NewAttendance.ScanResult = "D";
                                NewAttendance.score1 = 0;
                                NewAttendance.score2 = 0;
                                strConfirmMessage = "No Match: " + ThisMatchedStudent.FirstName + " " + ThisMatchedStudent.LastName + ". Student number: " + ThisMatchedStudent.StudentNumber;
                                lblConfirmMessage.ForeColor = Color.Red;
                            }

                        }
                        else
                        {   //If the conversion failed then there were no grace period then we can update the user

                            NewAttendance.FingerAID = 6;
                            NewAttendance.FingerAImage = new byte[0];
                            NewAttendance.FingerATemplate = templateBuffer;
                            NewAttendance.FingerBID = 0;
                            NewAttendance.FingerBImage = new byte[0];
                            NewAttendance.FingerBTemplate = new byte[0];
                            NewAttendance.ScanResult = "C";
                            NewAttendance.score1 = 0;
                            NewAttendance.score2 = 0;
                            strConfirmMessage = "No Match: " + ThisMatchedStudent.FirstName + " " + ThisMatchedStudent.LastName + ". Student number: " + ThisMatchedStudent.StudentNumber;
                            lblConfirmMessage.ForeColor = Color.Red;
                            //We have to update the registration system with the grace number   
                        }   
                    }
                    
                }
                
                //If the grace setting is anything less than 3 then mark the student's record as graced
                //and increment the grace
                NewAttendance.StudentNumber = ThisMatchedStudent.StudentNumber;
                NewAttendance.CardNumber = ThisMatchedStudent.CardNumber;
                NewAttendance.AuditNumber = "";
                NewAttendance.ClassTime = DateTime.Now;
                NewAttendance.CourseCode = EnrolledCourse.CourseCode;
                
                NewAttendance.TimeStamp = DateTime.Now;
                NewAttendance.lat = "";
                NewAttendance.lon = "";
            }
            else
            {
                //What happens if the student is not enrolled?

                //If the student is not fingerprint enrolled then create a record in the registration DB for the user
                //and mark the grace as 1
                NewAttendance.StudentNumber = txtMatchStudentNumber.Text;
                NewAttendance.CardNumber = "";
                NewAttendance.AuditNumber = "";
                NewAttendance.ClassTime = DateTime.Now;
                NewAttendance.CourseCode = EnrolledCourse.CourseCode;
                NewAttendance.FingerAID = 6;
                NewAttendance.FingerAImage = new byte[0];
                NewAttendance.FingerATemplate = templateBuffer;
                NewAttendance.FingerBID = 0;
                NewAttendance.FingerBImage = new byte[0];
                NewAttendance.FingerBTemplate = new byte[0];
                NewAttendance.ScanResult = "X";
                NewAttendance.score1 = 0;
                NewAttendance.TimeStamp = DateTime.Now;
                NewAttendance.lat = "";
                NewAttendance.lon = "";

                universityStudent tempStudent = new universityStudent();
                tempStudent = dlRegistration.GetUniversityStudentFromStudentNumber(txtMatchStudentNumber.Text);


                if (tempStudent.FirstName != null || tempStudent.LastName != null)
                {
                    
                    strConfirmMessage = "Fingerprint not enrolled: " + ThisMatchedStudent.FirstName + " " + ThisMatchedStudent.LastName + ". Student number: " + ThisMatchedStudent.StudentNumber;
                    lblConfirmMessage.ForeColor = Color.Orange;
                }
                else
                {
                    strConfirmMessage = "Fingerprint not enrolled: Student number: " + ThisMatchedStudent.StudentNumber;
                    lblConfirmMessage.ForeColor = Color.Orange;
                }

                //We have to now register the student's fingerprints here
                
            }

            dlExamID.InsertAttendanceRecord(NewAttendance);

            //Prepare the screen for next student
            btnMatchConfirm.Enabled = false;
            txtMatchStudentNumber.Enabled = false;
            lblMatchResult.Text = "";
            txtMatchStudentNumber.Text = "";
            //nfView1.Template = null;
            //nfView1.Image = null;
            //nfView1.ResultImage = null;
            nFView1.Finger = null;
            lblIdentifyActionMessage.Text = "Please scan right index finger";

            loadConfirmMessage();

        }

        private void loadConfirmMessage()
        {
            
            pnlConfirmMessage.Dock = DockStyle.Fill;
            Application.DoEvents();
            lblConfirmMessage.AutoSize = true;
            lblConfirmMessage.TextAlign = ContentAlignment.MiddleCenter;
            lblConfirmMessage.Font = new Font("Arial", 25);
            lblConfirmMessage.MaximumSize = new Size(pnlMain.Width / 5 * 4 ,200);
            lblConfirmMessage.MinimumSize = new Size(pnlMain.Width / 10 * 3, 75);
            lblConfirmMessage.Text = strConfirmMessage;
            lblConfirmMessage.Location = new Point((pnlMain.Width - lblConfirmMessage.Width) / 2, 75);

            btnConfirmMessage.Font = new Font("Arial", 25);
            btnConfirmMessage.Width = pnlMain.Width / 5 * 4;
            btnConfirmMessage.Height = 75;
            btnConfirmMessage.Location = new Point(pnlMain.Width / 10 * 1, pnlMain.Bottom - 150);
            btnConfirmMessage.TextAlign = ContentAlignment.MiddleCenter;
            btnConfirmMessage.Text = "Continue";

            Application.DoEvents();
            pnlConfirmMessage.Visible = true;

        }

        private void btnConfirmMessage_Click(object sender, EventArgs e)
        {
            pnlConfirmMessage.Visible = false;

            //Start scanning fingerpint again
            matchedStudentNumber = "";
            matchedFingerID = 0;
            matchedScore = 0;
            while (bckScanFinger.IsBusy) { };
            bckScanFinger.RunWorkerAsync();
        }
        
        #endregion

        #region IdetnificationCancel
        private void LoadIdentificationCancel()
        {
            pnlConfirmCancel.Dock = DockStyle.Fill;
            int Width = pnlIdentifyAction.Width / 5 * 3;
            
            btnRetryFingerScan.Visible = true;
            btnRetryFingerScan.Font = new Font("Arial", 25);
            btnRetryFingerScan.Width = Width;
            btnRetryFingerScan.Height = 75;
            //btnMatchConfirm.Location = new Point(((pnlIdentifyAction.Width / 2) / 5 * 1), txtMatchStudentNumber.Location.Y + 76);
            btnRetryFingerScan.Location = new Point((pnlIdentifyAction.Width / 5 * 1), 10);
            btnRetryFingerScan.Enabled = true;
            btnRetryFingerScan.Text = "Retry fingerprint scan";

            btnCancelToMain.Visible = true;
            btnCancelToMain.Font = new Font("Arial", 25);
            btnCancelToMain.Width = Width;
            btnCancelToMain.Height = 75;
            //btnMatchConfirm.Location = new Point(((pnlIdentifyAction.Width / 2) / 5 * 1), txtMatchStudentNumber.Location.Y + 76);
            btnCancelToMain.Location = new Point((pnlIdentifyAction.Width / 5 * 1), btnRetryFingerScan.Bottom + 10);
            btnCancelToMain.Enabled = true;
            btnCancelToMain.Text = "Cancel class scan";

            txtCancelScanPin.Visible = true;
            txtCancelScanPin.Font = new Font("Arial", 25);
            txtCancelScanPin.Height = 75;
            //btnMatchConfirm.Location = new Point(((pnlIdentifyAction.Width / 2) / 5 * 1), txtMatchStudentNumber.Location.Y + 76);
            txtCancelScanPin.Location = new Point((pnlIdentifyAction.Width - txtCancelScanPin.Width) / 2, btnCancelToMain.Bottom + 10);
            txtCancelScanPin.Enabled = true;
            txtCancelScanPin.Text = "";

            

            pnlConfirmCancel.Visible = true;
        }

        private void btnRetryFingerScan_Click(object sender, EventArgs e)
        {
            RetryFingerScan();
        }

        private void btnMatchRetry_Click(object sender, EventArgs e)
        {
            RetryFingerScan();
        }

        private void RetryFingerScan()
        {
            pnlConfirmCancel.Visible = false;

            cancelMatchScan = true;
            bckScanFinger.CancelAsync();
            Application.DoEvents();
            const string Components = "Biometrics.FingerExtraction,Devices.FingerScanners";
            NLicense.ReleaseComponents(Components);

            //nfView1.Template = null;
            nFView1.Finger = null;
            //nfView1.ResultImage = null;
            
            txtMatchStudentNumber.Text = "";

            IdentifyActionScreen();
            pnlIdentifyAction.Refresh();
            ScannedTemplated = false;

            //See if a license can be obtained

            if (!NLicense.ObtainComponents(licServer, 5000, Components))
            {
                //Set the scanstatus to cancelled
                GlobalScanStatus = ScanStatus.NoLicenseObtained;
            }

            if (!bckScanFinger.IsBusy)
            {
                bckScanFinger.RunWorkerAsync();
            }
        }

        private void btnCancelToMain_Click(object sender, EventArgs e)
        {
            if (breakoutPin == txtCancelScanPin.Text)
            {

                pnlConfirmCancel.Visible = false;
                nFView1.Visible = false;
                cancelMatchScan = true;
                bckScanFinger.CancelAsync();
                Application.DoEvents();
                const string Components = "Biometrics.FingerExtraction,Devices.FingerScanners";
                NLicense.ReleaseComponents(Components);
                pnlIdentifyAction.Visible = false;
                LoadPanelMain();
            }
            else
            {
                pnlConfirmCancel.Visible = false;

                cancelMatchScan = true;
                bckScanFinger.CancelAsync();
                Application.DoEvents();
                const string Components = "Biometrics.FingerExtraction,Devices.FingerScanners";
                NLicense.ReleaseComponents(Components);

                //nfView1.Template = null;
                nFView1.Finger = null;
                //nfView1.ResultImage = null;
                
                txtMatchStudentNumber.Text = "";

                IdentifyActionScreen();
                pnlIdentifyAction.Refresh();
                ScannedTemplated = false;

                //See if a license can be obtained

                if (!NLicense.ObtainComponents(licServer, 5000, Components))
                {
                    //Set the scanstatus to cancelled
                    GlobalScanStatus = ScanStatus.NoLicenseObtained;
                }

                if (!bckScanFinger.IsBusy)
                {
                    bckScanFinger.RunWorkerAsync();
                }
            }
        }



        #endregion

        #region Main Verification

        private void LoadPanelVerification()
        {
            pnlMain.Visible = false;

            int Width, Height;
            Width = Main.ActiveForm.Width;
            Height = Main.ActiveForm.Height;
            pnlVerification.Dock = DockStyle.Fill;

            VerificationStartButton();
            VerificationCancel();
            pnlVerification.Visible = true;
        }

        private void VerificationStartButton()
        {
            int Width, Height;
            Width = (pnlMain.Width / 5) * 1;

            btnVerify.Width = Width;
            btnVerify.Height = 75;
            btnVerify.Font = new Font("Arial", 25);
            Height = pnlVerification.Height - btnVerify.Height;
            btnVerify.Location = new Point((pnlVerification.Width - Width) / 2, 375);
            btnVerify.Font = new Font("Arial", 25);
        }

        private void VerificationCancel()
        {
            int Width, Height;
            Width = (pnlMain.Width / 5) * 1;

            btnVerifyCancel.Width = Width;
            btnVerifyCancel.Height = 75;
            Height = pnlIdentification.Height - btnVerifyCancel.Height;
            btnVerifyCancel.Location = new Point(0, Height);
            btnVerifyCancel.Font = new Font("Arial", 25);

        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            loadVerificationAction(txtStudentNumber.Text.ToLower());
        }

        private void btnVerifyCancel_Click(object sender, EventArgs e)
        {
            pnlVerification.Visible = true;
            pnlMain.Visible = false;
        }
        #endregion

        #region Main VerificationAction

        private void loadVerificationAction(string StudentNumber)
        {
            student currentStudent = dlRegistration.GetStudentFromStudentNumber(StudentNumber);
            if (currentStudent.Enrolled)
            {

            }
            else
            {
            }
            
        }

        private void bckScanFinger_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (ScannedTemplated)
            {
                string matchedStudentNumber = "";
                studentFinger = new NFinger();

                //NSubject subject = new NSubject();
                studentFinger = studentSubject.Fingers[0];
                
                //nFView1.Finger = finger;
                //nFView1.MinutiaColor = Color.Red;
                //nFView1.Finger.GetBinarizedImage(false);
                //nFView1.ShownImage = Neurotec.Biometrics.Gui.ShownImage.Result;
                //nFView1.Finger.Image = fingerprintImage;

                //nfView1.Template = fingerprintTemplate;
                //nfView1.ShownImage = Neurotec.Biometrics.Gui.ShownImage.Result;
                //nfView1.ResultImage = fingerprintImage.ToBitmap();
                
                //nfView1.ShowTree = true;
                txtMatchStudentNumber.Enabled = true;
                if (fingerprintImage.Width > fingerprintImage.Height)
                {
                    nFView1.Zoom = (float)nFView1.Width / (float)fingerprintImage.Width;
                }
                else
                {
                    nFView1.Zoom = (float)nFView1.Height / (float)fingerprintImage.Height;
                }
                matchedStudentNumber = MatchStudents(fingerprintTemplate, lstStudents);
                DisplayMatchResult(matchedStudentNumber);
                              
                ScannedTemplated = false;
            }
        }


        #endregion

        #region StatusInfo

        private void StatusPanel()
        {
            pnlStatusInfo.Dock = DockStyle.Fill;

            lblRegisStatus.Location = new Point(10, 10);
            lblRegisStatus.Font = new Font("Arial", 25);

            GetSyncStatus();
            if (pvtSyncPullBusy)
            {
                lblRegisStatus.Text = "Synchronising " + (pvtSyncPullTotalRecords - pvtSyncPullRecordsLeft) + " registration records of " + pvtSyncPullTotalRecords + " records.";
            }
            if (pvtSyncPushBusy)
            {
                lblRegisStatus.Text = "Pushing registration records to server";
            }
            if (!pvtSyncPushBusy && !pvtSyncPullBusy)
            {
                lblRegisStatus.Text = "Reg synchronised: " + pvtSyncLastRegis.ToString();
            }

            lblExamIDStatus.Location = new Point(10, lblRegisStatus.Bottom + 10);
            lblExamIDStatus.Font = new Font("Arial", 25);
            if (pvtSyncExamIDBusy)
            {
                lblExamIDStatus.Text = "Synchronising " + pvtSyncExamIDTotalRecords + " captured records.";
            }
            if (!pvtSyncExamIDBusy)
            {
                lblExamIDStatus.Text = "Capt synchronised: " + pvtSyncLastExamID.ToString();
            }

            lblClassStatus.Location = new Point(10, lblExamIDStatus.Bottom + 10);
            lblClassStatus.Font = new Font("Arial", 25);
            if (pvtSyncClassBusy)
            {
                lblClassStatus.Text = "Synchronising " + (pvtSyncClassTotalRecords - pvtSyncClassRecordsLeft) + " class list records of " + pvtSyncClassTotalRecords;
            }
            if (!pvtSyncClassBusy)
            {
                lblClassStatus.Text = "Class synchronised: " + pvtSyncLastClass.ToString();
            }

            int Width = (pnlMain.Width / 5) * 4;
            btnStatusConfirm.Width = (pnlMain.Width / 5) * 4;
            btnStatusConfirm.Height = 75;
            btnStatusConfirm.Location = new Point((pnlMain.Width - Width) / 2, lblClassStatus.Bottom + 10);
            btnStatusConfirm.Font = new Font("Arial", 25);
            btnStatusConfirm.Text = "OK";
            
            pnlMain.Visible = false;
            pnlStatusInfo.Visible = true;
        }

        private void btnStatusConfirm_Click(object sender, EventArgs e)
        {
            pnlStatusInfo.Visible = false;
            pnlMain.Visible = true;
        }
        #endregion


        private void pnlIdentification_Resize(object sender, EventArgs e)
        {
            
        }

        private void bckScanFinger_DoWork(object sender, DoWorkEventArgs e)
        {
            const string Components = "Biometrics.FingerExtraction,Devices.FingerScanners";
            BackgroundWorker bck = (BackgroundWorker)sender;
            //try
            //{
                NBiometricClient biometricClient = new NBiometricClient { UseDeviceManager = true };
                var devMan = biometricClient.DeviceManager;
                studentSubject = new NSubject();
                studentFinger = new NFinger();
                
                devMan.DeviceTypes = NDeviceType.FingerScanner;

                if (!NLicense.ObtainComponents("/local", 5000, Components))
                {
                    throw new ApplicationException(string.Format("Could not obtain licenses for components: {0}", Components));
                }

                //initialize the NDeviceManager
                devMan.Initialize();

                int i;

                //get count of connected devices
                int count = devMan.Devices.Count;

                //See if a fingerprint scanner is connected
                if (count > 0)
                {
                    //What if there is more than one scanner
                }
                else
                {
                    GlobalScanStatus = ScanStatus.NoScanner;
                }

                if (count > 1)
                {
                    //What if there is more than one scanner.  We will use the first detected scanner.
                    for (i = 0; i < count; i++)
                    {
                        NDevice device = devMan.Devices[i];
                    }
                }
                    
                i = 0;

                //set the selected finger scanner as NBiometricClient Finger Scanner
                biometricClient.FingerScanner = (NFScanner)devMan.Devices[i];
                biometricClient.FingersCalculateNfiq = true;
                //biometricClient.FingersDeterminePatternClass = true;
                biometricClient.FingersReturnBinarizedImage = true;
                //biometricClient.FingersReturnRidgeSkeletonImage = true;
                
                //add NFinger to NSubject
                studentSubject.Fingers.Add(studentFinger);

                NImage image = null;
                while (image == null && !bckScanFinger.CancellationPending)
                {
                    if (bck.CancellationPending)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        //start capturing
                        
                        NBiometricStatus status = biometricClient.Capture(studentSubject);
                        if (status != NBiometricStatus.Ok)
                        {
                            Console.WriteLine("Failed to capture: " + status);
                            //return -1;
                        }

                        //Set finger template size (recommended, for enroll to database, is large) (optional)
                        biometricClient.FingersTemplateSize = NTemplateSize.Large;

                        //Create template from added finger image
                        status = biometricClient.CreateTemplate(studentSubject);
                        
                        //biometricClient.FingersMaximalRotation
                        if (status == NBiometricStatus.Ok && !bckScanFinger.CancellationPending)
                        {
                            //NBuffer templateBuffer = subject.GetTemplateBuffer();
                            fingerprintTemplate = studentSubject.GetTemplate();

                            nFView1.ShownImage = Neurotec.Biometrics.Gui.ShownImage.Result;
                            nFView1.Finger = studentFinger;
                            
                            fingerprintImage = studentSubject.Fingers[0].Image;
                            fingerprintImage = studentFinger.Image;
                            //finger.GetImage
                            image = fingerprintImage;
                            
                            GlobalScanStatus = ScanStatus.TemplateCreated;
                            ScannedTemplated = true;
                            bck.ReportProgress(1);
                        }
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    ScanError = ex.Message;
            //}

            GlobalScanStatus = ScanStatus.Cancelled;
            NLicense.ReleaseComponents(Components);
        }

        private void bckLoadTemplates_DoWork(object sender, DoWorkEventArgs e)
        {
            
            int TotalStudents = 0;
            int CurrentStudents = 0;
            int progress = 0;
            List<course> lstCourses = new List<course>();
            foreach (course Course in cmbCourses.Items)
            {
                lstCourses.Add(Course);
            }
            _getStudents = new dlDataHelper(lstCourses);
            _getStudents.StartGetStudents();
            TotalStudents = _getStudents.TotalNumberOfStudents;
            while ((!_getStudents.isStopped)&&(!_getStudents.isCompleted))
            {
                if (bckLoadTemplates.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {
                    TotalStudents = _getStudents.TotalNumberOfStudents;
                    CurrentStudents = _getStudents.CurrentStudent;
                    progress = (int)(((float)CurrentStudents / (float)TotalStudents) * 100);
                    if ((progress >= 0) && (progress <= 100))
                        bckLoadTemplates.ReportProgress(progress);
                    System.Threading.Thread.Sleep(200);
                }

                
            }

            lstStudents = _getStudents.ListOfStudents;
        }

        private void bckLoadTemplates_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((e.ProgressPercentage>=0)&&(e.ProgressPercentage<=100))
            {
                pgbLoadTemplates.Value = e.ProgressPercentage;
                
            } 
        }

        private void bckLoadTemplates_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void bckScanFinger_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            
            //pnlIdentifyAction.Visible = false;
            //LoadPanelMain();
        }

        private bool VerifyFingerprint(student originalStudent, NTemplate template2)
        {
            const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching";
            bool matched = false;

            try
            {
                // obtain license
                if (!NLicense.ObtainComponents("/local", 5000, Components))
                {
                    Console.WriteLine(@"Could not obtain licenses for components: {0}", Components);
                    matched = false;
                }

                // extract reference template
                NTemplate referenceTemplate = template2;
                
                //For each finger in the NFRecord
                
                // extract candidate template
                NTemplate leftPinkie;
                NTemplate leftRing;
                NTemplate leftMiddle;
                NTemplate leftIndex;
                NTemplate leftThumb;
                NTemplate rightThumb;
                NTemplate rightIndex;
                NTemplate rightMiddle;
                NTemplate rightRing;
                NTemplate rightPinkie;
                int scoreLeftPinkie = 0;
                int scoreLeftRing = 0;
                int scoreLeftMiddle = 0;
                int scoreLeftIndex = 0;
                int scoreLeftThumb = 0;
                int scoreRightPinkie = 0;
                int scoreRightRing = 0;
                int scoreRightMiddle = 0;
                int scoreRightIndex = 0;
                int scoreRightThumb = 0;

                var biometricClient = new NBiometricClient();
                NSubject referenceSubject = CreateSubject(referenceTemplate, "0");
                NSubject candidateSubject;
                // Set matching threshold
                biometricClient.MatchingThreshold = 48;

                // Set matching speed
                biometricClient.FingersMatchingSpeed = NMatchingSpeed.Low;

                NBiometricStatus status;
                int intHighestScore = 0;
                if (originalStudent.LeftPinkie != null)
                {
                    leftPinkie = originalStudent.LeftPinkie;
                    candidateSubject = CreateSubject(leftPinkie, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreLeftPinkie = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }
                
                if (originalStudent.LeftRing != null)
                {
                    leftRing = originalStudent.LeftRing;
                    candidateSubject = CreateSubject(leftRing, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreLeftRing = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }

                if (originalStudent.LeftMiddle != null)
                {
                    leftMiddle = originalStudent.LeftMiddle;
                    candidateSubject = CreateSubject(leftMiddle, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreLeftMiddle = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }
                if (originalStudent.LeftIndex != null)
                {
                    leftIndex = originalStudent.LeftIndex;
                    candidateSubject = CreateSubject(leftIndex, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreLeftIndex = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }
                if (originalStudent.LeftThumb != null)
                {
                    leftThumb = originalStudent.LeftThumb;
                    candidateSubject = CreateSubject(leftThumb, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreLeftThumb = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }
                if (originalStudent.RightThumb != null)
                {
                    rightThumb = originalStudent.RightThumb;
                    candidateSubject = CreateSubject(rightThumb, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreRightThumb = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }

                if (originalStudent.RightIndex !=null)
                {
                    rightIndex = originalStudent.RightIndex;
                    candidateSubject = CreateSubject(rightIndex, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreRightIndex = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }
                if (originalStudent.RightMiddle != null)
                {
                    rightMiddle = originalStudent.RightMiddle;
                    candidateSubject = CreateSubject(rightMiddle, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreRightMiddle = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }
                if (originalStudent.RightRing != null)
                {
                    rightRing = originalStudent.RightRing;
                    candidateSubject = CreateSubject(rightRing, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreRightRing = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }
                if (originalStudent.RightPinkie != null)
                {
                    rightPinkie = originalStudent.RightPinkie;
                    candidateSubject = CreateSubject(rightPinkie, "1");
                    status = biometricClient.Verify(referenceSubject, candidateSubject);
                    if (status == NBiometricStatus.Ok || status == NBiometricStatus.MatchNotFound)
                    {
                        scoreRightPinkie = referenceSubject.MatchingResults[0].Score;
                        if (referenceSubject.MatchingResults[0].Score > intHighestScore)
                            intHighestScore = referenceSubject.MatchingResults[0].Score;
                    }
                }

                if (scoreLeftPinkie == intHighestScore)
                {
                    matched = true;
                    matchedFingerID = 1;
                }
                if (scoreLeftRing == intHighestScore)
                {
                    matched = true;
                    matchedFingerID = 2;
                }

                if (scoreLeftMiddle == intHighestScore)
                {
                    matched = true;
                    matchedFingerID = 3;
                }
                if (scoreLeftIndex == intHighestScore)
                {
                    matched = true;
                    matchedFingerID = 4;
                }
                if (scoreLeftThumb == intHighestScore)
                {
                    matched = true;
                    matchedFingerID = 5;
                }
                if (scoreRightPinkie == intHighestScore)
                {
                    matched = true;
                    matchedFingerID = 6;
                }
                if (scoreRightRing == intHighestScore)
                {
                    matched = true;
                    matchedFingerID = 7;
                }
                if (scoreRightMiddle == intHighestScore)
                { 
                    matched = true;
                    matchedFingerID = 8;
                }
                if (scoreRightIndex == intHighestScore)
                {
                    matched = true;
                    matchedFingerID = 9;
                }
                if (scoreRightThumb == intHighestScore)
                {
                    matched = true;
                    matchedFingerID = 10;
                }          
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //INeurotecException neurotecException = ex as INeurotecException;
                //if (neurotecException != null)
                //{
                //    return neurotecException.Code;
                //}
                matched = false;
            }
            finally
            {
                NLicense.ReleaseComponents(Components);                
            }
            return matched;
            
        }

        //private void tmrSync_Tick(object sender, EventArgs e)
        //{
        //    clsSynchronise mySync = new clsSynchronise();              
        //    mySync.syncRegis();
        //    mySync.syncExamID();
        //    mySync.syncClass();
        //}

        private void tmrSyncDBEvent(Object source, ElapsedEventArgs e)
        {
            if (!busySync)
            {
                string sSource;
                string sLog;
                string sEvent;

                sSource = "FingerMatch";
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
                EventLog.WriteEntry(sSource, "Completed Registration Sync");

                mySync.syncExamID();
                GetSyncStatus();
                EventLog.WriteEntry(sSource, "Completed ExamID Sync");

                mySync.syncClass();
                GetSyncStatus();
                EventLog.WriteEntry(sSource, "Completed Class Sync");
                busySync = false;
            }

            
            
        }

        private void GetSyncStatus()
            {
                pvtSyncClassBusy = !FingerprintMatch.Properties.Settings.Default.SyncComplClass;
                pvtSyncClassRecordsLeft = FingerprintMatch.Properties.Settings.Default.SyncClassRecordsLeft;
                pvtSyncClassTotalRecords = FingerprintMatch.Properties.Settings.Default.SyncClassTotalRecords;
                pvtSyncExamIDBusy = !FingerprintMatch.Properties.Settings.Default.SyncComplExamID;
                pvtSyncExamIDRecordsLeft = FingerprintMatch.Properties.Settings.Default.SyncExamIDRecordsLeft;
                pvtSyncExamIDTotalRecords = FingerprintMatch.Properties.Settings.Default.SyncExamIDRecordsLeft;
                pvtSyncPullBusy = !FingerprintMatch.Properties.Settings.Default.SyncComplPull;
                pvtSyncPullRecordsLeft = FingerprintMatch.Properties.Settings.Default.SyncPullRecordsLeft;
                pvtSyncPullTotalRecords = FingerprintMatch.Properties.Settings.Default.SyncPullTotalRecords;
                pvtSyncPushBusy = !FingerprintMatch.Properties.Settings.Default.SyncComplPush;
                pvtSyncLastClass = FingerprintMatch.Properties.Settings.Default.SyncLastClass;
                pvtSyncLastExamID = FingerprintMatch.Properties.Settings.Default.SyncLastExamID;
                pvtSyncLastRegis = FingerprintMatch.Properties.Settings.Default.SyncLastRegis;
            }

        private void InitSyncStatusInfo()
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
            pvtSyncLastClass = FingerprintMatch.Properties.Settings.Default.SyncLastClass;
            pvtSyncLastExamID = FingerprintMatch.Properties.Settings.Default.SyncLastExamID;
            pvtSyncLastRegis = FingerprintMatch.Properties.Settings.Default.SyncLastRegis;
        }

        private void txtMatchStudentNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((txtMatchStudentNumber.Text.Trim()).Length == 0)
            {
                txtMatchStudentNumber.Focus();
                SendKeys.Send("{HOME}");
                txtMatchStudentNumber.SelectionStart = 0;
                //txtMatchStudentNumber.Text = e.KeyChar.ToString();
            }
        }

        private void txtMatchStudentNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if ((txtMatchStudentNumber.Text.Trim()).Length == 1)
            {
                txtMatchStudentNumber.SelectionStart = 1;
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            Utilities.StartKeyboard();
        }

        private void Timer_ElapsedCallback(IAsyncResult result)
        {
            ElapsedEventHandler handler = result.AsyncState as ElapsedEventHandler;
            if (handler != null)
            {
                handler.EndInvoke(result);
            }
        }
        



        

    }
}
