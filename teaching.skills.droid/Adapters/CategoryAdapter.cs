using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Teaching.Skills.Models;

namespace Teaching.Skills.Droid.Adapters
{
    public class CategoryAdapter : BaseAdapter<Category>
    {
        public CategoryAdapter(IEnumerable<Category> source) : base(source)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
                convertView = CreateView(parent);

            var item = Get(position);
            var viewHolder = (ViewHolder)convertView.Tag;
            viewHolder.textViewTitle.Text = item.Name;
            viewHolder.textViewSubTitle.Text = item.Description;

            return convertView;
        }

        private View CreateView(ViewGroup parent)
        {
            View convertView;
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            var scorecardItem = (ViewGroup)inflater.Inflate(Resource.Layout.TwoLineItem, parent, false);
            convertView = scorecardItem;
            var holder = new ViewHolder(scorecardItem);
            convertView.Tag = holder;
            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            internal TextView textViewTitle;
            internal TextView textViewSubTitle;

            public ViewHolder(ViewGroup item)
            {
                textViewTitle = item.FindViewById<TextView>(Resource.Id.textViewTitle);
                textViewSubTitle = item.FindViewById<TextView>(Resource.Id.textViewSubTitle);
            }
        }
    }
}