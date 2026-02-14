using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DoorBehaviour : MonoBehaviour
{
    private Button button;
    private SpellButtonSpawner spellButtonSpawner;
    private DoorAnimator animator;
    private bool isCliked = false;
    private List<SpellButtonFacade> prefabs = new();

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
        animator = GetComponent<DoorAnimator>();
        spellButtonSpawner = FindAnyObjectByType<SpellButtonSpawner>();
        int prefabsCount = 3;
        for (int i = 0; i < prefabsCount; i++)
        {
            prefabs.Add(GetRandomPrefab());
        }
    }

    private void OnButtonClicked()
    {
        // Отрабатывает 1 раз
        if (isCliked) return;
        isCliked = true;
        float delay = animator.doorDisappearDuration;

        StartCoroutine(SpawnCoroutine(delay));
    }

    private SpellButtonFacade GetRandomPrefab()
    {
        return PrefabSelector<SpellButtonFacade>.Instance.GetRandomPrefab();
    }




    private IEnumerator SpawnCoroutine(float delay = 1f)
    {
        int buttonCount = prefabs.Count;
        yield return new WaitForSeconds(delay);
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;

        // Рассчитываем доступную ширину для кнопок (с отступами по краям)
        const float margin = 0.1f; // отступ в мировых
        float availableWidth = screenWidth - 2 * margin;
        
        float ratio = Screen.width / screenWidth;

        float buttonWidth = availableWidth / buttonCount;
        float buttonWidthPixel = buttonWidth * ratio;
        float duration = 1f;


        for (int i = 0; i < buttonCount; i++)
        {
            // Рассчитываем позицию X для текущей кнопки
            // Центрируем кнопки относительно экрана
            float xPos = -availableWidth / 2f + buttonWidth * (i + 0.5f);

            Vector3 position = new Vector3(xPos, 0f, 0f); // y = 0, как требуется

            // Создаём кнопку
            spellButtonSpawner.SetPrefab(prefabs[i]);
            SpellButtonFacade facade = spellButtonSpawner.Spawn(Vector3.zero, Quaternion.identity);
            RectTransform rect = facade.GetComponent<RectTransform>();

            // Устанавливаем размер (ширина — рассчитанная, высоту оставляем как есть или задаём явно)
            rect.sizeDelta = new Vector2(1, 1); // высоту можно зафиксировать

            // Анимации (как в вашем коде)
            Vector2 targetSize = new Vector2(buttonWidthPixel, 500f); // пример: высота 500
            facade.animator.StartChangeShapeAnimation(targetSize, duration);
            facade.mover.MoveEaseOut(position, duration);
        }
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

}
