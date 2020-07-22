using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private DialogManager dialogManager;
    private Enemy enemy;
    public bool isCollider;

    // Start is called before the first frame update
    void Start()
    {
        dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollider = true;

        if (collision.gameObject.name == "Player")
        {
            float totalDamage = Calculator.fatigueCalc(dialogManager.playerData, enemy.power, isHit());
            dialogManager.playerData.fatigue -= totalDamage * (Random.Range(enemy.mastery, 101) / 10);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isCollider = false;
    }

    public bool isHit()
    {
        int hit = enemy.accuracy - dialogManager.playerData.avoid;

        if (hit <= 0)
        {
            return false;
        }

        if (Random.Range(0, 20) > hit)
        {
            return false;
        }

        return true;
    }
}
