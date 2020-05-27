using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KLD_LdHelper : MonoBehaviour
{
    private Transform respawnPointsObject;
    private Rigidbody2D rb;
    private new Camera camera;
    private GameObject debugMenu;

    public bool canDebugTp = true;

    private float lastDpadX;
    private float lastDpadY;

    public float unitsAddedPerDPadClick;

    private string currentcode;

    private bool debugOpen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.GetComponent<Camera>();
        respawnPointsObject = GameObject.Find("RespawnPointsObject").transform;
        debugMenu = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (canDebugTp) {
            teleportOnClickPosition();
            teleportToRespawnPoint();
            teleportOnControllerCrossDirection();
        }
        doCharacterReturn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            transform.position = respawnPointsObject.GetChild(0).position;
            rb.velocity = Vector2.zero;
        }
    }

    void teleportOnClickPosition ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !debugOpen)
        {
            transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            rb.velocity = Vector2.zero;
        }
    }

    void teleportOnControllerCrossDirection ()
    {
        float curDPadX = Input.GetAxis("DPad X");
        float curDPadY = Input.GetAxis("DPad Y");
        if (curDPadX == 1f && lastDpadX != 1f)
        {
            transform.position += Vector3.right * unitsAddedPerDPadClick;
            rb.velocity = Vector2.zero;
        }
        else if (curDPadX == -1f && lastDpadX != -1f)
        {
            transform.position += Vector3.left * unitsAddedPerDPadClick;
            rb.velocity = Vector2.zero;
        }
        if (curDPadY == 1f && lastDpadY != 1f)
        {
            transform.position += Vector3.up * unitsAddedPerDPadClick;
            rb.velocity = Vector2.zero;
        }
        else if (curDPadY == -1f && lastDpadY != -1f)
        {
            transform.position += Vector3.down * unitsAddedPerDPadClick;
            rb.velocity = Vector2.zero;
        }
        lastDpadX = Input.GetAxis("DPad X");
        lastDpadY = Input.GetAxis("DPad Y");
    }

    void teleportToRespawnPoint ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = respawnPointsObject.GetChild(0).position;
            rb.velocity = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.position = respawnPointsObject.GetChild(1).position;
            rb.velocity = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.position = respawnPointsObject.GetChild(2).position;
            rb.velocity = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            transform.position = respawnPointsObject.GetChild(3).position;
            rb.velocity = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            transform.position = respawnPointsObject.GetChild(4).position;
            rb.velocity = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            transform.position = respawnPointsObject.GetChild(5).position;
            rb.velocity = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            transform.position = respawnPointsObject.GetChild(6).position;
            rb.velocity = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            transform.position = respawnPointsObject.GetChild(7).position;
            rb.velocity = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            transform.position = respawnPointsObject.GetChild(8).position;
            rb.velocity = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            transform.position = respawnPointsObject.GetChild(9).position;
            rb.velocity = Vector2.zero;
        }
    }

    void doCharacterReturn ()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {

            }
            else if ((c == '\n') || (c == '\r'))
            {
                checkCode();
                currentcode = "";
            }
            else
            {
                currentcode += c;
            }
        }
    }

    void checkCode ()
    {
        if (currentcode == "debug")
        {
            debugOpen = true;
            debugMenu.SetActive(true);
        }
    }

    #region Debug Buttons Functions

    public void resetScene ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    public void closeDebug ()
    {
        debugOpen = false;
        debugMenu.SetActive(false);
    }

    public void disableDebugTp ()
    {
        canDebugTp = false;
    }

    public void enableDebugTp ()
    {
        canDebugTp = true;
    }

    #endregion

}
