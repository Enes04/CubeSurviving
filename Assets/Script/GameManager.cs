using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public GameObject bulletPrefab;
    public GameObject bulletParent;
    public GameObject[] crsytal;
    public int minX, minZ, maxX,maxZ;
    public GameObject crsytalMainParent;
    private void Awake()
    {
        for (int i = 0; i < 80; i++)
        {
            GameObject current = Instantiate(crsytal[Random.Range(0, 2)],crsytalMainParent.transform);
            current.transform.position = new Vector3(Random.Range(minX,maxX),0,Random.Range(minZ,maxZ));
            current.transform.rotation = new Quaternion(0f, Random.Range(-180f, 180f), 0f,0);
        }
        
    }
}
