using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_HeadController : MonoBehaviour {

    Animator anim;
    [SerializeField] Camera camM;

    [Header("Head")]
    //[SerializeField]

    [SerializeField]
    float mouseSensitivity;
    [SerializeField] Transform rotator;
    Transform target;

    Vector3 rotationV;
    Quaternion globalRot;

    [Header("Limits")]
    [SerializeField]
    float LimitRight = 90;
    [SerializeField] float LimitLeft = 270;
    float LR = 180;

    [SerializeField] float LimitUp = 250;
    [SerializeField] float LimitDown = 120;
    float UD = 180;



    [Header("Weight")]
    [SerializeField]
    float weight;
    [SerializeField] float body;
    [SerializeField] float head;
    [SerializeField] float eyes;
    [SerializeField] float clamp;


    [Header("Move")]
    [SerializeField]
    float rotateSpeed = 1;
    [SerializeField] float moveAcceleration = 10;
    bool moving = false;



    private void Start() {
        anim = GetComponent<Animator>();
        target = rotator.GetChild(0);
    }



    void OnAnimatorIK(int layerIndex) {
        anim.SetBool("TurnActive", false);

        HeadRotate();
        Move();

        anim.SetLookAtWeight(weight, body, head, eyes, clamp);
        anim.SetLookAtPosition(target.position);

        Vector3 cam = camM.transform.rotation.eulerAngles;
        camM.transform.rotation = Quaternion.Euler(cam.x, cam.y, cam.z * 0);
        rotator.transform.position = new Vector3(rotator.transform.position.x, camM.transform.position.y, rotator.transform.position.z);
    }

    private void FixedUpdate() {


    }

    List<float> veloc = new List<float>();
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;

        //Gizmos.DrawLine(rotator.transform.position, target.transform.position);

        veloc.Add(GetComponent<Rigidbody>().velocity.magnitude);

        float i = 0;
        foreach (float v in veloc) {
            i += v;

        }
        i /= veloc.Count;
        //print(i);

        if (veloc.Count > 30) veloc.RemoveAt(0);
    }



    void HeadRotate() {
        Vector3 rot = new Vector3(-Input.GetAxis("Mouse Y") * mouseSensitivity, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
        rotationV = Limit(rotator.transform.localRotation.eulerAngles, globalRot.eulerAngles, rot);

        if (moving && !anim.GetCurrentAnimatorStateInfo(0).IsName("Base.Turn")) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, rotationV.y, 0)), Time.deltaTime * rotateSpeed);
        }


        rotator.transform.rotation = globalRot = Quaternion.Euler(rotationV);

    }


    Vector3 Limit(Vector3 localRot, Vector3 globalRot, Vector3 rot) {

        localRot += rot;

        if (localRot.y > LR && localRot.y < LimitLeft) {
            Turn(-1);
            globalRot.y += LimitLeft - localRot.y;
        }
        else if (localRot.y > LimitRight && localRot.y < LR) {
            Turn(1);
            globalRot.y += LimitRight - localRot.y;
        }

        globalRot.y += rot.y;


        if (localRot.x > UD && localRot.x < LimitUp) {
            //Up limit
        }
        else if (localRot.x > LimitDown && localRot.x < UD) {
            //Down limit
        }
        else {
            globalRot.x = globalRot.x + rot.x;
        }



        return globalRot;
    }

    float h = 0, v = 0, h2, v2;

    void Move() {

        h = Mathf.Lerp(h, Input.GetAxis("Horizontal") / 2 * (Input.GetAxis("Run") + 1), Time.deltaTime * moveAcceleration);
        v = Mathf.Lerp(v, Input.GetAxis("Vertical") / 2 * (Input.GetAxis("Run") + 1), Time.deltaTime * moveAcceleration);

        anim.SetFloat("Speed", v);
        anim.SetFloat("Direction", h);

        bool turn = anim.GetCurrentAnimatorStateInfo(0).IsName("Base.Turn");

        h2 = Mathf.Abs(h);
        v2 = Mathf.Abs(v);

        if (h2 < 0.1f && v2 < 0.1f && moving && !turn) moving = false;
        else if (!moving && (h2 >= 0.1f || v2 >= 0.1f || turn)) moving = true;


    }

    void Turn(float t) {
        if (!moving) {
            moving = true;
            anim.SetFloat("Turn", t);
            anim.SetBool("TurnActive", true);
        }
    }

}
