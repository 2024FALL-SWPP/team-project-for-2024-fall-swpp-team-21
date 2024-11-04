public abstract class ItemBehaviour : SelectableBehaviour
{
    protected PlayerStat playerStat;

    public override void Initialize()
    {
        playerStat = Player.GetComponent<PlayerController>().playerStat;
    }

}
