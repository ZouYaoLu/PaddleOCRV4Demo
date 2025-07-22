namespace PaddleOCRApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnDownloadModels = new Button();
            btnInitializeEngine = new Button();
            groupBoxSettings = new GroupBox();
            labelModelSelection = new Label();
            comboBoxModelSelection = new ComboBox();
            checkBoxUseGPU = new CheckBox();
            checkBoxAllowRotateDetection = new CheckBox();
            checkBoxEnable180Classification = new CheckBox();
            btnSelectImage = new Button();
            pictureBoxImage = new PictureBox();
            btnOCR = new Button();
            textBoxResult = new TextBox();
            labelResult = new Label();
            labelExecutionTime = new Label();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolStripProgressBar = new ToolStripProgressBar();
            groupBoxImage = new GroupBox();
            groupBoxResult = new GroupBox();
            groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).BeginInit();
            statusStrip.SuspendLayout();
            groupBoxImage.SuspendLayout();
            groupBoxResult.SuspendLayout();
            SuspendLayout();
            // 
            // btnDownloadModels
            // 
            btnDownloadModels.Location = new Point(380, 25);
            btnDownloadModels.Margin = new Padding(4);
            btnDownloadModels.Name = "btnDownloadModels";
            btnDownloadModels.Size = new Size(150, 34);
            btnDownloadModels.TabIndex = 0;
            btnDownloadModels.Text = "立即下载";
            btnDownloadModels.UseVisualStyleBackColor = true;
            btnDownloadModels.Click += btnDownloadModels_Click;
            // 
            // btnInitializeEngine
            // 
            btnInitializeEngine.Location = new Point(19, 134);
            btnInitializeEngine.Margin = new Padding(4);
            btnInitializeEngine.Name = "btnInitializeEngine";
            btnInitializeEngine.Size = new Size(193, 34);
            btnInitializeEngine.TabIndex = 15;
            btnInitializeEngine.Text = "初始化引擎";
            btnInitializeEngine.UseVisualStyleBackColor = true;
            btnInitializeEngine.Click += btnInitializeEngine_Click;
            // 
            // groupBoxSettings
            //
            groupBoxSettings.Controls.Add(labelModelSelection);
            groupBoxSettings.Controls.Add(comboBoxModelSelection);
            groupBoxSettings.Controls.Add(checkBoxUseGPU);
            groupBoxSettings.Controls.Add(checkBoxAllowRotateDetection);
            groupBoxSettings.Controls.Add(checkBoxEnable180Classification);
            groupBoxSettings.Controls.Add(btnInitializeEngine);
            groupBoxSettings.Controls.Add(btnDownloadModels);
            groupBoxSettings.Location = new Point(15, 4);
            groupBoxSettings.Margin = new Padding(4);
            groupBoxSettings.Name = "groupBoxSettings";
            groupBoxSettings.Padding = new Padding(4);
            groupBoxSettings.Size = new Size(998, 186);
            groupBoxSettings.TabIndex = 1;
            groupBoxSettings.TabStop = false;
            groupBoxSettings.Text = "初始化";
            // 
            // checkBoxUseGPU
            // 
            checkBoxUseGPU.AutoSize = true;
            checkBoxUseGPU.Checked = true;
            checkBoxUseGPU.CheckState = CheckState.Checked;
            checkBoxUseGPU.Location = new Point(19, 102);
            checkBoxUseGPU.Margin = new Padding(4);
            checkBoxUseGPU.Name = "checkBoxUseGPU";
            checkBoxUseGPU.Size = new Size(92, 24);
            checkBoxUseGPU.TabIndex = 0;
            checkBoxUseGPU.Text = "启用GPU";
            checkBoxUseGPU.UseVisualStyleBackColor = true;
            checkBoxUseGPU.CheckedChanged += checkBoxUseGPU_CheckedChanged;
            // 
            // checkBoxUseClassifier
            // 
            checkBoxAllowRotateDetection.AutoSize = true;
            checkBoxAllowRotateDetection.Checked = true;
            checkBoxAllowRotateDetection.CheckState = CheckState.Checked;
            checkBoxAllowRotateDetection.Location = new Point(19, 70);
            checkBoxAllowRotateDetection.Margin = new Padding(4);
            checkBoxAllowRotateDetection.Name = "checkBoxUseClassifier";
            checkBoxAllowRotateDetection.Size = new Size(181, 24);
            checkBoxAllowRotateDetection.TabIndex = 1;
            checkBoxAllowRotateDetection.Text = "允许识别有角度的文字";
            checkBoxAllowRotateDetection.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseDetection
            // 
            checkBoxEnable180Classification.AutoSize = true;
            checkBoxEnable180Classification.Location = new Point(351, 69);
            checkBoxEnable180Classification.Margin = new Padding(4);
            checkBoxEnable180Classification.Name = "checkBoxUseDetection";
            checkBoxEnable180Classification.Size = new Size(244, 24);
            checkBoxEnable180Classification.TabIndex = 2;
            checkBoxEnable180Classification.Text = "许识别旋转角度大于90度的文字";
            checkBoxEnable180Classification.UseVisualStyleBackColor = true;
            //
            // labelModelSelection
            //
            labelModelSelection.AutoSize = true;
            labelModelSelection.Location = new Point(19, 30);
            labelModelSelection.Margin = new Padding(4, 0, 4, 0);
            labelModelSelection.Name = "labelModelSelection";
            labelModelSelection.Size = new Size(84, 20);
            labelModelSelection.TabIndex = 16;
            labelModelSelection.Text = "选择模型：";
            //
            // comboBoxModelSelection
            //
            comboBoxModelSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxModelSelection.FormattingEnabled = true;
            comboBoxModelSelection.Location = new Point(111, 27);
            comboBoxModelSelection.Margin = new Padding(4);
            comboBoxModelSelection.Name = "comboBoxModelSelection";
            comboBoxModelSelection.Size = new Size(250, 28);
            comboBoxModelSelection.TabIndex = 17;
            comboBoxModelSelection.SelectedIndexChanged += comboBoxModelSelection_SelectedIndexChanged;
            //
            // btnSelectImage
            // 
            btnSelectImage.Location = new Point(8, 26);
            btnSelectImage.Margin = new Padding(4);
            btnSelectImage.Name = "btnSelectImage";
            btnSelectImage.Size = new Size(154, 41);
            btnSelectImage.TabIndex = 0;
            btnSelectImage.Text = "选择图片";
            btnSelectImage.UseVisualStyleBackColor = true;
            btnSelectImage.Click += btnSelectImage_Click;
            // 
            // pictureBoxImage
            // 
            pictureBoxImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxImage.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxImage.Location = new Point(8, 74);
            pictureBoxImage.Margin = new Padding(4);
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Size = new Size(449, 518);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.TabIndex = 1;
            pictureBoxImage.TabStop = false;
            // 
            // btnOCR
            // 
            btnOCR.Enabled = false;
            btnOCR.Location = new Point(170, 26);
            btnOCR.Margin = new Padding(4);
            btnOCR.Name = "btnOCR";
            btnOCR.Size = new Size(154, 41);
            btnOCR.TabIndex = 2;
            btnOCR.Text = "开始识别";
            btnOCR.UseVisualStyleBackColor = true;
            btnOCR.Click += btnOCR_Click;
            // 
            // textBoxResult
            // 
            textBoxResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxResult.Location = new Point(8, 47);
            textBoxResult.Margin = new Padding(4);
            textBoxResult.Multiline = true;
            textBoxResult.Name = "textBoxResult";
            textBoxResult.ReadOnly = true;
            textBoxResult.ScrollBars = ScrollBars.Vertical;
            textBoxResult.Size = new Size(505, 589);
            textBoxResult.TabIndex = 0;
            // 
            // labelResult
            // 
            labelResult.AutoSize = true;
            labelResult.Location = new Point(8, 22);
            labelResult.Margin = new Padding(4, 0, 4, 0);
            labelResult.Name = "labelResult";
            labelResult.Size = new Size(84, 20);
            labelResult.TabIndex = 1;
            labelResult.Text = "识别结果：";
            // 
            // labelExecutionTime
            // 
            labelExecutionTime.AutoSize = true;
            labelExecutionTime.Location = new Point(257, 22);
            labelExecutionTime.Margin = new Padding(4, 0, 4, 0);
            labelExecutionTime.Name = "labelExecutionTime";
            labelExecutionTime.Size = new Size(90, 20);
            labelExecutionTime.TabIndex = 2;
            labelExecutionTime.Text = "执行时间：-";
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripProgressBar });
            statusStrip.Location = new Point(0, 846);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 18, 0);
            statusStrip.Size = new Size(1029, 26);
            statusStrip.TabIndex = 4;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(39, 20);
            toolStripStatusLabel.Text = "就绪";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(129, 18);
            toolStripProgressBar.Visible = false;
            // 
            // groupBoxImage
            // 
            groupBoxImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBoxImage.Controls.Add(btnSelectImage);
            groupBoxImage.Controls.Add(pictureBoxImage);
            groupBoxImage.Controls.Add(btnOCR);
            groupBoxImage.Location = new Point(15, 198);
            groupBoxImage.Margin = new Padding(4);
            groupBoxImage.Name = "groupBoxImage";
            groupBoxImage.Padding = new Padding(4);
            groupBoxImage.Size = new Size(478, 644);
            groupBoxImage.TabIndex = 5;
            groupBoxImage.TabStop = false;
            groupBoxImage.Text = "图片选择与识别";
            // 
            // groupBoxResult
            // 
            groupBoxResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxResult.Controls.Add(labelResult);
            groupBoxResult.Controls.Add(textBoxResult);
            groupBoxResult.Controls.Add(labelExecutionTime);
            groupBoxResult.Location = new Point(504, 198);
            groupBoxResult.Margin = new Padding(4);
            groupBoxResult.Name = "groupBoxResult";
            groupBoxResult.Padding = new Padding(4);
            groupBoxResult.Size = new Size(512, 644);
            groupBoxResult.TabIndex = 6;
            groupBoxResult.TabStop = false;
            groupBoxResult.Text = "识别结果";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1029, 872);
            Controls.Add(groupBoxResult);
            Controls.Add(groupBoxImage);
            Controls.Add(statusStrip);
            Controls.Add(groupBoxSettings);
            Margin = new Padding(4);
            MinimumSize = new Size(1044, 686);
            Name = "MainForm";
            Text = "PaddleOCR V4 文字识别工具";
            Load += MainForm_Load;
            groupBoxSettings.ResumeLayout(false);
            groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            groupBoxImage.ResumeLayout(false);
            groupBoxResult.ResumeLayout(false);
            groupBoxResult.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnDownloadModels;
        private Button btnInitializeEngine;
        private GroupBox groupBoxSettings;
        private Label labelModelSelection;
        private ComboBox comboBoxModelSelection;
        private CheckBox checkBoxUseGPU;
        private CheckBox checkBoxAllowRotateDetection;
        private CheckBox checkBoxEnable180Classification;
        private Button btnSelectImage;
        private PictureBox pictureBoxImage;
        private Button btnOCR;
        private TextBox textBoxResult;
        private Label labelResult;
        private Label labelExecutionTime;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolStripProgressBar toolStripProgressBar;
        private GroupBox groupBoxImage;
        private GroupBox groupBoxResult;
    }
}