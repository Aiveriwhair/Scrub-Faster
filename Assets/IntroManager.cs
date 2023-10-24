using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public PlayerMovement playerMovement;
    
    
    public ItemCleanable itemCleanable;
    public ItemStorage itemStorage;
    public ItemFixable itemFixable;
    public ItemPickable itemPickable;
    public ToolFix toolFix;
    public ToolClean toolClean;

    public Canvas topCanvas;
    public Text topMessages;
    public Canvas fullCanvas;
    public Text fullMessages;
    public float fullCanvasDisplayTime = 5f;

    private bool coroutinePlaying = false;
    public enum IntroStates
    {
        INTROMESSAGE,
        PICKPHONE,
        STOREPHONE,
        PICKCLEANER,
        CLEANVOMIT,
        PICKHAMMER,
        FIXTABLE,
        FINISHED
    }
    private IntroStates _currentState;
    
    void Start()
    {
        _currentState = IntroStates.INTROMESSAGE;
        topCanvas.enabled = false;
        topMessages.enabled = false;
        fullCanvas.enabled = false;
        fullMessages.enabled = false;
    }
    void Update()
    {
        IntroStateHandler();
    }

    public bool IsIntroCompleted()
    {
        if (!itemCleanable.isClean()) return false;
        if (!itemStorage.GetStoredItems().Contains(itemPickable)) return false;
        if (!itemFixable.IsFixed()) return false;
        return true;
    }

    public void ShowTopMessage(string message)
    {
        topMessages.text = message; 
        topCanvas.enabled = true; 
        topMessages.enabled = true; 
    }

    public void HideTopMessage()
    {
        topCanvas.enabled = false; 
        topMessages.enabled = false;
    }
    
    IEnumerator ShowFullMessage(string message, float duration, IntroStates nextState)
    {
        coroutinePlaying = true;
        playerMovement.acceptPlayerInput = false;
        HideTopMessage();
        fullMessages.text = message;
        fullCanvas.enabled = true;
        fullMessages.enabled = true;
        yield return new WaitForSeconds(duration);
        playerMovement.acceptPlayerInput = true;
        fullCanvas.enabled = false;
        fullMessages.enabled = false;
        _currentState = nextState;
        coroutinePlaying = false;
    }
    
    IEnumerator CompleteIntroduction(string message, float duration)
    {
        coroutinePlaying = true;
        yield return new WaitForSeconds(3);
        StartCoroutine(ShowFullMessage(message, duration, nextState: IntroStates.FINISHED));
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
        coroutinePlaying = false;
    }
    

    private void IntroStateHandler()
    {
        if (coroutinePlaying) return;
        switch (_currentState)
        {
            case IntroStates.INTROMESSAGE:
                StartCoroutine(ShowFullMessage("Let's learn the basics...\nFollow the instructions in the top right corner!", fullCanvasDisplayTime, IntroStates.PICKPHONE));
                break;
            case IntroStates.PICKPHONE:
                ShowTopMessage("Pick the phone on the table (E)");
                if (playerInventory.Contains(itemPickable))
                {
                    _currentState = IntroStates.STOREPHONE;
                }
                break;
            case IntroStates.STOREPHONE:
                ShowTopMessage("Store the phone in the wardrobe (E)");
                if (itemStorage.GetStoredItems().Contains(itemPickable))
                {
                    _currentState = IntroStates.PICKCLEANER;
                }
                break;

            case IntroStates.PICKCLEANER:
                ShowTopMessage("Pick the water gun (E)");
                if (playerInventory.Contains(toolClean))
                {
                    _currentState = IntroStates.CLEANVOMIT;
                }
                break;

            case IntroStates.CLEANVOMIT:
                ShowTopMessage("Clean the wall using the gun (E)");
                if (itemCleanable.isClean())
                {
                    _currentState = IntroStates.PICKHAMMER;
                }
                break;

            case IntroStates.PICKHAMMER:
                ShowTopMessage("Pick the hammer (E)");
                if (playerInventory.Contains(toolFix))
                {
                    _currentState = IntroStates.FIXTABLE;
                }
                break;

            case IntroStates.FIXTABLE:
                ShowTopMessage("Use the hammer on the table to fix it (E)");
                if (itemFixable.IsFixed())
                {
                    _currentState = IntroStates.FINISHED;
                }
                break;
            case IntroStates.FINISHED:
                StartCoroutine(CompleteIntroduction("Well done! Introduction completed. Returning to menu...", 3f));
                break;
        }
    }
}
