public class I_PTE : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.EvadeProbability += 5;
                break;
            case 2:
                playerStat.EvadeProbability += 5;
                break;
            case 3:
                playerStat.EvadeProbability += 5;
                break;
            case 4:
                playerStat.EvadeProbability += 5;
                break;
            case 5:
                playerStat.EvadeProbability += 5;
                break;
            case 6:
                playerStat.EvadeProbability += 5;
                break;
            case 7:
                playerStat.EvadeProbability += 5;
                break;
            case 8:
                playerStat.EvadeProbability += 5;
                break;
            case 9:
                playerStat.EvadeProbability += 10;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "메모리의 보안 레벨을 증가시켜 바이러스의 공격을 회피합니다\n\n회피율을 <color=#FF00C7>5%</color> 증가시킵니다";
                break;
            case 2:
                explanations[1] = "회피율 <color=#FF00C7>5%</color> 증가";
                goto case 1;
            case 3:
                explanations[2] = "회피율 <color=#FF00C7>5%</color> 증가";
                goto case 2;
            case 4:
                explanations[3] = "회피율 <color=#FF00C7>5%</color> 증가";
                goto case 3;
            case 5:
                explanations[4] = "회피율 <color=#FF00C7>5%</color> 증가";
                goto case 4;
            case 6:
                explanations[5] = "회피율 <color=#FF00C7>5%</color> 증가";
                goto case 5;
            case 7:
                explanations[6] = "회피율 <color=#FF00C7>5%</color> 증가";
                goto case 6;
            case 8:
                explanations[7] = "회피율 <color=#FF00C7>5%</color> 증가";
                goto case 7;
            case 9:
                explanations[8] = "회피율 <color=#FF00C7>10%</color> 증가";
                goto case 8;
        }
    }
}
