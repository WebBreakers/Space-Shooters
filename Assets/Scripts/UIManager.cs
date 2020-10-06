using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoCount;
    [SerializeField]
    private int _score;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private GameObject _gameOvertxt;
    [SerializeField]
    private GameObject _restartTxt;
    private GameManager _gameManager;
    

    // Start is called before the first frame update
    
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
        _gameOvertxt.gameObject.SetActive(false);
        _scoreText.text = "Score: 0";
        _restartTxt.gameObject.SetActive(false);
        _ammoCount.text = "Ammo: 15";
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    public void UpdateScore( int playerScore)
    {
                    
            _scoreText.text = "Score: " + playerScore;

        
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoCount.text = "Ammo: " + ammoCount;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprite[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
           
        }
            
    }
    IEnumerator GameOverFlckerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOvertxt.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOvertxt.gameObject.SetActive(true);
            
            
        }
                
            
        
    }

    public void GameOverSequence()
    {
        _gameManager.GameOver();         
        _gameOvertxt.gameObject.SetActive(true);
        _restartTxt.gameObject.SetActive(true);
        StartCoroutine(GameOverFlckerRoutine());
        
    }
}

