using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyTestAppFramework;

namespace PerformanceTestApp
{
	class TestObject : ITestObject
	{
		public string GetFormTitle()
		{
			return "ClownFish Performance Test";
		}

		public void ExceuteTest(object param)
		{
			TestMainForm.ShowExecuteMessage("Thread: " + System.Threading.Thread.CurrentThread.Name +  ", Enter ExceuteTest().");
			UiParameters uiParam = (UiParameters)param;

			using( IPerformanceTest instance = (IPerformanceTest)TestFactory<UiParameters>.GetTestInstance(uiParam) ) {
				for( int i = 1; i <= uiParam.RunTimes; i++ )
					instance.Run();
			}
		}

		public void PrepareExecute(object param)
		{
			UiParameters uiParam = (UiParameters)param;
			TestMainForm.ShowExecuteMessage(uiParam.TestName +  " Enter PrepareExecute().");
			

			using( IPerformanceTest instance = (IPerformanceTest)TestFactory<UiParameters>.GetTestInstance(uiParam) ) {
				instance.Run();
			}
		}


	}
}
