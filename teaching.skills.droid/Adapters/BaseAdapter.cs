using Android.Widget;
using System.Collections.Generic;
using System.Linq;

namespace Teaching.Skills.Droid.Adapters
{
    public abstract class BaseAdapter<T> : Android.Widget.BaseAdapter //where T : Java.Lang.Object
    {
        private readonly List<T> list;

        protected BaseAdapter(IEnumerable<T> source)
        {
            list = source.ToList();
        }

        public override int Count
        {
            get
            {
                if (list != null)
                    return list.Count;
                else
                    return 0;
            }
        }

        public T Get(int position)
        {
            return list[position];
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return (position > Count || position < 0) ? AdapterView.InvalidPosition : position;
        }
    }
}