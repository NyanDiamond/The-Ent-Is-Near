using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [SerializeField] float speed, damagerate;
    Tree target;
    bool canMove = true;
    bool slowed = false;
    GameManager gm;
    Animator an;
    Coroutine cr;
    [SerializeField] GameObject swirl;
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
        slowed = false;
        an = GetComponent<Animator>();
        swirl.SetActive(false);
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

        RotateTarget();
    }

    void RotateTarget()
    {
        if (an!=null)
        {
            Vector2 temp = (target.transform.position - transform.position).normalized;
            an.SetFloat("xMove", temp.x);
            an.SetFloat("yMove", temp.y);
        }
        else
        {

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null && Time.timeScale==1) UpdateTarget();
        Vector2 dir = (target.transform.position - transform.position).normalized;
        if(canMove)
        {
            if(slowed)
                transform.Translate(dir * speed/10 *Time.deltaTime);
            else
                transform.Translate(dir * speed * Time.deltaTime);
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
            StopCoroutine(Attack());
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

    

    public void Damaged(float damage, bool slow)
    {
        currentHp -= damage;
        UpdateHealth();
        if (currentHp <= 0)
        {
            Death();
        }
        if(slow)
        {
            slowed = slow;
            if(cr!=null)
            {
                StopCoroutine(cr);
            }
            cr = StartCoroutine(Slowed());
        }
    }

    IEnumerator Slowed()
    {
        swirl.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        slowed = false;
        swirl.SetActive(false);
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
