using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
    public float health = 150f;
    public GameObject projectile;
    public float projectileSpeed = 10f;
    public float firingRate = 0.2f;
    public float shotsPerSecond = 0.5f;

    void Update() {
        float probability = Time.deltaTime * shotsPerSecond;
        if (Random.value < probability) {
            EnemyFire();
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile) {
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0) {
                Destroy(gameObject);
            }
        }
    }

    void EnemyFire() {
        Vector3 startPosition = transform.position + new Vector3(0.0f, -1f, 0f);
        GameObject enemyMissile = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
        enemyMissile.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        enemyMissile.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, -projectileSpeed, 0f);
    }
}
