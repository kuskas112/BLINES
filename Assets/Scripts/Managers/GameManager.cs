using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpellButtonPlacementManager playerButtonsPlacer;
    public SpellButtonPlacementManager enemyButtonsPlacer;
    public DoorSpawner doorSpawner;

    private BattleManager battleManager;

    void Start()
    {
        battleManager = BattleManager.Instance;

        // Убрать соперника за карту
        battleManager.Enemy.PolygonObject.transform.position = new(0, 10, 0);
        battleManager.Player.PolygonObject.transform.position = new(0, -10, 0);
        StartCoroutine(StartDoorCoroutine());
    }

    private IEnumerator StartDoorCoroutine()
    {
        float duration = 2f;
        MovePlayerToWaitPos(duration);
        yield return new WaitForSeconds(duration);

        var doorFacade = doorSpawner.Spawn(Vector3.zero, Quaternion.identity);

        doorFacade.doorBehaviour.OnSpellButtonsSpawned.AddListener(SlideButtons);

        Color color = doorFacade.materialSetter.EdgeNeonColor;
        doorFacade.materialSetter.EdgeNeonColor *= 0;

        doorFacade.animator.StartChangeColorAnimation(color, 2f);
    }

    void SlideButtons(List<SpellButtonFacade> facades)
    {
        foreach (var facade in facades)
        {
            facade.behaviour.button.onClick.AddListener(() =>
            {
                MovePlayerToFightPos();
                MoveEnemyToFightPos();
                battleManager.Player.spells.Add(
                    facade.behaviour.GetSpell()
                    );

                playerButtonsPlacer.SlideButtonsOnPositions();
                enemyButtonsPlacer.SlideButtonsOnPositions();

                foreach (var f in facades)
                {
                    Destroy(f.gameObject);
                }
            });
        }
    }

    void MovePlayerToWaitPos(float duration = 3f)
    {
        float rotation = battleManager.Player.polygonFacade.transform.rotation.eulerAngles.z;
        battleManager.Player.polygonFacade.animator.StartRotationChangeAnimation(
            battleManager.Player.polygonFacade.transform, duration, rotation + 360f);

        battleManager.Player.polygonFacade.mover.MoveEaseOut(
            new(0, -3.5f), duration);
    }

    void MovePlayerToFightPos(float duration = 3f)
    {
        float rotation = battleManager.Player.polygonFacade.transform.rotation.eulerAngles.z;
        battleManager.Player.polygonFacade.animator.StartRotationChangeAnimation(
            battleManager.Player.polygonFacade.transform, duration, rotation - 360f);

        battleManager.Player.polygonFacade.mover.MoveEaseOut(
            new(0, -1f), duration);
    }

    void MoveEnemyToFightPos(float duration = 3f)
    {
        float rotation = battleManager.Enemy.polygonFacade.transform.rotation.eulerAngles.z;
        battleManager.Enemy.polygonFacade.animator.StartRotationChangeAnimation(
            battleManager.Enemy.polygonFacade.transform, duration, rotation - 360f);

        battleManager.Enemy.polygonFacade.mover.MoveEaseOut(
            new(0, 1f), duration);
    }
}
