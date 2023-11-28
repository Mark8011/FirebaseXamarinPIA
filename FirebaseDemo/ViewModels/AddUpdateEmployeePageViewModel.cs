using FirebaseDemo.Models;
using FirebaseDemo.Services.Implementations;
using FirebaseDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FirebaseDemo.ViewModels
{
    public class AddUpdateEmployeePageViewModel : BaseViewModel
    {
        #region Properties
        private readonly IEmployeeService _employeeService;

        private ProductoModel _employeeDetail = new ProductoModel();
        public ProductoModel EmployeeDetail
        {
            get => _employeeDetail;
            set => SetProperty(ref _employeeDetail, value);
        }
        #endregion

        #region Constructor
        public AddUpdateEmployeePageViewModel()
        {
            _employeeService = DependencyService.Resolve<IEmployeeService>();
        }

        public AddUpdateEmployeePageViewModel(ProductoModel employeeResponse)
        {


            _employeeService = DependencyService.Resolve<IEmployeeService>();
            EmployeeDetail =  new ProductoModel
            {
                Key = employeeResponse.Key,
                NombreProducto = employeeResponse.NombreProducto,
                Marca = employeeResponse.Marca,
                Precio = employeeResponse.Precio,
                Departamento = employeeResponse.Departamento
              
            };
        }
        #endregion

        #region Commands
        public ICommand SaveEmployeeCommand => new Command(async () =>
        {
            int nombreProducto;
            bool isNombreProductoInt = int.TryParse(EmployeeDetail.NombreProducto, out nombreProducto);

            int marcaProducto;
            bool isMarcaProductoInt = int.TryParse(EmployeeDetail.Marca, out marcaProducto);

            int departamentoProducto;
            bool isDepaProductoInt = int.TryParse(EmployeeDetail.Marca, out departamentoProducto);

            if (isNombreProductoInt)
            {
                await App.Current.MainPage.DisplayAlert("Error!", "Nombre no puede ser un número", "Ok");
            }
            if (isMarcaProductoInt)
            {
                await App.Current.MainPage.DisplayAlert("Error!", "Marca no puede ser un número", "Ok");
            }
            if (isDepaProductoInt)
            {
                await App.Current.MainPage.DisplayAlert("Error!", "Marca no puede ser un número", "Ok");
            }

            else
            {

                if (IsBusy) return;
                IsBusy = true;
                bool res = await _employeeService.AddOrUpdateEmployee(EmployeeDetail);
                if (res)
                {

                    if (!string.IsNullOrWhiteSpace(EmployeeDetail.Key))
                    {
                        await App.Current.MainPage.DisplayAlert("Aviso!", "Producto Modificado.", "Ok");
                    }
                    else
                    {
                        EmployeeDetail = new ProductoModel() { };
                        await App.Current.MainPage.DisplayAlert("Aviso!", "Producto Agregado.", "Ok");
                    }
                }
                IsBusy = false;

            }
            
        });
        #endregion
    }
}
