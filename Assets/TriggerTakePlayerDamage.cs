using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTakePlayerDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            collision.GetComponent<PlayerSC>().TakeDamage();
        }      
    }
  
}
