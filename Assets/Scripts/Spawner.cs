using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> Characters;


    private float spawnTimer;
    private float spawnTimerMax = 6f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTimerMax)
        {
            Transform character = Instantiate(Characters[Random.Range(0, Characters.Count - 1)], transform).transform;
            character.GetComponent<Character>().InitializeCharacter(6);
            spawnTimer = 0;
        }
    }
}
