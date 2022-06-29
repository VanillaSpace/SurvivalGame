using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    public int damage;
    public float damageRate;

    private List<IDamageable> thingsToDamage = new List<IDamageable>();

    void Start ()
    {
        StartCoroutine(DealDamageOverTime());
    }

    IEnumerator DealDamageOverTime()
    {
        while(true)
        {
            for (int i = 0; i < thingsToDamage.Count; i++)
            {
                thingsToDamage[i].TakePhysicalDmg(damage);
            }

            yield return new WaitForSeconds(damageRate);
        }
    }

    // whenever we touch the cactus we check if whatever we collide has the interface "IDamageable"
    private void OnCollisionEnter (Collision collision)
    {
        if(collision.gameObject.GetComponent<IDamageable>() != null)
        {
            thingsToDamage.Add(collision.gameObject.GetComponent<IDamageable>());
        }
    }

    private void OnCollisionExit (Collision collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            thingsToDamage.Remove(collision.gameObject.GetComponent<IDamageable>());
        }
    }
}
