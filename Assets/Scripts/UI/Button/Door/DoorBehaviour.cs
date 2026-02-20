using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DoorBehaviour : ButtonBehaviour
{
    // Событие, которое вызывается после спавна
    public UnityEvent<List<SpellButtonFacade>> OnSpellButtonsSpawned;
    public ISpellButtonPrefabBundler spellBundler = new DefaultRarenessSpellBundler();

    private SpellButtonSpawner spellButtonSpawner;
    private DoorAnimator animator;
    private List<SpellButtonFacade> prefabs = new();

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<DoorAnimator>();
        spellButtonSpawner = FindAnyObjectByType<SpellButtonSpawner>();
        prefabs = spellBundler.GetAmountOfPrefabs(2);
    }

    public override void OnClickListener()
    {
        base.OnClickListener();
        animator.StartGlowToggleAnimation(
            animator.maxGlow,
            animator.duration
        );

        animator.StartDoorDisappearAnimation(
            animator.doorDisappearDuration
        );

        float delay = animator.doorDisappearDuration;
        StartCoroutine(SpawnCoroutine(delay));
    }

    private SpellButtonFacade GetRandomPrefab()
    {
        return SpellButtonPrefabSelector.Instance.GetRandomPrefab();
    }

    private IEnumerator SpawnCoroutine(float delay = 1f)
    {

        int buttonCount = prefabs.Count;
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;

        // Рассчитываем доступную ширину для кнопок (с отступами по краям)
        const float margin = 0.1f; // отступ в мировых
        float availableWidth = screenWidth - 2 * margin;

        float buttonWidth = availableWidth / buttonCount;
        float buttonWidthPixel = CameraManager.WorldXToPixels(buttonWidth);
        float duration = 1f;
        List<SpellButtonFacade> instances = new(); 

        yield return new WaitForSeconds(delay);
        for (int i = 0; i < buttonCount; i++)
        {
            float xPos = -availableWidth / 2f + buttonWidth * (i + 0.5f);

            Vector3 position = new Vector3(xPos, 0f, 0f); // y = 0, как требуется

            // Создаём кнопку
            spellButtonSpawner.SetPrefab(prefabs[i]);
            SpellButtonFacade facade = spellButtonSpawner.Spawn(Vector3.zero, Quaternion.identity);
            RectTransform rect = facade.GetComponent<RectTransform>();

            rect.sizeDelta = new Vector2(1, 1); // высоту можно зафиксировать

            Vector2 targetSize = new Vector2(buttonWidthPixel, 500f); // пример: высота 500
            facade.animator.StartChangeShapeAnimation(targetSize, duration);

            Transform objectTransform = facade.behaviour.spellObject.transform;
            Vector3 targetScale = objectTransform.localScale; // сохраняем начальный масштаб
            objectTransform.localScale = Vector3.zero; // начинаем с нулевого масштаба

            facade.animator.StartScaleTo(objectTransform, targetScale, duration);
            facade.mover.MoveEaseOut(position, duration);
            instances.Add(facade);
        }

        OnSpellButtonsSpawned.Invoke(instances);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    protected override void PlaySpecificAnimation()
    {
    }

    protected override void PlayDefaultAnimation()
    {
        base.PlayDefaultAnimation();
        animator.StartOnClickAnimation();
    }
}
