using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tulip_BlazorUI.Models;

namespace Tulip_BlazorUI.Contratcs
{
    public interface IAuthenticationRepository
    {
        Task<bool> Register(RegistrationModel registrationModel);
    }
}
