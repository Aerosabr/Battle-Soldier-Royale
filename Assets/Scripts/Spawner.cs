using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> Characters;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private int layerMask;

    private float spawnTimer;
    private float spawnTimerMax = 6f;

    private void Update()
    {

    }

    public void SpawnCharacter(GameObject characterToSpawn)
    {
        PlayerManager.Instance.SubtractGold(characterToSpawn.GetComponent<Character>().GetCost());
        Transform character = Instantiate(characterToSpawn, transform).transform;
        float spawnPos = Random.Range(-0.5f, 0.5f);
        character.transform.position = new Vector3(transform.position.x, spawnPos * 0.2f, spawnPos);
        character.GetComponent<Character>().InitializeCharacter(layerMask, rotation);
        spawnTimer = 0;
    }
}
