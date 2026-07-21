using System;
using _Project.Scripts.Data.Card;

namespace _Project.Scripts.Gameplay.Card
{
    /// <summary>
    /// A runtime representation of a card. 
    /// Each time a card is added to a deck or drawn, it is wrapped in this class
    /// to guarantee it has a 100% unique identity, even if it's a duplicate card.
    /// </summary>
    public class CardInstance
    {
        public string InstanceID { get; private set; }
        public CardData BaseData { get; private set; }

        public CardInstance(CardData baseData)
        {
            // Generate a universally unique identifier (UUID) for this specific card
            InstanceID = Guid.NewGuid().ToString(); 
            BaseData = baseData;
        }
    }
}
