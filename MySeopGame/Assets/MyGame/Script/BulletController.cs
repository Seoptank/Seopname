using UnityEngine;

public class BulletController : MonoBehaviour
{
    // public GameObject enemy;

    public float speed;
    public float distance;
    public LayerMask isLayer;

    private Animator ani; 

    private int bullet;
    private int maxBullet;
    private int minBullet;

    void Awake()
    {
        ani = GetComponent<Animator>();   
    }

    void Start()
    {
        maxBullet = 0;
        maxBullet = 20;
        minBullet = 0;
        //this.enemy = GameObject.Find("Enemy");


    }

    private void Update()
    {
        if (transform.rotation.y == 0)
            transform.Translate(transform.right * speed * Time.deltaTime);
        else
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);

        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy")
            {
                DestroyBullet();
                print("ИэСп!");
            }
        }

    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }


}

