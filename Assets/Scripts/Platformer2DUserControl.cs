using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (PlayerScript))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlayerScript m_Character;
    private bool m_Jump;


    private void Awake()
    {
        m_Character = GetComponent<PlayerScript>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}
	
	
	private void FixedUpdate()
    {
		bool crouch = false;
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
        // Read the inputs.
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey (KeyCode.LeftShift)){
			Transform crouchCheck = transform.Find ("CrouchCheck");
			if (!Physics2D.OverlapPoint(crouchCheck.position)){
				crouch = true;
			}
		}
        //bool crouch = Input.GetKey(KeyCode.LeftShift);
        // Pass all parameters to the character control script.
        m_Character.Move(h, crouch, m_Jump);
        m_Jump = false;
    }
}
