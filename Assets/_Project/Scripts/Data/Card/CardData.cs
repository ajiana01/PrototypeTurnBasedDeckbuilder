using UnityEngine;

namespace _Project.Scripts.Data.Card
{
    public enum CardType 
    { 
        Attack, 
        Skill,
        Heal
    }

    [CreateAssetMenu(fileName = "NewCard", menuName = "Data/CardData")]
    public class CardData : ScriptableObject
    {
        [Header("Card Info")]
        public string cardName;
        [TextArea] public string description;
    
        [Header("Mechanics")]
        public int manaCost;
        public CardType type;
    
        [Tooltip("Nilai efek kartu (Bisa berupa Damage atau Heal)")]
        public int effectValue; 
    }
}