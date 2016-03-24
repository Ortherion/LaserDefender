using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float speed = 15.0f;
    public float padding = 1f;
    public GameObject projectile;
    public float projectileSpeed = 0;
    public float firingRate = 0.2f;
    public float health = 250f;
    public AudioClip fireLaser;

    float xMin;
    float xMax;

    void Start() {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));

        xMin = leftMost.x + padding;
        xMax = rightMost.x - padding;
    }
    // Update is called once per frame
    void Update() {
        PlayerMovement();
        LaunchProjectile();
    }

    void PlayerMovement() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            //transform.position += new Vector3(-speed * Time.deltaTime, 0f, 0f);
            transform.position += Vector3.left * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            //transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        // restrict the player to the gamespace.
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// Creates a projectile using the position of the Player and moves the projectile on the Y-axis
    /// depending on the set projectile speed.  
    /// </summary>
    void LaunchProjectile() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke("Fire");
        }
    }

    void Fire() {
        GameObject beam = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, projectileSpeed, 0f);
        AudioSource.PlayClipAtPoint(fireLaser, transform.position, 2f);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile) {
            Debug.Log("Player Collided With the Missile.");
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0) {
                Die();
            }
        }
    }

    void Die() {
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
        Destroy(gameObject);
    }
}
