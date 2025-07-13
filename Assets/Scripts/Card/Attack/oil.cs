using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

public class oil : CardButtonBase
{


    // 全方向二级
    private static readonly Vector2Int[] oilDirections = new Vector2Int[]
   {
    new Vector2Int(-2, -2), new Vector2Int(-2, -1), new Vector2Int(-2, 0), new Vector2Int(-2, 1), new Vector2Int(-2, 2),
    new Vector2Int(-1, -2), new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(-1, 2),
    new Vector2Int(0, -2),  new Vector2Int(0, -1),     /* center */        new Vector2Int(0, 1),  new Vector2Int(0, 2),
    new Vector2Int(1, -2),  new Vector2Int(1, -1),  new Vector2Int(1, 0),  new Vector2Int(1, 1),  new Vector2Int(1, 2),
    new Vector2Int(2, -2),  new Vector2Int(2, -1),  new Vector2Int(2, 0),  new Vector2Int(2, 1),  new Vector2Int(2, 2)
   };

    public override void Initialize(Card card, DeckManager deckManager)
    {
        base.Initialize(card, deckManager);
        card.isQuick = true;
        card.isTemporary = true; // 不确定消耗与临时牌能否通用一个变量——表现形式还不明确
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
                int damage = 0; //伤害为0
                player.damage = damage;
                player.ShowAttackOptions(oilDirections, card);
            }
        }
    }
}

public class Oil : Card
{
    // 构造函数中设置卡牌类型、编号和费用
    public Oil() : base(CardType.Attack, "FA04", 1)
    {
    }

    public override GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Card/Attack/oil_card");
    }

    public override Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/Card/Attack/oil_card");
    }

    public override string GetDescription()
    {
        return "消耗，快速，仅在地形火域上时可以打出；以目标处为中心，所有十字相邻格创造地形燃点。";
    }


    public override void OnCardExecuted()
    {
        base.OnCardExecuted();
        // 攻击伤害的逻辑假设在基类或其他部分已经执行
        // 此处我们在攻击目标处铺设燃点

        Vector2Int pos = GetAttackTargetPosition();

        // 获取目标周围的4个相邻格子
        // 检测是否为合法地图位置，例如有越界或障碍物
        List<Vector2Int> adjacentPositions = new List<Vector2Int>();
        if (IsValidPosition(pos + Vector2Int.up))
        {
            adjacentPositions.Add(pos + Vector2Int.up);
        }
        if (IsValidPosition(pos + Vector2Int.down))
        {
            adjacentPositions.Add(pos + Vector2Int.down);
        }
        if (IsValidPosition(pos + Vector2Int.left))
        {
            adjacentPositions.Add(pos + Vector2Int.left);
        }
        if (IsValidPosition(pos + Vector2Int.right))
        {
            adjacentPositions.Add(pos + Vector2Int.right);
        }

        foreach (var position in adjacentPositions)
        {
            PlaceFirePointAt(position);
        }


    }

    /// <summary>
    /// 获取攻击目标格子，取决于你的战斗系统
    /// 例如，这里假设 player 已经保存了本次攻击的目标位置
    /// </summary>
    private Vector2Int GetAttackTargetPosition()
    {
        // return player.lastAttackSnapshot;
        
        // 不适用instance直接调用偶尔会出现坐标错误问题，原因目前位置
        return Player.Instance.targetAttackPosition;

    }


    private void PlaceFirePointAt(Vector2Int gridPosition)
    {
        Debug.Log("Placing FirePoint at grid position: " + gridPosition);
        LocationManager locationManager = UnityEngine.Object.FindObjectOfType<LocationManager>();
        GameObject firePointPrefab = Resources.Load<GameObject>("Prefabs/Location/FirePoint");
        locationManager.CreateFirePoint(firePointPrefab, gridPosition);
    }

    private bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < player.boardSize && position.y >= 0 && position.y < player.boardSize;
    }
}
