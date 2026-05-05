using UnityEngine;
using UnityEngine.SceneManagement;

public class MenueScript : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("MissionOne");
    }
    public void menue()
    {
        SceneManager.LoadScene("Main Menue");
    }
}
