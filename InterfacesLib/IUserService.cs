using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace InterfacesLib
{
    public interface IUserService : Orleans.IGrainWithIntegerKey
    {
        Task<bool> Exist(string mobileNumber);
    }

    public class UserService : Grain, IUserService
    {
        public Task<bool> Exist(string mobileNumber)
        {
            return Task.FromResult<bool>(mobileNumber == "123");
        }
    }

}
