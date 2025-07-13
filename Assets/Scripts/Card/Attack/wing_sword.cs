using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

public class wing_sword_card : CardButtonBase
{
    Vector2Int[] wingSwordDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

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
                // Debug.Log("wing_sword_cardd deselected.");
            }
            else
            {
                int damage = 2;
                player.damage = damage;
                player.ShowAttackOptions(wingSwordDirections, card);
                Debug.Log("wing_sword_card: Showing attack options with flame effect.");
            }
        }
        else
        {
            Debug.LogError("Card is null in wing_sword_card.OnClick");
        }
    }
}

public class WingSword : Card
{
    // 构造函数中设置卡牌类型、编号和费用
    public WingSword() : base(CardType.Attack, "BA01", 1)
    {
    }
    
    public override GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Card/Attack/wing_sword_card");
    }
    
    public override Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/Card/Attack/wing_sword_card");
    }
    
    public override string GetDescription()
    {
        return "造成2点伤害";
    }
    
    public override void OnCardExecuted(Vector2Int gridPosition)
    {
        base.OnCardExecuted();
        
        // 攻击伤害的逻辑假设在基类或其他部分已经执行
        // 此处我们在攻击目标处铺设燃点
        
        Vector2Int targetPos = GetAttackTargetPosition(); // 以下保留为后续升级做准备
    }
    
    /// <summary>
    /// 一下保留为后续有翼之剑升级做准备
    /// 获取攻击目标格子，取决于你的战斗系统
    /// 例如，这里假设 player 已经保存了本次攻击的目标位置
    /// </summary>
    private Vector2Int GetAttackTargetPosition()
    {
        return player.lastAttackSnapshot;
    }

    private void PlaceFirePointAt(Vector2Int gridPosition)
    {
        Debug.Log("Placing FirePoint at grid position: " + gridPosition);
        LocationManager locationManager = UnityEngine.Object.FindObjectOfType<LocationManager>();
        GameObject firePointPrefab = Resources.Load<GameObject>("Prefabs/Location/FirePoint");
        locationManager.CreateFirePoint(firePointPrefab, gridPosition);
    }
}
