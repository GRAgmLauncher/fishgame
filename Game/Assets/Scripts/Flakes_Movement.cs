using UnityEngine;
using System.Collections;

public class Flakes_Movement : MonoBehaviour {

	bool swimming;
	bool jumping;
	Vector2 targetPosition;
	Vector3 newTargetPosition;
	Vector2 upperBound;
	Vector2 lowerBound;
	Vector3 camPosition;
	float sinkRate;
	float jumpDistance;
	float jumpSpeed;
	float yVel = 1.0f;
	float SmoothTime = 3.0f;
	float gravity = 4.0f;
	Vector3 velocity;


	// Use this for initialization
	void Start () 
	{
		swimming = false;
		targetPosition = new Vector2(0.0f, 0.0f);
		newTargetPosition = new Vector2(0.0f, 0.0f);
		upperBound = new Vector2(0.0f, 2.75f);
		lowerBound = new Vector2(0.0f, -2.75f);
		velocity = new Vector3(0.0f,0.0f,0.0f);
		transform.position = new Vector2(0.0f, 0.0f);
		camPosition = Camera.main.transform.position;
		sinkRate = 0.015f;
	}
	
	// Update is called once per frame
	void Update() 
	{
		Down();
		SwimUp();
		MoveCamera();
	}

	void MoveCamera()
	{
		if (transform.position.y < Camera.main.transform.position.y - 1.2f) {
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Clamp(transform.position.y + 1.2f, -2.75f, 2.75f), Camera.main.transform.position.z);
		}

		if (transform.position.y > Camera.main.transform.position.y + 0.5f) {
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Camera.main.transform.position.x, Mathf.Clamp(targetPosition.y - 0.5f, -2.75f, 2.75f), Camera.main.transform.position.z), Time.deltaTime * 2.0f);
		}
	}

	void Down()
	{
		if (transform.position.y > -3.7f && swimming == false && jumping == false){
			Sink();
		} else if (jumping == true && swimming == false) {
			Fall();
		}
	}

	void Fall()
	{
		velocity.y -= gravity * Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
		
		if (Vector2.Distance(transform.position, newTargetPosition) < 0.1f) {
			jumping = false;
			velocity.y = 0.0f;
		}
	}

	void Sink()
	{
		Vector2 newPosition = transform.position;
		newPosition.y -= sinkRate;
		transform.position = newPosition;
	}

	void SwimUp()
	{
		if (Input.GetMouseButtonDown(0)) {
			if (transform.position.y < 2.9f){
				if (transform.position.y > 2.2f) {
					jumpDistance = 1.8f;
					jumpSpeed = 5.0f;
					jumping = true;
				} else {
					jumpDistance = 0.8f;
					jumpSpeed = 7.0f;
				}

				swimming = true;
				targetPosition = transform.position;
				targetPosition.y += jumpDistance;
			}
		}

		if (swimming == true) {
			transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * jumpSpeed);

			if (Vector2.Distance(transform.position, targetPosition) < 0.05f) {
				if (jumping == true) {
					newTargetPosition.y = 2.8f;
				}
				swimming = false;
			}
		}
	}
}
