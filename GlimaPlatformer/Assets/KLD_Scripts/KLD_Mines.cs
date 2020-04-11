using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Mines : MonoBehaviour
{
    private Color buttonOff = Color.black;
    private Color buttonOn = Color.red;

    private bool isTriggered, exploded = false;

    private SpriteRenderer thisSr;
    //private SpriteRenderer buttonSr;

    private KLD_DamageTaker damageTaker;

    [SerializeField]
    private float timeBeforeExplosion = 0f, explosionRadius = 0f, explosionForce = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        damageTaker = GameObject.Find("Player").GetComponent<KLD_DamageTaker>();
        thisSr = GetComponent<SpriteRenderer>();
        //buttonSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //buttonSr.color = buttonOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered && !exploded && !damageTaker.isInvulnerable && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(triggerMine());
        }
    }

    private IEnumerator triggerMine ()
    {
        isTriggered = true;
        //buttonSr.color = buttonOn;
        yield return new WaitForSeconds(timeBeforeExplosion);
        explode();
    }

    private void explode ()
    {
        print("exploded");
        thisSr.enabled = false;
        //buttonSr.enabled = false;
        Collider2D[] collidersInExplosion = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D coll in collidersInExplosion)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                print("player is in explosion radius");
                damageTaker.doDamageTaking(DamageType.Explosion, transform, explosionForce);
            }
        }
    }
}
