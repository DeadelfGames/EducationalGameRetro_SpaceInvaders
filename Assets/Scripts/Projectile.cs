using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 directionBullet;
    public float speedBullet;

    public System.Action destroyed;

    private void Update()
    {
        transform.position += directionBullet * speedBullet * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (destroyed != null)
        {
            destroyed.Invoke();
        }
        Destroy(this.gameObject);
    }
}
