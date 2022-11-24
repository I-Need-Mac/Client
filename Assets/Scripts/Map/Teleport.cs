using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject telleportTo;

    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(TeleportRoutine());
        }
    }
    IEnumerator TeleportRoutine()
    {
        yield return null;
        player.transform.position = telleportTo.transform.position;
    }
}
