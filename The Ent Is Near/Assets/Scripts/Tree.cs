using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f, damageRate = 5f;
    float currentHp;
    [SerializeField] Image healthBar;
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHealth;
        UpdateHealth();
        gm = FindObjectOfType<GameManager>();
        gm.AddTree(this);
    }
    private void OnDestroy()
    {
        gm.RemoveTree(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateHealth()
    {
        healthBar.fillAmount= currentHp / maxHealth;
    }

    public void Damaged()
    {
        currentHp -= damageRate;
        UpdateHealth();
        if(currentHp<=0)
        {
            Death();
        }
    }

    void Death()
    {
        //TODO: call game manager to say that this tree is destroyed
        //TODO: call tree falling animation and anything that goes with it
        Destroy(this.gameObject);
    }





}
