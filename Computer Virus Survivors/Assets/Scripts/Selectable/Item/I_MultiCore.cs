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
                explanations[0] = "백신을 구동하는 코어가 추가되어 바이러스 정화 능력이 향상됩니다\n투사체가 1개 추가됩니다";
                break;
            case 2:
                explanations[1] = "투사체가 1개 더 추가됩니다";
                goto case 1;
        }
    }
}
