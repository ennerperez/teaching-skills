using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Teaching.Skills.Droid.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private static readonly string SettingsDefault = string.Empty;

		private const string app_userid = "app_userid";
		private const string app_username = "app_username";

		#endregion

		public static string AppUserId
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(app_userid, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(app_userid, value);
			}
		}
		public static string AppUserName
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(app_username, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(app_username, value);
			}
		}

	}
}