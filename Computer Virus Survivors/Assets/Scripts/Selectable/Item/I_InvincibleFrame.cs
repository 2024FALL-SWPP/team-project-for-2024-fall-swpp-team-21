public class I_InvincibleFrame : ItemBehaviour
{

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                playerStat.InvincibleFrame += 4;
                break;
            case 2:
                playerStat.InvincibleFrame += 4;
                break;
            case 3:
                playerStat.InvincibleFrame += 4;
                break;
            case 4:
                playerStat.InvincibleFrame += 4;
                break;
            case 5:
                playerStat.InvincibleFrame += 4;
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "커널 레벨에서 백신을 보호해 바이러스가 백신을 발견하기 어려워집니다\n무적 시간 <color=#FF00C7>4</color>프레임 증가";
                break;
            case 2:
                explanations[1] = "무적 시간 <color=#FF00C7>4</color>프레임 증가";
                goto case 1;
            case 3:
                explanations[2] = "무적 시간 <color=#FF00C7>4</color>프레임 증가";
                goto case 2;
            case 4:
                explanations[3] = "무적 시간 <color=#FF00C7>4</color>프레임 증가";
                goto case 3;
            case 5:
                explanations[4] = "무적 시간 <color=#FF00C7>4</color>프레임 증가";
                goto case 4;
        }
    }
}
