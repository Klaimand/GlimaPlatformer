using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_TurretBullet : MonoBehaviour
{
    [SerializeField]
    float speed;
    
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisiongo;
        collisiongo = collision.gameObject;
        if (collisiongo.CompareTag("Player"))
        {
            KLD_DamageTaker damagetaker = collisiongo.GetComponent<KLD_DamageTaker>();
            if (!damagetaker.isInvulnerable)
            {
                damagetaker.doDamageTaking(DamageType.Explosion, transform, 0f);
            }
        }
        Destroy(gameObject);
    }
}
