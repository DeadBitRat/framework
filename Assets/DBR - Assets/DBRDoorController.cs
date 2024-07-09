using UnityEngine;

public class DBRDoorController : MonoBehaviour
{
    [Header("Door State")]
    public bool isLocked;

    [Header("Door Utilities")]
    public string nameOfKey;
    public DBRFunctionDescriptor descriptor; 


    public Sprite openDoor;
    public Sprite ClosedDoor;

    public DescriptorDialogue descriptionWhenClosed;
    public DescriptorDialogue descriptionWhenOpened;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer; 



    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
    }
}
