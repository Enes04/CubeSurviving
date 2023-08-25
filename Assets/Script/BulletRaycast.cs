using Script;
using UnityEngine;

public class BulletRaycast : MonoBehaviour
{
    private Bullet _bullet;
    private void Awake()
    {
        _bullet = GetComponentInParent<Bullet>();
    }

    void Update()
    {
        RaycastHit objectHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * 1, Color.green);
        if (Physics.Raycast(transform.position, fwd, out objectHit, 1))
        {
            if (objectHit.collider.CompareTag("Enemy"))
            {
                objectHit.transform.GetComponent<Enemy>().Hit(10,transform.gameObject,5);
                _bullet.DestroyBullet();
            }
        }
    }
}
