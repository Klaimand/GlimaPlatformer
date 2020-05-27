using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_DamageTaker : MonoBehaviour
{
    [SerializeField]
    GameObject QTEPrefab = null;
    [SerializeField]
    float invulnerableTimeAfterDamageTaking = 0f;
    //[HideInInspector]
    public bool isInvulnerable;
    public bool isDamageInvulnerable;

    PlayerController2D controller;
    KLD_EgoManager egoManager;
    KLD_PlayerEvents events;


    private void Awake()
    {
        controller = GetComponent<PlayerController2D>();
        egoManager = GetComponent<KLD_EgoManager>();
        events = GetComponent<KLD_PlayerEvents>();
    }
    
    void Update()
    {
        doInvulnerability();
    }
    
    public void doDamageTaking (DamageType damageType, Transform mine, float explosionForce)
    {
        events.InvokeDamageTaking();
        //arreter le joueur
        isDamageInvulnerable = true;
        controller.cantMove = true;
        if (damageType == DamageType.Explosion) {
            controller.addExplosionForce(mine, explosionForce);
        }
        else if (damageType == DamageType.Grab)
        {
            transform.position = mine.position;
            controller.grabbed = true;
        }

        //summon le QTE avec les bonnes valeurs
        if ((int)egoManager.curEgoState < 4)
        {
            InstantiateQTE(damageType, (int)egoManager.curEgoState);
        }
        else
        {
            controller.cantMove = false;
            startInvulnerability();
        }

        //actualiser l'égo
        egoManager.removeEgo();


    }

    public void InstantiateQTE(DamageType damageType, int difficulty)
    {
        GameObject curQTE = Instantiate(QTEPrefab, transform.position + new Vector3(-3.7f, 2.75f, 0f), Quaternion.identity);
        KLD_TestQTE qteScript = curQTE.transform.GetChild(0).GetComponent<KLD_TestQTE>();

        if (damageType == DamageType.Explosion)
        {
            qteScript.qteMode = KLD_TestQTE.QteMode.button;
            if (difficulty == 0)
            {
                qteScript.maxPoints = HardButtonValues.maxPoints;
                qteScript.pointsPerInput = HardButtonValues.pointsPerInput;
                qteScript.pointsLostPerSecond = HardButtonValues.pointsLostPerSecond;
            }
            else if (difficulty == 1)
            {
                qteScript.maxPoints = MediumButtonValues.maxPoints;
                qteScript.pointsPerInput = MediumButtonValues.pointsPerInput;
                qteScript.pointsLostPerSecond = MediumButtonValues.pointsLostPerSecond;
            }
            else if (difficulty <= 3)
            {
                qteScript.maxPoints = EasyButtonValues.maxPoints;
                qteScript.pointsPerInput = EasyButtonValues.pointsPerInput;
                qteScript.pointsLostPerSecond = EasyButtonValues.pointsLostPerSecond;
            }
        }
        else if (damageType == DamageType.Grab)
        {
            qteScript.qteMode = KLD_TestQTE.QteMode.joystick;
            if (difficulty == 0)
            {
                qteScript.maxPoints = HardJoystickValues.maxPoints;
                qteScript.pointsPerInput = HardJoystickValues.pointsPerInput;
                qteScript.pointsLostPerSecond = HardJoystickValues.pointsLostPerSecond;
            }
            else if (difficulty == 1)
            {
                qteScript.maxPoints = MediumJoystickValues.maxPoints;
                qteScript.pointsPerInput = MediumJoystickValues.pointsPerInput;
                qteScript.pointsLostPerSecond = MediumJoystickValues.pointsLostPerSecond;
            }
            else if (difficulty <= 3)
            {
                qteScript.maxPoints = EasyJoystickValues.maxPoints;
                qteScript.pointsPerInput = EasyJoystickValues.pointsPerInput;
                qteScript.pointsLostPerSecond = EasyJoystickValues.pointsLostPerSecond;
            }
        }
    }
    
    public void startInvulnerability ()
    {
        StartCoroutine(startInvul());
    }

    private IEnumerator startInvul ()
    {
        yield return new WaitForSeconds(invulnerableTimeAfterDamageTaking);
        isDamageInvulnerable = false;
    }

    private void doInvulnerability ()
    {
        if (isDamageInvulnerable || egoManager.getSprintState())
        {
            isInvulnerable = true;
        }
        else
        {
            isInvulnerable = false;
        }
    }

}
