using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] GameObject rangedAttack;
    [SerializeField] float speed;
    [SerializeField] float xBound, yBound;
    float xMove, yMove;
    Animator an;
    bool attacking = false;
    GameObject child1, child2;

    Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        an = GetComponent<Animator>();
        child1 = transform.GetChild(0).gameObject;
        child2 = child1.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            attacking = false;
        }
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            xMove--;
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            xMove++;
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            yMove++;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyUp(KeyCode.W))
        {
            yMove--;
        }
        if (!attacking)
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack(1);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Attack(2);
            }
        }
    }
    
    private void Move()
    {
        Vector2 moveDir = (Vector2.right * xMove + Vector2.up * yMove).normalized;
        if(moveDir!=Vector2.zero)
        {
            //an.SetBool("Moving", true);
        }
        else
        {
            //an.SetBool("Moving", false);
        }
        //Debug.Log(moveDir);
        Rotate(moveDir);
        transform.position+=(Vector3)(moveDir * speed * Time.deltaTime);
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -xBound, xBound), Mathf.Clamp(transform.position.y, -yBound, yBound));
    }
    private void Rotate(Vector2 dir)
    {
        if(dir.x<0)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        an.SetFloat("yMove", yMove);
        if(yMove == 0)
        {
            an.SetFloat("yMove", -.01f);
        }
    }

    private void Attack(int num)
    {
        attacking = true;
        if(num==1)
        {
            //Debug.Log("Melee attacking");
            an.SetTrigger("Attack1");
        }
        else if (num==2)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            an.SetTrigger("Attack2");
        }
    }

    public void AttackEnd()
    {
        //Debug.Log("AttackFinish");
        attacking = false;
    }

    public void MeleeStart()
    {
        child2.SetActive(false);
        child1.SetActive(true);

    }

    public void MeleeOff()
    {
        child2.SetActive(true);
    }

    public void RangedAttackOff()
    {
        float rotation = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //Debug.Log("Ranged attacking towards " + summonLocation);
        Instantiate(rangedAttack, transform.position, Quaternion.Euler(0f, 0f, rotation));
    }
}
