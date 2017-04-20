using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Analytics;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Teaching.Skills.Contexts;
using Teaching.Skills.Droid.Adapters;
using Teaching.Skills.Models;

namespace Teaching.Skills.Droid.Activities
{
    [Activity(Label = "@string/item_title",
              Name = Core.Program.PackageName + ".ItemActivity",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ItemActivity : BaseActivity
    {
        private Category category;
        public Category Category { get { return category; } }

        private IEnumerable<Question> questions;
        private User user;

        private int activeIndex;
        public int ActiveIndex { get { return activeIndex; } }

        private SeekBar seekBarItem;
        private TextView textViewItemIndicator;
        private TextView textViewItemTitle;
        private TextView textViewItemDescription;
        private TextView textViewItemValue;
        private Button buttonNext;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.Item);
            base.OnCreate(savedInstanceState);

            var serializer = new XmlSerializer(typeof(Category));

            var buffer = Intent.GetByteArrayExtra("Model");
            var modelStream = new MemoryStream(buffer);

            category = (Category)serializer.Deserialize(modelStream);

            this.Title = category.Name;
            this.SupportActionBar.Title = this.Title;

            seekBarItem = FindViewById<SeekBar>(Resource.Id.seekBarItem);
            textViewItemValue = FindViewById<TextView>(Resource.Id.textViewItemValue);

            textViewItemIndicator = FindViewById<TextView>(Resource.Id.textViewItemIndicator);
            textViewItemTitle = FindViewById<TextView>(Resource.Id.textViewItemTitle);
            textViewItemDescription = FindViewById<TextView>(Resource.Id.textViewItemDescription);
            buttonNext = FindViewById<Button>(Resource.Id.buttonNext);

            seekBarItem.ProgressChanged += seekBarItem_ProgressChanged;
            seekBarItem_ProgressChanged(seekBarItem, new SeekBar.ProgressChangedEventArgs(seekBarItem, 0, true));

            buttonNext.Click += buttonNext_Click;

            user = DefaultContext.Instance.Users.FirstOrDefault(u => u.Id == Helpers.Settings.AppUserId);

            questions = (from item in category.Indicators
                         select item.Questions.Select((a) =>
                         {
                             a.Indicator = item;
                             return a;
                         })).SelectMany((q) => q);

            setQuestion(activeIndex);
        }

        private void setQuestion(int id)
        {
            if (activeIndex < questions.Count())
            {
                var item = questions.ToArray()[id];

                textViewItemIndicator.Text = item.Indicator.Name;
                textViewItemTitle.Text = item.Name;
                textViewItemDescription.Text = item.Description;
                seekBarItem.Progress = 0;

                if (user != null && user.Answers != null)
                {
                    var answer = user.Answers.LastOrDefault(a => a.Question != null && a.Question.Id == item.Id);
                    if (answer != null)
                        seekBarItem.Progress = answer.Value;
                }

                (Application as MainApplication).DefaultTracker.SetScreenName(item.Indicator.Name);
                (Application as MainApplication).DefaultTracker.Send(new HitBuilders.ScreenViewBuilder().Build());
            }

            buttonNext.Text = Resources.GetString((activeIndex < questions.Count() - 1 ? Resource.String.item_next : Resource.String.item_last));
        }

        private async Task SaveAnswer()
        {
            if (user != null)
            {
                if (user.Answers == null)
                    user.Answers = new HashSet<Answer>();

                var question = questions.ToArray()[activeIndex];
                var answer = user.Answers.FirstOrDefault(a => a.Question != null && a.Question.Id == question.Id);
                if (answer == null)
                {
                    user.Answers.Add(new Answer()
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now,
                        Question = question,
                        User = user,
                        Value = seekBarItem.Progress
                    });
                }
                else
                {
                    answer = new Answer()
                    {
                        Date = DateTime.Now,
                        Value = seekBarItem.Progress
                    };
                }

                if ((int)Build.VERSION.SdkInt < 23 || ((int)Build.VERSION.SdkInt >= 23 && Permissions.Request(this)))
                    await DefaultContext.Instance.SaveAsync();
            }
        }

        public override void OnBackPressed()
        {
            if (activeIndex == 0 || activeIndex >= questions.Count())
            {
                var intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent);
            }
            else
            {
                activeIndex--;
                setQuestion(activeIndex);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                OnBackPressed();

            return true;
        }

        private void seekBarItem_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            switch (e.Progress)
            {
                case 1:
                    textViewItemValue.Text = Resources.GetString(Resource.String.likert_level_1);
                    break;

                case 2:
                    textViewItemValue.Text = Resources.GetString(Resource.String.likert_level_2);
                    break;

                case 3:
                    textViewItemValue.Text = Resources.GetString(Resource.String.likert_level_3);
                    break;

                case 4:
                    textViewItemValue.Text = Resources.GetString(Resource.String.likert_level_4);
                    break;

                default:
                    textViewItemValue.Text = Resources.GetString(Resource.String.likert_level_0);
                    break;
            }
        }

        private async void buttonNext_Click(object sender, EventArgs e)
        {
            await SaveAnswer();
            activeIndex++;

            if (activeIndex >= questions.Count())
            {
                var item = DefaultContext.Instance.Categories.First(c => c.Id == category.Id);
                var avg = (short)Math.Round(SummaryAdapter.GetAverage(user, item), 0);
                Feedback.displayAlert(this, avg, OnBackPressed);
            }
            else
                setQuestion(activeIndex);
        }
    }
}