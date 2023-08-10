﻿using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Repositories
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Client FindById(long id)
        {
            return FindByCondition(client => client.Id == id)
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoan)
                    .ThenInclude(cl => cl.loan)
                    .Include(client => client.Cards)
                .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
                .Include(client => client.Accounts)
                 .Include(client => client.ClientLoan)
                    .ThenInclude(cl => cl.loan)
                    .Include(client => client.Cards)
                .ToList();
        }

        public void Save(Client client)

        {
            Create(client);
            SaveChanges();
        }
        public Client FindByEmail(string email)
        {
            return FindByCondition(client => client.Email.ToUpper() == email.ToUpper())
            .Include(client => client.Accounts)
            .Include(client => client.ClientLoan)
                .ThenInclude(cl => cl.loan)
            .Include(client => client.Cards)
            .FirstOrDefault();
        }
    }
}
