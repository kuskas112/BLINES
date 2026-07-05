using UnityEngine;

// Интерфейс для присваивания действиям (спеллам) количественной
// оценки полезности, по которой ИИ врага будет определять наилучшее действие
public interface IActionScorer
{
    public float EvaluateAction(BattleContext context);
}
