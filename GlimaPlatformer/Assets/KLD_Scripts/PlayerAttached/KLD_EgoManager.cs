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
    
    private Image egoBarUI, egoBarAUI;

    private float[] egoBarFillPoints = { 0f, 0.325f, 0.5935f, 0.8615f};

    [SerializeField]
    private bool isSprinting;

    PlayerController2D controller;
    KLD_DamageTaker damageTaker;

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
        checkEgoState();
    }

    // Update is called once per frame
    void Update()
    {
        doSprintInput();
        checkEgoState();
        updateEgoBarUI();
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

    private void updateEgoBarUI()
    {
        egoBarUI.fillAmount = egoBarFillPoints[(int)curEgoState];
        
        if ((int)curEgoState != 3)
        {
            float thisBarFillingRatio = (float)(curEgoPoints % egoPointsPerBar) / (float)egoPointsPerBar;
            float fillingDifferenceBetweenActualBar = egoBarFillPoints[(int)curEgoState + 1] - egoBarFillPoints[(int)curEgoState];
            
            egoBarAUI.fillAmount = egoBarFillPoints[(int)curEgoState] + (fillingDifferenceBetweenActualBar * thisBarFillingRatio);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Can") && curEgoPoints < egoPointsPerBar * 3)
        {
            addEgo(egoPointsPerCan);
            Destroy(collision.gameObject);
        }
    }

    void doSprintInput ()
    {
        if (!isSprinting && curEgoPoints >= minimumEgoToSprint && Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
        }

        else if (isSprinting && curEgoPoints <= 0f || isSprinting && !Input.GetButton("Sprint"))
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
}
