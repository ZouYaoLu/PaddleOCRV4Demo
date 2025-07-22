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
            groupBoxSettings = new GroupBox();
            checkBoxUseGPU = new CheckBox();
            checkBoxUseClassifier = new CheckBox();
            checkBoxUseDetection = new CheckBox();
            checkBoxUseRecognition = new CheckBox();
            labelThreads = new Label();
            numericUpDownThreads = new NumericUpDown();
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
            ((System.ComponentModel.ISupportInitialize)numericUpDownThreads).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).BeginInit();
            statusStrip.SuspendLayout();
            groupBoxImage.SuspendLayout();
            groupBoxResult.SuspendLayout();
            SuspendLayout();
            // 
            // btnDownloadModels
            // 
            btnDownloadModels.Location = new Point(12, 12);
            btnDownloadModels.Name = "btnDownloadModels";
            btnDownloadModels.Size = new Size(150, 35);
            btnDownloadModels.TabIndex = 0;
            btnDownloadModels.Text = "下载模型";
            btnDownloadModels.UseVisualStyleBackColor = true;
            btnDownloadModels.Click += btnDownloadModels_Click;
            // 
            // groupBoxSettings
            // 
            groupBoxSettings.Controls.Add(checkBoxUseGPU);
            groupBoxSettings.Controls.Add(checkBoxUseClassifier);
            groupBoxSettings.Controls.Add(checkBoxUseDetection);
            groupBoxSettings.Controls.Add(checkBoxUseRecognition);
            groupBoxSettings.Controls.Add(labelThreads);
            groupBoxSettings.Controls.Add(numericUpDownThreads);
            groupBoxSettings.Location = new Point(12, 53);
            groupBoxSettings.Name = "groupBoxSettings";
            groupBoxSettings.Size = new Size(776, 100);
            groupBoxSettings.TabIndex = 1;
            groupBoxSettings.TabStop = false;
            groupBoxSettings.Text = "配置设置";
            // 
            // checkBoxUseGPU
            // 
            checkBoxUseGPU.AutoSize = true;
            checkBoxUseGPU.Location = new Point(15, 25);
            checkBoxUseGPU.Name = "checkBoxUseGPU";
            checkBoxUseGPU.Size = new Size(87, 21);
            checkBoxUseGPU.TabIndex = 0;
            checkBoxUseGPU.Text = "启用GPU";
            checkBoxUseGPU.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseClassifier
            // 
            checkBoxUseClassifier.AutoSize = true;
            checkBoxUseClassifier.Checked = true;
            checkBoxUseClassifier.CheckState = CheckState.Checked;
            checkBoxUseClassifier.Location = new Point(120, 25);
            checkBoxUseClassifier.Name = "checkBoxUseClassifier";
            checkBoxUseClassifier.Size = new Size(111, 21);
            checkBoxUseClassifier.TabIndex = 1;
            checkBoxUseClassifier.Text = "启用方向分类";
            checkBoxUseClassifier.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseDetection
            // 
            checkBoxUseDetection.AutoSize = true;
            checkBoxUseDetection.Checked = true;
            checkBoxUseDetection.CheckState = CheckState.Checked;
            checkBoxUseDetection.Location = new Point(250, 25);
            checkBoxUseDetection.Name = "checkBoxUseDetection";
            checkBoxUseDetection.Size = new Size(99, 21);
            checkBoxUseDetection.TabIndex = 2;
            checkBoxUseDetection.Text = "启用文本检测";
            checkBoxUseDetection.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseRecognition
            // 
            checkBoxUseRecognition.AutoSize = true;
            checkBoxUseRecognition.Checked = true;
            checkBoxUseRecognition.CheckState = CheckState.Checked;
            checkBoxUseRecognition.Location = new Point(370, 25);
            checkBoxUseRecognition.Name = "checkBoxUseRecognition";
            checkBoxUseRecognition.Size = new Size(99, 21);
            checkBoxUseRecognition.TabIndex = 3;
            checkBoxUseRecognition.Text = "启用文本识别";
            checkBoxUseRecognition.UseVisualStyleBackColor = true;
            // 
            // labelThreads
            // 
            labelThreads.AutoSize = true;
            labelThreads.Location = new Point(15, 60);
            labelThreads.Name = "labelThreads";
            labelThreads.Size = new Size(68, 17);
            labelThreads.TabIndex = 4;
            labelThreads.Text = "线程数量：";
            // 
            // numericUpDownThreads
            // 
            numericUpDownThreads.Location = new Point(89, 58);
            numericUpDownThreads.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
            numericUpDownThreads.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownThreads.Name = "numericUpDownThreads";
            numericUpDownThreads.Size = new Size(60, 23);
            numericUpDownThreads.TabIndex = 5;
            numericUpDownThreads.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // btnSelectImage
            // 
            btnSelectImage.Location = new Point(6, 22);
            btnSelectImage.Name = "btnSelectImage";
            btnSelectImage.Size = new Size(120, 35);
            btnSelectImage.TabIndex = 0;
            btnSelectImage.Text = "选择图片";
            btnSelectImage.UseVisualStyleBackColor = true;
            btnSelectImage.Click += btnSelectImage_Click;
            // 
            // pictureBoxImage
            // 
            pictureBoxImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxImage.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxImage.Location = new Point(6, 63);
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Size = new Size(350, 250);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.TabIndex = 1;
            pictureBoxImage.TabStop = false;
            // 
            // btnOCR
            // 
            btnOCR.Enabled = false;
            btnOCR.Location = new Point(132, 22);
            btnOCR.Name = "btnOCR";
            btnOCR.Size = new Size(120, 35);
            btnOCR.TabIndex = 2;
            btnOCR.Text = "开始识别";
            btnOCR.UseVisualStyleBackColor = true;
            btnOCR.Click += btnOCR_Click;
            // 
            // textBoxResult
            // 
            textBoxResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxResult.Location = new Point(6, 40);
            textBoxResult.Multiline = true;
            textBoxResult.Name = "textBoxResult";
            textBoxResult.ReadOnly = true;
            textBoxResult.ScrollBars = ScrollBars.Vertical;
            textBoxResult.Size = new Size(394, 250);
            textBoxResult.TabIndex = 0;
            // 
            // labelResult
            // 
            labelResult.AutoSize = true;
            labelResult.Location = new Point(6, 19);
            labelResult.Name = "labelResult";
            labelResult.Size = new Size(68, 17);
            labelResult.TabIndex = 1;
            labelResult.Text = "识别结果：";
            // 
            // labelExecutionTime
            // 
            labelExecutionTime.AutoSize = true;
            labelExecutionTime.Location = new Point(200, 19);
            labelExecutionTime.Name = "labelExecutionTime";
            labelExecutionTime.Size = new Size(80, 17);
            labelExecutionTime.TabIndex = 2;
            labelExecutionTime.Text = "执行时间：-";
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripProgressBar });
            statusStrip.Location = new Point(0, 529);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(800, 22);
            statusStrip.TabIndex = 4;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(56, 17);
            toolStripStatusLabel.Text = "就绪";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(100, 16);
            toolStripProgressBar.Visible = false;
            // 
            // groupBoxImage
            // 
            groupBoxImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBoxImage.Controls.Add(btnSelectImage);
            groupBoxImage.Controls.Add(pictureBoxImage);
            groupBoxImage.Controls.Add(btnOCR);
            groupBoxImage.Location = new Point(12, 159);
            groupBoxImage.Name = "groupBoxImage";
            groupBoxImage.Size = new Size(372, 357);
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
            groupBoxResult.Location = new Point(390, 159);
            groupBoxResult.Name = "groupBoxResult";
            groupBoxResult.Size = new Size(398, 357);
            groupBoxResult.TabIndex = 6;
            groupBoxResult.TabStop = false;
            groupBoxResult.Text = "识别结果";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 551);
            Controls.Add(groupBoxResult);
            Controls.Add(groupBoxImage);
            Controls.Add(statusStrip);
            Controls.Add(groupBoxSettings);
            Controls.Add(btnDownloadModels);
            MinimumSize = new Size(816, 590);
            Name = "MainForm";
            Text = "PaddleOCR V4 文字识别工具";
            Load += MainForm_Load;
            groupBoxSettings.ResumeLayout(false);
            groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownThreads).EndInit();
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
        private GroupBox groupBoxSettings;
        private CheckBox checkBoxUseGPU;
        private CheckBox checkBoxUseClassifier;
        private CheckBox checkBoxUseDetection;
        private CheckBox checkBoxUseRecognition;
        private Label labelThreads;
        private NumericUpDown numericUpDownThreads;
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