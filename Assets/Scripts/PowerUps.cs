using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerUpId;//0 = Tripleshot 1 = Speed 2 = Shield 3 = Ammo
    [SerializeField]
    private AudioSource _powerUpAudioSource;
    [SerializeField]
    private AudioClip _powerUpClip;
    // Start is called before the first frame update
    void Start()
    {
        _powerUpAudioSource = GetComponent<AudioSource>();
        if(_powerUpAudioSource == null)
        {
            Debug.LogError("AudioSource on a powerup is NULL");

        }
        else
        {
            _powerUpAudioSource.clip = _powerUpClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _powerUpAudioSource.Play();
            
            Destroy(this.gameObject,0.9f);
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                
                switch (_powerUpId)
                {
                    case 0:
                        
                        player.TripleShotActive();
                        break;

                    case 1:
                        
                        player.SpeedEnabled();
                        break;

                    case 2:
                        
                        player.ShieldsEnabled();
                        player.SetShieldStrength();
                        break;

                    case 3:
                        player.AddAmmoCount();
                        break;

                    default:
                        Debug.Log("Default Called");
                        break;

                           
                }
                
            }
           
        }
    }
}
