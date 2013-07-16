namespace PerformanceTestApp
{
	partial class OptionControl
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if( disposing && (components != null) ) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDown_runTimes = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnShowResult = new System.Windows.Forms.Button();
			this.cboTestMethod = new MyTestAppFramework.SeparatorComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnRun = new System.Windows.Forms.Button();
			this.numericUpDown_pageSize = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown_threadCount = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_runTimes)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pageSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threadCount)).BeginInit();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(281, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "分页大小";
			// 
			// numericUpDown_runTimes
			// 
			this.numericUpDown_runTimes.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.numericUpDown_runTimes.Location = new System.Drawing.Point(195, 21);
			this.numericUpDown_runTimes.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numericUpDown_runTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown_runTimes.Name = "numericUpDown_runTimes";
			this.numericUpDown_runTimes.Size = new System.Drawing.Size(79, 21);
			this.numericUpDown_runTimes.TabIndex = 3;
			this.numericUpDown_runTimes.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(136, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "循环次数";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnShowResult);
			this.groupBox1.Controls.Add(this.cboTestMethod);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.btnRun);
			this.groupBox1.Controls.Add(this.numericUpDown_pageSize);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.numericUpDown_runTimes);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.numericUpDown_threadCount);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(892, 52);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "测试参数";
			// 
			// btnShowResult
			// 
			this.btnShowResult.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnShowResult.Location = new System.Drawing.Point(777, 19);
			this.btnShowResult.Name = "btnShowResult";
			this.btnShowResult.Size = new System.Drawing.Size(90, 23);
			this.btnShowResult.TabIndex = 9;
			this.btnShowResult.Text = "查看测试结果";
			this.btnShowResult.UseVisualStyleBackColor = true;
			this.btnShowResult.Click += new System.EventHandler(this.btnShowResult_Click);
			// 
			// cboTestMethod
			// 
			this.cboTestMethod.FormattingEnabled = true;
			this.cboTestMethod.Location = new System.Drawing.Point(472, 21);
			this.cboTestMethod.Name = "cboTestMethod";
			this.cboTestMethod.Size = new System.Drawing.Size(217, 22);
			this.cboTestMethod.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(413, 25);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 12);
			this.label4.TabIndex = 7;
			this.label4.Text = "测试方法";
			// 
			// btnRun
			// 
			this.btnRun.Location = new System.Drawing.Point(695, 19);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(75, 23);
			this.btnRun.TabIndex = 6;
			this.btnRun.Text = "开始测试";
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// numericUpDown_pageSize
			// 
			this.numericUpDown_pageSize.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.numericUpDown_pageSize.Location = new System.Drawing.Point(343, 21);
			this.numericUpDown_pageSize.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numericUpDown_pageSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDown_pageSize.Name = "numericUpDown_pageSize";
			this.numericUpDown_pageSize.Size = new System.Drawing.Size(62, 21);
			this.numericUpDown_pageSize.TabIndex = 5;
			this.numericUpDown_pageSize.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// numericUpDown_threadCount
			// 
			this.numericUpDown_threadCount.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.numericUpDown_threadCount.Location = new System.Drawing.Point(67, 21);
			this.numericUpDown_threadCount.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericUpDown_threadCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown_threadCount.Name = "numericUpDown_threadCount";
			this.numericUpDown_threadCount.Size = new System.Drawing.Size(62, 21);
			this.numericUpDown_threadCount.TabIndex = 1;
			this.numericUpDown_threadCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "线程数量";
			// 
			// OptionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "OptionControl";
			this.Size = new System.Drawing.Size(892, 52);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_runTimes)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pageSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threadCount)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numericUpDown_runTimes;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private MyTestAppFramework.SeparatorComboBox cboTestMethod;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.NumericUpDown numericUpDown_pageSize;
		private System.Windows.Forms.NumericUpDown numericUpDown_threadCount;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnShowResult;
	}
}
