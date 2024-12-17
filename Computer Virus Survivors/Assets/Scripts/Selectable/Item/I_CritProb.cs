public class I_CritProb : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.CritProbability += 3;
                break;
            case 2:
                playerStat.CritProbability += 3;
                break;
            case 3:
                playerStat.CritProbability += 3;
                break;
            case 4:
                playerStat.CritProbability += 3;
                break;
            case 5:
                playerStat.CritProbability += 3;
                break;
            case 6:
                playerStat.CritProbability += 3;
                break;
            case 7:
                playerStat.CritProbability += 3;
                break;
            case 8:
                playerStat.CritProbability += 3;
                break;
            case 9:
                playerStat.CritProbability += 6;
                break;
        }
    }

    protected override void InitExplanation()
    {

        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "바이러스의 행동 패턴을 분석하여 확실히 치료할 확률이 높아집니다\n\n치명타 확률 <color=#FF00C7>3%</color> 증가";
                break;
            case 2:
                explanations[1] = "치명타 확률 <color=#FF00C7>3%</color> 증가";
                goto case 1;
            case 3:
                explanations[2] = "치명타 확률 <color=#FF00C7>3%</color> 증가";
                goto case 2;
            case 4:
                explanations[3] = "치명타 확률 <color=#FF00C7>3%</color> 증가";
                goto case 3;
            case 5:
                explanations[4] = "치명타 확률 <color=#FF00C7>3%</color> 증가";
                goto case 4;
            case 6:
                explanations[5] = "치명타 확률 <color=#FF00C7>3%</color> 증가";
                goto case 5;
            case 7:
                explanations[6] = "치명타 확률 <color=#FF00C7>3%</color> 증가";
                goto case 6;
            case 8:
                explanations[7] = "치명타 확률 <color=#FF00C7>3%</color> 증가";
                goto case 7;
            case 9:
                explanations[8] = "치명타 확률 <color=#FF00C7>6%</color> 증가";
                goto case 8;
        }
    }
}
