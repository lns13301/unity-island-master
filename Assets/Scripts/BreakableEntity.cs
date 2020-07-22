using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakableEntity : MonoBehaviour
{
    private Animator playerAnimator;
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
    public int armor;
    public int avoid;
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

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isRemoval = false;
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();

        isInside = false;
        maxHp = hp;
        healthBar.fillAmount = 1f;

        GetComponent<ObjectData>().canvas.SetActive(true);
        GetComponent<ObjectData>().hud.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInside)
        {
            attackDetection();
        }

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
                    Debug.Log("아이템 소환");
                    ItemDatabase.instance.spawnItemByCode(transform.position, dropItemCode[i], Random.Range(dropCountMin[i], dropCountMax[i] + 1));
                }
            }

            GameObject.Find("DialogManager").GetComponent<DialogManager>().isDataChange();

            dialogManager.addExp(exp);

            if (gameObject.tag == "Tree")
            {
                Destroy(gameObject.transform.parent.gameObject);
            }

            Destroy(gameObject);
        }

        damagedMotion();

        if (isRemoval)
        {
            if (gameObject.tag == "Tree")
            {
                Destroy(gameObject.transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        isInside = true;
        melee = collision;
    }

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
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.6f);
            }
        }
    }

    // 최대 소수점 한 자리
    public bool dropChances(float num)
    {
        num *= 10;
        return Random.Range(0, 1000) < num;
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
}
