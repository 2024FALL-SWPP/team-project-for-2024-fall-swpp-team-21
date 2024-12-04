public class I_CritPoint : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.CritPoint += 15;
                break;
            case 2:
                playerStat.CritPoint += 15;
                break;
            case 3:
                playerStat.CritPoint += 15;
                break;
            case 4:
                playerStat.CritPoint += 15;
                break;
            case 5:
                playerStat.CritPoint += 20;
                break;
            case 6:
                playerStat.CritPoint += 20;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "백신이 바이러스의 취약점을 더 강하게 공격합니다\n치명타 데미지 15% 증가";
                break;
            case 2:
                explanations[1] = "치명타 데미지 15% 증가";
                goto case 1;
            case 3:
                explanations[2] = "치명타 데미지 15% 증가";
                goto case 2;
            case 4:
                explanations[3] = "치명타 데미지 15% 증가";
                goto case 3;
            case 5:
                explanations[4] = "치명타 데미지 20% 증가";
                goto case 4;
            case 6:
                explanations[5] = "치명타 데미지 20% 증가";
                goto case 5;
        }
    }
}
