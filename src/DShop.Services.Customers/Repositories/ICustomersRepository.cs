using System;
using System.Threading.Tasks;
using DShop.Services.Customers.Domain;

namespace DShop.Services.Customers.Repositories
{
    public interface ICustomersRepository
    {
        Task<Customer> GetAsync(Guid id);
        Task CreateAsync(Customer customer);
        Task UpdateAsync(Customer customer);
    }
}