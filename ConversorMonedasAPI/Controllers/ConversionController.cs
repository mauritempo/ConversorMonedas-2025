using DTO.CURRENCY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace ConversorMonedasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversionController : ControllerBase
    {
        private readonly ICurrencyServices _conversionService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IUserServices _userServices;

        public ConversionController(ICurrencyServices conversionService, ISubscriptionService subscriptionService, IUserServices userServices)
        {
            _conversionService = conversionService;
            _subscriptionService = subscriptionService;
            _userServices = userServices;
        }

        // Método auxiliar para verificar si el usuario tiene conversiones restantes
        

        // Endpoint para obtener todas las monedas
        [HttpGet("currencies")]
        public IActionResult GetAllCurrencies()
        {
            

            var currencies = _conversionService.GetAllCurrencies();
            return Ok(currencies);
        }
        // Endpoint para obtener una moneda específica por su ID
        [HttpGet("currency/{id}")]
        public IActionResult GetCurrencyById(int id)
        {          

            var currency = _conversionService.GetCurrencyById(id);
            if (currency == null)
            {
                return NotFound("Moneda no encontrada / estado de la moneda BAJA.");
            }
            return Ok(currency);
        }

        
        // Endpoint para crear una nueva moneda
        [HttpPost("currency")]
        public IActionResult AddCurrency([FromBody] GlobalCurrencyDTO currencyDto)
        {
            

            var currencyId = _conversionService.AddCurrency(currencyDto);
            return CreatedAtAction(nameof(GetCurrencyById), new { id = currencyId}, currencyDto);
        }

        
        // Endpoint para actualizar una moneda existente
        [HttpPut("currency/{id}")]
        public IActionResult UpdateCurrency(int id, [FromBody] GlobalCurrencyDTO currencyDto)
        {
            

            bool result = _conversionService.UpdateCurrency(id, currencyDto);
            bool resultDeleted = _conversionService.UpdateCurrencyDeleted(id, currencyDto);
            if (!result & !resultDeleted)
            {
                
                return NotFound("No se pudo actualizar la moneda, puede que no exista.");
            }
            return NoContent();
        }

        
        // Endpoint para eliminar una moneda (cambio de estado a NonFunctional)
        [HttpDelete("currency/{id}")]
        public IActionResult DeleteCurrency(int id)
        {
            

            bool result = _conversionService.DeleteCurrency(id);
            if (!result)
            {
                return NotFound("No se pudo eliminar la moneda, puede que no exista.");
            }
            return NoContent();
        }

        
        [HttpPost("convert")]
        public IActionResult ConvertCurrency([FromBody] ConversionRequestDTO request)
        {
            // Extraer el token del encabezado
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Extraer el userId del token
            var userId = _userServices.GetUserIdFromToken(token);

            if (userId == 0)
            {
                return NotFound("No se pudo extraer el UserId del token.");
            }

            // Obtener el usuario desde el servicio
            var user = _userServices.GetById(userId);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Verificar si el usuario puede realizar la conversión
            if (!_userServices.CanConvert(userId))
            {
                return BadRequest("No puedes realizar más conversiones. Actualiza tu suscripción.");
            }

            // Realizar la conversión
            var conversionResult = _conversionService.ConvertCurrency(request);
            if (conversionResult == null)
            {
                return BadRequest("No se pudo realizar la conversión" +
                    "1- La moneda esta en estado BAJA." +
                    "2- La moneda no existe." + "Verifica los códigos de moneda.");
            }

            // Incrementar conversiones usadas
            _userServices.IncrementConversionsUsed(userId);

            // Validar si el usuario ha superado el límite de conversiones
            if (user.ConversionsUsed >= user.Subscription.MaxConversions)
            {
                user.canMakeConversions = false; // Cambiar el estado a inactivo si superó el límite
                _userServices.UpdateUser(user); // Actualizar el estado en la base de datos
            }

            return Ok(conversionResult);
        }



    }
}