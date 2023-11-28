using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Auth;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Evidencia2EZS.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Text;
using FirebaseDemo.Views;
using FirebaseDemo;

namespace Evidencia2EZS.ViewModels
{
    public class LoginViewModel : BaseViewModel2
    {
        #region Attribute
        public string email;
        public string password;
        public bool isRunning;
        public bool isVisible;
        public bool isEnabled;
        #endregion

        #region Properties
        public string EmailTxt
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string PasswordTxt
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRunningTxt
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }


        public bool IsVisibleTxt
        {
            get { return this.isVisible; }
            set { SetValue(ref this.isVisible, value); }
        }

        public bool IsEnabledTxt
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(LoginMethod);
            }
        }
        #endregion

        #region Methods


        public async void LoginMethod()
        {


            if (string.IsNullOrEmpty(this.email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese un email.",
                    "Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese una constraseña.",
                    "Accept");
                return;
            }

            string WebAPIkey = "AIzaSyA2kWDUIBKFcrBDlKIvkOg_vdUeNr1CYEc";


            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(EmailTxt.ToString(), PasswordTxt.ToString());
                var content = await auth.GetFreshAuthAsync();
                var serializedcontnet = JsonConvert.SerializeObject(content);

                Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);

                // Create a new LoginPage
                var homePage = new MainPage();

                // Hide the back button on the navigation bar
                //NavigationPage.SetHasBackButton(homePage, false);

                //var homeViewModel = new IndexViewModel();
                //homeViewModel.email = EmailTxt;
                //homePage.BindingContext = homeViewModel;

                // Push the HomePage onto the navigation stack
                await Application.Current.MainPage.Navigation.PushAsync(homePage);

                
                



            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Email o contraseña invalidos", "OK");
            }

            this.IsVisibleTxt = true;
            this.IsRunningTxt = true;
            this.IsEnabledTxt = false;

            await Task.Delay(20);

            this.IsRunningTxt = false;
            this.IsVisibleTxt = false;
            this.IsEnabledTxt = true;

        }

       


        public async void ResetPasswordEmail()
        {
            string WebAPIkey = "AIzaSyBoiuQpzhW98ELg3IH25RTKkaMERWRWksA";

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
                await authProvider.SendPasswordResetEmailAsync(email);
            }
            catch (Exception ex)
            {

            }

        }

        #endregion

        #region Constructor
        public LoginViewModel()
        {
            this.IsEnabledTxt = true;
        }
        #endregion
    }
}
