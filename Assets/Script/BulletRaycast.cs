using UnityEngine;

public class BulletRaycast : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        RaycastHit objectHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * 1, Color.green);
        if (Physics.Raycast(transform.position, fwd, out objectHit, 1))
        {
            if (objectHit.collider.CompareTag("Enemy"))
            {
                objectHit.transform.GetComponent<Enemy>().Damage(3,objectHit.transform.gameObject);
            }
        }
    }
}
