using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using System;
using System.Linq;
using System.Threading.Tasks;
using Teaching.Skills.Contexts;
using Teaching.Skills.Models;

namespace Teaching.Skills.Droid.Activities
{
    [Activity(Label = "@string/login_title", LaunchMode = LaunchMode.SingleTop, NoHistory = true,
              Name = Core.Program.PackageName + ".LoginActivity",
               ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : BaseActivity, TextView.IOnEditorActionListener
    {
        private EditText editTextUserId;
        private EditText editTextUserName;
        private Button buttonLogin;

        public LoginActivity()
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.Login);

            // Get our controls from the layout resource,
            // and attach an event to it
            buttonLogin = FindViewById<Button>(Resource.Id.buttonLogin);

            editTextUserId = FindViewById<EditText>(Resource.Id.editTextUserId);
            editTextUserId.SetOnEditorActionListener(this);
            editTextUserId.TextChanged += editTextUser_TextChanged;

            editTextUserName = FindViewById<EditText>(Resource.Id.editTextUserName);
            editTextUserName.SetOnEditorActionListener(this);
            editTextUserName.TextChanged += editTextUser_TextChanged;

            //LogIn button click event
            buttonLogin.Click += buttonLogin_Click;

            //request focus to the edit text to start on username.
            editTextUserId.RequestFocus();

#if DEBUG
            editTextUserId.Text = "ennerperez@gmail.com";
            editTextUserName.Text = "Enner Pérez";
#endif

            base.OnCreate(savedInstanceState);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            var user = new User()
            {
                Id = editTextUserId.Text.Trim().ToLower(),
                Name = editTextUserName.Text.Trim()
            };

            if (isValidEmail(user.Id))
            {
                Helpers.Settings.AppUserId = user.Id;
                Helpers.Settings.AppUserName = user.Name;

                if (DefaultContext.Instance.Users.FirstOrDefault(u => u.Id == user.Id) == null)
                {
                    DefaultContext.Instance.Users.Add(user);
                    if ((int)Build.VERSION.SdkInt < 23 || ((int)Build.VERSION.SdkInt >= 23 && Permissions.Request(this)))
                        Task.Run(() => DefaultContext.Instance.SaveAsync());
                }

                StartActivity(typeof(MainActivity));
            }
            else
            {
                editTextUserId.SelectAll();
                editTextUserId.RequestFocus();
                Toast.MakeText(ApplicationContext, Resource.String.login_invalid_user_id, ToastLength.Short).Show();
            }
        }

        internal bool isValidEmail(string target)
        {
            return !string.IsNullOrEmpty(target.Trim()) && Android.Util.Patterns.EmailAddress.Matcher(target.Trim()).Matches();
        }

        protected void editTextUser_TextChanged(object sender, EventArgs e)
        {
            var user_Id = editTextUserId.Text.Trim().ToLower();
            var user_Name = editTextUserName.Text.Trim();
            buttonLogin.Enabled = !string.IsNullOrEmpty(user_Id) && !string.IsNullOrEmpty(user_Name);
        }

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            //go edit action will login
            if (actionId == ImeAction.Go)
            {
                if (!string.IsNullOrEmpty(editTextUserName.Text))
                    buttonLogin.PerformClick();
                else
                    editTextUserName.RequestFocus();

                return true;
                //next action will set focus to password edit text.
            }
            else if (actionId == ImeAction.Next)
            {
                if (!string.IsNullOrEmpty(editTextUserName.Text))
                    buttonLogin.RequestFocus();

                return true;
            }
            return false;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return true;
        }
    }
}