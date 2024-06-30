using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Configuration;
using Poslasticarnica.Core;

using Poslasticarnica.Model;
using Poslasticarnica.Model.dto;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Poslasticarnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class KorisnikController : BaseController<Korisnik>
    {
        public KorisnikController(ProjectConfiguration configuration, IKorisnikService userService) : base(configuration, userService)
        {

        }

        [HttpGet("current")]
        public IActionResult GetCurrentAction()
        {
            Korisnik korisnik = GetCurrentUser();

            return Ok(korisnik);
        }


        [HttpPost("register")]
        public IActionResult Register(RegistrationDTO korisnik)
        {
            Korisnik k = _userService.Register(korisnik);


            return Ok(k);
        }

        [HttpPost("addSeller")]
        public IActionResult AddSeller(RegistrationDTO korisnik)
        {
            Korisnik k = _userService.AddSeller(korisnik);


            return Ok(k);
        }

    }
}

