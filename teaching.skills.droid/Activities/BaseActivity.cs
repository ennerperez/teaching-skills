using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using System;
using System.IO;

namespace Teaching.Skills.Droid.Activities
{

	[Activity(Label = "@string/app_title", Icon = "@drawable/icon", Theme = "@style/App.Default",
			  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class BaseActivity : AppCompatActivity
	{
		public BaseActivity()
		{
		}

		protected override void OnPause()
		{
			base.OnPause();

			MainApplication.LastUseTime = DateTime.UtcNow;
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			if (toolbar != null)
			{
				SetSupportActionBar(toolbar);
				SupportActionBar.Title = this.Title;
				SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				SupportActionBar.SetHomeButtonEnabled(true);
			}

		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			Permissions.Verify(grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		public static Intent CreateIntent<T>(object model = null) where T : Activity
		{
			var intent = new Intent(Application.Context, typeof(T));
			if (model != null)
			{
				var serializer = new System.Xml.Serialization.XmlSerializer(model.GetType());
				var modelStream = new MemoryStream();
				serializer.Serialize(modelStream, model);
				intent.PutExtra("Model", modelStream.ToArray());
			}
			return intent;
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
				Finish();

			return base.OnOptionsItemSelected(item);
		}
	}
}

