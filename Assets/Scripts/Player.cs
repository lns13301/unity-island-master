using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float h;
    private float v;
    private float verticalSpeed;
    private float horizontalSpeed = 5.0f;
    private float motionStop = 0.1f;
    private int flipReverse = -1;
    private float footPosition = 0.59f;
    private float armPosition = 0.3f;
    private float rayRange = 0.8f;
    public float attackRange;
    private string[] layers = new string[3];
    private int removeMessageTimer;
    private float doSomething;

    public JoystickValue value;
    public float joystickSpeed = 0.03f;
    public int buttonTimer = 0;

    private Vector3 direction;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public DialogManager dialogManager;
    public GameObject scanObject;
    public GameObject scanEnemy;

    private AudioSource playerAudioSource;
    public bool isPickedItem;
    public bool isPunching;
    public bool isTreatingTool;
    public bool isCritical;
    public bool isMove;
    public bool isEquipedTool;
    public bool isSting;
    public bool isEatting;
    public bool isCollecting;
    public bool isShitting;

    // attack
    public Transform posRight;
    public Transform posLeft;
    public GameObject monster;
    public Vector2 boxSize;
    public bool isAttacking;
    public int damageTimer;
    public GameObject meleeRight;
    public GameObject meleeLeft;

    //levelUp
    public GameObject hudDamageText;
    public GameObject hudLevelUpText;
    public Transform hudPos;
    public bool isLevelUp;
    public bool isPointUp;

    //damaged
    private bool isMiss;
    public StatUI statUI;

    //equipment
    public GameObject leftHand;
    public GameObject[] leftHands;
    public int activeLeftHand;

    public GameObject rightHand;
    public GameObject[] rightHands;
    public int activeRightHand;

    public GameObject head;
    public GameObject[] heads;
    public int activeHead;

    public GameObject top;
    public GameObject[] tops;
    public int activeTop;

    public GameObject pant;
    public GameObject[] pants;
    public int activePants;

    public GameObject glove;
    public GameObject[] gloves;
    public int activeGloves;

    public GameObject shoe;
    public GameObject[] shoes;
    public int activeShoes;

    public GameObject neckless;
    public GameObject[] necklesses;
    public int activeNeckless;

    public GameObject earing;
    public GameObject[] earings;
    public int activeEaring;

    public GameObject ring;
    public GameObject[] rings;
    public int activeRing;

    public GameObject hair;
    public GameObject[] hairs;
    public int activeHair;

    [SerializeField] private AudioClip[] clip;

    public GameObject joystick;
    public bool isKnockDown;
    public GameObject knockDownSet;
    public GameObject popUp;
    public GameObject camera;

    public Button toiletButton;
    public Button collectButton;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        statUI = GameObject.Find("Canvas").GetComponent<StatUI>();
        verticalSpeed = horizontalSpeed / 3;
        layers[0] = "Entity";
        layers[1] = "Item";
        layers[2] = "Enemy";
        removeMessageTimer = 0;
        doSomething = 1f;
        isMove = false;
        isPickedItem = false;
        isAttacking = false;
        isCritical = false;
        isPunching = false;
        isTreatingTool = false;
        isEquipedTool = false;
        isKnockDown = false;
        isSting = false;
        isEatting = false;
        isCollecting = false;
        isShitting = false;
        damageTimer = 0;
        knockDownSet.SetActive(false);
        popUp = knockDownSet.transform.GetChild(1).gameObject;

        attackRange = 1.5f;

        toiletButton.gameObject.SetActive(false);
        collectButton.gameObject.SetActive(false);

        setEquipmentGameObject();

        activeLeftHand = -1;
        activeRightHand = -1;
        activeHead = -1;
        activeTop = -1;
        activePants = -1;
        activeGloves = -1;
        activeShoes = -1;
        activeNeckless = -1;
        activeEaring = -1;
        activeRing = -1;
        activeHair = -1;
    }

    // Update is called once per frame
    void Update()
    {
        setIdleState();
        doKnockDown();
        controlWalk();

        if (isEquipedTool)
        {
            if (dialogManager.GetComponent<PlayerEquipment>().items[2].handType == HandType.Spear)
            {
                controlSting();
            }
            else
            {
                controlTools();
            }
        }
        else
        {
            controlPunch();
        }

        controlPickUp();
        removeMessage();
        controlSound();
        setEquipmentMotion();

        //ButtonController
        if (buttonTimer > 0)
        {
            buttonTimer--;
/*            if (buttonTimer == 1)
            {
                GameObject.Find("Canvas").GetComponent<StatUI>().isDataChanged = true;
            }*/
        }

        //levelUpSign
        if (isLevelUp)
        {
            playerAudioSource.clip = clip[3];
            playerAudioSource.Play();
            GameObject hudText = Instantiate(hudLevelUpText);
            hudText.transform.position = hudPos.position;
            isLevelUp = false;
        }

        if (isPointUp)
        {
            playerAudioSource.clip = clip[4];
            playerAudioSource.Play();
            isPointUp = false;
        }
    }

    void FixedUpdate()
    {
        //Ray
        rayController();
        rayControllerAttack();

        //Move
        rigid.velocity = new Vector2(h * horizontalSpeed / doSomething, v * verticalSpeed / doSomething);
    }

    // 장비 아이템 모션 적용
    public void setEquipmentMotion()
    {
        if (spriteRenderer.flipX)
        {
            if (activeLeftHand != -1)
            {
                leftHands[activeLeftHand].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeRightHand != -1)
            {
                rightHands[activeRightHand].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeHead != -1)
            {
                heads[activeHead].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeTop != -1)
            {
                tops[activeTop].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activePants != -1)
            {
                pants[activePants].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeGloves != -1)
            {
                gloves[activeGloves].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeShoes != -1)
            {
                shoes[activeShoes].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeNeckless != -1)
            {
                necklesses[activeNeckless].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeEaring != -1)
            {
                earings[activeEaring].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeRing != -1)
            {
                rings[activeRing].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeHair != -1)
            {
                hairs[activeHair].GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            if (activeLeftHand != -1)
            {
                leftHands[activeLeftHand].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeRightHand != -1)
            {
                rightHands[activeRightHand].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeHead != -1)
            {
                heads[activeHead].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeTop != -1)
            {
                tops[activeTop].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activePants != -1)
            {
                pants[activePants].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeGloves != -1)
            {
                gloves[activeGloves].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeShoes != -1)
            {
                shoes[activeShoes].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeNeckless != -1)
            {
                necklesses[activeNeckless].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeEaring != -1)
            {
                earings[activeEaring].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeRing != -1)
            {
                rings[activeRing].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeHair != -1)
            {
                hairs[activeHair].GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    private void setEquipmentGameObject()
    {
        for (int i = 0; i < leftHands.Length; i++)
        {
            leftHands[i] = leftHand.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < rightHands.Length; i++)
        {
            rightHands[i] = rightHand.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < heads.Length; i++)
        {
            heads[i] = head.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < tops.Length; i++)
        {
            tops[i] = top.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < pants.Length; i++)
        {
            pants[i] = pant.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < gloves.Length; i++)
        {
            gloves[i] = glove.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < shoes.Length; i++)
        {
            shoes[i] = shoe.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < necklesses.Length; i++)
        {
            necklesses[i] = neckless.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < earings.Length; i++)
        {
            earings[i] = earing.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < rings.Length; i++)
        {
            rings[i] = ring.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < hairs.Length; i++)
        {
            hairs[i] = hair.transform.GetChild(i).gameObject;
        }
    }

    public void controlWalk()
    {
        if (!pickUpAnimationDone())
        {
            doSomething = 5f;
        }

        if (!punchAnimationDone())
        {
            doSomething = 5f;
        }

        if (!toolsAnimationDone())
        {
            doSomething = 5f;
        }

        if (!stingAnimationDone())
        {
            doSomething = 5f;
        }

        if (!collectAnimationDone())
        {
            doSomething = 100f;
        }

        if (!eatAnimationDone())
        {
            doSomething = 5f;
        }

        if (!shitAnimationDone())
        {
            doSomething = 100f;
        }

        if (punchAnimationDone() && toolsAnimationDone() && pickUpAnimationDone() && collectAnimationDone() && eatAnimationDone() && shitAnimationDone() && stingAnimationDone())
        {
            doSomething = 1f;
        }

        // PC
        h = dialogManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v = dialogManager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonDown("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");

        if (hDown)
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == flipReverse;
        }

        if (spriteRenderer.flipX)
        {
            meleeRight.SetActive(false);
            meleeLeft.SetActive(true);
        }
        else
        {
            meleeRight.SetActive(true);
            meleeLeft.SetActive(false);
        }

        if (rigid.velocity.normalized.x < motionStop && rigid.velocity.normalized.x > -motionStop
            && rigid.velocity.normalized.y < motionStop && rigid.velocity.normalized.y > -motionStop)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        rayController(hDown, hUp);
        //rayControllerAttack(hDown, hUp);

        //Joystick

        scanObjectData();

        if (dialogManager.isAction)
        {
            return;
        }

        transform.Translate(new Vector2(value.joyTouch.x * joystickSpeed * 2 / doSomething, value.joyTouch.y * joystickSpeed / doSomething));

        if (value.joyTouch.x != 0 || value.joyTouch.y != 0)
        {
            animator.SetBool("isWalking", true);
            isMove = true;
        }
        else
        {
            animator.SetBool("isWalking", false);
            isMove = false;
        }

        if (value.joyTouch.x < 0)
        {
            spriteRenderer.flipX = true;

        }
        if (value.joyTouch.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (value.joyTouch.x < 0)
        {
            direction = Vector3.left;
        }
        else if (value.joyTouch.x > 0)
        {
            direction = Vector3.right;
        }
    }

    private void controlPunch()
    {
        if (dialogManager.isAction)
        {
            return;
        }

        if ((Input.GetButtonDown("Fire3") || (value.aTouch && buttonTimer == 0))
            && punchAnimationDone() && toolsAnimationDone() && pickUpAnimationDone() 
            && collectAnimationDone() && eatAnimationDone() && shitAnimationDone() && stingAnimationDone())
        {
            statUI.isDataChanged = true;
            buttonTimer = 50;
            animator.Play("PlayerPunch");
            animator.SetBool("isPunching", true);
            dialogManager.playerData.satiety -= Calculator.satietyCalc(dialogManager.playerData);


        }
        else
        {
            animator.SetBool("isPunching", false);
        }
    }

    private void controlTools()
    {
        if (dialogManager.isAction)
        {
            return;
        }

        if ((Input.GetButtonDown("Fire3") || (value.aTouch && buttonTimer == 0))
            && punchAnimationDone() && toolsAnimationDone() && pickUpAnimationDone() 
            && collectAnimationDone() && eatAnimationDone() && shitAnimationDone() && stingAnimationDone())
        {
            statUI.isDataChanged = true;
            buttonTimer = 80;
            animator.Play("PlayerTools");
            animator.SetBool("isTreatingTool", true);
            dialogManager.playerData.satiety -= Calculator.satietyCalc(dialogManager.playerData);
        }
        else
        {
            animator.SetBool("isTreatingTool", false);
        }
    }

    private void controlSting()
    {
        if (dialogManager.isAction)
        {
            return;
        }

        if ((Input.GetButtonDown("Fire3") || (value.aTouch && buttonTimer == 0))
            && punchAnimationDone() && toolsAnimationDone() && pickUpAnimationDone() 
            && collectAnimationDone() && eatAnimationDone() && shitAnimationDone() && stingAnimationDone())
        {
            statUI.isDataChanged = true;
            buttonTimer = 80;
            animator.Play("PlayerStab");
            animator.SetBool("isSting", true);
            dialogManager.playerData.satiety -= Calculator.satietyCalc(dialogManager.playerData);
        }
        else
        {
            animator.SetBool("isSting", false);
        }
    }

    private void controlPickUp()
    {
        if (dialogManager.isAction)
        {
            return;
        }

        if (Input.GetButtonDown("Jump") || (value.dTouch && buttonTimer == 0) 
            && punchAnimationDone() && toolsAnimationDone() && pickUpAnimationDone() && collectAnimationDone() && eatAnimationDone() && shitAnimationDone())
        {
            buttonTimer = 30;
            animator.Play("PlayerPickUp");
            animator.SetBool("isPicking", true);
        }
        else
        {
            animator.SetBool("isPicking", false);
        }
    }

    public void controlCollect()
    {
        if (dialogManager.isAction)
        {
            return;
        }

        if (buttonTimer == 0 && punchAnimationDone() && toolsAnimationDone() && pickUpAnimationDone() && collectAnimationDone() && eatAnimationDone() && shitAnimationDone())
        {
            buttonTimer = 80;
            animator.Play("PlayerCollect");
            animator.SetBool("isCollecting", true);
        }
        else
        {
            animator.SetBool("isCollecting", false);
        }
    }

    public void controlEating()
    {
        if (dialogManager.isAction)
        {
            return;
        }

        if (buttonTimer == 0 && punchAnimationDone() && toolsAnimationDone() && pickUpAnimationDone() && collectAnimationDone() && eatAnimationDone() && shitAnimationDone())
        {
            buttonTimer = 50;
            animator.Play("PlayerEat");
            animator.SetBool("isEating", true);
        }
        else
        {
            animator.SetBool("isEating", false);
        }
    }

    public void controlShitting()
    {
        if (dialogManager.isAction)
        {
            return;
        }

        if (buttonTimer == 0 && punchAnimationDone() && toolsAnimationDone() && pickUpAnimationDone() && collectAnimationDone() && eatAnimationDone() && shitAnimationDone())
        {
            buttonTimer = 100;
            dialogManager.playerData.catharsis += (dialogManager.playerData.catharsisMax * 0.7f);
            animator.Play("PlayerShit");
            animator.SetBool("isShitting", true);
        }
        else
        {
            animator.SetBool("isShitting", false);
        }
    }

    private void setIdleState()
    {
        if (buttonTimer == 40)
        {
            if (animator.GetBool("isCollecting"))
            {
                animator.SetBool("isCollecting", false);
            }

            if (animator.GetBool("isEating"))
            {
                animator.SetBool("isEating", false);
            }

            if (animator.GetBool("isShitting"))
            {
                animator.SetBool("isShitting", false);
            }
        }
    }

    public bool walkingAnimationDone()
    {
        return !(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerWalk"))
            || animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerWalk") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f;
    }

    public bool pickUpAnimationStart()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPickUp") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPickUp") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f;
    }

    public bool pickUpAnimationProgress()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPickUp") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPickUp") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f;
    }

    public bool pickUpAnimationDone()
    {
        return !(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPickUp"))
            || animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPickUp") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f;
    }

    public bool punchAnimationProgress()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPunch") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPunch") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f;
    }

    public bool punchAnimationDone()
    {
        return !(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPunch"))
            || animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPunch") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f;
    }

    public bool toolsAnimationProgress()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerTools") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerTools") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f;
    }

    public bool toolsAnimationDone()
    {
        return !(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerTools"))
            || animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerTools") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f;
    }

    public bool stingAnimationProgress()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerStab") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerStab") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f;
    }

    public bool stingAnimationDone()
    {
        return !(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerStab"))
            || (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerStab") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f);
    }

    public bool eatAnimationProgress()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerEat") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerEat") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f;
    }

    public bool eatAnimationDone()
    {
        return !(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerEat"))
            || animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerEat") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f;
    }

    public bool collectAnimationProgress()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerCollect") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerCollect") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f;
    }

    public bool collectAnimationDone()
    {
        return !(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerCollect"))
            || animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerCollect") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f;
    }

    public bool shitAnimationProgress()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerShit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f
            && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerShit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f;
    }

    public bool shitAnimationDone()
    {
        return !(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerShit"))
            || animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerShit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f;
    }


    private void rayController(bool hDown, bool hUp)
    {
        if (hDown && h == -1)
        {
            direction = Vector3.left;
        }
        else if (hDown && h == 1)
        {
            direction = Vector3.right;
        }
        scanObjectData();
    }

    private void scanObjectData()
    {
        if ((Input.GetButtonDown("Jump") || (value.dTouch && buttonTimer == 0)) && scanObject != null && scanObject.tag == "Entity")
        {
            dialogManager.setNPCName(scanObject.GetComponent<ObjectData>().getName());
            //Debug.Log(scanObject.GetComponent<ObjectData>().getName());
            buttonTimer = 10;
            dialogManager.action(scanObject);
        }

        if ((Input.GetButtonDown("Jump") || (value.dTouch && buttonTimer == 0)) && scanObject == null && dialogManager.isAction)
        {
            buttonTimer = 10;
            dialogManager.action();
        }
    }

    public void questTalkStart()
    {
        dialogManager.setNPCName(scanObject.GetComponent<ObjectData>().getName());
        dialogManager.action(scanObject);
    }

    private void rayController()
    {
        Vector3 colliderVec = new Vector3(rigid.position.x, rigid.position.y - footPosition);

        Debug.DrawRay(colliderVec, direction * rayRange, new Color(0, 1, 0));
        RaycastHit2D raycastHit = Physics2D.Raycast(colliderVec, direction, rayRange, LayerMask.GetMask(layers));

        if (raycastHit.collider != null)
        {
            scanObject = raycastHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }

    private void rayControllerAttack(bool hDown, bool hUp)
    {
        if (hDown && h == -1)
        {
            direction = Vector3.left;
        }
        else if (hDown && h == 1)
        {
            direction = Vector3.right;
        }
    }

    private void rayControllerAttack()
    {
        Vector3 colliderVec = new Vector3(rigid.position.x, rigid.position.y - armPosition);

        Debug.DrawRay(colliderVec, direction * attackRange, new Color(0, 0, 1));
        RaycastHit2D raycastHit = Physics2D.Raycast(colliderVec, direction, attackRange, LayerMask.GetMask(layers));

        if (raycastHit.collider != null)
        {
            scanEnemy = raycastHit.collider.gameObject;
        }
        else
        {
            scanEnemy = null;
        }
    }

    private void scanObjectDataAttack()
    {
        if (scanEnemy != null && scanEnemy.tag == "Enemy")
        {
            
        }
    }

    private void removeMessage()
    {
        if (pickUpAnimationProgress())
        {
            removeMessageTimer = 100;
        }

        if (removeMessageTimer == 1)
        {
            dialogManager.pickUpMessageClear();
        }

        if (removeMessageTimer > 0)
        {
            removeMessageTimer--;
        }
    }

    public void controlSound()
    {
        if (isPunching)
        {
            if (isCritical)
            {
                playerAudioSource.clip = clip[1];
            }
            else
            {
                playerAudioSource.clip = clip[0];
            }

            playerAudioSource.Play();
            isPunching = false;
        }

        if (isPickedItem)
        {
            playerAudioSource.clip = clip[2];
            playerAudioSource.Play();
            isPickedItem = false;
        }

        if (isTreatingTool)
        {
            playerAudioSource.clip = clip[5];
            playerAudioSource.Play();
            isTreatingTool = false;
        }

        if (isSting)
        {
            playerAudioSource.clip = clip[5];
            playerAudioSource.Play();
            isSting = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EntityEnemy")
        {
            isMiss = false;
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            float totalDamage = Calculator.fatigueCalc(dialogManager.playerData, enemy.power, isHit(enemy.accuracy));

            if (totalDamage <= 0)
            {
                isMiss = true;
            }
            float damageValue = totalDamage * (Random.Range(enemy.mastery, 101) / 100);

            onDamaged(collision.transform.position, isMiss, damageValue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Toilet")
        {
            toiletButton.gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "Collect")
        {
            collectButton.gameObject.SetActive(true);
            collision.gameObject.GetComponent<ObjectData>().isCollecting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Toilet")
        {
            toiletButton.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Collect")
        {
            collectButton.gameObject.SetActive(false);
        }
    }

    void onDamaged(Vector2 targetPos, bool isMiss, float damageValue)
    {
        statUI.isDataChanged = true;

        // playerAudioSource.clip = clip[3];
        //playerAudioSource.Play();
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;

        if (damageValue <= 0 || isMiss)
        {
            hudText.GetComponent<DamageText>().damage = 0;
            gameObject.layer = 15;
            Invoke("offDamage", 3);

            return;
        }

        dialogManager.playerData.fatigue -= damageValue;

        hudText.GetComponent<DamageText>().damage = (int)damageValue;

        gameObject.layer = 15;

        if (!isMiss)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            int dirc = transform.position.x - targetPos.y > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 1) * 100, ForceMode2D.Impulse);
        }

        Invoke("offDamage", 3);
    }

    void offDamage()
    {
        gameObject.layer = 12;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public bool isHit(int accuracy)
    {
        int hit = accuracy - dialogManager.playerData.avoid;

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

    public void doKnockDown()
    {
        if (dialogManager.playerData.fatigue > 0)
        {
            return;
        }

        if (!isKnockDown)
        {
            animator.SetTrigger("doKnockDown");
            knockDownSet.SetActive(true);
            popUp.SetActive(false);

            // 죽었을 때, 아이템 잃는 이벤트 처리해야할 위치

            isKnockDown = true;
            Invoke("viewPopUp", 3);
        }

        repeatKnockDown();
    }

    public void viewPopUp()
    {
        popUp.SetActive(true);
    }

    public void repeatKnockDown()
    {
        if (!isKnockDown)
        {
            return;
        }

        animator.SetBool("repeatKnockDown", true);
    }

    public void uiOnOffKnockDownSet()
    {
        dialogManager.playerData.fatigue = dialogManager.playerData.fatigueMax;
        animator.SetBool("repeatKnockDown", false);
        isKnockDown = false;
        transform.position = new Vector2(-0.3f, -1f);
        camera.transform.position = new Vector2(-0.3f, -1f);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().mapCode = 0;
        GameObject.Find("SoundManager").GetComponent<SoundManager>().refreshSounds();

        knockDownSet.SetActive(false);
    }
}
