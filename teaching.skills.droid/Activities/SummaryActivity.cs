using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System.Linq;
using Teaching.Skills.Contexts;
using Teaching.Skills.Droid.Adapters;

namespace Teaching.Skills.Droid.Activities
{
	[Activity(Label = "@string/summary_title",
			  Name = Program.PackageName + ".SummaryActivity",
			  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SummaryActivity : BaseActivity
	{
		private ListView listViewCategoriesSummary;

		private SummaryAdapter adapterSummary;
		private SummaryAdapter AdapterSummary => adapterSummary = adapterSummary ?? new SummaryAdapter(DefaultContext.Instance.Categories.SelectMany(item => item.Indicators));

		protected override void OnCreate(Bundle savedInstanceState)
		{
			SetContentView(Resource.Layout.Summary);
			base.OnCreate(savedInstanceState);

			listViewCategoriesSummary = FindViewById<ListView>(Resource.Id.listViewCategoriesSummary);
			listViewCategoriesSummary.Adapter = new SummaryAdapter(DefaultContext.Instance.Categories.SelectMany(item => item.Indicators));

			listViewCategoriesSummary.ItemClick += listViewCategoriesSummary_ItemClick;
		}

		private void listViewCategoriesSummary_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var view = e.View;
			var ratingBar = view.FindViewById<RatingBar>(Resource.Id.ratingBarValue);
			var value = (short)System.Math.Round(ratingBar.Rating, 0);

			Feedback.displayAlert(this, value);
		}
	}
}