using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace bg.hd.banca.juridica.api.Controllers
{
    [Authorize]
    [ApiController]
    public class BaseApiAuthController: ControllerBase
    {
                
    }
}
