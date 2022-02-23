using UnityEngine;
using UnityEngine.Events;
public static class EventManager 
{

    #region Variables
    public static event UnityAction UpdateCoinUI;
    public static void CoinCollected() => UpdateCoinUI?.Invoke();

    public static event UnityAction<int> UpdateHealthUI;
    public static void HealthChange(int value) => UpdateHealthUI?.Invoke(value);

    public static event UnityAction<int> UpdateEnergyUI;
    public static void EnergyChange(int value) => UpdateEnergyUI?.Invoke(value);

    public static event UnityAction<string> ToggleHint;
    public static void Hint(string text) => ToggleHint?.Invoke(text);

    public static event UnityAction UpdateComboUI;
    public static void EnemyKilled() => UpdateComboUI?.Invoke();

    #endregion

}
