public class I_ItemBase : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                // playerStat.EvadeProbability += 5;
                break;
            case 2:
                // playerStat.EvadeProbability += 5;
                break;
            case 3:
                // playerStat.EvadeProbability += 5;
                break;
            case 4:
                // playerStat.EvadeProbability += 5;
                break;
            case 5:
                // playerStat.EvadeProbability += 5;
                break;
            case 6:
                // playerStat.EvadeProbability += 5;
                break;
            case 7:
                // playerStat.EvadeProbability += 5;
                break;
            case 8:
                // playerStat.EvadeProbability += 5;
                break;
            case 9:
                // playerStat.EvadeProbability += 10;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "<아이템 획득 설명>";
                break;
            case 2:
                explanations[1] = "레벨2 설명";
                goto case 1;
            case 3:
                explanations[2] = "레벨3 설명";
                goto case 2;
            case 4:
                explanations[3] = "레벨4 설명";
                goto case 3;
            case 5:
                explanations[4] = "레벨5 설명";
                goto case 4;
            case 6:
                explanations[5] = "레벨6 설명";
                goto case 5;
            case 7:
                explanations[6] = "레벨7 설명";
                goto case 6;
            case 8:
                explanations[7] = "레벨8 설명";
                goto case 7;
            case 9:
                explanations[8] = "레벨9 설명";
                goto case 8;
        }
    }
}
