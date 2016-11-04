using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Teaching.Skills.Contexts;
using Teaching.Skills.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Teaching.Skills.Droid.Activities
{
    [Activity(Label = "@string/login_title", LaunchMode = LaunchMode.SingleTop, NoHistory = true,
              Name = "teaching.skills.LoginActivity")]
    public class LoginActivity : BaseActivity, TextView.IOnEditorActionListener
    {

        private readonly User user;
        private EditText editTextUserName;
        private Button buttonLogin;

        public LoginActivity()
        {
            user = new User();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            SetContentView(Resource.Layout.Login);

            // Get our controls from the layout resource,
            // and attach an event to it
            buttonLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            editTextUserName = FindViewById<EditText>(Resource.Id.editTextUserName);

            editTextUserName.SetOnEditorActionListener(this);

            editTextUserName.TextChanged += editTextUserName_TextChanged;

            //initially set username
            user.Name = editTextUserName.Text;

            //LogIn button click event
            buttonLogin.Click += buttonLogin_Click;

            //request focus to the edit text to start on username.
            editTextUserName.RequestFocus();

            base.OnCreate(savedInstanceState);

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Teaching.Skills.Droid.Helpers.Settings.AppUserName = user.Name;

            if (DefaultContext.Instance.Users.FirstOrDefault(u => u.Name == user.Name) == null)
            {
                DefaultContext.Instance.Users.Add(user);
                if ((int)Build.VERSION.SdkInt < 23 || ((int)Build.VERSION.SdkInt >= 23 && MainApplication.RequestPermissions(this)))
                    Task.Run(() => DefaultContext.Instance.SaveAsync());
            }

            StartActivity(typeof(MainActivity));
        }

        protected void editTextUserName_TextChanged(object sender, EventArgs e)
        {
            user.Name = editTextUserName.Text.Trim();
            buttonLogin.Enabled = !string.IsNullOrEmpty(user.Name);
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

