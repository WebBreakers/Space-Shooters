using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.8f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Vector3 offset;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private bool _isSpeedEnabled = false;
    [SerializeField]
    private bool _isShieldEnabled = false;
    [SerializeField]
    private GameObject _shield;//child object of player that holds animation and activates and deactivates
    [SerializeField]
    private int _score;
    private UIManager _uiManger;
    [SerializeField]
    private GameObject _leftEngineFire;
    [SerializeField]
    private GameObject _rightEngineFire;
    private Animator _leftEngineAnim;
    private Animator _rightEngineAnim;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
    private float _thruster;
    [SerializeField]
    private int _shieldStrength;


    // Start is called before the first frame update
    void Start()
    {
       
        _score = 0;
        _shield.SetActive(false);
        _shieldStrength = 3;
        _leftEngineFire.SetActive(false);
        _rightEngineFire.SetActive(false);
        _leftEngineAnim = _leftEngineFire.GetComponent<Animator>();
        _rightEngineAnim = _rightEngineFire.GetComponent<Animator>();
        _uiManger = GameObject.Find("Canvas").GetComponent<UIManager>();
        _thruster = 3f;
        _audioSource = GetComponent<AudioSource>();
        
        
        //make the player starting position 0 0 0
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("SPAWN MANAGER IS NULL");
        }

        if(_uiManger == null)
        {
            Debug.LogError("UI MANAGER IS NULL");
        }

        if(_leftEngineFire == null)
        {
            Debug.LogError("LeftFire is NULL");
        }

        if(_rightEngineFire == null)
        {
            Debug.LogError("Right Engine is NULL");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audiosource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

       


    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        
        //key code plus cool down system
        if (Input.GetKeyDown(KeyCode.Space)&& Time.time > _canFire)
        {
            FireLaser();
        }
       
    }


    void CalculateMovement()
    {
        //Get horizontal and vertical axes.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //Move player by speed, time variable, horizontal and vertical inputs

        if(_isSpeedEnabled == true)
        {
            _speed = 10.0f;
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        }

        else
        {
            _speed = 5.0f;
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        }
        
        if(Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (_speed * _thruster) * Time.deltaTime);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9, 9), Mathf.Clamp(transform.position.y, -3.7f, 0),0);     
      
     
    }

     void FireLaser()
    {
        offset = new Vector3(0, 1.05f, 0);
        _canFire = Time.time + _fireRate;
        if(Input.GetKeyDown(KeyCode.Space) && isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + offset, Quaternion.identity);
            

        }
        else
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            
        }

        _audioSource.Play();
        
        
    }

   public void Damage()
    {
        //if shields is active do nothing
        //deactivate shields
        //return
        if(_isShieldEnabled == true)
        {
            DamageShieldStrength();
            if(_shieldStrength < 1)
            {
                _isShieldEnabled = false;
                _shield.SetActive(false);
            }
           
            return;
        }

        
        _lives -= 1;
        _uiManger.UpdateLives(_lives);

        if(_lives == 2)
        {
            
            _leftEngineFire.SetActive(true);
            _leftEngineAnim.SetTrigger("LeftEngineTrigger");
        }
        else if(_lives == 1)
        {
            
            _rightEngineFire.SetActive(true);
            _rightEngineAnim.SetTrigger("RightEngineTrigger");
        }

        if (_lives < 1)
        {
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
            
        }
    }

   public void TripleShotActive()
    {
        isTripleShotActive = true;
        //start coroutine to powerdown triple shot in five seconds
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        
        yield return new WaitForSeconds(5.0f);
        isTripleShotActive = false;
    }

    public void SpeedEnabled()
    {
        _isSpeedEnabled = true;
        StartCoroutine(SpeedPowerUpRoutine());
    }

    IEnumerator SpeedPowerUpRoutine()
    {
        
        yield return new WaitForSeconds(8.0f);
        _isSpeedEnabled = false;
    }

    public void ShieldsEnabled()
    {
        _isShieldEnabled = true;
        _shield.SetActive(true);
        _shield.GetComponent<SpriteRenderer>().material.color = Color.red;
       
       
        
    }
    public void AddtoScore(int points)
    {
        _score += points;
        _uiManger.UpdateScore(_score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Damage();
            Destroy(other.gameObject);
        }
    }

    public void DamageShieldStrength()
    {
        _shieldStrength -= 1;
    }

    public void SetShieldStrength()
    {
        _shieldStrength = 3;
    }

}
