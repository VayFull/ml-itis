using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnPaintClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnSuperPixelClick()
    {
        SceneManager.LoadScene(2);
    }
}
