using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;

namespace Teaching.Skills.Droid
{
	public static class Permissions
	{
		#region Permissions

		internal static string[] PERMISSIONS_EXTERNALSTORAGE = { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage };

		public static bool Verify(Permission[] grantResults)
		{

			if (grantResults.Length < 1)
				return false;
			foreach (var result in grantResults)
				if (result != Permission.Granted)
					return false;

			return true;
		}
		public static bool Verify(Context context, string[] permissions)
		{

			if (permissions.Length < 1)
				return false;
			foreach (var result in permissions)
				if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(context, result) != Permission.Granted)
					return false;

			return true;
		}

		internal static bool Request(Activity activity)
		{
			if (!Verify(Application.Context, PERMISSIONS_EXTERNALSTORAGE))
				activity.RequestPermissions(PERMISSIONS_EXTERNALSTORAGE, 1);

			return Verify(Application.Context, PERMISSIONS_EXTERNALSTORAGE);
		}

		#endregion
	}
}
