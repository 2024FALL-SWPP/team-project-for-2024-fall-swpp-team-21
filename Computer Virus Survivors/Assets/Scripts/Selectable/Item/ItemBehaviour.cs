using UnityEngine;

abstract public class ItemBehaviour : SelectableBehaviour
{

    [SerializeField] private ItemData itemData;

    protected int level
    {
        get
        {
            return itemData.currentLevel;
        }
        set
        {
            itemData.currentLevel = value;
        }
    }

    protected PlayerStat playerStat;

    abstract protected void LevelUpEffect(int level);

    protected override void LevelUp()
    {
        if (level < itemData.levelMax)
        {
            level++;
            Debug.Log($"Item<{itemData.itemName}> Level Up! Level: " + level);
            LevelUpEffect(level);
        }

    }
    public override void GetSelectable(PlayerController player)
    {
        if (level == 0)
        {
            InitializeItem(player.playerStat);
        }

        LevelUp();
    }

    protected void InitializeItem(PlayerStat playerStat)
    {
        this.playerStat = playerStat;
    }

}
