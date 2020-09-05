using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player player;
    private Animator anim;
    [SerializeField]
    private AudioClip _enemyClip;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject enemyLaserPrefab;
    Vector3 offset;
    
    // Start is called before the first frame update

    void Start()
    {
        offset = new Vector3(0, -1.0f, 0);
        _audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
        if(player == null)
        {
            Debug.LogError("Player is NULL");
        }
        

        if (anim == null)
        {
            Debug.LogError("Animator is NULL");
        }
        

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource on enemy is null");
        }
        else
        {
            _audioSource.clip = _enemyClip;
        }

        StartCoroutine(FireLaserRoutine());

       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -6f)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            
            anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
         
            Destroy(this.gameObject, 2.8f);
            _audioSource.Play();

            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
                

            }

            
        }

        if (other.tag == "Laser")
        {
            
            Destroy(other.gameObject);
            if(player != null)
            {
                player.AddtoScore(Random.Range(3, 11));
            }

            
            anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;

            Destroy(GetComponent<BoxCollider2D>());
            Destroy(this.gameObject, 2.8f);
            _audioSource.Play();
            
        }
    }

    IEnumerator FireLaserRoutine()
    {
        while(true)
        {
            
            Instantiate(enemyLaserPrefab, transform.position + offset, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
        
    }
}
