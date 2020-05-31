using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KLD_MainMenu : MonoBehaviour
{
    public string[] categoryNames;

    public string[] gdNames;
    public string[] gaNames;

    public GameObject namesEmpty;
    
    [SerializeField]
    private Button buttonToSelect;

    KLD_AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<KLD_AudioManager>();
        buttonToSelect.Select();
    }

    public void randomizeNames ()
    {
        int randomNumber = Mathf.RoundToInt(Random.Range(0f, 100f));

        if (randomNumber < 50)
        {
            //gd
            namesEmpty.transform.GetChild(0).GetComponent<Text>().text = categoryNames[0];
            namesEmpty.transform.GetChild(2).GetComponent<Text>().text = categoryNames[1];

            for (int i = 0; i < 2; i++)
            {

                List<int> namesPutted = new List<int>();

                while (namesPutted.Count < 3)
                {
                    int _randomNumber = Mathf.RoundToInt(Random.value * 2);
                    if (!namesPutted.Contains(_randomNumber))
                    {
                        namesPutted.Add(_randomNumber);
                    }
                }
                if (i == 0)
                {
                namesEmpty.transform.GetChild(1).GetComponent<Text>().text =
                    gdNames[namesPutted[0]] + "\n" + gdNames[namesPutted[1]] + "\n" + gdNames[namesPutted[2]];
                }
                else if (i == 1)
                {
                    namesEmpty.transform.GetChild(3).GetComponent<Text>().text =
                    gaNames[namesPutted[0]] + "\n" + gaNames[namesPutted[1]] + "\n" + gaNames[namesPutted[2]];
                }

            }

        }
        else if (randomNumber > 50)
        {
            //ga
            namesEmpty.transform.GetChild(0).GetComponent<Text>().text = categoryNames[1];
            namesEmpty.transform.GetChild(2).GetComponent<Text>().text = categoryNames[0];


            for (int i = 0; i < 2; i++)
            {

                List<int> namesPutted = new List<int>();

                while (namesPutted.Count < 3)
                {
                    int _randomNumber = Mathf.RoundToInt(Random.value * 2);
                    if (!namesPutted.Contains(_randomNumber))
                    {
                        namesPutted.Add(_randomNumber);
                    }
                }


                if (i == 0)
                {
                    namesEmpty.transform.GetChild(1).GetComponent<Text>().text =
                        gaNames[namesPutted[0]] + "\n" + gaNames[namesPutted[1]] + "\n" + gaNames[namesPutted[2]];
                }
                else if (i == 1)
                {
                    namesEmpty.transform.GetChild(3).GetComponent<Text>().text =
                        gdNames[namesPutted[0]] + "\n" + gdNames[namesPutted[1]] + "\n" + gdNames[namesPutted[2]];
                }

            }

        }
        else if (randomNumber == 50)
        {
            print("SHITTY NAMES TIME");
            audioManager.PlaySound("Avengers");
            audioManager.GetSound("RingsOfJupiter").GetSource().volume = 0.1f;
            for (int i = 0; i < 2; i++)
            {

                List<int> namesPutted = new List<int>();

                while (namesPutted.Count < 3)
                {
                    int _randomNumber = Mathf.RoundToInt(Random.value * 2);
                    if (!namesPutted.Contains(_randomNumber))
                    {
                        namesPutted.Add(_randomNumber);
                    }
                }


                if (i == 0)
                {
                    namesEmpty.transform.GetChild(1).GetComponent<Text>().text =
                        gaNames[namesPutted[0] + 3] + "\n" + gaNames[namesPutted[1] + 3] + "\n" + gaNames[namesPutted[2] + 3];
                }
                else if (i == 1)
                {
                    namesEmpty.transform.GetChild(3).GetComponent<Text>().text =
                        gdNames[namesPutted[0] + 3] + "\n" + gdNames[namesPutted[1] + 3] + "\n" + gdNames[namesPutted[2] + 3];
                }

            }
        }
    }

    public void putMusicSoundBack ()
    {
        audioManager.GetSound("RingsOfJupiter").GetSource().volume = 0.7f;
    }

    public void launchScene2705 ()
    {
        GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(true);
        StartCoroutine(waitOneSecThenLoadScene());
        audioManager.FadeOutInst(audioManager.GetSound("RingsOfJupiter").GetSource(), 1.5f);
    }

    IEnumerator waitOneSecThenLoadScene ()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("KLD_2705_LD");
    }

    public void quitApplication ()
    {
        Application.Quit();
    }

}
