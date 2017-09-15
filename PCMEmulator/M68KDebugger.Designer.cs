namespace PCMEmulator
{
    partial class M68KDebugger
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnStep = new System.Windows.Forms.Button();
            this.txtPPC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.cbS = new System.Windows.Forms.CheckBox();
            this.cbX = new System.Windows.Forms.CheckBox();
            this.cbN = new System.Windows.Forms.CheckBox();
            this.cbZ = new System.Windows.Forms.CheckBox();
            this.cbV = new System.Windows.Forms.CheckBox();
            this.cbC = new System.Windows.Forms.CheckBox();
            this.btnStep2 = new System.Windows.Forms.Button();
            this.txtPPCTill = new System.Windows.Forms.TextBox();
            this.txtIntMaskLevel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUSP = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSSP = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.txtTotalCycles = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbM = new System.Windows.Forms.CheckBox();
            this.txtPC = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtD0 = new System.Windows.Forms.TextBox();
            this.txtA0 = new System.Windows.Forms.TextBox();
            this.txtD6 = new System.Windows.Forms.TextBox();
            this.txtD5 = new System.Windows.Forms.TextBox();
            this.txtD4 = new System.Windows.Forms.TextBox();
            this.txtD3 = new System.Windows.Forms.TextBox();
            this.txtD2 = new System.Windows.Forms.TextBox();
            this.txtD1 = new System.Windows.Forms.TextBox();
            this.txtD7 = new System.Windows.Forms.TextBox();
            this.txtA1 = new System.Windows.Forms.TextBox();
            this.txtA6 = new System.Windows.Forms.TextBox();
            this.txtA5 = new System.Windows.Forms.TextBox();
            this.txtA4 = new System.Windows.Forms.TextBox();
            this.txtA3 = new System.Windows.Forms.TextBox();
            this.txtA2 = new System.Windows.Forms.TextBox();
            this.txtA7 = new System.Windows.Forms.TextBox();
            this.lblOp = new System.Windows.Forms.Label();
            this.txtOp = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtReadByte = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.btnWriteByte = new System.Windows.Forms.Button();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.btnReadWord = new System.Windows.Forms.Button();
            this.btnReadLong = new System.Windows.Forms.Button();
            this.btnWriteWord = new System.Windows.Forms.Button();
            this.btnWriteLong = new System.Windows.Forms.Button();
            this.btnTriggerInterrupt = new System.Windows.Forms.Button();
            this.txtInterrupt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSavePCLog = new System.Windows.Forms.Button();
            this.btnLogPC = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPacket = new System.Windows.Forms.TextBox();
            this.btnSendPacket = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "D";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "A";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(442, 254);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(70, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Get_Click);
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(441, 283);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(70, 23);
            this.btnStep.TabIndex = 4;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.Step_Click);
            // 
            // txtPPC
            // 
            this.txtPPC.Location = new System.Drawing.Point(14, 259);
            this.txtPPC.Name = "txtPPC";
            this.txtPPC.Size = new System.Drawing.Size(88, 20);
            this.txtPPC.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "PPC";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(441, 312);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(70, 23);
            this.btnRun.TabIndex = 7;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.Run_Click);
            // 
            // cbS
            // 
            this.cbS.AutoSize = true;
            this.cbS.Location = new System.Drawing.Point(17, 285);
            this.cbS.Name = "cbS";
            this.cbS.Size = new System.Drawing.Size(33, 17);
            this.cbS.TabIndex = 8;
            this.cbS.Text = "S";
            this.cbS.UseVisualStyleBackColor = true;
            // 
            // cbX
            // 
            this.cbX.AutoSize = true;
            this.cbX.Location = new System.Drawing.Point(17, 333);
            this.cbX.Name = "cbX";
            this.cbX.Size = new System.Drawing.Size(33, 17);
            this.cbX.TabIndex = 8;
            this.cbX.Text = "X";
            this.cbX.UseVisualStyleBackColor = true;
            // 
            // cbN
            // 
            this.cbN.AutoSize = true;
            this.cbN.Location = new System.Drawing.Point(17, 356);
            this.cbN.Name = "cbN";
            this.cbN.Size = new System.Drawing.Size(34, 17);
            this.cbN.TabIndex = 8;
            this.cbN.Text = "N";
            this.cbN.UseVisualStyleBackColor = true;
            // 
            // cbZ
            // 
            this.cbZ.AutoSize = true;
            this.cbZ.Location = new System.Drawing.Point(17, 380);
            this.cbZ.Name = "cbZ";
            this.cbZ.Size = new System.Drawing.Size(33, 17);
            this.cbZ.TabIndex = 8;
            this.cbZ.Text = "Z";
            this.cbZ.UseVisualStyleBackColor = true;
            // 
            // cbV
            // 
            this.cbV.AutoSize = true;
            this.cbV.Location = new System.Drawing.Point(17, 404);
            this.cbV.Name = "cbV";
            this.cbV.Size = new System.Drawing.Size(33, 17);
            this.cbV.TabIndex = 8;
            this.cbV.Text = "V";
            this.cbV.UseVisualStyleBackColor = true;
            // 
            // cbC
            // 
            this.cbC.AutoSize = true;
            this.cbC.Location = new System.Drawing.Point(17, 428);
            this.cbC.Name = "cbC";
            this.cbC.Size = new System.Drawing.Size(33, 17);
            this.cbC.TabIndex = 8;
            this.cbC.Text = "C";
            this.cbC.UseVisualStyleBackColor = true;
            // 
            // btnStep2
            // 
            this.btnStep2.Location = new System.Drawing.Point(517, 283);
            this.btnStep2.Name = "btnStep2";
            this.btnStep2.Size = new System.Drawing.Size(70, 23);
            this.btnStep2.TabIndex = 9;
            this.btnStep2.Text = "Step Till";
            this.btnStep2.UseVisualStyleBackColor = true;
            this.btnStep2.Click += new System.EventHandler(this.btnStep2_Click);
            // 
            // txtPPCTill
            // 
            this.txtPPCTill.Location = new System.Drawing.Point(628, 285);
            this.txtPPCTill.Name = "txtPPCTill";
            this.txtPPCTill.Size = new System.Drawing.Size(70, 20);
            this.txtPPCTill.TabIndex = 13;
            // 
            // txtIntMaskLevel
            // 
            this.txtIntMaskLevel.Location = new System.Drawing.Point(206, 258);
            this.txtIntMaskLevel.Name = "txtIntMaskLevel";
            this.txtIntMaskLevel.Size = new System.Drawing.Size(96, 20);
            this.txtIntMaskLevel.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(204, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Interrupt Mask Level";
            // 
            // txtUSP
            // 
            this.txtUSP.Location = new System.Drawing.Point(206, 305);
            this.txtUSP.Name = "txtUSP";
            this.txtUSP.Size = new System.Drawing.Size(96, 20);
            this.txtUSP.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(204, 285);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "USP";
            // 
            // txtSSP
            // 
            this.txtSSP.Location = new System.Drawing.Point(206, 350);
            this.txtSSP.Name = "txtSSP";
            this.txtSSP.Size = new System.Drawing.Size(96, 20);
            this.txtSSP.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(204, 332);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "SSP";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(593, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "PPC:";
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(204, 13);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResult.Size = new System.Drawing.Size(494, 221);
            this.tbResult.TabIndex = 15;
            // 
            // txtTotalCycles
            // 
            this.txtTotalCycles.Location = new System.Drawing.Point(316, 257);
            this.txtTotalCycles.Name = "txtTotalCycles";
            this.txtTotalCycles.Size = new System.Drawing.Size(120, 20);
            this.txtTotalCycles.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(314, 236);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Total Executed Cycles";
            // 
            // cbM
            // 
            this.cbM.AutoSize = true;
            this.cbM.Location = new System.Drawing.Point(17, 309);
            this.cbM.Name = "cbM";
            this.cbM.Size = new System.Drawing.Size(35, 17);
            this.cbM.TabIndex = 8;
            this.cbM.Text = "M";
            this.cbM.UseVisualStyleBackColor = true;
            // 
            // txtPC
            // 
            this.txtPC.Location = new System.Drawing.Point(316, 304);
            this.txtPC.Name = "txtPC";
            this.txtPC.Size = new System.Drawing.Size(70, 20);
            this.txtPC.TabIndex = 12;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(314, 285);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "PC";
            // 
            // txtD0
            // 
            this.txtD0.Location = new System.Drawing.Point(15, 32);
            this.txtD0.Name = "txtD0";
            this.txtD0.Size = new System.Drawing.Size(86, 20);
            this.txtD0.TabIndex = 22;
            // 
            // txtA0
            // 
            this.txtA0.Location = new System.Drawing.Point(112, 32);
            this.txtA0.Name = "txtA0";
            this.txtA0.Size = new System.Drawing.Size(86, 20);
            this.txtA0.TabIndex = 23;
            // 
            // txtD6
            // 
            this.txtD6.Location = new System.Drawing.Point(15, 189);
            this.txtD6.Name = "txtD6";
            this.txtD6.Size = new System.Drawing.Size(86, 20);
            this.txtD6.TabIndex = 24;
            // 
            // txtD5
            // 
            this.txtD5.Location = new System.Drawing.Point(14, 162);
            this.txtD5.Name = "txtD5";
            this.txtD5.Size = new System.Drawing.Size(86, 20);
            this.txtD5.TabIndex = 25;
            // 
            // txtD4
            // 
            this.txtD4.Location = new System.Drawing.Point(14, 136);
            this.txtD4.Name = "txtD4";
            this.txtD4.Size = new System.Drawing.Size(86, 20);
            this.txtD4.TabIndex = 26;
            // 
            // txtD3
            // 
            this.txtD3.Location = new System.Drawing.Point(14, 110);
            this.txtD3.Name = "txtD3";
            this.txtD3.Size = new System.Drawing.Size(86, 20);
            this.txtD3.TabIndex = 27;
            // 
            // txtD2
            // 
            this.txtD2.Location = new System.Drawing.Point(14, 84);
            this.txtD2.Name = "txtD2";
            this.txtD2.Size = new System.Drawing.Size(86, 20);
            this.txtD2.TabIndex = 28;
            // 
            // txtD1
            // 
            this.txtD1.Location = new System.Drawing.Point(14, 58);
            this.txtD1.Name = "txtD1";
            this.txtD1.Size = new System.Drawing.Size(86, 20);
            this.txtD1.TabIndex = 29;
            // 
            // txtD7
            // 
            this.txtD7.Location = new System.Drawing.Point(14, 215);
            this.txtD7.Name = "txtD7";
            this.txtD7.Size = new System.Drawing.Size(86, 20);
            this.txtD7.TabIndex = 30;
            // 
            // txtA1
            // 
            this.txtA1.Location = new System.Drawing.Point(112, 58);
            this.txtA1.Name = "txtA1";
            this.txtA1.Size = new System.Drawing.Size(86, 20);
            this.txtA1.TabIndex = 31;
            // 
            // txtA6
            // 
            this.txtA6.Location = new System.Drawing.Point(112, 189);
            this.txtA6.Name = "txtA6";
            this.txtA6.Size = new System.Drawing.Size(86, 20);
            this.txtA6.TabIndex = 32;
            // 
            // txtA5
            // 
            this.txtA5.Location = new System.Drawing.Point(112, 162);
            this.txtA5.Name = "txtA5";
            this.txtA5.Size = new System.Drawing.Size(86, 20);
            this.txtA5.TabIndex = 33;
            // 
            // txtA4
            // 
            this.txtA4.Location = new System.Drawing.Point(112, 136);
            this.txtA4.Name = "txtA4";
            this.txtA4.Size = new System.Drawing.Size(86, 20);
            this.txtA4.TabIndex = 34;
            // 
            // txtA3
            // 
            this.txtA3.Location = new System.Drawing.Point(112, 110);
            this.txtA3.Name = "txtA3";
            this.txtA3.Size = new System.Drawing.Size(86, 20);
            this.txtA3.TabIndex = 35;
            // 
            // txtA2
            // 
            this.txtA2.Location = new System.Drawing.Point(112, 84);
            this.txtA2.Name = "txtA2";
            this.txtA2.Size = new System.Drawing.Size(86, 20);
            this.txtA2.TabIndex = 36;
            // 
            // txtA7
            // 
            this.txtA7.Location = new System.Drawing.Point(112, 214);
            this.txtA7.Name = "txtA7";
            this.txtA7.Size = new System.Drawing.Size(86, 20);
            this.txtA7.TabIndex = 37;
            // 
            // lblOp
            // 
            this.lblOp.AutoSize = true;
            this.lblOp.Location = new System.Drawing.Point(111, 241);
            this.lblOp.Name = "lblOp";
            this.lblOp.Size = new System.Drawing.Size(22, 13);
            this.lblOp.TabIndex = 39;
            this.lblOp.Text = "OP";
            // 
            // txtOp
            // 
            this.txtOp.Location = new System.Drawing.Point(113, 258);
            this.txtOp.Name = "txtOp";
            this.txtOp.Size = new System.Drawing.Size(86, 20);
            this.txtOp.TabIndex = 38;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(206, 378);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(120, 20);
            this.txtAddress.TabIndex = 41;
            // 
            // txtReadByte
            // 
            this.txtReadByte.Location = new System.Drawing.Point(332, 376);
            this.txtReadByte.Name = "txtReadByte";
            this.txtReadByte.Size = new System.Drawing.Size(94, 23);
            this.txtReadByte.TabIndex = 40;
            this.txtReadByte.Text = "Read Byte";
            this.txtReadByte.UseVisualStyleBackColor = true;
            this.txtReadByte.Click += new System.EventHandler(this.txtReadByte_Click);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(206, 404);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(120, 20);
            this.txtValue.TabIndex = 42;
            // 
            // btnWriteByte
            // 
            this.btnWriteByte.Location = new System.Drawing.Point(332, 404);
            this.btnWriteByte.Name = "btnWriteByte";
            this.btnWriteByte.Size = new System.Drawing.Size(94, 23);
            this.btnWriteByte.TabIndex = 43;
            this.btnWriteByte.Text = "Write Byte";
            this.btnWriteByte.UseVisualStyleBackColor = true;
            this.btnWriteByte.Click += new System.EventHandler(this.btnWriteByte_Click);
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(155, 381);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 44;
            this.lblAddress.Text = "Address";
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(166, 408);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(34, 13);
            this.lblValue.TabIndex = 45;
            this.lblValue.Text = "Value";
            // 
            // btnReadWord
            // 
            this.btnReadWord.Location = new System.Drawing.Point(430, 376);
            this.btnReadWord.Name = "btnReadWord";
            this.btnReadWord.Size = new System.Drawing.Size(94, 23);
            this.btnReadWord.TabIndex = 46;
            this.btnReadWord.Text = "Read Word";
            this.btnReadWord.UseVisualStyleBackColor = true;
            this.btnReadWord.Click += new System.EventHandler(this.btnReadWord_Click);
            // 
            // btnReadLong
            // 
            this.btnReadLong.Location = new System.Drawing.Point(530, 376);
            this.btnReadLong.Name = "btnReadLong";
            this.btnReadLong.Size = new System.Drawing.Size(94, 23);
            this.btnReadLong.TabIndex = 47;
            this.btnReadLong.Text = "Read Long";
            this.btnReadLong.UseVisualStyleBackColor = true;
            this.btnReadLong.Click += new System.EventHandler(this.btnReadLong_Click);
            // 
            // btnWriteWord
            // 
            this.btnWriteWord.Location = new System.Drawing.Point(430, 404);
            this.btnWriteWord.Name = "btnWriteWord";
            this.btnWriteWord.Size = new System.Drawing.Size(94, 23);
            this.btnWriteWord.TabIndex = 48;
            this.btnWriteWord.Text = "Write Word";
            this.btnWriteWord.UseVisualStyleBackColor = true;
            this.btnWriteWord.Click += new System.EventHandler(this.btnWriteWord_Click);
            // 
            // btnWriteLong
            // 
            this.btnWriteLong.Location = new System.Drawing.Point(530, 404);
            this.btnWriteLong.Name = "btnWriteLong";
            this.btnWriteLong.Size = new System.Drawing.Size(94, 23);
            this.btnWriteLong.TabIndex = 49;
            this.btnWriteLong.Text = "Write Long";
            this.btnWriteLong.UseVisualStyleBackColor = true;
            this.btnWriteLong.Click += new System.EventHandler(this.btnWriteLong_Click);
            // 
            // btnTriggerInterrupt
            // 
            this.btnTriggerInterrupt.Location = new System.Drawing.Point(332, 433);
            this.btnTriggerInterrupt.Name = "btnTriggerInterrupt";
            this.btnTriggerInterrupt.Size = new System.Drawing.Size(94, 23);
            this.btnTriggerInterrupt.TabIndex = 50;
            this.btnTriggerInterrupt.Text = "Trigger Interrupt";
            this.btnTriggerInterrupt.UseVisualStyleBackColor = true;
            this.btnTriggerInterrupt.Click += new System.EventHandler(this.btnTriggerInterrupt_Click);
            // 
            // txtInterrupt
            // 
            this.txtInterrupt.Location = new System.Drawing.Point(206, 433);
            this.txtInterrupt.Name = "txtInterrupt";
            this.txtInterrupt.Size = new System.Drawing.Size(120, 20);
            this.txtInterrupt.TabIndex = 51;
            this.txtInterrupt.Text = "1c0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(153, 436);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Address";
            // 
            // btnSavePCLog
            // 
            this.btnSavePCLog.Location = new System.Drawing.Point(306, 459);
            this.btnSavePCLog.Name = "btnSavePCLog";
            this.btnSavePCLog.Size = new System.Drawing.Size(94, 23);
            this.btnSavePCLog.TabIndex = 54;
            this.btnSavePCLog.Text = "Save PC Log";
            this.btnSavePCLog.UseVisualStyleBackColor = true;
            this.btnSavePCLog.Click += new System.EventHandler(this.btnSavePCLog_Click);
            // 
            // btnLogPC
            // 
            this.btnLogPC.Location = new System.Drawing.Point(206, 459);
            this.btnLogPC.Name = "btnLogPC";
            this.btnLogPC.Size = new System.Drawing.Size(94, 23);
            this.btnLogPC.TabIndex = 53;
            this.btnLogPC.Text = "Start PC Log";
            this.btnLogPC.UseVisualStyleBackColor = true;
            this.btnLogPC.Click += new System.EventHandler(this.btnLogPC_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(153, 491);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 57;
            this.label9.Text = "Packet";
            // 
            // txtPacket
            // 
            this.txtPacket.Location = new System.Drawing.Point(206, 488);
            this.txtPacket.Name = "txtPacket";
            this.txtPacket.Size = new System.Drawing.Size(120, 20);
            this.txtPacket.TabIndex = 56;
            this.txtPacket.Text = "6C 10 F1 3C 01 20";
            // 
            // btnSendPacket
            // 
            this.btnSendPacket.Location = new System.Drawing.Point(332, 488);
            this.btnSendPacket.Name = "btnSendPacket";
            this.btnSendPacket.Size = new System.Drawing.Size(94, 23);
            this.btnSendPacket.TabIndex = 55;
            this.btnSendPacket.Text = "Send Packet";
            this.btnSendPacket.UseVisualStyleBackColor = true;
            this.btnSendPacket.Click += new System.EventHandler(this.btnSendPacket_Click);
            // 
            // M68KDebugger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtPacket);
            this.Controls.Add(this.btnSendPacket);
            this.Controls.Add(this.btnSavePCLog);
            this.Controls.Add(this.btnLogPC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtInterrupt);
            this.Controls.Add(this.btnTriggerInterrupt);
            this.Controls.Add(this.btnWriteLong);
            this.Controls.Add(this.btnWriteWord);
            this.Controls.Add(this.btnReadLong);
            this.Controls.Add(this.btnReadWord);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.btnWriteByte);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtReadByte);
            this.Controls.Add(this.lblOp);
            this.Controls.Add(this.txtOp);
            this.Controls.Add(this.txtA7);
            this.Controls.Add(this.txtA2);
            this.Controls.Add(this.txtA3);
            this.Controls.Add(this.txtA4);
            this.Controls.Add(this.txtA5);
            this.Controls.Add(this.txtA6);
            this.Controls.Add(this.txtA1);
            this.Controls.Add(this.txtD7);
            this.Controls.Add(this.txtD1);
            this.Controls.Add(this.txtD2);
            this.Controls.Add(this.txtD3);
            this.Controls.Add(this.txtD4);
            this.Controls.Add(this.txtD5);
            this.Controls.Add(this.txtD6);
            this.Controls.Add(this.txtA0);
            this.Controls.Add(this.txtD0);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtPPCTill);
            this.Controls.Add(this.btnStep2);
            this.Controls.Add(this.cbC);
            this.Controls.Add(this.cbV);
            this.Controls.Add(this.cbZ);
            this.Controls.Add(this.cbN);
            this.Controls.Add(this.cbX);
            this.Controls.Add(this.cbM);
            this.Controls.Add(this.cbS);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSSP);
            this.Controls.Add(this.txtUSP);
            this.Controls.Add(this.txtPC);
            this.Controls.Add(this.txtTotalCycles);
            this.Controls.Add(this.txtIntMaskLevel);
            this.Controls.Add(this.txtPPC);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "M68KDebugger";
            this.Size = new System.Drawing.Size(707, 520);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.TextBox txtPPC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.CheckBox cbS;
        private System.Windows.Forms.CheckBox cbX;
        private System.Windows.Forms.CheckBox cbN;
        private System.Windows.Forms.CheckBox cbZ;
        private System.Windows.Forms.CheckBox cbV;
        private System.Windows.Forms.CheckBox cbC;
        private System.Windows.Forms.Button btnStep2;
        private System.Windows.Forms.TextBox txtPPCTill;
        private System.Windows.Forms.TextBox txtIntMaskLevel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUSP;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSSP;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.TextBox txtTotalCycles;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbM;
        private System.Windows.Forms.TextBox txtPC;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtD0;
        private System.Windows.Forms.TextBox txtA0;
        private System.Windows.Forms.TextBox txtD6;
        private System.Windows.Forms.TextBox txtD5;
        private System.Windows.Forms.TextBox txtD4;
        private System.Windows.Forms.TextBox txtD3;
        private System.Windows.Forms.TextBox txtD2;
        private System.Windows.Forms.TextBox txtD1;
        private System.Windows.Forms.TextBox txtD7;
        private System.Windows.Forms.TextBox txtA1;
        private System.Windows.Forms.TextBox txtA6;
        private System.Windows.Forms.TextBox txtA5;
        private System.Windows.Forms.TextBox txtA4;
        private System.Windows.Forms.TextBox txtA3;
        private System.Windows.Forms.TextBox txtA2;
        private System.Windows.Forms.TextBox txtA7;
        private System.Windows.Forms.Label lblOp;
        private System.Windows.Forms.TextBox txtOp;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Button txtReadByte;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Button btnWriteByte;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Button btnReadWord;
        private System.Windows.Forms.Button btnReadLong;
        private System.Windows.Forms.Button btnWriteWord;
        private System.Windows.Forms.Button btnWriteLong;
        private System.Windows.Forms.Button btnTriggerInterrupt;
        private System.Windows.Forms.TextBox txtInterrupt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSavePCLog;
        private System.Windows.Forms.Button btnLogPC;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPacket;
        private System.Windows.Forms.Button btnSendPacket;
    }
}