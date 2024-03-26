using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTest : MonoBehaviour
{
    public GameObject circleObject;

    public void InstantObjects()
    {
        for (int i = 0; i < 1000; i++)
        {
            Instantiate(circleObject, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
        }
    }
}
