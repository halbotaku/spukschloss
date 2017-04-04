using UnityEngine;

/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
	/// <summary>
	/// The speed of the ghost (Movement)
	/// </summary>
	public Vector2 speed = new Vector2(1, 1);

	// 2 - Store the movement and the component
	private Vector2 movement;
	private Rigidbody2D rigidbodyComponent;

    // variable to hold a reference to our SpriteRenderer component (Flipping the Sprite)
    private SpriteRenderer mySpriteRenderer;

    // This function is called just one time by Unity the moment the game loads
    private void Awake()
    {
        // get a reference to the SpriteRenderer component on this gameObject (Flipping the Sprite)
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
	{
		// Retrieve axis information (Movement)
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// Movement per direction (Movement)
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);


        // if the variable isn't empty (we have a reference to our SpriteRenderer)
        if (mySpriteRenderer != null)
        {
            //When movement is going to the left
            if (movement.x < 0)
            {
                // flip the sprite
                mySpriteRenderer.flipX = true;
            }
            else if (movement.x > 0)
            {
                // revert the sprite to normal
                mySpriteRenderer.flipX = false;
            }
        }
    }

	void FixedUpdate()
	{
		// Get the component and store the reference (Movement)
		if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

		// Move the game object (Movement)
		rigidbodyComponent.velocity = movement;



        //Code for detecting the Screensize & Stopping the Ghost from movin out of it
        var pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.04f, 0.96f);
        pos.y = Mathf.Clamp(pos.y, 0.07f, 0.93f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

    }
}