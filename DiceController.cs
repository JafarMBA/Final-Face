using UnityEngine;
using System.Collections;
using UnityEngine.TextCore;
using System.Collections.Generic;
using System.Linq;

public class DiceRotator : MonoBehaviour
{
    public int speed = 300; // Speed of rotation
    private bool isMoving = false; // Flag to check if the dice is currently moving
    public GameObject[] faces; // {Top, Front, Right, Left, Back, Bottom}

    public Vector2 rot3DOrigin;
    private int top;

    void Update()
    {
        // Check if the dice is already moving
        if (isMoving)
        {
            return;
        }

        // Check for user input and start the corresponding rotation coroutine

        if (Input.GetAxisRaw("Horizontal") == 1){
            StartCoroutine(Roll(Vector3.right));
        }else if(Input.GetAxisRaw("Horizontal") == -1){
            StartCoroutine(Roll(Vector3.left));
        }else if(Input.GetAxisRaw("Vertical") == 1){
            StartCoroutine(Roll(Vector3.forward));
        }else if (Input.GetAxisRaw("Vertical") == -1){
            StartCoroutine(Roll(Vector3.back));
        }
    }

    IEnumerator Roll(Vector3 direction)
    {
        isMoving = true; // Set the flag to true to indicate the dice is moving

        float remainingAngle = 90; // Total angle to rotate
        
        Vector3 rotationCenter = transform.position + direction / rot3DOrigin.x + Vector3.down / rot3DOrigin.y; // Calculate the rotation center
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction); // Calculate the rotation axis

        // Rotate the dice gradually
        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle); // Calculate the rotation angle for this frame
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle); // Rotate the dice
            remainingAngle -= rotationAngle; // Update the remaining angle
            yield return null; // Wait until the next frame
        }

        List<int> Ypos = new List<int>();

        for (int i = 0; i < faces.Length; i++)
        {
            Ypos.Add((int)(faces[i].transform.position.y*100));
        }
        
        GetMaxElement(Ypos.ToArray(), out top);
        print(top+1);

        isMoving = false; // Set the flag to false once the rotation is complete
    }
    private void OnDrawGizmos() {
        
        Gizmos.DrawSphere(transform.position + Vector3.left / rot3DOrigin.x + Vector3.down / rot3DOrigin.y, 0.01f);
    }

    public int GetMaxElement(int[] array, out int index)
{
	if (array==null) 
	{
		index = -1;
		return int.MinValue;
	}
	int max = array.Max();
	index = System.Array.FindIndex(array, x=>x==max);
	return max;
}

}