using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KLD_EgoManager : MonoBehaviour
{
    [SerializeField, Range(1, 20)]
    private int egoPointsPerBar = 0, egoPointsPerCan = 0;

    [SerializeField]
    private float curEgoPoints;
    [SerializeField, Tooltip("Cost of 1 second of sprinting")]
    private float sprintSecondEnergyConsuption;
    [SerializeField]
    private float minimumEgoToSprint = 0f;

    private Image egoBarUI, egoBarAUI, egoBarEclairs;

    //private float[] egoBarFillPoints = { 0f, 0.325f, 0.5935f, 0.8615f};
    private float[] egoBarFillPoints = { 0f, 0.272f, 0.543f, 0.815f };

    [SerializeField]
    private bool isSprinting;

    PlayerController2D controller;
    KLD_DamageTaker damageTaker;
    KLD_AudioManager audioManager;

    Animator egoEmptyAnimator;

    public enum EgoState
    {
        ZeroBarFilled,
        OneBarFilled,
        TwoBarsFilled,
        ThreeBarsFilled
    }

    public EgoState curEgoState;

    public void Awake()
    {
        controller = GetComponent<PlayerController2D>();
        damageTaker = GetComponent<KLD_DamageTaker>();
    }

    // Start is called before the first frame update
    void Start()
    {
        egoBarUI = GameObject.Find("EgoBar").GetComponent<Image>();
        egoBarAUI = GameObject.Find("EgoBarA").GetComponent<Image>();
        egoBarEclairs = GameObject.Find("EgoBarEclairs").GetComponent<Image>();
        audioManager = GameObject.Find("AudioManager").GetComponent<KLD_AudioManager>();
        egoEmptyAnimator = GameObject.Find("EgoBar_Ego").GetComponent<Animator>();
        checkEgoState();
    }

    // Update is called once per frame
    void Update()
    {
        doSprintInput();
        checkEgoState();
        updateEgoBarUI();
        updateEclairsOnSprint();
        updateEgoBarShake();
    }

    public void addEgo(int egoToAdd)
    {
        curEgoPoints = Mathf.Min(curEgoPoints + egoToAdd, (egoPointsPerBar * 3));
    }

    public void removeEgo ()
    {
        curEgoPoints = Mathf.Max(0, curEgoPoints - egoPointsPerBar);
    }

    void checkEgoState()
    {
        if (curEgoPoints < egoPointsPerBar)
        {
            curEgoState = EgoState.ZeroBarFilled;
        }
        else if (curEgoPoints >= egoPointsPerBar && curEgoPoints < (egoPointsPerBar * 2))
        {
            curEgoState = EgoState.OneBarFilled;
        }
        else if (curEgoPoints >= egoPointsPerBar * 2 && curEgoPoints < (egoPointsPerBar * 3))
        {
            curEgoState = EgoState.TwoBarsFilled;
        }
        else if (curEgoPoints >= egoPointsPerBar * 3)
        {
            curEgoState = EgoState.ThreeBarsFilled;
        }
    }

    /*
    private void updateEgoBarUI()
    {
        egoBarUI.fillAmount = egoBarFillPoints[(int)curEgoState];
        
        if ((int)curEgoState != 3)
        {
            float thisBarFillingRatio = (float)(curEgoPoints % egoPointsPerBar) / (float)egoPointsPerBar;
            float fillingDifferenceBetweenActualBar = egoBarFillPoints[(int)curEgoState + 1] - egoBarFillPoints[(int)curEgoState];
            
            egoBarAUI.fillAmount = egoBarFillPoints[(int)curEgoState] + (fillingDifferenceBetweenActualBar * thisBarFillingRatio);
        }
    }*/

    private void updateEgoBarUI ()
    {
        float filledAmount = ((float)curEgoPoints * egoBarFillPoints[3]) / ((float)egoPointsPerBar * 3f);
        egoBarAUI.fillAmount = filledAmount;
        egoBarUI.fillAmount = 1f - filledAmount;
    }

    private void updateEclairsOnSprint ()
    {
        if (isSprinting)
        {
            egoBarEclairs.color = Color.white;
        }
        else
        {
            egoBarEclairs.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Can"))
        {
            if (curEgoPoints < egoPointsPerBar * 3) {
                addEgo(egoPointsPerCan);
                audioManager.PlaySound("CanPickup");
            }
            else if (curEgoPoints == egoPointsPerBar * 3)
            {
                egoEmptyAnimator.SetTrigger("Pop");
            }
            Destroy(collision.gameObject);
        }
    }

    void doSprintInput ()
    {
        if (!isSprinting && curEgoPoints >= minimumEgoToSprint && Input.GetButton("Sprint") && !controller.getSlopeSlideStatus() && !controller.getSlopeStandStatus())
        {
            isSprinting = true;
        }

        else if (isSprinting && curEgoPoints <= 0f || isSprinting && !Input.GetButton("Sprint") || controller.getSlopeSlideStatus() || controller.getSlopeStandStatus())
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            curEgoPoints -= sprintSecondEnergyConsuption * Time.deltaTime;
        }

        controller.SetSprint(isSprinting);



    }

    public bool getSprintState()
    {
        return isSprinting;
    }


    void updateEgoBarShake ()
    {
        egoEmptyAnimator.SetBool("isShaking", isSprinting);
    }
}
