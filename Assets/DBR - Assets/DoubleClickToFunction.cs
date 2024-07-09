using UnityEngine;
using System.Collections;

public class DoubleClickToFunction : MonoBehaviour
{
    public bool doubleClickToFunctionActive;
    public float doubleClickTimeLimit = 0.25f; // Time limit for double click detection
    private float lastClickTime;
    private bool isFirstClick = false;
    public bool functionClicked;
    public InputDetector inputDetector;

    void Update()
    {
        //doubleClickToFunctionActive = inputDetector.inputActivated; 

        if (doubleClickToFunctionActive)
        {
            // Detect a mouse button click (left mouse button)
            if (Input.GetMouseButtonDown(0))
            {
                if (isFirstClick && Time.time - lastClickTime < doubleClickTimeLimit)
                {
                    // Detected a double click
                    OnDoubleClick();
                    isFirstClick = false; // Reset for the next double click detection
                }
                else
                {
                    // Detected a single click, start waiting for the second click
                    isFirstClick = true;
                    lastClickTime = Time.time;
                }
            }

            // Reset if the time limit has passed without a second click
            if (isFirstClick && Time.time - lastClickTime >= doubleClickTimeLimit)
            {
                isFirstClick = false;
            }
        }
    }

    void OnDoubleClick()
    {
        Debug.Log("Double click detected!");
        // Simulate pressing the "Function" axis
        StartCoroutine(SimulateFunctionAxisPress());
    }

    IEnumerator SimulateFunctionAxisPress()
    {
        // Simulate pressing the "Function" key
        // You can call any method or trigger any action you need here
        Debug.Log("Function axis action triggered by double click!");

        // Call the function that you normally call when "Function" is pressed
        FunctionAction();

        // Wait for one frame to simulate the duration of the key press
        yield return null;
        functionClicked = false;
        // Set the axis to 0 after a while (this part is conceptual as you cannot directly set the axis value in Unity)
        Debug.Log("Function axis reset to 0.");
    }

    void FunctionAction()
    {
        functionClicked = true;
    }
}
