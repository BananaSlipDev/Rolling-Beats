using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHoldersManager : MonoBehaviour
{
    // -- ONLY FOR TESTING --
    [SerializeField] private bool testMode = false;
    //-----------------------

    // NoteHoldes
    [SerializeField] private NoteHolderController NoteHolderTop;
    [SerializeField] private NoteHolderController NoteHolderBottom;
    private bool topPressed = false;
    private bool bottomPressed = false;

    //Mobile variables
    private Touch mytouch;
    [SerializeField] private int multiplier;
    private bool wasTouching = false;


    [SerializeField] private KeyCode keyToPressTop;
    [SerializeField] private KeyCode keyToPressBottom;
    private RullesController rulles;

    private void Start()
    {
        rulles = this.GetComponent<RullesController>();
        NoteHolderTop = transform.Find("NoteHolderTop").GetComponent<NoteHolderController>();
        NoteHolderBottom = transform.Find("NoteHolderBottom").GetComponent<NoteHolderController>();
    }

    void Update()
    {
        if (testMode) // ONLY FOR TESTING IN UNITY
        {
            ControlsPC();
        }
        else
        {
            if (CheckMobileManager.SharedInstance.IsMobileGet)
            {
                if (!GameUI.instance.PauseMenu.activeInHierarchy)
                {
                    ControlsMobile();
                }
            }
            else
                ControlsPC();
        }

    }


    #region Controls
    private void ControlsPC()
    {
        // Top NoteHolder
        if (Input.GetKeyDown(keyToPressTop))
        {
            NoteHolderTop.BeatNote();
            rulles.JumpSprite();
            topPressed = true;
        }
        else if (Input.GetKeyUp(keyToPressTop))
        {
            NoteHolderTop.ReleaseControl();
            rulles.IdleSprite();
            topPressed = false;
        }
   
        // Bottom NoteHolder
        if (Input.GetKeyDown(keyToPressBottom))
        {
            NoteHolderBottom.BeatNote();
            rulles.DownSprite();
            bottomPressed = true;
        }
        else if (Input.GetKeyUp(keyToPressBottom))
        {
            NoteHolderBottom.ReleaseControl();
            rulles.IdleSprite();
            bottomPressed = false;
        }
        
        // WORK IN PROGRESS
        //if(topPressed && bottomPressed)
        //{
        //    // This is true SEVERAL TIMES
        //}

    }


    // A REHACER POR EL DAVID GANFORNINA
    private void ControlsMobile()
    {
        bool isTouching = Input.touchCount > 0;
        if (isTouching && Input.touches[0].phase == TouchPhase.Began && !wasTouching)
        {
            Input.touches[0].phase = TouchPhase.Canceled;
            mytouch = Input.GetTouch(0);


            if (multiplier * mytouch.position.x < multiplier * Screen.width / 2f && mytouch.position.y < (Screen.height - Screen.height / 3))
            {
                switch (multiplier) //Changes the sprite depending on the touch
                {
                    case 1:
                        NoteHolderTop.BeatNote();
                        rulles.JumpSprite();
                        break;
                    case -1:
                        NoteHolderBottom.BeatNote();
                        rulles.DownSprite();
                        break;
                }
            }
        }

        wasTouching = isTouching;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            //spriteRenderer.sprite = defaultSprite;
            NoteHolderTop.ReleaseControl();
            rulles.IdleSprite();
        }
    }

    #endregion

}
