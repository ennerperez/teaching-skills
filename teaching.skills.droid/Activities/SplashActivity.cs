using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using System.Globalization;
using System.Threading.Tasks;
using Teaching.Skills.Contexts;

namespace Teaching.Skills.Droid.Activities
{
    [Activity(Theme = "@style/App.Splash", MainLauncher = true, NoHistory = true,
              Name = Core.Program.PackageName + ".SplashActivity",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        private static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(async () =>
           {
               Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
               if ((int)Build.VERSION.SdkInt < 23 || ((int)Build.VERSION.SdkInt >= 23 && Permissions.Request(this)))
               {
                   var localizableDataSource = string.Format("DataSource.{0}.json", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower());
                   await DefaultContext.Instance.LoadAsync(Android.App.Application.Context.Assets.Open(localizableDataSource));
               }

               Log.Debug(TAG, "Working in the background - important stuff.");
           });

            startupWork.ContinueWith(t =>
            {
                Log.Debug(TAG, "Work is finished - start MainActivity.");
                StartActivity(typeof(MainActivity));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Permissions.Verify(grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}