using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KLD_EgoManager : MonoBehaviour
{
    [SerializeField, Range(1, 40)]
    private int egoPointsPerBar;
    
    [SerializeField]
    private int curEgoPoints, egoPointsPerCan, pointsAfter3BarsToFillSuperSayan;

    
    private Image egoBarUI, egoBarAUI;

    private float[] egoBarFillPoints = { 0f, 0.325f, 0.5935f, 0.8615f, 1f };

    public enum EgoState
    {
        ZeroBarFilled,
        OneBarFilled,
        TwoBarsFilled,
        ThreeBarsFilled,
        FourBarsFilled
    }

    public EgoState curEgoState;

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
       if (Input.GetKeyDown(KeyCode.Space))
       {
            addEgo(egoPointsPerCan);
       }
       if (Input.GetKeyDown(KeyCode.A))
       {
            removeEgo();
       }
    }

    public void addEgo(int egoToAdd)
    {
        curEgoPoints = Mathf.Min(curEgoPoints + egoToAdd, (egoPointsPerBar * 3) + pointsAfter3BarsToFillSuperSayan);
        checkEgoState();
    }

    public void removeEgo ()
    {
        curEgoPoints = Mathf.Max(0, curEgoPoints - egoPointsPerBar);
        checkEgoState();
    }

    void checkEgoState()
    {
        if (curEgoPoints < egoPointsPerBar - 1)
        {
            curEgoState = EgoState.ZeroBarFilled;
        }
        else if (curEgoPoints >= egoPointsPerBar && curEgoPoints < (egoPointsPerBar * 2) - 1)
        {
            curEgoState = EgoState.OneBarFilled;
        }
        else if (curEgoPoints >= egoPointsPerBar * 2 && curEgoPoints < (egoPointsPerBar * 3) - 1)
        {
            curEgoState = EgoState.TwoBarsFilled;
        }
        else if (curEgoPoints >= egoPointsPerBar * 3 && curEgoPoints < (egoPointsPerBar * 3) + pointsAfter3BarsToFillSuperSayan)
        {
            curEgoState = EgoState.ThreeBarsFilled;
        }
        else if (curEgoPoints == egoPointsPerBar * 3 + pointsAfter3BarsToFillSuperSayan)
        {
            curEgoState = EgoState.FourBarsFilled;
        }
        updateEgoBarUI();
    }

    private void updateEgoBarUI()
    {
        egoBarUI.fillAmount = egoBarFillPoints[(int)curEgoState];
        
        if ((int)curEgoState != 4)
        {
            float thisBarFillingRatio = (int)curEgoState == 3 ? (float)(curEgoPoints % pointsAfter3BarsToFillSuperSayan) / (float)pointsAfter3BarsToFillSuperSayan : (float)(curEgoPoints % egoPointsPerBar) / (float)egoPointsPerBar;
            float fillingDifferenceBetweenActualBar = (int)curEgoState == 3 ? 1f - egoBarFillPoints[(int)curEgoState] : egoBarFillPoints[(int)curEgoState + 1] - egoBarFillPoints[(int)curEgoState];
            
            egoBarAUI.fillAmount = egoBarFillPoints[(int)curEgoState] + (fillingDifferenceBetweenActualBar * thisBarFillingRatio);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Can"))
        {
            addEgo(egoPointsPerCan);
            Destroy(collision.gameObject);
        }
    }
}
