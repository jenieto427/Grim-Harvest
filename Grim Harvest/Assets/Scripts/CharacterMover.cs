using UnityEngine;

public class RigidController : MonoBehaviour
{
	public float speed = 5;
	private Rigidbody _rigidbody;

	// Start is called before the first frame update
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.freezeRotation = true;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		var right = Vector3.right * Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;
		var forward = Vector3.forward * Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime;

		_rigidbody.MovePosition(_rigidbody.position + right + forward);
	}

	/*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Stepped on trigger " + other.gameObject);
    }
    */
}