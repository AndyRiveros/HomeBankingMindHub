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
                    return Forbid("Email vacío. ");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid("No existe el cliente.");
                }

                //CardType cardType;
                //if(!Enum.TryParse(card.Type, out cardType))
                //{
                //    return StatusCode(403, $"El tipo de tarjeta {card.Type} no es válido");
                //}
                //CardColor cardColor;
                //if (!Enum.TryParse(card.Color, out cardColor))
                //{
                //    return StatusCode(403, $"El tipo de tarjeta {card.Type} no es válido");
                //}
                //int numberOfCards= client.Cards.Where( card => card.Type == card.Type).Count();
                //if (numberOfCards>=3)
                //{
                //    return StatusCode(403, $"El cliente tiene 3 tarjetas de tipo {card.Type}, no es posible crear otra");
                //}

               if (client.Cards.Count >= 3)
                {
                   return Forbid();
                }

                Random random = new Random();
                int next()
                {
                    return random.Next(1000, 10000);
                }

                var newCard = new Card
                {
                    ClientId = client.Id,
                    CardHolder = client.FirstName + " " + client.LastName,
                    Type = card.Type,
                    Color = card.Color,
                    Number = $"{next()}-{next()}-{next()}-{next()}",
                    Cvv = random.Next(100, 1000),
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(4),
                   
                };

                _cardRepository.Save(newCard);
                return Ok(newCard);
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
