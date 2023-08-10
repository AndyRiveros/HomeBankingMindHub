using HomeBankingMindHub.Models;
using System.Collections;
using System.Collections.Generic;

namespace HomeBankingMindHub.Repositories
{
    public interface ICardRepository
    {
        IEnumerable<Card> FindAllCards();
        Card FindById( long  id );
        IEnumerable<Card> GetCardsByClient ( long clientId );

        void Save (Card card );
    }
}
