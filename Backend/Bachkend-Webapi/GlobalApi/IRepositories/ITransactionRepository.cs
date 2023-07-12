using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalApi.Models;

namespace GlobalApi.IRepositories
{
    public interface ITransactionRepository
    {
        string Add(Transaction transaction);

    }
}