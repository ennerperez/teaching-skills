using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System.Linq;
using System.Reflection;

namespace Teaching.Skills.Droid.Activities
{
    [Activity(Label = "@string/about_title",
              Name = Core.Program.PackageName + ".AboutActivity",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
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

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            textViewTitle.Text = Resources.GetString(Resource.String.app_title);
            textViewVersion.Text = assemblyName.Version.ToString();
            textViewDescription.Text = Resources.GetString(Resource.String.app_description);

            var attribs = (AssemblyMetadataAttribute[])assembly.GetCustomAttributes(typeof(AssemblyMetadataAttribute), true);
            textViewDescription.Text += System.Environment.NewLine + System.Environment.NewLine +
                string.Join(System.Environment.NewLine, attribs.Select(m => m.Value).ToArray());
        }
    }
}