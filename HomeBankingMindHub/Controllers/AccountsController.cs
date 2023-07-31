using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using HomeBankingMindHub.DTOs;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET api/accounts
        [HttpGet]
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
        [HttpGet("{id}")]
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
    }
}
