using FirebaseDemo.Models;
using FirebaseDemo.Services.Interfaces;
using FirebaseDemo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FirebaseDemo.ViewModels
{
    public class EmployeeListPageViewModel : BaseViewModel
    {
        #region Properties
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private readonly IEmployeeService _employeeService;

        public ObservableCollection<ProductoModel> Employees { get; set; } = new ObservableCollection<ProductoModel>();
        #endregion

        #region Constructor
        public EmployeeListPageViewModel()
        {
            _employeeService = DependencyService.Resolve<IEmployeeService>();
            GetAllEmployee();
        }
        #endregion

        #region Methods
        private void GetAllEmployee()
        {
            IsBusy = true;
            Task.Run(async () =>
            {
                var employeeLIst = await _employeeService.GetAllEmployee();

                Device.BeginInvokeOnMainThread(() =>
                {

                    Employees.Clear();
                    if (employeeLIst?.Count > 0)
                    {
                        foreach (var employee in employeeLIst)
                        {
                            Employees.Add(employee);
                        }
                    }
                    IsBusy = IsRefreshing = false;
                });

            });
        }
        #endregion

        #region Commands

        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;
            GetAllEmployee();
        });

        public ICommand SelectedEmployeeCommand => new Command<ProductoModel>(async (employee) =>
        {
            if (employee != null)
            {
                var response = await App.Current.MainPage.DisplayActionSheet("Opciones!", "Cancelar", null, "Modificar Producto", "Eliminar Producto");

                if (response == "Modificar Producto")
                {
                    await App.Current.MainPage.Navigation.PushAsync(new AddUpdateEmployee(employee));
                }
                else if (response == "Eliminar Producto")
                {
                    IsBusy = true;
                    bool deleteResponse = await _employeeService.DeleteEmployee(employee.Key);
                    if (deleteResponse)
                    {
                        GetAllEmployee();
                    }
                }
            }
        });
        #endregion
    }
}
