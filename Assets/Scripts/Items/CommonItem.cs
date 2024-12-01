using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class CommonItem : MonoBehaviour
{
    protected abstract void Use(GameObject Player);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Use(collision.gameObject);
        }
    }
}
