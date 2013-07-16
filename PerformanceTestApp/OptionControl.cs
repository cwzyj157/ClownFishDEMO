using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyTestAppFramework;

namespace PerformanceTestApp
{
	public partial class OptionControl : BaseTestOptionsUserControl
	{
		public OptionControl()
		{
			InitializeComponent();
			TestFactory<UiParameters>.FillComboBox(cboTestMethod);
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			this.ExecuteTest();
		}

		public override UiParametersBase GetTestParameters()
		{
			UiParameters uiParam = new UiParameters();
			uiParam.ThreadCount = (int)numericUpDown_threadCount.Value;
			uiParam.RunTimes = (int)numericUpDown_runTimes.Value;
			uiParam.PageSize = (int)numericUpDown_pageSize.Value;
			uiParam.TestIndex = cboTestMethod.SelectedIndex;
			uiParam.TestName = cboTestMethod.SelectedItem.ToString();
			return uiParam;
		}

		public override string[] GetTestMethodNames()
		{
			return GetTestMethodNamesFromComboBox(cboTestMethod);
		}

		public override void SetRunButtonEnable(bool enabled)
		{
			this.btnRun.Enabled = enabled;
		}

		private void btnShowResult_Click(object sender, EventArgs e)
		{
			this.ShowTestHistory();
		}


	}
}
