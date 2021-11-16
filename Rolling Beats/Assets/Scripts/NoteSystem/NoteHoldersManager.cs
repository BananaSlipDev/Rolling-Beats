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

    private List<int> fingers;

    [SerializeField] private KeyCode keyToPressTop;
    [SerializeField] private KeyCode keyToPressBottom;
    private RullesController rulles;


    private void Start()
    {
        rulles = this.GetComponent<RullesController>();
        NoteHolderTop = transform.Find("NoteHolderTop").GetComponent<NoteHolderController>();
        NoteHolderBottom = transform.Find("NoteHolderBottom").GetComponent<NoteHolderController>();

        fingers = new List<int>();
    }

    void Update()
    {
        if (testMode) // ONLY FOR TESTING IN UNITY, SHOULD BE FALSE IN RELEASE VERSION
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

    }

    private void ControlsMobile()
    {
        int nbTouches = Input.touchCount;

        if(nbTouches > 0) // If there are any touches...
        {
            for(int i = 0; i < nbTouches; i++)
            {
                Touch touch = Input.GetTouch(i);

                if(touch.phase == TouchPhase.Began &&
                    !fingers.Contains(touch.fingerId)) // If there's a new touch on screen
                {
                    fingers.Add(touch.fingerId); // Adds the touch to the list of touches onScreen

                    // Left side down
                    if (touch.position.x < Screen.width/2f &&
                         touch.position.y < (Screen.height - Screen.height / 3))
                    {
                        NoteHolderTop.BeatNote();
                        rulles.JumpSprite();
                    }
                    // Right side down
                    else if (touch.position.x > Screen.width/2f &&
                             touch.position.y < (Screen.height - Screen.height / 3))
                    {
                        NoteHolderBottom.BeatNote();
                        rulles.DownSprite();
                    }
                }
                else if(touch.phase == TouchPhase.Ended &&
                        fingers.Contains(touch.fingerId)) // If the touch was onScreen and ended
                {
                    fingers.Remove(touch.fingerId); // Deletes the touch from the list of touches onScreen

                    // Left side up
                    if (touch.position.x < Screen.width/2f &&
                         touch.position.y < (Screen.height - Screen.height / 3))
                    {
                        NoteHolderTop.ReleaseControl();
                    }
                    // Right side up
                    else if (touch.position.x > Screen.width/2f &&
                             touch.position.y < (Screen.height - Screen.height / 3))
                    {
                        NoteHolderBottom.ReleaseControl();
                    }

                }
            }
        }
        else
            rulles.IdleSprite();
                
        
    }

    #endregion

}
