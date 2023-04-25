using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Projectile lazerPrefab;
    public float speedHero = 5f;

    private bool _isActiveLazer;
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speedHero * Time.deltaTime; ;
        Vector2 offset = new Vector2(horizontal, 0f);
        transform.Translate(offset);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!_isActiveLazer)
        {
            Projectile projectile = Instantiate(lazerPrefab, transform.position, Quaternion.identity);
            projectile.destroyed += LazerDestroyed;
            _isActiveLazer = true;
        }
    }

    private void LazerDestroyed()
    {
        _isActiveLazer = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invaders") ||
            other.gameObject.layer == LayerMask.NameToLayer("Roket"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
