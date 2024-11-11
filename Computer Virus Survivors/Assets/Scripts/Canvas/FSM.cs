
public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}

public class FSM
{
    public IState currentState { get; private set; }

    public FSM(IState initialState)
    {
        currentState = initialState;
        currentState.OnEnter();
    }

    public void SetState(IState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}

public class CanvasFSM : FSM, IPlayerStatObserver
{
    public enum CanvasName
    {
        Canvas,
        Paused,
        ItemSelect,
        GameOver
    }

    public enum Signal
    {
        LevelUp,
        GameOver,
        OnPauseClicked,
        OnResumeClicked,
        OnItemSelectDone
    }

    public CanvasFSM(PlayerStatEventCaller caller) : base(new EmptyState())
    {
        playerStatEventCaller = caller;
        playerStatEventCaller.StatChangedHandler += OnStatChanged;
    }

    private PlayerStatEventCaller playerStatEventCaller;
    private FSM canvasFSM;

    private IState playingState;
    private IState pausedState;
    private IState itemSelectState;
    private IState itemSelectPausedState;
    private IState gameOverState;

    public void Mapping(CanvasName canvasName, IState canvas)
    {
        if (canvasName == CanvasName.Canvas)
        {
            playingState = canvas;
        }
        else if (canvasName == CanvasName.Paused)
        {
            pausedState = canvas;
            itemSelectPausedState = canvas;
        }
        else if (canvasName == CanvasName.ItemSelect)
        {
            itemSelectState = canvas;
        }
        else if (canvasName == CanvasName.GameOver)
        {
            gameOverState = canvas;
        }
    }

    public void OnStatChanged(object sender, StatChangedEventArgs args)
    {
        if (args.StatName == nameof(PlayerStat.PlayerLevel))
        {
            canvasFSM.SetState(itemSelectState);
        }
    }

    private void StateMachine(Signal signal)
    {

        if (currentState == playingState)
        {
            switch (signal)
            {
                case Signal.LevelUp:
                    canvasFSM.SetState(itemSelectState);
                    break;
                case Signal.GameOver:
                    canvasFSM.SetState(gameOverState);
                    break;
                case Signal.OnPauseClicked:
                    canvasFSM.SetState(pausedState);
                    break;
            }
        }
        else if (currentState == pausedState)
        {
            switch (signal)
            {
                case Signal.OnResumeClicked:
                    canvasFSM.SetState(playingState);
                    break;
            }
        }
        else if (currentState == itemSelectState)
        {
            switch (signal)
            {
                case Signal.OnItemSelectDone:
                    canvasFSM.SetState(playingState);
                    break;
                case Signal.OnPauseClicked:
                    canvasFSM.SetState(itemSelectPausedState);
                    break;
            }
        }
        else if (currentState == itemSelectPausedState)
        {
            switch (signal)
            {
                case Signal.OnResumeClicked:
                    canvasFSM.SetState(itemSelectState);
                    break;
            }
        }
        else if (currentState == gameOverState)
        {
            // TODO : GameOverState에서의 Signal 처리
        }
    }

}


public class EmptyState : IState
{
    public void OnEnter()
    {
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }
}
