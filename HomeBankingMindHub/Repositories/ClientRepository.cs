using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
        public bool ValidatePassword(string password)
        {
            string upperPassword = @"[A-Z]";
            string lowerPassword = @"[a-z]";
            string specialPassword = @"[\W_]";

            bool hasUpper = Regex.IsMatch(password, upperPassword);
            bool hasLower = Regex.IsMatch(password, lowerPassword);
            bool hasSpecial = Regex.IsMatch(password, specialPassword);

            return hasUpper && hasLower && hasSpecial;
        }
    }
}
