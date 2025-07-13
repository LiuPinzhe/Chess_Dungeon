using UnityEngine;
using System.Collections.Generic;


public class tidewyrm_oil_candle : CardButtonBase
{
    Vector2Int[] tidewyrm_oil_candleDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };


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
                int damage = 0; //伤害为0
                player.damage = damage;
                player.ShowAttackOptions(tidewyrm_oil_candleDirections, card);
            }
        }
    }
}

public class Tidewyrm_oil_candle : Card
{
    // 构造函数中设置卡牌类型、编号和费用
    public Tidewyrm_oil_candle() : base(CardType.Attack, "FA03", 1)
    {
    }

    public override GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Card/Attack/tidewyrm_oil_candle_card");
    }

    public override Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/Card/Attack/tidewyrm_oil_candle_card");
    }

    public override string GetDescription()
    {
        return "在临近位置随机创造3处地形燃点";
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
        if (IsValidPosition(pos + new Vector2Int(1, 0)))
        {
            adjacentPositions.Add(pos + new Vector2Int(1, 0));
        }
        if (IsValidPosition(pos + new Vector2Int(-1, 0)))
        {
            adjacentPositions.Add(pos + new Vector2Int(-1, 0));
        }
        if (IsValidPosition(pos + new Vector2Int(0, 1)))
        {
            adjacentPositions.Add(pos + new Vector2Int(0, 1));
        }
        if (IsValidPosition(pos + new Vector2Int(0, -1)))
        {
            adjacentPositions.Add(pos + new Vector2Int(0, -1));
        }


        Debug.Log("adjacentPositions: " + adjacentPositions.Count);

        // 随机选择最多3个位置
        int count = Mathf.Min(3, adjacentPositions.Count);
        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, adjacentPositions.Count);
            Vector2Int chosenPos = adjacentPositions[index];
            PlaceFirePointAt(chosenPos); //放燃点
            adjacentPositions.RemoveAt(index); // 保证不重复
        }
    }

    /// <summary>
    /// 获取攻击目标格子，取决于你的战斗系统
    /// 例如，这里假设 player 已经保存了本次攻击的目标位置 ?
    /// </summary>
    private Vector2Int GetAttackTargetPosition()
    {
        // return player.lastAttackSnapshot;
        return Player.Instance.targetAttackPosition;

    }


    // firePoint effect 是不是也放到 KeyWordEffects.cs 比较好

    private void PlaceFirePointAt(Vector2Int gridPosition)
    {
        Debug.Log("Placing FirePoint at grid position: " + gridPosition);
        LocationManager locationManager = UnityEngine.Object.FindObjectOfType<LocationManager>();
        GameObject firePointPrefab = Resources.Load<GameObject>("Prefabs/Location/FirePoint");
        locationManager.CreateFirePoint(firePointPrefab, gridPosition);
    }

    private bool IsValidPosition(Vector2Int position)
    {
            bool valid = position.x >= 0 && position.x < player.boardSize &&
                 position.y >= 0 && position.y < player.boardSize;

    if (!valid)
    {
        Debug.Log($"越界格子被排除: {position}");
    }

    return valid;
    }
}

