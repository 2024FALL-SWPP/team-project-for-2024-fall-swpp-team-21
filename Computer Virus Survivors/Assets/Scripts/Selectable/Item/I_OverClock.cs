public class I_OverClock : ItemBehaviour
{
    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.AttackSpeed += 5; // 5
                break;
            case 2:
                playerStat.AttackSpeed += 5; // 10
                break;
            case 3:
                playerStat.AttackSpeed += 5; // 15
                break;
            case 4:
                playerStat.AttackSpeed += 5; // 20
                break;
            case 5:
                playerStat.AttackSpeed += 5; // 25
                break;
            case 6:
                playerStat.AttackSpeed += 5; // 30
                break;
            case 7:
                playerStat.AttackSpeed += 5; // 35
                break;
            case 8:
                playerStat.AttackSpeed += 5; // 40
                break;
            case 9:
                playerStat.AttackSpeed += 10; // 50
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "CPU클럭을 높여 백신의 속도가 빨라집니다\n공격 속도를 <color=#FF00C7>5%</color> 증가시킵니다";
                break;
            case 2:
                explanations[1] = "공격 속도 <color=#FF00C7>5%</color> 증가";
                goto case 1;
            case 3:
                explanations[2] = "공격 속도 <color=#FF00C7>5%</color> 증가";
                goto case 2;
            case 4:
                explanations[3] = "공격 속도 <color=#FF00C7>5%</color> 증가";
                goto case 3;
            case 5:
                explanations[4] = "공격 속도 <color=#FF00C7>5%</color> 증가";
                goto case 4;
            case 6:
                explanations[5] = "공격 속도 <color=#FF00C7>5%</color> 증가";
                goto case 5;
            case 7:
                explanations[6] = "공격 속도 <color=#FF00C7>5%</color> 증가";
                goto case 6;
            case 8:
                explanations[7] = "공격 속도 <color=#FF00C7>5%</color> 증가";
                goto case 7;
            case 9:
                explanations[8] = "공격 속도 <color=#FF00C7>10%</color> 증가";
                goto case 8;
        }
    }
}
