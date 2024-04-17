/******** Copyright (c) John P. Doran, All rights reserved. ******************
//
// File Name :  CarAvoidHelpers.cs
// Author :     John P. Doran
//
// Purpose :    Helper utility to assist in the car chase cinematic. Sends
//              animation events and communicates with the QTE system.
//
 * *****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarAvoidHelpers : MonoBehaviour
{
    public QTESystem.QTE qte;
    public void StartQTE()
    {
        qte.transform.parent.gameObject.SetActive(true);
        qte.Reset();
        qte.BeginQTE();
    }

    public Animator ethan;

    public void MoveEthan()
    {
        ethan.SetFloat("Forward", 10);
        ethan.SetFloat("Turn", 5);

        Invoke("StopTurn", 1);
    }

    void StopTurn()
    {
        ethan.SetFloat("Turn", 0);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
