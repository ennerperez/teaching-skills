using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using Teaching.Skills.Contexts;
using Teaching.Skills.Models;

namespace Teaching.Skills.Droid.Adapters
{
    public class SummaryAdapter : BaseAdapter<Indicator>
    {
        public SummaryAdapter(IEnumerable<Indicator> source) : base(source)
        {
        }

        public static double GetAverage(User user, Indicator indicator)
        {
            double avg = -1;

            if (user != null)
            {
                var questions = from q in DefaultContext.Instance.Questions
                                join a in user.Answers on q.Id equals a.Question.Id
                                select q;

                var data = from y in indicator.Questions
                           join z in user.Answers on y.Id equals z.Question.Id
                           where questions.Select(q => q.Id).Contains(y.Id)
                           select z.Value + 1;

                avg = 0;
                if (data != null && data.Count() > 0)
                    avg = data.Average();
            }

            return avg;
        }

        public static double GetAverage(User user, Category category)
        {
            double avg = -1;

            if (user != null)
            {
                var questions = from q in DefaultContext.Instance.Questions
                                join a in user.Answers on q.Id equals a.Question.Id
                                select q;

                var data = from x in category.Indicators
                           from y in x.Questions
                           join z in user.Answers on y.Id equals z.Question.Id
                           where questions.Select(q => q.Id).Contains(y.Id)
                           select z.Value + 1;

                avg = 0;
                if (data != null && data.Count() > 0)
                    avg = data.Average();
            }
            return avg;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
                convertView = CreateView(parent);

            var user = DefaultContext.Instance.Users.FirstOrDefault(u => u.Id == Helpers.Settings.AppUserId);
            var item = Get(position);

            double avg = GetAverage(user, item);

            var viewHolder = (ViewHolder)convertView.Tag;
            viewHolder.textViewIndicator.Text = item.Name;
            viewHolder.ratingBarValue.Rating = (float)avg;

            return convertView;
        }

        private View CreateView(ViewGroup parent)
        {
            View convertView;
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            var scorecardItem = (ViewGroup)inflater.Inflate(Resource.Layout.SummaryItem, parent, false);
            convertView = scorecardItem;
            var holder = new ViewHolder(scorecardItem);
            convertView.Tag = holder;
            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            internal TextView textViewIndicator;
            internal RatingBar ratingBarValue;

            public ViewHolder(ViewGroup item)
            {
                textViewIndicator = item.FindViewById<TextView>(Resource.Id.textViewIndicator);
                ratingBarValue = item.FindViewById<RatingBar>(Resource.Id.ratingBarValue);
            }
        }
    }
}