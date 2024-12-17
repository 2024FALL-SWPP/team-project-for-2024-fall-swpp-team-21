public class I_ExpGain : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.ExpGainRatio += 7;
                break;
            case 2:
                playerStat.ExpGainRatio += 7;
                break;
            case 3:
                playerStat.ExpGainRatio += 7;
                break;
            case 4:
                playerStat.ExpGainRatio += 7;
                break;
            case 5:
                playerStat.ExpGainRatio += 7;
                break;
            case 6:
                playerStat.ExpGainRatio += 7;
                break;
            case 7:
                playerStat.ExpGainRatio += 7;
                break;
            case 8:
                playerStat.ExpGainRatio += 7;
                break;
            case 9:
                playerStat.ExpGainRatio += 14;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "하드웨어 가속의 도움을 받아 바이러스를 학습하기 쉬워집니다\n\n경험치 획득량을 <color=#FF00C7>7%</color> 증가";
                break;
            case 2:
                explanations[1] = "경험치 획득량을 <color=#FF00C7>7%</color> 증가";
                goto case 1;
            case 3:
                explanations[2] = "경험치 획득량을 <color=#FF00C7>7%</color> 증가";
                goto case 2;
            case 4:
                explanations[3] = "경험치 획득량을 <color=#FF00C7>7%</color> 증가";
                goto case 3;
            case 5:
                explanations[4] = "경험치 획득량을 <color=#FF00C7>7%</color> 증가";
                goto case 4;
            case 6:
                explanations[5] = "경험치 획득량을 <color=#FF00C7>7%</color> 증가";
                goto case 5;
            case 7:
                explanations[6] = "경험치 획득량을 <color=#FF00C7>7%</color> 증가";
                goto case 6;
            case 8:
                explanations[7] = "경험치 획득량을 <color=#FF00C7>7%</color> 증가";
                goto case 7;
            case 9:
                explanations[8] = "경험치 획득량을 <color=#FF00C7>14%</color> 증가";
                goto case 8;
        }
    }
}
