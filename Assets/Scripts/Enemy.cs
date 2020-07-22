using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Animator playerAnimator;
    public Animator animator;
    public DialogManager dialogManager;
    private Player playerScript;
    private bool hit = false;

    // 몬스터 정보
    public int id;
    public string mobName;
    public int level;
    public float maxHp;
    public float maxMobSpeed = 1;
    public int maxPower;
    public int maxArmor;
    public int maxAccuracy;
    public int maxAvoid;
    public float maxCritRate;
    public float maxCritDam;
    public float maxMastery;

    public float hp;
    public float mobSpeed = 1;
    public int power;
    public int armor;
    public int accuracy;
    public int avoid;
    public float critRate;
    public float critDam;
    public float mastery = 40;
    public int exp;

    public int damagedTimer;
    public GameObject healthBarBackground;
    public Image healthBar;

    public bool isDamaged = false;
    public int[] dropItemCode;
    public float[] dropChance;
    public int[] dropCountMin;
    public int[] dropCountMax;
    private bool isInside;
    private Collider2D melee;
    public GameObject hudDamageText;
    public Transform hudPos;

    //AI
    public Rigidbody2D rigid;
    public float nextMoveX;
    public float nextMoveY;

    public bool isRemoval;

    private bool isPreviousMove;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //Invoke("think", 2);
        InvokeRepeating("think", 2, 4);
    }

    private void Start()
    {
        isRemoval = false;
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
        animator = GetComponent<Animator>();

        isInside = false;
        maxHp = hp;
        healthBar.fillAmount = 1f;

        GetComponent<ObjectData>().canvas.SetActive(true);
        GetComponent<ObjectData>().hud.SetActive(true);

        isPreviousMove = false;
    }
    private void Update()
    {
        if (isInside)
        {
            attackDetection();
        }

        // 피격 판정 처리
        if (hit && ((playerScript.punchAnimationProgress() && playerScript.toolsAnimationDone() && playerScript.stingAnimationDone())
            || (playerScript.toolsAnimationProgress() && playerScript.punchAnimationDone() && playerScript.stingAnimationDone())
            || (playerScript.stingAnimationProgress() && playerScript.punchAnimationDone() && playerScript.toolsAnimationDone())
            ))
        {
            takeDamage(dialogManager.playerData);
            hit = false;
        }

        if (hp <= 0)
        {
            SpawnManager.instance.enemyCount[id]--;

            for (int i = 0; i < dropItemCode.Length; i++)
            {
                if (dropChance == null)
                {
                    dropChance[i] = 100;
                }

                if (dropCountMax[i] == 0)
                {
                    dropCountMax = dropCountMin;
                }

                if (dropChances(dropChance[i]))
                {
                    ItemDatabase.instance.spawnItemByCode(transform.position, dropItemCode[i], Random.Range(dropCountMin[i] + 1, dropCountMax[i] + 1));
                }
            }

            GameObject.Find("DialogManager").GetComponent<DialogManager>().isDataChange();

            dialogManager.addExp(exp);
            Destroy(gameObject);
        }

        damagedMotion();

        if (isRemoval)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        enemyMoveNonTargeting();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInside = true;
        melee = collision;

        if (collision.gameObject.tag == "Removal")
        {
            isRemoval = true;
        }
    }

