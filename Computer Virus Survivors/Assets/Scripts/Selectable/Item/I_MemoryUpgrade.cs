public class I_MemoryUpgrade : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.MaxHp += 20;
                break;
            case 2:
                playerStat.MaxHp += 20;
                break;
            case 3:
                playerStat.MaxHp += 20;
                break;
            case 4:
                playerStat.MaxHp += 20;
                break;
            case 5:
                playerStat.MaxHp += 20;
                break;
            case 6:
                playerStat.MaxHp += 40;
                break;
            case 7:
                playerStat.MaxHp += 40;
                break;
            case 8:
                playerStat.MaxHp += 40;
                break;
            case 9:
                playerStat.MaxHp += 80;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "플레이어의 최대 체력을 20 증가시킵니다";
                break;
            case 2:
                explanations[1] = "최대 체력 추가 20 증가";
                goto case 1;
            case 3:
                explanations[2] = "최대 체력 추가 20 증가";
                goto case 2;
            case 4:
                explanations[3] = "최대 체력 추가 20 증가";
                goto case 3;
            case 5:
                explanations[4] = "최대 체력 추가 20 증가";
                goto case 4;
            case 6:
                explanations[5] = "최대 체력 추가 40 증가";
                goto case 5;
            case 7:
                explanations[6] = "최대 체력 추가 40 증가";
                goto case 6;
            case 8:
                explanations[7] = "최대 체력 추가 40 증가";
                goto case 7;
            case 9:
                explanations[8] = "최대 체력 추가 80 증가";
                goto case 8;
        }
    }
}
