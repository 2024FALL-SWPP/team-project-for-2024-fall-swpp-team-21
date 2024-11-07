using System;
using UnityEngine;


// Player의 스탯이 변경되면 이벤트를 호출하는 ScriptableObject
[CreateAssetMenu(fileName = "PlayerStatEventCaller", menuName = "ScriptableObj/PlayerStatEventCaller", order = 0)]
public class PlayerStatEventCaller : ScriptableObject
{
    public event EventHandler<StatChangedEventArgs> StatChanged;

    public void OnStatChanged(string statName, object newValue)
    {
        StatChanged?.Invoke(this, new StatChangedEventArgs(statName, newValue));
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
