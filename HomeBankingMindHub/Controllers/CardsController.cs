using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;

namespace HomeBankingMindHub.Controllers
{
    [Route("api")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private ICardRepository _cardRepository;
        private IClientRepository _clientRepository;

        public CardsController(ICardRepository cardRepository, IClientRepository clientRepository)
        {
            _cardRepository = cardRepository;
            _clientRepository = clientRepository;
        }
        [HttpPost("clients/current/cards")]
        public IActionResult Post([FromBody] NewCardDTO card )
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }
                Client currentClient = _clientRepository.FindByEmail(email);
                if (currentClient == null)
                {
                    return Forbid();
                }

                CardType cardType;
                if (!Enum.TryParse(card.Type, out cardType))
                {
                    return StatusCode(403, $"El tipo de tarjeta {card.Type} no es válido");
                }
                CardColor cardColor;
                if (!Enum.TryParse(card.Color, out cardColor))
                {
                    return StatusCode(403, $"El color de tarjeta {card.Color} no es válido");
                }

                int numberOfCards = currentClient.Cards.Where(c => c.Type == card.Type).Count();
                if (numberOfCards >= 3)
                {
                    return StatusCode(403, $"El cliente tiene 3 tarjetas de tipo {card.Type}, no es posible crear otra");
                }

                static string RandomNumber(int digits)
                {
                    Random random = new Random();
                    if (digits <= 0)
                    {
                        throw new ArgumentException("Debe ser mayor que cero.", nameof(digits));
                    }
                    int min = (int)Math.Pow(10, digits - 1);
                    int max = (int)Math.Pow(10, digits);

                    int randomNumber = random.Next(min, max);
                    return randomNumber.ToString();
                }

                ///pending check existing card number
                Card newCard = new Card
                {
                    ClientId = currentClient.Id,
                    CardHolder = $"{currentClient.FirstName} {currentClient.LastName}",
                    Type = cardType.ToString(),
                    Color = cardColor.ToString(),
                    Number = $"{RandomNumber(4)}-{RandomNumber(4)}-{RandomNumber(4)}-{RandomNumber(4)}",
                    Cvv = int.Parse(RandomNumber(3)),
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(4),
                };
                _cardRepository.Save(newCard);

                return Created("Tarjeta creada exitosamente", newCard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("clients/current/cards")]
        public IActionResult Get()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid("Email vacío. ");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("No existe el cliente.");
                }

                var cards = _cardRepository.GetCardsByClient(client.Id);
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