/*    private void OnTriggerStay2D(Collider2D collision)
    {
        isInside = true;
        melee = collision;
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInside = false;
        melee = null;
    }

    public void attackDetection()
    {
        if (!playerScript.punchAnimationDone() && !playerScript.toolsAnimationDone() && !playerScript.stingAnimationDone())
        {
            return;
        }

        if (melee.gameObject.tag == "Player" && (playerAnimator.GetBool("isPunching") || playerAnimator.GetBool("isTreatingTool") || playerAnimator.GetBool("isSting")))
        {
            if ((melee.gameObject.name == "MeleeRight" && !playerScript.GetComponent<SpriteRenderer>().flipX)
                || (melee.gameObject.name == "MeleeLeft" && playerScript.GetComponent<SpriteRenderer>().flipX))
            {
                hit = true;
                if (playerAnimator.GetBool("isPunching"))
                {
                    playerScript.isPunching = true;
                }
                if (playerAnimator.GetBool("isTreatingTool"))
                {
                    playerScript.isTreatingTool = true;
                }
                if (playerAnimator.GetBool("isSting"))
                {
                    playerScript.isSting = true;
                }
            }
        }
    }

    public void takeDamage(PlayerData playerdata)
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;

        int avoidValue = avoid - playerdata.accuracy;
        int damage = -((armor - playerdata.power * Random.Range(600, 1010)) / 1000);

        if (damage <= 0)
        {
            hudText.GetComponent<DamageText>().damage = 0;
            return;
        }

        if (avoidValue > 0)
        {
            if (Random.Range(0, 30) < avoidValue)
            {
                hudText.GetComponent<DamageText>().damage = 0;
                return;
            }
        }

        dialogManager.playerData.money += 1;
        hp -= damage;
        damagedTimer = 30;
        isDamaged = true;
        healthBar.fillAmount = hp / maxHp;
        healthBarBackground.SetActive(true);

        hudText.GetComponent<DamageText>().damage = damage;

        // 뒤집기
        // healthBar.transform.localScale = transform.localScale;
    }

    public void damagedMotion()
    {
        if (damagedTimer > 0)
        {
            damagedTimer--;

            if (isDamaged)
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 0.6f);
                isDamaged = false;
            }

            if (damagedTimer == 1)
            {
                if (id == 0)
                {
                    GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.6f);
                    return;
                }

                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    // 최대 소수점 한 자리
    public bool dropChances(float num)
    {
        num *= 10;
        return Random.Range(0, 1000) < num;
    }

    public void enemyMoveNonTargeting()
    {
        //Move
        rigid.velocity = new Vector2(nextMoveX, nextMoveY);

        //collider Check
        Vector2 horizontalVec = new Vector2(rigid.position.x + nextMoveX * 0.3f, rigid.position.y);
        Vector2 verticalVec = new Vector2(rigid.position.x + nextMoveX * 0.3f, rigid.position.y + nextMoveY);

        RaycastHit2D rayHitDown = Physics2D.Raycast(horizontalVec, Vector3.down, 3, LayerMask.GetMask("Collider"));
        RaycastHit2D rayHitUp = Physics2D.Raycast(horizontalVec, Vector3.up, 3, LayerMask.GetMask("Collider"));
        RaycastHit2D rayHitLeft = Physics2D.Raycast(verticalVec, Vector3.left, 3, LayerMask.GetMask("Collider"));
        RaycastHit2D rayHitRight = Physics2D.Raycast(verticalVec, Vector3.right, 3, LayerMask.GetMask("Collider"));

        if (rayHitDown.collider != null || rayHitUp.collider != null)
        {
            nextMoveY *= -1;
            CancelInvoke();
            Invoke("think", 4);
        }

        if (rayHitRight.collider != null || rayHitLeft.collider != null)
        {
            nextMoveX *= -1;
            CancelInvoke();
            Invoke("think", 4);
            doFlip();
        }
    }

    private void doMove()
    {
        try
        {
            animator.SetTrigger("doMove");
            animator.SetBool("isWait", false);
            animator.SetBool("isMove", true);
            animator.SetBool("isRush", false);
        }
        catch
        {

        }
    }

    private void doStop()
    {
        try
        {
            animator.SetTrigger("doStop");
            animator.SetBool("isWait", false);
            animator.SetBool("isMove", false);
            animator.SetBool("isRush", false);
        }
        catch
        {

        }
    }

    private void think()
    {
        nextMoveX = Random.Range(-1, 2) * mobSpeed;
        nextMoveY = Random.Range(-1, 2) * 0.5f * mobSpeed;

        if ((nextMoveX == 0 && nextMoveY == 0) && isPreviousMove)
        {
            doStop();
            isPreviousMove = false;
        }
        else if ((nextMoveX != 0 || nextMoveY != 0) && !isPreviousMove)
        {
            doMove();
            isPreviousMove = true;
        }

        doFlip();

        //Invoke("think", 4);
    }

    private void doFlip()
    {
        if (nextMoveX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void stuckWhenGetDamage()
    {
        nextMoveX = 0;
        nextMoveY = 0;
        doStop();

        CancelInvoke();
        InvokeRepeating("think", 2, 4);
        isPreviousMove = false;
    }
}
