using System;

public class TitleCanvasManager : CanvasBase
{

    public event Action<int> BtnCliked;

    public override void Initialize()
    {
        // TODO : 초기화
        // 배경음악 재생
        gameObject.SetActive(true);
    }

    public void OnGameStartClicked()
    {
        BtnCliked?.Invoke(0);
    }

    public void OnExitClicked()
    {
        BtnCliked?.Invoke(1);
    }

    public override void OnEnter()
    {
        // TODO : 메인화면 진입하면 하는것? 애니메이션 정도?
        ;
    }

    public override void OnExit()
    {
    }
}
