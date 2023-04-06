using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float speed;
    public float distance;
    public LayerMask isLayer;

    public GameObject effect;
    public Transform effectPos;
    private void Update()
    {
        if (transform.rotation.y == 0)
            transform.Translate(transform.right * speed * Time.deltaTime);
        else
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);

        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Floor")
            {
                DestroyBullet();
                Instantiate(effect, effectPos.position, transform.rotation);

            }
        }


    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DestroyBullet();
            Instantiate(effect, effectPos.position, transform.rotation);
        }
    }
}

