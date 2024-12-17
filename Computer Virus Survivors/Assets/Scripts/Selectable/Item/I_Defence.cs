public class I_Defence : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.DefencePoint += 1;
                break;
            case 2:
                playerStat.DefencePoint += 1;
                break;
            case 3:
                playerStat.DefencePoint += 1;
                break;
            case 4:
                playerStat.DefencePoint += 1;
                break;
            case 5:
                playerStat.DefencePoint += 1;
                break;
            case 6:
                playerStat.DefencePoint += 1;
                break;
            case 7:
                playerStat.DefencePoint += 1;
                break;
            case 8:
                playerStat.DefencePoint += 1;
                break;
            case 9:
                playerStat.DefencePoint += 2;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "암호화/복호화 하드웨어 모듈을 설치하여 바이러스가 공격하기 어려워집니다\n\n방어력 <color=#FF00C7>1</color> 증가";
                break;
            case 2:
                explanations[1] = "방어력 <color=#FF00C7>1</color> 증가";
                goto case 1;
            case 3:
                explanations[2] = "방어력 <color=#FF00C7>1</color> 증가";
                goto case 2;
            case 4:
                explanations[3] = "방어력 <color=#FF00C7>1</color> 증가";
                goto case 3;
            case 5:
                explanations[4] = "방어력 <color=#FF00C7>1</color> 증가";
                goto case 4;
            case 6:
                explanations[5] = "방어력 <color=#FF00C7>1</color> 증가";
                goto case 5;
            case 7:
                explanations[6] = "방어력 <color=#FF00C7>1</color> 증가";
                goto case 6;
            case 8:
                explanations[7] = "방어력 <color=#FF00C7>1</color> 증가";
                goto case 7;
            case 9:
                explanations[8] = "방어력 <color=#FF00C7>2</color> 증가";
                goto case 8;
        }
    }
}
