using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    private String url_1="https://github.com/VagrantTrack149";
    private String url_2= "https://github.com/brDaniel";
    // Start is called before the first frame update
    public void Salir()
    {
        SceneManager.LoadScene(0);
    }
    public void cargar_pagina()
    {
        Application.OpenURL(url_1);
    }

    public void cargar_pagina1()
    {
        Application.OpenURL(url_2);
    }
}
