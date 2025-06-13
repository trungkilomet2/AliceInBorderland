using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("Character Database")]
    public CharacterData[] allCharacters; 

    [Header("Spawn Settings")]
    public Transform spawnPoint; 

    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera; 

    [Header("Enemies Settings")]
    public EnemiesManager enemiesManager; 

    void Start()
    {
        SpawnSelectedCharacter();
    }

    void SpawnSelectedCharacter()
    {
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter", "");

        if (string.IsNullOrEmpty(selectedCharacterName))
        {
            Debug.LogWarning("Không có nhân vật nào được chọn!");
            return;
        }

        CharacterData selectedCharacter = FindCharacterByName(selectedCharacterName);

        if (selectedCharacter == null)
        {
            Debug.LogError($"Không tìm thấy nhân vật: {selectedCharacterName}");
            return;
        }

        if (selectedCharacter.characterPrefab == null)
        {
            Debug.LogError($"Nhân vật {selectedCharacter.characterName} không có prefab!");
            return;
        }

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion spawnRotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        GameObject spawnedPlayer = Instantiate(selectedCharacter.characterPrefab, spawnPosition, spawnRotation);

        SetCameraTarget(spawnedPlayer);

        SetPlayerForEnemies(spawnedPlayer);
    }

    CharacterData FindCharacterByName(string characterName)
    {
        foreach (CharacterData character in allCharacters)
        {
            if (character.name == characterName)
            {
                return character;
            }
        }
        return null;
    }

    void SetCameraTarget(GameObject target)
    {
        if (virtualCamera != null && target != null)
        {
            virtualCamera.Follow = target.transform;
            virtualCamera.LookAt = target.transform;
        }
        else
        {
            Debug.LogWarning("Virtual Camera hoặc Target không hợp lệ!");
        }
    }

    void SetPlayerForEnemies(GameObject player)
    {
        if (enemiesManager != null && player != null)
        {
            enemiesManager.SetPlayer(player);
        }
        else
        {
            EnemiesManager foundEnemiesManager = FindObjectOfType<EnemiesManager>();
            if (foundEnemiesManager != null && player != null)
            {
                foundEnemiesManager.SetPlayer(player);
                Debug.Log($"Tự động tìm và gán player cho EnemiesManager: {player.name}");
            }
            else
            {
                Debug.LogWarning("Không tìm thấy EnemiesManager hoặc Player không hợp lệ!");
            }
        }
    }
}