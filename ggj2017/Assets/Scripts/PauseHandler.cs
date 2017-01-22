using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour {

    public GameObject Canvas;
    public GameObject Camera;
    bool Paused = false;

    void Start()
    {
   //     Canvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Pressed escape.");
            if (Paused)
            {
                Time.timeScale = 1.0f;
                Canvas.gameObject.SetActive(false);
                Cursor.visible = false;
           //     Screen.lockCursor = true;
           //     Camera.GetComponent<AudioSource>().Play();
                Paused = false;
            }
            else
            {
                Time.timeScale = 0.0f;
                Canvas.gameObject.SetActive(true);
                Cursor.visible = true;
              //  Screen.lockCursor = false;
             //   Camera.GetComponent<AudioSource>().Pause();
                Paused = true;
            }
        }
    }
    public void Resume()
    {
        print("resuming");
        Time.timeScale = 1.0f;
        Canvas.gameObject.SetActive(false);
        Cursor.visible = false;
     //   Screen.lockCursor = true;
     //   Camera.audio.Play();
    }
}    
//	}
//}
