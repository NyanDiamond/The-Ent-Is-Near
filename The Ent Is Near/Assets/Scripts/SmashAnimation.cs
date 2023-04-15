using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashAnimation : MonoBehaviour
{

    // Start is called before the first frame update
    void OnEnable()
    {
        transform.localScale = new Vector2(0, 0);
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        while(transform.localScale.x<=1)
        {
            yield return new WaitForSeconds(.03f);
            transform.localScale = new Vector2(transform.localScale.x + .1f, transform.localScale.y + .1f);
        }
        transform.parent.gameObject.SetActive(false);
    }
}
