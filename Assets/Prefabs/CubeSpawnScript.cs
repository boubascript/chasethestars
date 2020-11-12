using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnScript : MonoBehaviour
{
    public Transform prefab;
    public Vector3 localOffset;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
            localOffset = transform.position;
            Instantiate(prefab, localOffset, Quaternion.identity);
    }
}
