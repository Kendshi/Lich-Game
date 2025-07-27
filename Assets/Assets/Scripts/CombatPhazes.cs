using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombatPhazes : MonoBehaviour
{
    private CombatStateMachine _combatStateMachine;
    [SerializeField] private TMP_Text phazeText;
    [SerializeField] private Button nextPhazeButton;

    private void Awake()
    {
        if (Application.isPlaying)
            _combatStateMachine = new CombatStateMachine();

        phazeText.text = "Текущая фаза: Подготовка";
        nextPhazeButton.GetComponentInChildren<TMP_Text>().text = "Завершить подготовку";
        _combatStateMachine.EnterIn<PrepareState>();
    }
}
