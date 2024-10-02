using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip damaged;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Damaged(int damage)
    {
        currentHealth -= damage;
        DamageVisuals(damage);
        AudioSource.PlayClipAtPoint(damaged, transform.position, GameManager.Instance.GetSoundVolume());
        Debug.Log("Dummy took " + damage + " damage!");
        if (currentHealth <= 0)
            StartCoroutine(Died());
    }

    private IEnumerator Died()
    {
        gameObject.layer = 0;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
