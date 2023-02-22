using System.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float x = 0f;
	public float y = 0f;
	public float z = 0f;
	void OnEnable()
    {
		InvokeRepeating("Rotate", 0f, 0.0167f);
	}

	private void Start()
	{
		CancelInvoke();
		InvokeRepeating("Rotate", 0f, 0.0167f);
	}
	private void Update()
	{
		transform.Rotate(x, y * Time.deltaTime, z);
	}
	void OnDisable()
    {
		CancelInvoke();
	}
	void Rotate()
    {
		this.transform.localEulerAngles += new Vector3(x,y,z);
	}
}
