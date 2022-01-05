using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public GameObject[] Asteroids;
    public int X = 5;
    public int Y = 5;
    public float spacing = 20;
    public int seed = 1;
    public float randomOffset = 10;
    public float noiseScale = 10;
    public Vector2 noiseOffset = new Vector2(100, 100);

    public void Generate()
    {
        Random.InitState(seed);
        foreach (Transform child in transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
        for (float x = -X; x < X; x++)
        {
            for (float y = -Y; y < Y; y++)
            {
                float size = Mathf.Pow(Mathf.PerlinNoise(x/noiseScale + noiseOffset.x, y/noiseScale + noiseOffset.y) * 2, 2);
                Vector3 position = new Vector3(x / 2f * spacing + Random.Range(-randomOffset, randomOffset), y / 2f * spacing + Random.Range(-randomOffset, randomOffset)) + transform.position;
                if (size >= 1 && Physics2D.OverlapCircle(position, size*4f) == null)
                {
                    CreateAsteroid(position, size, Random.Range(0f, 360f));
                }
            }
        }
    }

    void CreateAsteroid(Vector3 position, float size, float rotation)
    {
        GameObject Asteroid = Instantiate(Asteroids[Random.Range(0, Asteroids.Length)], position, Quaternion.Euler(0, 0, rotation), transform);
        Asteroid.transform.localScale = new Vector3(size, size, 0);
    }
}
