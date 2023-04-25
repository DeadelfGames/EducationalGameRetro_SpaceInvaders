using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public float intervalInvader = 2f;
    public AnimationCurve speedInvader;
    public Projectile roketPrefab;
    public float roketAttackRate = 1f;

    public int amountKilled { get; private set; }
    public int amountAlive => totalInvaders - amountKilled;
    public int totalInvaders => rows * columns;
    public float percentKilled => (float)amountKilled / totalInvaders;

    private Vector3 _direction = Vector2.right;

    private void Awake()
    {
        for (int row = 0; row < rows; row++)
        {
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);

            Vector2 centring = new Vector2(-width / 2f, -height / 2f);
            Vector3 rowPosition = new Vector3(centring.x, centring.y + (row * intervalInvader), 0f);

            for (int col = 0; col < columns; col++)
            {
                Invader invader = Instantiate(prefabs[row], transform);
                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;
                position.x += col * intervalInvader;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating("RoketAttack", roketAttackRate, roketAttackRate);
    }

    private void Update()
    {
        transform.position += _direction * speedInvader.Evaluate(percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvancedRow();
            }
            else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvancedRow();
            }
        }
    }

    private void AdvancedRow()
    {
        _direction.x *= -1f;
        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }

    private void RoketAttack()
    {
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1.0f / (float)amountAlive))
            {
                Instantiate(roketPrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void InvaderKilled()
    {
        amountKilled++;

        if (amountKilled >= totalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
