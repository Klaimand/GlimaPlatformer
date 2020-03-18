using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_QTESummoner : MonoBehaviour
{

    //TRYING TO REMOVE THAT

    [SerializeField]
    private GameObject QTEObject;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateQTE (DamageType damageType, int difficulty)
    {
        GameObject curQTE = Instantiate(QTEObject, playerTransform.position + new Vector3(-3.7f, 2.75f, 0f), Quaternion.identity);
        KLD_TestQTE qteScript = curQTE.GetComponent<KLD_TestQTE>();
        if (difficulty == 0)
        {
            //hard
            if (damageType == DamageType.Explosion)
            {
                qteScript.maxPoints = HardButtonValues.maxPoints;
                qteScript.pointsPerInput = HardButtonValues.pointsPerInput;
                qteScript.pointsLostPerSecond = HardButtonValues.pointsLostPerSecond;
            }
            else if (damageType == DamageType.Grab)
            {

            }
        }
        else if (difficulty == 1)
        {
            //medium
            if (damageType == DamageType.Explosion)
            {
                qteScript.maxPoints = MediumButtonValues.maxPoints;
                qteScript.pointsPerInput = MediumButtonValues.pointsPerInput;
                qteScript.pointsLostPerSecond = MediumButtonValues.pointsLostPerSecond;
            }
            else if (damageType == DamageType.Grab)
            {

            }
        }
        else if (difficulty == 2)
        {
            //easy
            if (damageType == DamageType.Explosion)
            {
                qteScript.maxPoints = EasyButtonValues.maxPoints;
                qteScript.pointsPerInput = EasyButtonValues.pointsPerInput;
                qteScript.pointsLostPerSecond = EasyButtonValues.pointsLostPerSecond;
            }
            else if (damageType == DamageType.Grab)
            {

            }
        }
    }

}
