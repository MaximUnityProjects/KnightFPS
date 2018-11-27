using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToTarget : MonoBehaviour {

    [SerializeField] Transform target;
    [SerializeField] bool lockX, lockY, lockZ;

    // Update is called once per frame
    void Update() {
        if (target == null) return;

        var rot = transform.rotation.eulerAngles;
        transform.LookAt(target);

        rot.x = lockX ? rot.x : transform.rotation.eulerAngles.x;
        rot.y = lockY ? rot.y : transform.rotation.eulerAngles.y;
        rot.z = lockZ ? rot.z : transform.rotation.eulerAngles.z;

        transform.rotation = Quaternion.Euler(rot);


    }
}
