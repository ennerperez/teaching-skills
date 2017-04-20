using Android.App;
using System;

namespace Teaching.Skills.Droid
{
    internal static class Feedback
    {
        internal static string getTitleSummary(short value)
        {
            if (value > 4)
                return Application.Context.Resources.GetString(Resource.String.feedback_level_4);
            else if (value > 3)
                return Application.Context.Resources.GetString(Resource.String.feedback_level_3);
            else if (value > 2)
                return Application.Context.Resources.GetString(Resource.String.feedback_level_2);
            else if (value > 2)
                return Application.Context.Resources.GetString(Resource.String.feedback_level_1);
            else
                return Application.Context.Resources.GetString(Resource.String.feedback_level_0);
        }

        internal static string getDetailSummary(short value)
        {
            if (value > 4)
                return Application.Context.Resources.GetString(Resource.String.feedback_detail_level_4);
            else if (value > 3)
                return Application.Context.Resources.GetString(Resource.String.feedback_detail_level_3);
            else if (value > 2)
                return Application.Context.Resources.GetString(Resource.String.feedback_detail_level_2);
            else if (value > 2)
                return Application.Context.Resources.GetString(Resource.String.feedback_detail_level_1);
            else
                return Application.Context.Resources.GetString(Resource.String.feedback_detail_level_0);
        }

        internal static void displayAlert(Activity @this, short value, Action action = null)
        {
            new AlertDialog.Builder(@this)
                           .SetPositiveButton("Ok", (s, args) =>
            {
                if (action != null) action();
            })
                           .SetTitle(getTitleSummary(value))
                           .SetMessage(getDetailSummary(value))
                           .Show();
        }
    }
}