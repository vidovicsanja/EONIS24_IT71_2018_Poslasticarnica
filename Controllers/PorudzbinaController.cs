using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Configuration;
using Poslasticarnica.Core;
using Poslasticarnica.Model;
using Stripe;
using Stripe.Checkout;
using System.Data;

namespace Poslasticarnica.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PorudzbinaController : BaseController<Model.KorisnikController>
    {
        public PorudzbinaController(ProjectConfiguration configuration, IKorisnikService userService) : base(configuration, userService)
        {

        }

        [Authorize(Roles = "Kupac")]
        [HttpPost]
        public override IActionResult Add(Model.KorisnikController entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            Model.KorisnikController response = _baseService.Add(entity);

            return Ok(response);
        }

        [Authorize(Roles = "Kupac")]
        [HttpPut("{Id}")]
        public override IActionResult Update(int Id, Model.KorisnikController entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }
            bool response = _baseService.Update(Id, entity);

            return Ok(response);
        }

        [Authorize(Roles = "Kupac")]
        [HttpDelete("{Id}")]
        public override IActionResult Delete(int Id)
        {
            bool response = _baseService.Delete(Id);

            return Ok(response);
        }

        [Authorize(Roles ="Kupac")]
        [HttpGet("byKorisnik/{id}")]
        public override IActionResult Get(int id)
        
        {
            Model.KorisnikController entity = _baseService.Get(id);
            if (entity == null)
            {
                return BadRequest();

            }
            return Ok(entity);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "whsec_SWD3YE07Isi3iyGQOdvTHPFxHAFXv9VS");

            if (stripeEvent.Type == Events.CheckoutSessionAsyncPaymentSucceeded)
            {

            }
            else if (stripeEvent.Type == Events.CheckoutSessionAsyncPaymentFailed)
            {

            }
            else if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                Session? session = stripeEvent.Data.Object as Session;
                _baseService.Add(new Poslasticarnica.Model.KorisnikController { KID = long.Parse(session.Metadata["korisnikId"]), DatumVremePorudzbine = DateTime.Parse(session.Metadata["datumVremePorudzbine"]) });
            }
            else if (stripeEvent.Type == Events.CheckoutSessionExpired)
            {

            }

            return Ok();
        }

    }
}
