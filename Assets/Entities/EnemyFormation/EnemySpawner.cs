﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5.0f;

    private bool movingRight = true;
    private float xMin;
    private float xMax;

    // Use this for initialization
    void Start() {
        determineBoundaries();
        generateEnemies();
    }

    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update() {
        enemyMovement();
    }

    /// <summary>
    /// Determines the boundary for enemy formation by taking the distance to the camera and
    /// setting it to the rightmost or leftmost edge of said camera. 
    /// </summary>
    void determineBoundaries() {

        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));

        xMin = leftBoundary.x;
        xMax = rightBoundary.x;
    }

    /// <summary>
    /// Generates an enemy at each child of the enemy formation.
    /// </summary>
    void generateEnemies() {
        foreach (Transform child in transform) {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    /// <summary>
    /// Determines enemy movement. If enemy formation reaches boundary as determined in determineBoundaries
    /// then the formation will change direction.    
    /// </summary>
    void enemyMovement() {
        if (movingRight){
            transform.position += Vector3.right * speed * Time.deltaTime;
        } else {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);
        if (leftEdgeOfFormation < xMin) {
            movingRight = true;
        } else if (rightEdgeOfFormation > xMax) {
            movingRight = false;
        }

        restrictFormationPosition();
    }

    /// <summary>
    /// Restricts field of movement for enemy formation by taking x value of formation
    /// and setting it against the minimum and maximum values for the camera position.    
    /// </summary>
    void restrictFormationPosition() {
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
