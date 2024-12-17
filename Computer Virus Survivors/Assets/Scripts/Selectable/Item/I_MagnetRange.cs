public class I_MagnetRange : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.ExpGainRange += 0.707f * 0.5f;
                break;
            case 2:
                playerStat.ExpGainRange += 0.707f * 0.5f;
                break;
            case 3:
                playerStat.ExpGainRange += 0.707f * 0.5f;
                break;
            case 4:
                playerStat.ExpGainRange += 0.707f * 0.5f;
                break;
            case 5:
                playerStat.ExpGainRange += 0.707f * 0.5f;
                break;
            case 6:
                playerStat.ExpGainRange += 0.707f * 0.5f;
                break;
            case 7:
                playerStat.ExpGainRange += 0.707f * 0.5f;
                break;
            case 8:
                playerStat.ExpGainRange += 0.707f * 0.5f;
                break;
            case 9:
                playerStat.ExpGainRange += 0.707f;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "백신이 더 넓은 범위의 정보를 수집할 수 있습니다\n아이템 획득 범위 <color=#FF00C7>50%</color> 증가";
                break;
            case 2:
                explanations[1] = "아이템 획득 범위 <color=#FF00C7>50%</color> 증가";
                goto case 1;
            case 3:
                explanations[2] = "아이템 획득 범위 <color=#FF00C7>50%</color> 증가";
                goto case 2;
            case 4:
                explanations[3] = "아이템 획득 범위 <color=#FF00C7>50%</color> 증가";
                goto case 3;
            case 5:
                explanations[4] = "아이템 획득 범위 <color=#FF00C7>50%</color> 증가";
                goto case 4;
            case 6:
                explanations[5] = "아이템 획득 범위 <color=#FF00C7>50%</color> 증가";
                goto case 5;
            case 7:
                explanations[6] = "아이템 획득 범위 <color=#FF00C7>50%</color> 증가";
                goto case 6;
            case 8:
                explanations[7] = "아이템 획득 범위 <color=#FF00C7>50%</color> 증가";
                goto case 7;
            case 9:
                explanations[8] = "아이템 획득 범위 <color=#FF00C7>100%</color> 증가";
                goto case 8;
        }
    }
}
