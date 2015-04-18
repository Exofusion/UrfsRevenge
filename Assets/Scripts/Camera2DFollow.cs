using System;
using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

	private Vector3 offset;

	private float m_StartingY;

    // Use this for initialization
    private void Start()
    {
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
		offset = new Vector3 (2f, 0);
		m_StartingY = transform.position.y;
		m_LastTargetPosition = target.position+offset;
    }


    // Update is called once per frame
    private void Update()
    {
		Vector3 updatedTarget = target.position;

		if (!LevelManager.finalBoss)
			updatedTarget += offset;

        // only update lookahead pos if accelerating or changed direction
		//float xMoveDelta = (target.position - m_LastTargetPosition).x;
		float xMoveDelta = (updatedTarget - m_LastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
        }
        else
        {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
        }

		//Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
		Vector3 aheadTargetPos = updatedTarget + m_LookAheadPos + Vector3.forward*m_OffsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

		// Do not move the camera in the Y-axis
		newPos.y = m_StartingY;
        transform.position = newPos;

        //m_LastTargetPosition = target.position;
		m_LastTargetPosition = updatedTarget;
	}
}
