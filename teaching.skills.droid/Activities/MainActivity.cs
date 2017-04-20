using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;
using Teaching.Skills.Contexts;
using Teaching.Skills.Droid.Adapters;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace Teaching.Skills.Droid.Activities
{
    [Activity(Name = Core.Program.PackageName + ".MainActivity",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity
    {
        private TextView textViewUserName;
        private ListView listViewCategories;

        private CategoryAdapter categoryAdapter;
        private CategoryAdapter CategoryAdapter => categoryAdapter = categoryAdapter ?? new CategoryAdapter(DefaultContext.Instance.Categories);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.Main);
            base.OnCreate(savedInstanceState);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);

            textViewUserName = FindViewById<TextView>(Resource.Id.textViewUserName);
            textViewUserName.Text = string.Format(Resources.GetString(Resource.String.main_welcome), Helpers.Settings.AppUserName);

            listViewCategories = FindViewById<ListView>(Resource.Id.listViewCategories);
            listViewCategories.Adapter = CategoryAdapter;

            listViewCategories.ItemClick += listViewCategories_ItemClick; ;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Home, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_about:
                    StartActivity(typeof(AboutActivity));
                    break;

                case Resource.Id.menu_summary:
                    StartActivity(typeof(SummaryActivity));
                    break;

                case Resource.Id.menu_logout:
                    var builder = new AlertDialog.Builder(this);
                    builder.SetTitle(Resources.GetString(Resource.String.logout_title))
                           .SetMessage(Resources.GetString(Resource.String.logout_confirm))
                           .SetPositiveButton(Resources.GetString(Resource.String.yes), logOut)
                           .SetNegativeButton(Resources.GetString(Resource.String.no), delegate { });

                    builder.Create().Show();
                    break;
            }

            return true; //base.OnOptionsItemSelected(item);
        }

        private void listViewCategories_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var category = DefaultContext.Instance.Categories[e.Position];
            StartActivity(CreateIntent<ItemActivity>(category));
        }

        private void logOut(object sender, EventArgs e)
        {
            Helpers.Settings.AppUserId = string.Empty;
            Helpers.Settings.AppUserName = string.Empty;
            OnResume();
        }

        protected override void OnResume()
        {
            base.OnResume();

            var user = DefaultContext.Instance.Users.FirstOrDefault(m => m.Id == Helpers.Settings.AppUserId);

            if (user == null)
                StartActivity(typeof(LoginActivity));
        }
    }
}