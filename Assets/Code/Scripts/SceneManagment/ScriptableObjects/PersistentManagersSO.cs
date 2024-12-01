using UnityEngine;

// Важно : Настройки Persistent Manager могут быть только одни! 
// Persistent Managers — это сцены/объекты,
// сохраняющиеся между загрузками других сцен, обеспечивающие глобальную целостность (настройки/данные)
[CreateAssetMenu(fileName = "PersistentManagers", menuName = "Scene Data/PersistentManagers")]
public class PersistentManagersSO : GameSceneSO { }