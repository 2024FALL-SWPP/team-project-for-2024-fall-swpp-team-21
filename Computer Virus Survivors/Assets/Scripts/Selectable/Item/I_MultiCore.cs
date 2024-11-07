public class I_MultiCore : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.MultiProjectile += 1;
                break;
            case 2:
                playerStat.MultiProjectile += 1;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "투사체가 1개 추가됩니다";
                break;
            case 2:
                explanations[1] = "투사체가 추가로 1개 추가됩니다";
                goto case 1;
        }
    }
}
