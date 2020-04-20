using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float moveTime;
    [SerializeField] Animator anim;



    Rigidbody rb;

    bool moveForward = true;
    float timer;
    int moveDirection = -1;
    bool[] directionsGone = new bool[4];
    

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        timer = moveTime;
        anim.SetFloat("speed", 0.5f);
    }

    private void Update()
    {
        if (moveForward)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                moveForward = false;
                moveDirection = -1;
                timer = moveTime;
                StartCoroutine(Move());
            }
        }
        else {
            if (moveDirection == -1) {
                moveDirection = PickDirection();
            }
        }
    }

    private void FixedUpdate()
    {
        // do some animation stuff later
        if (moveForward)
        {

            switch (moveDirection)
            {
                case 0:
                    // forward
                    rb.MovePosition(transform.position + new Vector3(0f, 0f, speed) * Time.fixedDeltaTime);
                    break;
                case 1:
                    // backward
                    rb.MovePosition(transform.position + new Vector3(0f, 0f, -speed) * Time.fixedDeltaTime);
                    break;
                case 2:
                    // right
                    rb.MovePosition(transform.position + new Vector3(speed, 0f, 0f) * Time.fixedDeltaTime);
                    break;
                case 3:
                    // left
                    rb.MovePosition(transform.position + new Vector3(-speed, 0f, 0f) * Time.fixedDeltaTime);
                    break;
            }

        }
    }

    int PickDirection() {
        bool allAreTrue = true;
        for (int i = 0; i < directionsGone.Length; i++) {
            if (!directionsGone[i])
            {
                allAreTrue = false;
                break;
            }
        }

        if (allAreTrue)
        {
            // make them all false and senda random in
            for (int i = 0; i < directionsGone.Length; i++)
            {
                directionsGone[i] = false;
            }

            return Random.Range(0, 4);
        }
        else {
            List<int> list = new List<int>();

            for (int i = 0; i < directionsGone.Length; i++) {
                if (!directionsGone[i]) {
                    list.Add(i);
                }
            }
            int choice = Random.Range(0, list.Count);
            directionsGone[list[choice]] = true;

            return list[choice];
        }

    }

    IEnumerator Move() {
        yield return new WaitForSeconds(4f);
        moveForward = true;
    }
}
