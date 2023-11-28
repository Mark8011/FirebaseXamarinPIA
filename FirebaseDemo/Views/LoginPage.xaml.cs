using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evidencia2EZS.ViewModels;
using FirebaseDemo.Services.Implementations;
using FirebaseDemo.Services.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Evidencia2EZS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);


            DependencyService.Register<IEmployeeService, EmployeeService>();

            BindingContext = new LoginViewModel();

        }

        private async void SignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
        

    }
}