using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Teaching.Skills.Contexts;

namespace Teaching.Skills.Droid.Activities
{
	[Activity(Theme = "@style/App.Splash", MainLauncher = true, NoHistory = true,
			  Name = Program.PackageName + ".SplashActivity",
			  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashActivity : Activity
	{
		private static readonly string TAG = "X:" + typeof(SplashActivity).Name;

		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
		{
			base.OnCreate(savedInstanceState, persistentState);
			Log.Debug(TAG, "SplashActivity.OnCreate");
		}

		private Task startupWork = null;

		protected override void OnResume()
		{
			base.OnResume();

			startupWork = new Task(async () =>
					   {
						   await Skills.PackageManager.Instance.RequestPackagesAsync();
						   await DefaultContext.Instance.LoadAsync(Skills.PackageManager.Instance.Packages.FirstOrDefault());
					   });

			startupWork.ContinueWith(t =>
			{
				StartActivity(typeof(MainActivity));
				TaskScheduler.FromCurrentSynchronizationContext();
			});

			if ((int)Build.VERSION.SdkInt >= 23)
				Permissions.Request(this);
			else
				startupWork.Start();

		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			if (Permissions.Evaluate(grantResults) && startupWork != null)
				startupWork.Start();

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}