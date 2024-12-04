public class I_MemoryUpgrade : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.MaxHP += 20; // 20
                break;
            case 2:
                playerStat.MaxHP += 20; // 40
                break;
            case 3:
                playerStat.MaxHP += 20; // 60
                break;
            case 4:
                playerStat.MaxHP += 20; // 80
                break;
            case 5:
                playerStat.MaxHP += 20; // 100
                break;
            case 6:
                playerStat.MaxHP += 40; // 140
                break;
            case 7:
                playerStat.MaxHP += 40; // 180
                break;
            case 8:
                playerStat.MaxHP += 40; // 220
                break;
            case 9:
                playerStat.MaxHP += 80; // 300
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "백신 코드를 보호하여 바이러스의 공격을 더 많이 버틸 수 있습니다\n최대 체력 20 증가";
                break;
            case 2:
                explanations[1] = "최대 체력 20 증가";
                goto case 1;
            case 3:
                explanations[2] = "최대 체력 20 증가";
                goto case 2;
            case 4:
                explanations[3] = "최대 체력 20 증가";
                goto case 3;
            case 5:
                explanations[4] = "최대 체력 20 증가";
                goto case 4;
            case 6:
                explanations[5] = "최대 체력 40 증가";
                goto case 5;
            case 7:
                explanations[6] = "최대 체력 40 증가";
                goto case 6;
            case 8:
                explanations[7] = "최대 체력 40 증가";
                goto case 7;
            case 9:
                explanations[8] = "최대 체력 80 증가";
                goto case 8;
        }
    }
}
