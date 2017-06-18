using Android.App;
using Android.Gms.Analytics;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using System;
using System.Threading.Tasks;
using Android.Content.PM;

namespace Teaching.Skills.Droid
{
	//You can specify additional application information in this attribute
	[Application(Label = "@string/app_title", Theme = "@style/App.Default", Icon = "@drawable/Icon")]
	public class MainApplication : Application, Application.IActivityLifecycleCallbacks
	{
		public MainApplication(IntPtr handle, JniHandleOwnership transer)
		  : base(handle, transer)
		{
			DefaultTracker.EnableExceptionReporting(true);
			//DefaultTracker.EnableAutoActivityTracking(true);
		}

		private Tracker defaultTracker;

		public Tracker DefaultTracker
		{
			get
			{
				if (defaultTracker == null)
				{
					var analytics = GoogleAnalytics.GetInstance(this);
					// Add your Tracking ID here
					defaultTracker = analytics.NewTracker(Program.TrackingID);
				}
				return defaultTracker;
			}
		}

		private Task startupWork = null;

		/// <summary>
		/// Last time the device was used.
		/// </summary>
		public static DateTime LastUseTime { get; set; }

		public override void OnCreate()
		{
			base.OnCreate();
			RegisterActivityLifecycleCallbacks(this);

			//A great place to initialize Xamarin.Insights and Dependency Services!
			Task.Run(async () => await Teaching.Skills.PackageManager.Instance.RequestPackagesAsync());

		}

		public override void OnTerminate()
		{
			base.OnTerminate();
			UnregisterActivityLifecycleCallbacks(this);
		}

		public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
		{
			CrossCurrentActivity.Current.Activity = activity;
			trackActivity(activity);
		}

		public void OnActivityDestroyed(Activity activity)
		{
		}

		public void OnActivityPaused(Activity activity)
		{
		}

		public void OnActivityResumed(Activity activity)
		{
			CrossCurrentActivity.Current.Activity = activity;
			trackActivity(activity);
		}

		public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
		{
		}

		public void OnActivityStarted(Activity activity)
		{
			CrossCurrentActivity.Current.Activity = activity;
			trackActivity(activity);
		}

		public void OnActivityStopped(Activity activity)
		{
		}

		private void trackActivity(Activity activity)
		{
			DefaultTracker.SetScreenName(activity.Title);
			DefaultTracker.Send(new HitBuilders.ScreenViewBuilder().Build());
		}
	}
}