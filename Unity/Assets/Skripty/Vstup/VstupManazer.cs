using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VstupManazer : MonoBehaviour
{
    [SerializeField]
    AutoManazer autoManazer;

    private void Awake(){
        if (!CompareTag("Player")){
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vstup = Vector2.zero;

        vstup.x = Input.GetAxis("Horizontal");
        vstup.y = Input.GetAxis("Vertical");

        autoManazer.NastavVstup(vstup);

        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
 