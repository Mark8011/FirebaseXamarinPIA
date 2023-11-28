using Firebase.Database;
using Firebase.Database.Query;
using FirebaseDemo.Models;
using FirebaseDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseDemo.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        FirebaseClient firebase = new FirebaseClient(Setting.FireBaseDatabaseUrl, new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(Setting.FireBaseSeceret)
        });

        public async Task<bool> AddOrUpdateEmployee(ProductoModel employeeModel)
        {
            if (!string.IsNullOrWhiteSpace(employeeModel.Key))
            {
                try
                {
                    await firebase.Child(nameof(ProductoModel)).Child(employeeModel.Key).PutAsync(employeeModel);
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            else
            {
                var response = await firebase.Child(nameof(ProductoModel)).PostAsync(employeeModel);
                if (response.Key != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
         
        }

        public async Task<bool> DeleteEmployee(string key)
        {
            try
            {
                await firebase.Child(nameof(ProductoModel)).Child(key).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<ProductoModel>> GetAllEmployee()
        {
            return (await firebase.Child(nameof(ProductoModel)).OnceAsync<ProductoModel>()).Select(f => new ProductoModel
            {
                NombreProducto = f.Object.NombreProducto,
                Marca = f.Object.Marca,
                Precio = f.Object.Precio,
                Departamento = f.Object.Departamento,
                Key = f.Key
            }).ToList();

        }
    }
}
