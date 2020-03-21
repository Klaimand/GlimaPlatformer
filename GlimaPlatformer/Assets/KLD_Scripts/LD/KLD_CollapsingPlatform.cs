using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_CollapsingPlatform : MonoBehaviour
{
    private BoxCollider2D[] colliders;
    private bool isCollapsing;

    [SerializeField]
    private float timeToCollapse;
    private SpriteRenderer sr;

    private void Awake()
    {
        colliders = GetComponents<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void collapseInst()
    {
        StartCoroutine(collapse());
    }

    private IEnumerator collapse ()
    {
        Color startColor = sr.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        float time = 0f;
        while (time < timeToCollapse)
        {
            sr.color = Color.Lerp(startColor, endColor, time / timeToCollapse);
            time += Time.deltaTime;
            yield return null;
        }
        colliders[0].enabled = false;
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollapsing && collision.gameObject.CompareTag("Player"))
        {
            isCollapsing = true;
            collapseInst();
        }
    }
}
