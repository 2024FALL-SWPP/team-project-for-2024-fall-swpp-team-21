public class I_OverClock : ItemBehaviour
{
    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.AttackSpeed += 10; // 공격 속도 10% 증가
                break;
            case 2:
                playerStat.AttackSpeed += 10; // 공격 속도 10% 증가
                break;
            case 3:
                playerStat.AttackSpeed += 10; // 공격 속도 10% 증가
                break;
            case 4:
                playerStat.AttackSpeed += 10; // 공격 속도 10% 증가
                break;
            case 5:
                playerStat.AttackSpeed += 10; // 공격 속도 10% 증가
                break;
            case 6:
                playerStat.AttackSpeed += 10; // 공격 속도 10% 증가
                break;
            case 7:
                playerStat.AttackSpeed += 10; // 공격 속도 10% 증가
                break;
            case 8:
                playerStat.AttackSpeed += 10; // 공격 속도 10% 증가
                break;
            case 9:
                playerStat.AttackSpeed += 10; // 공격 속도 10% 증가
                break;
        }
    }
}
