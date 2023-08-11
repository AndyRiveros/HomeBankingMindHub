using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using HomeBankingMindHub.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HomeBankingMindHub.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }

        // GET api/accounts
        [HttpGet("accounts")]
        public IActionResult Get()
        {

            try
            {
                var accounts = _accountRepository.GetAllAccounts();
                var accountDTOs = new List<AccountDTO>();
                foreach (var account in accounts) {
                    AccountDTO newAccountDTO = new AccountDTO
                    
                    {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(transaction => new TransactionDTO
                    {
                        Id = transaction.Id,
                        Description = transaction.Description,
                        Date = transaction.Date,
                        Amount = transaction.Amount,
                        Type = transaction.Type,


                    }).ToList()
                };
                    accountDTOs.Add(newAccountDTO);
            }
                
                return Ok(accountDTOs);    
               

                
            }
            catch (Exception ex)
            {
                // Manejo de la excepción y devolución de una respuesta de error
                return StatusCode(500, "Ocurrió un error al obtener las cuentas: " + ex.Message);
            }
        }

        // GET api/accounts/{id}
        [HttpGet("accounts/{id}")]
        public IActionResult get(long id) 
        {
            try
            {
                var account = _accountRepository.FindById(id);

                if (account == null)
                {
                    return NotFound();
                }

                var accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(transaction => new TransactionDTO
                    {
                        Id = transaction.Id,
                        Description = transaction.Description,
                        Date = transaction.Date,
                        Amount = transaction.Amount,
                        Type = transaction.Type,

                    }).ToList()
                };

                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                // Manejo de la excepción y devolución de una respuesta de error
                return StatusCode(500, "Ocurrió un error al obtener la cuenta: " + ex.Message);
            }
        }
        [HttpGet("clients/current/accounts")]

        public IActionResult GetCurrentAccount () 
        {

            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }


                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }

                var userAccounts = _accountRepository.GetAccountsByClient(client.Id);
                return Ok(userAccounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [HttpPost("clients/current/accounts")]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }


                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }


                if (client.Accounts.Count >= 3)
                {
                    return Forbid();
                }


                Random random = new();

                var account = new Account
                {
                    Number = "VIN-" + random.Next(100000, 1000000).ToString(),
                    Balance = 0,
                    ClientId = client.Id,
                    CreationDate = DateTime.Now,
                };

                _accountRepository.Save(account);

                return Created("", account);
                    
            }catch(Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
            
        }
            

    }
}
