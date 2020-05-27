using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KLD_TriggerOnZone : MonoBehaviour
{

    [SerializeField]
    private bool selfDestroyAfterTriggering;

    public UnityEvent OnPlayerEnter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerEnter.Invoke();
            if (selfDestroyAfterTriggering)
            {
                Destroy(gameObject);
            }
        }
    }


}
