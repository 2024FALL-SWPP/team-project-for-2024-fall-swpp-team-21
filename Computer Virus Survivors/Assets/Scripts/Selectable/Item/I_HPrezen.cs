public class I_HPrezen : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.HealthRezenPer10 += 2; // 2
                break;
            case 2:
                playerStat.HealthRezenPer10 += 2; // 4
                break;
            case 3:
                playerStat.HealthRezenPer10 += 2; // 6
                break;
            case 4:
                playerStat.HealthRezenPer10 += 2; // 8
                break;
            case 5:
                playerStat.HealthRezenPer10 += 2; // 10
                break;
            case 6:
                playerStat.HealthRezenPer10 += 2; // 12
                break;
            case 7:
                playerStat.HealthRezenPer10 += 2; // 14
                break;
            case 8:
                playerStat.HealthRezenPer10 += 2; // 16
                break;
            case 9:
                playerStat.HealthRezenPer10 += 4; // 20
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "데이터 자가 복구 기술을 도입해 백신이 스스로 오염을 복구합니다\n\n체력 <color=#FF00C7>10초당 2</color> 회복";
                break;
            case 2:
                explanations[1] = "체력 <color=#FF00C7>10초당 4</color> 회복";
                goto case 1;
            case 3:
                explanations[2] = "체력 <color=#FF00C7>10초당 6</color> 회복";
                goto case 2;
            case 4:
                explanations[3] = "체력 <color=#FF00C7>10초당 8</color> 회복";
                goto case 3;
            case 5:
                explanations[4] = "체력 <color=#FF00C7>10초당 10</color> 회복";
                goto case 4;
            case 6:
                explanations[5] = "체력 <color=#FF00C7>10초당 12</color> 회복";
                goto case 5;
            case 7:
                explanations[6] = "체력 <color=#FF00C7>10초당 14</color> 회복";
                goto case 6;
            case 8:
                explanations[7] = "체력 <color=#FF00C7>10초당 16</color> 회복";
                goto case 7;
            case 9:
                explanations[8] = "체력 <color=#FF00C7>10초당 20</color> 회복";
                goto case 8;
        }
    }
}
