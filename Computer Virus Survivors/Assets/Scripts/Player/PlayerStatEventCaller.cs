using System;
using UnityEngine;


// Player의 스탯이 변경되면 이벤트를 호출하는 ScriptableObject
[CreateAssetMenu(fileName = "PlayerStatEventCaller", menuName = "ScriptableObj/PlayerStatEventCaller", order = 0)]
public class PlayerStatEventCaller : ScriptableObject
{
    public event EventHandler<StatChangedEventArgs> StatChangedHandler;

    public void Invoke(string statName, object newValue)
    {
        StatChangedHandler?.Invoke(this, new StatChangedEventArgs(statName, newValue));
    }


    /// <summary>
    /// 씬이 전환되어도 스크립터블 오브젝트의 데이터는 유지되므로, 씬이 전환되어도 이벤트 핸들러가 계속 쌓이는 문제가 발생할 수 있음
    /// 이를 해결하기 위해 이벤트 핸들러를 초기화하는 함수
    /// 플레이어가 죽으면 호출됨
    /// </summary>
    public void ClearSubscribers()
    {
        StatChangedHandler = null;
    }
}


// Player의 스탯이 변경됨을 감지하는 Observer interface
// GUI, 무기들, 적들 등이 Player의 스탯을 감지하여 반응할 수 있도록 함
public interface IPlayerStatObserver
{
    void OnStatChanged(object sender, StatChangedEventArgs args);
}


// Player의 스탯 변경 이벤트를 전달하는 EventArgs
public class StatChangedEventArgs : EventArgs
{
    public string StatName { get; }
    public object NewValue { get; }

    public StatChangedEventArgs(string statName, object newValue)
    {
        StatName = statName;
        NewValue = newValue;
        Debug.Log("StatChangedEventArgs : " + statName + " " + newValue);
    }
}
