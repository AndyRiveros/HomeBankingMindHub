
namespace HomeBankingMindHub.Models
{
    public class ClientLoan
    {
        public Client Client { get; set; }

        public long ClientId { get; set; }

        public double Amount { get; set; }

        public long Id { get; set; }

        public string Payments { get; set; }

        public long LoanId { get; set; }

        public Loan loan { get; set; }

        
    }
}
