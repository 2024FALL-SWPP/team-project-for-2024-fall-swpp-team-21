public class I_OverClock : ItemBehaviour
{
    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.AttackSpeed += 10;
                break;
            case 2:
                playerStat.AttackSpeed += 10;
                break;
            case 3:
                playerStat.AttackSpeed += 10;
                break;
            case 4:
                playerStat.AttackSpeed += 10;
                break;
            case 5:
                playerStat.AttackSpeed += 10;
                break;
            case 6:
                playerStat.AttackSpeed += 10;
                break;
            case 7:
                playerStat.AttackSpeed += 10;
                break;
            case 8:
                playerStat.AttackSpeed += 10;
                break;
            case 9:
                playerStat.AttackSpeed += 20;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "플레이어의 공격 속도를 10% 증가시킵니다";
                break;
            case 2:
                explanations[1] = "공격 속도 추가 10% 증가";
                goto case 1;
            case 3:
                explanations[2] = "공격 속도 추가 10% 증가";
                goto case 2;
            case 4:
                explanations[3] = "공격 속도 추가 10% 증가";
                goto case 3;
            case 5:
                explanations[4] = "공격 속도 추가 10% 증가";
                goto case 4;
            case 6:
                explanations[5] = "공격 속도 추가 10% 증가";
                goto case 5;
            case 7:
                explanations[6] = "공격 속도 추가 10% 증가";
                goto case 6;
            case 8:
                explanations[7] = "공격 속도 추가 10% 증가";
                goto case 7;
            case 9:
                explanations[8] = "공격 속도 추가 20% 증가";
                goto case 8;
        }
    }
}
