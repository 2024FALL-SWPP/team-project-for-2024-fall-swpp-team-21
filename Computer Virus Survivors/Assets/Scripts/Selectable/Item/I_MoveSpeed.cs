public class I_MoveSpeed : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.MoveSpeed += 0.5f;
                break;
            case 2:
                playerStat.MoveSpeed += 0.5f;
                break;
            case 3:
                playerStat.MoveSpeed += 0.5f;
                break;
            case 4:
                playerStat.MoveSpeed += 0.5f;
                break;
            case 5:
                playerStat.MoveSpeed += 0.5f;
                break;
            case 6:
                playerStat.MoveSpeed += 0.5f;
                break;
            case 7:
                playerStat.MoveSpeed += 0.5f;
                break;
            case 8:
                playerStat.MoveSpeed += 0.5f;
                break;
            case 9:
                playerStat.MoveSpeed += 1f;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "백신이 메모리 공간을 돌아다니기 쉬워집니다\n\n이동속도 <color=#FF00C7>10%</color> 증가";
                break;
            case 2:
                explanations[1] = "이동속도 <color=#FF00C7>10%</color> 증가";
                goto case 1;
            case 3:
                explanations[2] = "이동속도 <color=#FF00C7>10%</color> 증가";
                goto case 2;
            case 4:
                explanations[3] = "이동속도 <color=#FF00C7>10%</color> 증가";
                goto case 3;
            case 5:
                explanations[4] = "이동속도 <color=#FF00C7>10%</color> 증가";
                goto case 4;
            case 6:
                explanations[5] = "이동속도 <color=#FF00C7>10%</color> 증가";
                goto case 5;
            case 7:
                explanations[6] = "이동속도 <color=#FF00C7>10%</color> 증가";
                goto case 6;
            case 8:
                explanations[7] = "이동속도 <color=#FF00C7>10%</color> 증가";
                goto case 7;
            case 9:
                explanations[8] = "이동속도 <color=#FF00C7>20%</color> 증가";
                goto case 8;
        }
    }
}
