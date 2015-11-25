namespace FingerprintMatch
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnMainButton2 = new System.Windows.Forms.Button();
            this.btnMainButton1 = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Button();
            this.pnlIdentification = new System.Windows.Forms.Panel();
            this.cmbCourses = new System.Windows.Forms.ListBox();
            this.btnAddCourse = new System.Windows.Forms.Button();
            this.cmbCourseList = new System.Windows.Forms.TextBox();
            this.btnStartId = new System.Windows.Forms.Button();
            this.txtBreakoutPin = new System.Windows.Forms.TextBox();
            this.lblBreakOutPin = new System.Windows.Forms.Label();
            this.lblSelectedValue = new System.Windows.Forms.Label();
            this.btnIdentificationCancel = new System.Windows.Forms.Button();
            this.lblIdentification = new System.Windows.Forms.Button();
            this.pnlIdentifyAction = new System.Windows.Forms.Panel();
            this.txtMatchStudentNumber = new System.Windows.Forms.MaskedTextBox();
            this.btnMatchRetry = new System.Windows.Forms.Button();
            this.btnMatchConfirm = new System.Windows.Forms.Button();
            this.btnCancelScan = new System.Windows.Forms.Button();
            this.pgbLoadTemplates = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblMatchResult = new System.Windows.Forms.Label();
            this.nfView1 = new Neurotec.Biometrics.Gui.NFView();
            this.lblIdentifyActionMessage = new System.Windows.Forms.Label();
            this.bckScanFinger = new System.ComponentModel.BackgroundWorker();
            this.bckLoadTemplates = new System.ComponentModel.BackgroundWorker();
            this.pnlVerification = new System.Windows.Forms.Panel();
            this.btnVerifyCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStudentNumber = new System.Windows.Forms.TextBox();
            this.btnVerify = new System.Windows.Forms.Button();
            this.pnlConfirmMessage = new System.Windows.Forms.Panel();
            this.btnConfirmMessage = new System.Windows.Forms.Button();
            this.lblConfirmMessage = new System.Windows.Forms.Label();
            this.pnlConfirmCancel = new System.Windows.Forms.Panel();
            this.txtCancelScanPin = new System.Windows.Forms.TextBox();
            this.btnCancelToMain = new System.Windows.Forms.Button();
            this.btnRetryFingerScan = new System.Windows.Forms.Button();
            this.tmrSync = new System.Windows.Forms.Timer(this.components);
            this.pnlStatusInfo = new System.Windows.Forms.Panel();
            this.btnStatusConfirm = new System.Windows.Forms.Button();
            this.lblExamIDStatus = new System.Windows.Forms.Label();
            this.lblRegisStatus = new System.Windows.Forms.Label();
            this.lblClassStatus = new System.Windows.Forms.Label();
            this.courseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pnlMain.SuspendLayout();
            this.pnlIdentification.SuspendLayout();
            this.pnlIdentifyAction.SuspendLayout();
            this.pnlVerification.SuspendLayout();
            this.pnlConfirmMessage.SuspendLayout();
            this.pnlConfirmCancel.SuspendLayout();
            this.pnlStatusInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.courseBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnSync);
            this.pnlMain.Controls.Add(this.btnMainButton2);
            this.pnlMain.Controls.Add(this.btnMainButton1);
            this.pnlMain.Controls.Add(this.lblTitle);
            this.pnlMain.Location = new System.Drawing.Point(13, 13);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(538, 161);
            this.pnlMain.TabIndex = 0;
            this.pnlMain.Visible = false;
            // 
            // btnSync
            // 
            this.btnSync.Location = new System.Drawing.Point(6, 94);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(75, 23);
            this.btnSync.TabIndex = 3;
            this.btnSync.Text = "Synchronise DB";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_click);
            // 
            // btnMainButton2
            // 
            this.btnMainButton2.Location = new System.Drawing.Point(6, 64);
            this.btnMainButton2.Name = "btnMainButton2";
            this.btnMainButton2.Size = new System.Drawing.Size(75, 23);
            this.btnMainButton2.TabIndex = 2;
            this.btnMainButton2.Text = "btnMainButton2";
            this.btnMainButton2.UseVisualStyleBackColor = true;
            this.btnMainButton2.Click += new System.EventHandler(this.btnMainbutton2_click);
            // 
            // btnMainButton1
            // 
            this.btnMainButton1.Location = new System.Drawing.Point(4, 34);
            this.btnMainButton1.Name = "btnMainButton1";
            this.btnMainButton1.Size = new System.Drawing.Size(75, 23);
            this.btnMainButton1.TabIndex = 1;
            this.btnMainButton1.Text = "btnMainButton1";
            this.btnMainButton1.UseVisualStyleBackColor = true;
            this.btnMainButton1.Click += new System.EventHandler(this.btnMainButton1_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(4, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(75, 23);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            this.lblTitle.UseVisualStyleBackColor = true;
            // 
            // pnlIdentification
            // 
            this.pnlIdentification.Controls.Add(this.cmbCourses);
            this.pnlIdentification.Controls.Add(this.btnAddCourse);
            this.pnlIdentification.Controls.Add(this.cmbCourseList);
            this.pnlIdentification.Controls.Add(this.btnStartId);
            this.pnlIdentification.Controls.Add(this.txtBreakoutPin);
            this.pnlIdentification.Controls.Add(this.lblBreakOutPin);
            this.pnlIdentification.Controls.Add(this.lblSelectedValue);
            this.pnlIdentification.Controls.Add(this.btnIdentificationCancel);
            this.pnlIdentification.Controls.Add(this.lblIdentification);
            this.pnlIdentification.Location = new System.Drawing.Point(13, 181);
            this.pnlIdentification.Name = "pnlIdentification";
            this.pnlIdentification.Size = new System.Drawing.Size(538, 373);
            this.pnlIdentification.TabIndex = 1;
            this.pnlIdentification.Visible = false;
            this.pnlIdentification.Resize += new System.EventHandler(this.pnlIdentification_Resize);
            // 
            // cmbCourses
            // 
            this.cmbCourses.FormattingEnabled = true;
            this.cmbCourses.Location = new System.Drawing.Point(343, 141);
            this.cmbCourses.Name = "cmbCourses";
            this.cmbCourses.Size = new System.Drawing.Size(120, 95);
            this.cmbCourses.TabIndex = 10;
            // 
            // btnAddCourse
            // 
            this.btnAddCourse.Location = new System.Drawing.Point(319, 81);
            this.btnAddCourse.Name = "btnAddCourse";
            this.btnAddCourse.Size = new System.Drawing.Size(75, 23);
            this.btnAddCourse.TabIndex = 8;
            this.btnAddCourse.Text = "Add";
            this.btnAddCourse.UseVisualStyleBackColor = true;
            this.btnAddCourse.Click += new System.EventHandler(this.btnAddCourse_Click);
            // 
            // cmbCourseList
            // 
            this.cmbCourseList.Location = new System.Drawing.Point(319, 54);
            this.cmbCourseList.Name = "cmbCourseList";
            this.cmbCourseList.Size = new System.Drawing.Size(100, 20);
            this.cmbCourseList.TabIndex = 7;
            this.cmbCourseList.TextChanged += new System.EventHandler(this.cmbCourseList_TextChanged);
            this.cmbCourseList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbCourseList_KeyDown);
            this.cmbCourseList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbCourseList_KeyPress);
            // 
            // btnStartId
            // 
            this.btnStartId.Location = new System.Drawing.Point(4, 213);
            this.btnStartId.Name = "btnStartId";
            this.btnStartId.Size = new System.Drawing.Size(75, 23);
            this.btnStartId.TabIndex = 6;
            this.btnStartId.Text = "Start Identification";
            this.btnStartId.UseVisualStyleBackColor = true;
            this.btnStartId.Click += new System.EventHandler(this.btnStartId_Click);
            // 
            // txtBreakoutPin
            // 
            this.txtBreakoutPin.Font = new System.Drawing.Font("Arial", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBreakoutPin.Location = new System.Drawing.Point(4, 161);
            this.txtBreakoutPin.Name = "txtBreakoutPin";
            this.txtBreakoutPin.Size = new System.Drawing.Size(100, 45);
            this.txtBreakoutPin.TabIndex = 5;
            this.txtBreakoutPin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblBreakOutPin
            // 
            this.lblBreakOutPin.AutoSize = true;
            this.lblBreakOutPin.Font = new System.Drawing.Font("Arial", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBreakOutPin.Location = new System.Drawing.Point(6, 120);
            this.lblBreakOutPin.Name = "lblBreakOutPin";
            this.lblBreakOutPin.Size = new System.Drawing.Size(200, 38);
            this.lblBreakOutPin.TabIndex = 4;
            this.lblBreakOutPin.Text = "Breakout Pin";
            // 
            // lblSelectedValue
            // 
            this.lblSelectedValue.AutoSize = true;
            this.lblSelectedValue.Location = new System.Drawing.Point(3, 103);
            this.lblSelectedValue.Name = "lblSelectedValue";
            this.lblSelectedValue.Size = new System.Drawing.Size(35, 13);
            this.lblSelectedValue.TabIndex = 3;
            this.lblSelectedValue.Text = "label1";
            this.lblSelectedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnIdentificationCancel
            // 
            this.btnIdentificationCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIdentificationCancel.Location = new System.Drawing.Point(4, 298);
            this.btnIdentificationCancel.Name = "btnIdentificationCancel";
            this.btnIdentificationCancel.Size = new System.Drawing.Size(75, 75);
            this.btnIdentificationCancel.TabIndex = 1;
            this.btnIdentificationCancel.Text = "Cancel";
            this.btnIdentificationCancel.UseVisualStyleBackColor = true;
            this.btnIdentificationCancel.Click += new System.EventHandler(this.btnIdentificationCancel_Click);
            // 
            // lblIdentification
            // 
            this.lblIdentification.AutoSize = true;
            this.lblIdentification.Location = new System.Drawing.Point(4, 4);
            this.lblIdentification.Name = "lblIdentification";
            this.lblIdentification.Size = new System.Drawing.Size(87, 23);
            this.lblIdentification.TabIndex = 0;
            this.lblIdentification.Text = "lblIdentification";
            this.lblIdentification.UseVisualStyleBackColor = true;
            // 
            // pnlIdentifyAction
            // 
            this.pnlIdentifyAction.BackColor = System.Drawing.SystemColors.Control;
            this.pnlIdentifyAction.Controls.Add(this.txtMatchStudentNumber);
            this.pnlIdentifyAction.Controls.Add(this.btnMatchRetry);
            this.pnlIdentifyAction.Controls.Add(this.btnMatchConfirm);
            this.pnlIdentifyAction.Controls.Add(this.btnCancelScan);
            this.pnlIdentifyAction.Controls.Add(this.pgbLoadTemplates);
            this.pnlIdentifyAction.Controls.Add(this.lblStatus);
            this.pnlIdentifyAction.Controls.Add(this.lblMatchResult);
            this.pnlIdentifyAction.Controls.Add(this.nfView1);
            this.pnlIdentifyAction.Controls.Add(this.lblIdentifyActionMessage);
            this.pnlIdentifyAction.Location = new System.Drawing.Point(557, 13);
            this.pnlIdentifyAction.Name = "pnlIdentifyAction";
            this.pnlIdentifyAction.Size = new System.Drawing.Size(538, 541);
            this.pnlIdentifyAction.TabIndex = 2;
            // 
            // txtMatchStudentNumber
            // 
            this.txtMatchStudentNumber.Location = new System.Drawing.Point(13, 418);
            this.txtMatchStudentNumber.Mask = "000000000";
            this.txtMatchStudentNumber.Name = "txtMatchStudentNumber";
            this.txtMatchStudentNumber.Size = new System.Drawing.Size(100, 20);
            this.txtMatchStudentNumber.TabIndex = 10;
            this.txtMatchStudentNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMatchStudentNumber.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtMatchStudentNumber_InputRejected);
            this.txtMatchStudentNumber.TextChanged += new System.EventHandler(this.txtMatchStudentNumber_TextChanged);
            this.txtMatchStudentNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMatchStudentNumber_KeyPress);
            this.txtMatchStudentNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMatchStudentNumber_KeyUp);
            // 
            // btnMatchRetry
            // 
            this.btnMatchRetry.Location = new System.Drawing.Point(13, 484);
            this.btnMatchRetry.Name = "btnMatchRetry";
            this.btnMatchRetry.Size = new System.Drawing.Size(75, 23);
            this.btnMatchRetry.TabIndex = 9;
            this.btnMatchRetry.Text = "noButton";
            this.btnMatchRetry.UseVisualStyleBackColor = true;
            this.btnMatchRetry.Click += new System.EventHandler(this.btnMatchRetry_Click);
            // 
            // btnMatchConfirm
            // 
            this.btnMatchConfirm.Location = new System.Drawing.Point(13, 454);
            this.btnMatchConfirm.Name = "btnMatchConfirm";
            this.btnMatchConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnMatchConfirm.TabIndex = 8;
            this.btnMatchConfirm.Text = "confirmButton";
            this.btnMatchConfirm.UseVisualStyleBackColor = true;
            this.btnMatchConfirm.Click += new System.EventHandler(this.btnMatchConfirm_Click);
            // 
            // btnCancelScan
            // 
            this.btnCancelScan.Location = new System.Drawing.Point(9, 511);
            this.btnCancelScan.Name = "btnCancelScan";
            this.btnCancelScan.Size = new System.Drawing.Size(75, 23);
            this.btnCancelScan.TabIndex = 5;
            this.btnCancelScan.Text = "Cancel";
            this.btnCancelScan.UseVisualStyleBackColor = true;
            this.btnCancelScan.Click += new System.EventHandler(this.btnCancelScan_Click);
            // 
            // pgbLoadTemplates
            // 
            this.pgbLoadTemplates.BackColor = System.Drawing.Color.Maroon;
            this.pgbLoadTemplates.Location = new System.Drawing.Point(13, 380);
            this.pgbLoadTemplates.Name = "pgbLoadTemplates";
            this.pgbLoadTemplates.Size = new System.Drawing.Size(100, 23);
            this.pgbLoadTemplates.TabIndex = 4;
            this.pgbLoadTemplates.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(13, 360);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "label1";
            // 
            // lblMatchResult
            // 
            this.lblMatchResult.AutoSize = true;
            this.lblMatchResult.Location = new System.Drawing.Point(13, 336);
            this.lblMatchResult.Name = "lblMatchResult";
            this.lblMatchResult.Size = new System.Drawing.Size(35, 13);
            this.lblMatchResult.TabIndex = 2;
            this.lblMatchResult.Text = "label1";
            this.lblMatchResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMatchResult.Visible = false;
            // 
            // nfView1
            // 
            this.nfView1.AutoSize = true;
            this.nfView1.BackColor = System.Drawing.SystemColors.Control;
            this.nfView1.Location = new System.Drawing.Point(13, 29);
            this.nfView1.MaximumSize = new System.Drawing.Size(300, 300);
            this.nfView1.MinimumSize = new System.Drawing.Size(300, 300);
            this.nfView1.MinutiaColor = System.Drawing.Color.Red;
            this.nfView1.Name = "nfView1";
            this.nfView1.NeighborMinutiaColor = System.Drawing.Color.Orange;
            this.nfView1.SelectedMinutiaColor = System.Drawing.Color.Magenta;
            this.nfView1.SelectedSingularPointColor = System.Drawing.Color.Magenta;
            this.nfView1.SingularPointColor = System.Drawing.Color.Red;
            this.nfView1.Size = new System.Drawing.Size(300, 300);
            this.nfView1.TabIndex = 1;
            this.nfView1.TreeColor = System.Drawing.Color.Crimson;
            this.nfView1.TreeMinutiaNumberDiplayFormat = Neurotec.Biometrics.Gui.MinutiaNumberDiplayFormat.DontDisplay;
            this.nfView1.TreeMinutiaNumberFont = new System.Drawing.Font("Arial", 10F);
            this.nfView1.TreeWidth = 2D;
            // 
            // lblIdentifyActionMessage
            // 
            this.lblIdentifyActionMessage.AutoSize = true;
            this.lblIdentifyActionMessage.Location = new System.Drawing.Point(6, 4);
            this.lblIdentifyActionMessage.Name = "lblIdentifyActionMessage";
            this.lblIdentifyActionMessage.Size = new System.Drawing.Size(145, 13);
            this.lblIdentifyActionMessage.TabIndex = 0;
            this.lblIdentifyActionMessage.Text = "Please scan right index finger";
            // 
            // bckScanFinger
            // 
            this.bckScanFinger.WorkerReportsProgress = true;
            this.bckScanFinger.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckScanFinger_DoWork);
            this.bckScanFinger.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bckScanFinger_ProgressChanged);
            this.bckScanFinger.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bckScanFinger_Completed);
            // 
            // bckLoadTemplates
            // 
            this.bckLoadTemplates.WorkerReportsProgress = true;
            this.bckLoadTemplates.WorkerSupportsCancellation = true;
            this.bckLoadTemplates.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckLoadTemplates_DoWork);
            this.bckLoadTemplates.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bckLoadTemplates_ProgressChanged);
            this.bckLoadTemplates.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bckLoadTemplates_Completed);
            // 
            // pnlVerification
            // 
            this.pnlVerification.Controls.Add(this.btnVerifyCancel);
            this.pnlVerification.Controls.Add(this.label1);
            this.pnlVerification.Controls.Add(this.txtStudentNumber);
            this.pnlVerification.Controls.Add(this.btnVerify);
            this.pnlVerification.Location = new System.Drawing.Point(13, 561);
            this.pnlVerification.Name = "pnlVerification";
            this.pnlVerification.Size = new System.Drawing.Size(538, 300);
            this.pnlVerification.TabIndex = 3;
            // 
            // btnVerifyCancel
            // 
            this.btnVerifyCancel.Location = new System.Drawing.Point(6, 274);
            this.btnVerifyCancel.Name = "btnVerifyCancel";
            this.btnVerifyCancel.Size = new System.Drawing.Size(75, 23);
            this.btnVerifyCancel.TabIndex = 3;
            this.btnVerifyCancel.Text = "Cancel";
            this.btnVerifyCancel.UseVisualStyleBackColor = true;
            this.btnVerifyCancel.Click += new System.EventHandler(this.btnVerifyCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 24.75F);
            this.label1.Location = new System.Drawing.Point(6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(261, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Student Number:";
            // 
            // txtStudentNumber
            // 
            this.txtStudentNumber.Location = new System.Drawing.Point(11, 46);
            this.txtStudentNumber.Name = "txtStudentNumber";
            this.txtStudentNumber.Size = new System.Drawing.Size(100, 20);
            this.txtStudentNumber.TabIndex = 1;
            this.txtStudentNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(11, 72);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(75, 23);
            this.btnVerify.TabIndex = 2;
            this.btnVerify.Text = "Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // pnlConfirmMessage
            // 
            this.pnlConfirmMessage.BackColor = System.Drawing.SystemColors.Control;
            this.pnlConfirmMessage.Controls.Add(this.btnConfirmMessage);
            this.pnlConfirmMessage.Controls.Add(this.lblConfirmMessage);
            this.pnlConfirmMessage.Location = new System.Drawing.Point(558, 561);
            this.pnlConfirmMessage.Name = "pnlConfirmMessage";
            this.pnlConfirmMessage.Size = new System.Drawing.Size(537, 300);
            this.pnlConfirmMessage.TabIndex = 4;
            // 
            // btnConfirmMessage
            // 
            this.btnConfirmMessage.Location = new System.Drawing.Point(0, 186);
            this.btnConfirmMessage.Name = "btnConfirmMessage";
            this.btnConfirmMessage.Size = new System.Drawing.Size(75, 23);
            this.btnConfirmMessage.TabIndex = 1;
            this.btnConfirmMessage.Text = "button1";
            this.btnConfirmMessage.UseVisualStyleBackColor = true;
            this.btnConfirmMessage.Click += new System.EventHandler(this.btnConfirmMessage_Click);
            // 
            // lblConfirmMessage
            // 
            this.lblConfirmMessage.AutoSize = true;
            this.lblConfirmMessage.Location = new System.Drawing.Point(4, 4);
            this.lblConfirmMessage.Name = "lblConfirmMessage";
            this.lblConfirmMessage.Size = new System.Drawing.Size(35, 13);
            this.lblConfirmMessage.TabIndex = 0;
            this.lblConfirmMessage.Text = "label2";
            // 
            // pnlConfirmCancel
            // 
            this.pnlConfirmCancel.Controls.Add(this.txtCancelScanPin);
            this.pnlConfirmCancel.Controls.Add(this.btnCancelToMain);
            this.pnlConfirmCancel.Controls.Add(this.btnRetryFingerScan);
            this.pnlConfirmCancel.Location = new System.Drawing.Point(1102, 13);
            this.pnlConfirmCancel.Name = "pnlConfirmCancel";
            this.pnlConfirmCancel.Size = new System.Drawing.Size(335, 272);
            this.pnlConfirmCancel.TabIndex = 5;
            // 
            // txtCancelScanPin
            // 
            this.txtCancelScanPin.Location = new System.Drawing.Point(4, 58);
            this.txtCancelScanPin.Name = "txtCancelScanPin";
            this.txtCancelScanPin.Size = new System.Drawing.Size(100, 20);
            this.txtCancelScanPin.TabIndex = 2;
            this.txtCancelScanPin.Visible = false;
            // 
            // btnCancelToMain
            // 
            this.btnCancelToMain.Location = new System.Drawing.Point(4, 29);
            this.btnCancelToMain.Name = "btnCancelToMain";
            this.btnCancelToMain.Size = new System.Drawing.Size(75, 23);
            this.btnCancelToMain.TabIndex = 1;
            this.btnCancelToMain.Text = "button1";
            this.btnCancelToMain.UseVisualStyleBackColor = true;
            this.btnCancelToMain.Click += new System.EventHandler(this.btnCancelToMain_Click);
            // 
            // btnRetryFingerScan
            // 
            this.btnRetryFingerScan.Location = new System.Drawing.Point(4, 4);
            this.btnRetryFingerScan.Name = "btnRetryFingerScan";
            this.btnRetryFingerScan.Size = new System.Drawing.Size(75, 23);
            this.btnRetryFingerScan.TabIndex = 0;
            this.btnRetryFingerScan.Text = "button1";
            this.btnRetryFingerScan.UseVisualStyleBackColor = true;
            this.btnRetryFingerScan.Click += new System.EventHandler(this.btnRetryFingerScan_Click);
            // 
            // tmrSync
            // 
            this.tmrSync.Interval = 60000;
 
            // 
            // pnlStatusInfo
            // 
            this.pnlStatusInfo.Controls.Add(this.btnStatusConfirm);
            this.pnlStatusInfo.Controls.Add(this.lblExamIDStatus);
            this.pnlStatusInfo.Controls.Add(this.lblRegisStatus);
            this.pnlStatusInfo.Controls.Add(this.lblClassStatus);
            this.pnlStatusInfo.Location = new System.Drawing.Point(1102, 292);
            this.pnlStatusInfo.Name = "pnlStatusInfo";
            this.pnlStatusInfo.Size = new System.Drawing.Size(335, 149);
            this.pnlStatusInfo.TabIndex = 6;
            // 
            // btnStatusConfirm
            // 
            this.btnStatusConfirm.Location = new System.Drawing.Point(7, 81);
            this.btnStatusConfirm.Name = "btnStatusConfirm";
            this.btnStatusConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnStatusConfirm.TabIndex = 1;
            this.btnStatusConfirm.Text = "button1";
            this.btnStatusConfirm.UseVisualStyleBackColor = true;
            this.btnStatusConfirm.Click += new System.EventHandler(this.btnStatusConfirm_Click);
            // 
            // lblExamIDStatus
            // 
            this.lblExamIDStatus.AutoSize = true;
            this.lblExamIDStatus.Location = new System.Drawing.Point(4, 57);
            this.lblExamIDStatus.Name = "lblExamIDStatus";
            this.lblExamIDStatus.Size = new System.Drawing.Size(35, 13);
            this.lblExamIDStatus.TabIndex = 0;
            this.lblExamIDStatus.Text = "label2";
            // 
            // lblRegisStatus
            // 
            this.lblRegisStatus.AutoSize = true;
            this.lblRegisStatus.Location = new System.Drawing.Point(4, 29);
            this.lblRegisStatus.Name = "lblRegisStatus";
            this.lblRegisStatus.Size = new System.Drawing.Size(35, 13);
            this.lblRegisStatus.TabIndex = 0;
            this.lblRegisStatus.Text = "label2";
            // 
            // lblClassStatus
            // 
            this.lblClassStatus.AutoSize = true;
            this.lblClassStatus.Location = new System.Drawing.Point(4, 4);
            this.lblClassStatus.Name = "lblClassStatus";
            this.lblClassStatus.Size = new System.Drawing.Size(35, 13);
            this.lblClassStatus.TabIndex = 0;
            this.lblClassStatus.Text = "label2";
            // 
            // courseBindingSource
            // 
            this.courseBindingSource.DataSource = typeof(FingerprintMatch.classes.course);
            // 
            // Main
            // 
            this.AcceptButton = this.btnVerify;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 873);
            this.Controls.Add(this.pnlStatusInfo);
            this.Controls.Add(this.pnlConfirmCancel);
            this.Controls.Add(this.pnlConfirmMessage);
            this.Controls.Add(this.pnlVerification);
            this.Controls.Add(this.pnlIdentifyAction);
            this.Controls.Add(this.pnlIdentification);
            this.Controls.Add(this.pnlMain);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Class Management";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlIdentification.ResumeLayout(false);
            this.pnlIdentification.PerformLayout();
            this.pnlIdentifyAction.ResumeLayout(false);
            this.pnlIdentifyAction.PerformLayout();
            this.pnlVerification.ResumeLayout(false);
            this.pnlVerification.PerformLayout();
            this.pnlConfirmMessage.ResumeLayout(false);
            this.pnlConfirmMessage.PerformLayout();
            this.pnlConfirmCancel.ResumeLayout(false);
            this.pnlConfirmCancel.PerformLayout();
            this.pnlStatusInfo.ResumeLayout(false);
            this.pnlStatusInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.courseBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button lblTitle;
        private System.Windows.Forms.Button btnMainButton1;
        private System.Windows.Forms.Panel pnlIdentification;
        private System.Windows.Forms.Button lblIdentification;
        private System.Windows.Forms.Button btnIdentificationCancel;
        private System.Windows.Forms.Label lblSelectedValue;
        private System.Windows.Forms.BindingSource courseBindingSource;
        private System.Windows.Forms.Button btnStartId;
        private System.Windows.Forms.TextBox txtBreakoutPin;
        private System.Windows.Forms.Label lblBreakOutPin;
        private System.Windows.Forms.Panel pnlIdentifyAction;
        private Neurotec.Biometrics.Gui.NFView nfView1;
        private System.Windows.Forms.Label lblIdentifyActionMessage;
        private System.ComponentModel.BackgroundWorker bckScanFinger;
        private System.Windows.Forms.Label lblMatchResult;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker bckLoadTemplates;
        private System.Windows.Forms.ProgressBar pgbLoadTemplates;
        private System.Windows.Forms.Button btnMainButton2;
        private System.Windows.Forms.Panel pnlVerification;
        private System.Windows.Forms.Button btnVerifyCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStudentNumber;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Panel pnlConfirmMessage;
        private System.Windows.Forms.Button btnCancelScan;
        private System.Windows.Forms.Button btnMatchRetry;
        private System.Windows.Forms.Button btnMatchConfirm;
        private System.Windows.Forms.MaskedTextBox txtMatchStudentNumber;
        private System.Windows.Forms.Button btnConfirmMessage;
        private System.Windows.Forms.Label lblConfirmMessage;
        private System.Windows.Forms.TextBox cmbCourseList;
        private System.Windows.Forms.Button btnAddCourse;
        private System.Windows.Forms.ListBox cmbCourses;
        private System.Windows.Forms.Panel pnlConfirmCancel;
        private System.Windows.Forms.Button btnCancelToMain;
        private System.Windows.Forms.Button btnRetryFingerScan;
        private System.Windows.Forms.TextBox txtCancelScanPin;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Timer tmrSync;
        private System.Windows.Forms.Panel pnlStatusInfo;
        private System.Windows.Forms.Button btnStatusConfirm;
        private System.Windows.Forms.Label lblExamIDStatus;
        private System.Windows.Forms.Label lblRegisStatus;
        private System.Windows.Forms.Label lblClassStatus;


    }
}

