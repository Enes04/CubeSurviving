using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GunManager _gunManager;
    public float speed;
    private bool _fire;
    
    void Start()
    {
        _gunManager = FindObjectOfType<GunManager>();
    }

    void Update()
    {
        if (_fire)
        {
            transform.Translate(Vector3.forward * speed*Time.deltaTime);
            if (Vector3.Distance(_gunManager.transform.position, transform.position) > 100)
            {
                _fire = false;
                _gunManager.BulletPoolAdd(gameObject);
                gameObject.SetActive(false);
            }
        }
    }
    
    public void Shot()
    {
        _fire = true;
    }
}