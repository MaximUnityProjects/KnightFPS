using UnityEngine;

public class LockRotation : MonoBehaviour {

    [SerializeField] bool lockX, lockY, lockZ;
    Vector3 rot;

    void Update () {
        rot = transform.rotation.eulerAngles;
	}

    void LateUpdate() {
        if (!lockX) rot.x = transform.rotation.eulerAngles.x;
        if (!lockY) rot.y = transform.rotation.eulerAngles.y;
        if (!lockZ) rot.z = transform.rotation.eulerAngles.z;
    }
}
