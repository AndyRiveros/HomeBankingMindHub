using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;

namespace HomeBankingMindHub.Models
{
    public class DbInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())

            {
                var clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", LastName="Coronado", Password="123456"},

                    new Client {Email= "ariveros@gmail.com", FirstName="Andres", LastName="Fernando", Password="123456" }
                };



                foreach (Client client in clients)
                {
                    context.Clients.Add(client);
                }

                //guardamos
                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                var accountVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (accountVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId=accountVictor.Id, CreationDate = DateTime.Now, Number = "VIN001", Balance =10000,
                        }
                    };
                   foreach(Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();

                }
            }

            if (!context.Accounts.Any())
            {
                var accountAndres = context.Clients.FirstOrDefault(c => c.Email == "ariveros@gmail.com");
                if (accountAndres != null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId=accountAndres.Id, CreationDate = DateTime.Now, Number = "VIN002", Balance =1000,
                        }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();

                }
            }

            if (!context.Transactions.Any())

            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "VIN001");

                if (account1 != null)

                {

                    var transactions = new Transaction[]

                    {

                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia recibida", Type = TransactionType.CREDIT.ToString() },

                        new Transaction { AccountId= account1.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT.ToString() },

                        new Transaction { AccountId= account1.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT.ToString() },

                    };

                    foreach (Transaction transaction in transactions)

                    {

                        context.Transactions.Add(transaction);

                    }

                    context.SaveChanges();



                }

            }
        }
    }
}
