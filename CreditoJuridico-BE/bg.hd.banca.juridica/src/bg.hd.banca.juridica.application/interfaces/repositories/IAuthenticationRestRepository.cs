using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.interfaces.repositories
{
    public interface IAuthenticationRestRepository
    {
        Task<string> GetAccessToken();
    }
}
