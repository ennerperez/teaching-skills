using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Reflection;

namespace Teaching.Skills.Droid.Activities
{
    [Activity(Label = "@string/about_title",
              Name = Core.Program.PackageName + ".AboutActivity")]
    public class AboutActivity : BaseActivity
    {

        private TextView textViewTitle;
        private TextView textViewVersion;
        private TextView textViewDescription;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.About);
            base.OnCreate(savedInstanceState);

            textViewTitle = FindViewById<TextView>(Resource.Id.textViewTitle);
            textViewVersion = FindViewById<TextView>(Resource.Id.textViewVersion);
            textViewDescription = FindViewById<TextView>(Resource.Id.textViewDescription);

            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            textViewTitle.Text = Resources.GetString(Resource.String.app_title);
            textViewVersion.Text = assemblyName.Version.ToString();
            textViewDescription.Text = Resources.GetString(Resource.String.app_description);

        }

    }
}

