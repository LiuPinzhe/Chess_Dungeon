using UnityEngine;
using System.Collections.Generic;


public class flameberge_card : CardButtonBase
{
    Vector2Int[] flamebergeDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    
    public override void Initialize(Card card, DeckManager deckManager)
    {
        base.Initialize(card, deckManager);
    }

    protected override void Start()
    {
        base.Start();

        if (card != null && card.IsUpgraded())
        {
            Transform glow = transform.Find("UpgradeEffect");
            if (glow != null)
                glow.gameObject.SetActive(true);
        }
    }

    protected override void OnClick()
    {
        if (card != null)
        {
            if (player.currentCard == card)
            {
                player.DeselectCurrentCard();
            }
            else
            {
                int damage = 2;
                player.damage = damage;
                player.ShowAttackOptions(flamebergeDirections, card);
            }
        }
    }
}

public class Flameberge : Card
{

    public DeckManager deckManager;
    public LocationManager locationManager;

    public Flameberge() : base(CardType.Attack, "FA08", 1)
    {
        deckManager = UnityEngine.Object.FindObjectOfType<DeckManager>();
        locationManager = UnityEngine.Object.FindObjectOfType<LocationManager>();
    }
    
    public override GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Card/Attack/flameberge_card");
    }
    
    public override Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/Card/Attack/flameberge_card");
    }
    
    public override string GetDescription()
    {
        return "造成2点伤害；丢弃手牌区内所有卡牌，并触发火域伤害X次，X为丢弃数";
    }
    
    public override void OnCardExecuted()
    {
      base.OnCardExecuted();

      if (deckManager == null)
           deckManager = UnityEngine.Object.FindObjectOfType<DeckManager>();
    
       if (deckManager == null) return;

       int triggerTime = deckManager.hand.Count;
    
          // Use Player or another persistent MonoBehaviour instead
        Player player = UnityEngine.Object.FindObjectOfType<Player>();
      if (player != null)
          player.StartCoroutine(DiscardAfterFrame(triggerTime));
    }

private System.Collections.IEnumerator DiscardAfterFrame(int triggerTime)
{
    yield return null;
    
    var currentDeckManager = UnityEngine.Object.FindObjectOfType<DeckManager>();
    if (currentDeckManager != null)
    {
        currentDeckManager.DiscardHand();
        for (int i = 0; i < triggerTime; i++)
        {
            TriggerOnMonsterTurnStart();
        }
    }
}


    
    public void TriggerOnMonsterTurnStart()    // end turn trigger
    {
        foreach (FireZone zone in locationManager.activeFireZones)
        {
            if (zone != null)
            {
                zone.remainingEnemyTurns++; // don't affect the remaining turn
                zone.OnEnemyTurnStart(); // currently won't dicreases the remaining turn
            }
        }
        //MoveMonsters();
    }

}
