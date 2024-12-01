using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class TurnManager : MonoBehaviour
{
    public enum TurnState { PhaseOne, PhaseTwo, Result, Reset }
    public TurnState currentTurnState;

    public UIManager uiManager;
    public static TurnManager Instance;
    public bool passed = false;

    public GameObject ballPrefab; // Префаб для создания нового объекта
    public Transform spawnPoint; // Точка появления нового объекта
    public GameObject playerCamera; // Ссылка на объект PlayerCamera

    private CinemachineCamera virtualCamera; // Кэшируем компонент CinemachineVirtualCamera

    public Transform kegels;
    public GameObject smallHouse;
    public GameObject mediumHouse;
    public GameObject highHouse;
    public GameObject tallHouse;
    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>();
    

    private int currentTurn = 0;
    private int MaxTurns = 6;
    private List<int> results = new List<int>();

    private void Awake()
    {
        // Реализация Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем объект между сценами
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликаты
        }

        if (playerCamera != null)
        {
            virtualCamera = playerCamera.GetComponent<CinemachineCamera>();
        }
        //kegels = GameObject.Find("Kegels").transform;
        //playerCamera = GameObject.Find("PlayerCamera");
        //spawnPoint = GameObject.Find("SpawnPoint").transform;
    }

    private void Start()
    {
        currentTurnState = TurnState.PhaseOne;

        // Сохраняем стартовые позиции всех дочерних объектов
        foreach (Transform child in kegels)
        {
            initialPositions[child] = child.position;
        }
    }

    private void Update()
    {
        switch (currentTurnState)
        {
            case TurnState.PhaseOne:
                // Обработка действий игрока
                HandlePhaseOne();
                break;
            case TurnState.PhaseTwo:
                // Возврат объектов
                HandlePhaseTwo();
                break;
            case TurnState.Result:
                // Отображение результата
                HandleResult();
                break;
            case TurnState.Reset:
                // Перезапуск сцены для нового хода
                ResetTurn();
                break;
        }
    }

    private void HandlePhaseOne()
    {
        // Логика действий в первом этапе
        if (passed)
        {
            currentTurnState = TurnState.Result;
            passed = false;
            ReplaceBall();
            ReplaceObjects();
        }
    }

    private void HandlePhaseTwo()
    {
        // Логика действий в первом этапе
        if (passed)
        {
            currentTurnState = TurnState.Result;
            passed = false;
        }
    }

    private void HandleResult()
    {
        int result = CalculateResult();
        results.Add(result);
        uiManager.ShowResult(result, currentTurn); // Показ результата

        // Переход к следующему ходу
        ResetTurn();
    }

    private void ResetTurn()
    {
        currentTurn++;
        if (currentTurn >= MaxTurns)
        {
            ShowFinalResults();
        }
        else
        {
            switch (currentTurn)
            {
                case 2:
                case 4:
                    ReloadScene();
                    break;
                case 1:
                case 3:
                case 5:
                    currentTurnState = TurnState.PhaseTwo;
                    break;
            }
        }
    }

    private int CalculateResult()
    {
        int count = 0;

        foreach (Transform kegel in kegels) 
        {
            foreach (Transform elementTransform in kegel)
            {
                GameObject element = elementTransform.gameObject;
                if (!element.activeSelf) 
                {
                    count++;
                    Destroy(kegel.gameObject);
                    break;
                }
            }
        }

        Debug.Log($"Количество объектов с хотя бы одним невидимым элементом: {count}");
        return count;
    }

    public void ReplaceBall()
    {
        // Удаляем текущий объект
        GameObject currentBall = GameObject.FindWithTag("Ball");
        if (currentBall != null)
        {
            Destroy(currentBall); // Удаление объекта
            Debug.Log("Старый объект удалён.");
        }

        // Создаём новый объект
        GameObject newBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Создан новый объект.");

        if (virtualCamera != null)
        {
            virtualCamera.Follow = newBall.transform;
            virtualCamera.LookAt = newBall.transform;
            Debug.Log("Камера теперь отслеживает новый объект.");
        }
        else
        {
            Debug.LogWarning("VirtualCamera не найдена на объекте PlayerCamera.");
        }
    }

    public void ReplaceObjects()
    {
        List<Transform> objectsToReplace = new List<Transform>();

        foreach (Transform kegel in kegels)
        {
            bool hasInvisibleElement = false;

            // Проверяем каждый элемент на видимость
            foreach (Transform elementTransform in kegel)
            {
                GameObject element = elementTransform.gameObject;
                if (!element.activeSelf)
                {
                    hasInvisibleElement = true;
                    break;
                }
            }

            if (!hasInvisibleElement)
            {
                objectsToReplace.Add(kegel); // Помечаем объект для замены
            }
        }

        // Удаляем старые и создаём новые объекты
        foreach (Transform oldObject in objectsToReplace)
        {
            if (initialPositions.TryGetValue(oldObject, out Vector3 initialPosition))
            {
                GameObject newObject;
                switch (oldObject.name)
                {
                    case "1":
                        newObject = Instantiate(smallHouse, initialPosition, smallHouse.transform.rotation, kegels);
                        break;
                    case "2":
                    case "3":
                        newObject = Instantiate(mediumHouse, initialPosition, mediumHouse.transform.rotation, kegels);
                        break;
                    case "4":
                    case "5":
                    case "6":
                        newObject = Instantiate(highHouse, initialPosition, highHouse.transform.rotation, kegels);
                        break;
                    case "7":
                    case "8":
                    case "9":
                    case "10":
                        newObject = Instantiate(tallHouse, initialPosition, tallHouse.transform.rotation, kegels);
                        break;
                }
                Debug.Log($"Создан новый объект на месте {oldObject.name}");

                // Удаляем старый объект
                Destroy(oldObject.gameObject);
                Debug.Log($"{oldObject.name} удалён.");
            }
        }
    }

    private void ReloadScene()
    {
        // Перезагрузка текущей сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        currentTurnState = TurnState.PhaseOne;
    }

    private void ShowFinalResults()
    {
        // Логика финальной таблицы результатов
        uiManager.ShowFinalResults(results);
    }
}
