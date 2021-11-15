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
    private bool wasTouching1 = false;
    private bool wasTouching2 = false;


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
        
        // WORK IN PROGRESS
        //if(topPressed && bottomPressed)
        //{
        //    // This is true SEVERAL TIMES
        //}

    }


    // A REHACER POR EL DAVID GANFORNINA
    private void ControlsMobile()
    {
                //bool isTouching = Input.touchCount > 0;
                int iteration = 0;

                foreach (var touch in Input.touches)
                {
                    if (iteration == 0)
                    {
                        if (touch.phase == TouchPhase.Began && !wasTouching)
                        {
                            if (touch.position.x < Screen.width/2f && touch.position.y < (Screen.height - Screen.height / 3))
                            {
                                NoteHolderTop.BeatNote();
                                rulles.JumpSprite();
                            }
                            else if (touch.position.x > Screen.width/2f &&
                                     touch.position.y < (Screen.height - Screen.height / 3))
                            {
                                NoteHolderBottom.BeatNote();
                                rulles.DownSprite();
                            }
                        }
                    
                        wasTouching = touch.phase !=TouchPhase.Ended;

                        if (Input.touchCount > 0 && touch.phase == TouchPhase.Ended)
                        {
                            NoteHolderTop.ReleaseControl();
                            NoteHolderBottom.ReleaseControl();
                            rulles.IdleSprite();
                        }

                        iteration = 1;
                    }
                    else
                    {
                        Debug.Log("Ha entrado en la segunda");
                        if (touch.phase == TouchPhase.Began && !wasTouching1)
                        {
                            Debug.Log("Ha entrado en la segunda IF");
                            if (touch.position.x < Screen.width/2f && touch.position.y < (Screen.height - Screen.height / 3))
                            {
                                NoteHolderTop.BeatNote();
                                rulles.JumpSprite();
                            }
                            else if (touch.position.x > Screen.width/2f &&
                                     touch.position.y < (Screen.height - Screen.height / 3))
                            {
                                NoteHolderBottom.BeatNote();
                                rulles.DownSprite();
                            }
                        }
                    
                        wasTouching1 = touch.phase != TouchPhase.Ended;

                        if (Input.touchCount > 0 && touch.phase == TouchPhase.Ended)
                        {
                            NoteHolderTop.ReleaseControl();
                            NoteHolderBottom.ReleaseControl();
                            rulles.IdleSprite();
                        }      
                    }
                    
                }
                
               
            
          
        
    }

    #endregion

}
