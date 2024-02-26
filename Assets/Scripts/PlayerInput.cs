using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool isMove = false;
    public float maxSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            return;
        }
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var movementVector = new Vector2(x, y);
        if (movementVector != Vector2.zero)
        {
            Debug.Log($"v: {movementVector}");
            Debug.Log($"v normilized: {movementVector.normalized}");
            if (x != 0)
            {
                x = 1;
                y = 0;
            }
            else
            {
                y = 1;
            }

            isMove = true;
            StartCoroutine(MoveSmooth(x , y ));
        }

    }
    

    private IEnumerator MoveSmooth(float x, float y)
    {
        yield return new WaitForSeconds(1/3f);
        transform.position += new Vector3(x * maxSpeed, y * maxSpeed, 0);
        isMove = false;
    }

}
