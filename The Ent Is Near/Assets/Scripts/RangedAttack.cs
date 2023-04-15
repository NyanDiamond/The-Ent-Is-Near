using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] int waves=8;
    float movement;
    [SerializeField] float speed=.3f;
    // Start is called before the first frame update
    void Start()
    {
        movement = 1;
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        for (int wave = 0; wave<waves; wave++)
        {
            yield return new WaitForSeconds(speed);
            transform.position+=(transform.right * movement);
        }
        Destroy(this.gameObject);
    }
}
