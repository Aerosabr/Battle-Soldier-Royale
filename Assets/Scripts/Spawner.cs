using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> Characters;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private int layerMask;

    private float spawnTimer;
    private float spawnTimerMax = 6f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTimerMax)
        {
            Transform character = Instantiate(Characters[Random.Range(0, Characters.Count)], transform).transform;
            float spawnPos = Random.Range(-0.5f, 0.5f);
            character.transform.position = new Vector3(transform.position.x, spawnPos * 0.2f, spawnPos);
            character.GetComponent<Character>().InitializeCharacter(layerMask, rotation);
            spawnTimer = 0;
        }
    }
}
