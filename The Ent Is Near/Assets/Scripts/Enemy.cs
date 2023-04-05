using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [SerializeField] float speed, damagerate;
    Tree target;
    bool canMove = true;
    GameManager gm;
    //bool doDamage = true;

    [SerializeField] float maxHealth = 100f;
    float currentHp;
    [SerializeField] Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTarget();
        currentHp = maxHealth;
        UpdateHealth();
        gm = FindObjectOfType<GameManager>();
        gm.AddEnemy(this);
    }


    void UpdateTarget()
    {
        Tree[] trees = GameObject.FindObjectsOfType<Tree>();
        int currentIndex = 0;
        int closestIndex = 0;
        float closestValue = 9999999999;
        foreach (Tree tree in trees)
        {
            float value = Vector2.Distance(transform.position, tree.transform.position);
            if (value<closestValue)
            {
                closestValue = value;
                closestIndex = currentIndex;
            }
            currentIndex++;
        }

        target = trees[closestIndex];
        canMove = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null) UpdateTarget();
        Vector2 dir = (target.transform.position - transform.position).normalized;
        if(canMove)
        {
            transform.Translate(dir * speed *Time.deltaTime);
        }
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            Damaged(25);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Tree>()==target)
        {
            StartCoroutine(Attack());
            canMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Tree>() == target)
        {
            StopAllCoroutines();
            UpdateTarget();
        }
    }

    IEnumerator Attack()
    {
        while(true)
        {
            //TODO Start Animation (play with animation timing to match)
            yield return new WaitForSeconds(damagerate);
            target.Damaged();
        }
    }

    public void Damaged(float damage)
    {
        currentHp -= damage;
        UpdateHealth();
        if (currentHp <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        //TODO: call game manager to say that this tree is destroyed
        //TODO: call tree falling animation and anything that goes with it
        gm.RemoveEnemy(this);
        Destroy(this.gameObject);
    } 

    void UpdateHealth()
    {
        healthBar.fillAmount = currentHp / maxHealth;
    }
}
