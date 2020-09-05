using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 20.0f;
    [SerializeField]
    private GameObject explosionPrefab;
    private SpawnManager _spawnManger;
   
    // Start is called before the first frame update
    void Start()
    {
        _spawnManger = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        

        
    }
    


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManger.StartSpawning();
            
            Destroy(this.gameObject,0.25f);
            
        }
    }
}
