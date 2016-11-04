using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using System;
using System.IO;

namespace Teaching.Skills.Droid
{
    //You can specify additional application information in this attribute
    [Application(Label = "@string/app_title", Theme = "@style/App.Default", Icon = "@drawable/Icon")]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
            OnDebug();
        }

        #region Permissions

        internal static string[] PERMISSIONS_EXTERNALSTORAGE = { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage };

        public static bool VerifyPermissions(Permission[] grantResults)
        {

            if (grantResults.Length < 1)
                return false;
            foreach (var result in grantResults)
                if (result != Permission.Granted)
                    return false;

            return true;
        }
        public static bool VerifyPermissions(Context context, string[] permissions)
        {

            if (permissions.Length < 1)
                return false;
            foreach (var result in permissions)
                if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(context, result) != Permission.Granted)
                    return false;

            return true;
        }

        internal static bool RequestPermissions(Activity activity)
        {
            if (!VerifyPermissions(Application.Context, PERMISSIONS_EXTERNALSTORAGE))
                activity.RequestPermissions(PERMISSIONS_EXTERNALSTORAGE, 1);

            return VerifyPermissions(Application.Context, PERMISSIONS_EXTERNALSTORAGE);
        }

        #endregion

        /// <summary>
        /// Last time the device was used.
        /// </summary>
        public static DateTime LastUseTime { get; set; }

        internal static bool Clear = false;

        [System.Diagnostics.Conditional("DEBUG")]
        public void OnDebug()
        {
            if (MainApplication.Clear)
            {
                Helpers.Settings.AppUserName = string.Empty;
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string filename = Path.Combine(path, "Cache.json");
                File.Delete(filename);
            }
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Services!
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
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
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}