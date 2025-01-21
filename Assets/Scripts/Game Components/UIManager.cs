using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _gameOverTxt;
    [SerializeField]
    private TMP_Text _restartTxt;
    [SerializeField]
    private TMP_Text _waveText;

    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _spriteLives;

    private GameManager _gameManager;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverTxt.text = "";
        _restartTxt.gameObject.SetActive(false);
        _waveText.gameObject.SetActive(false); 

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _spriteLives[currentLives];
    }

    public void WaveTextUpdate(int _waveNumber)
    {
        _waveText.gameObject.SetActive(true);
        _waveText.text = "Wave: " + _waveNumber;
        StartCoroutine(WaveTextOff());
    }

    IEnumerator WaveTextOff()
    {
        yield return new WaitForSeconds(3f);
        _waveText.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        StartCoroutine(GameOverFlicker());
        _restartTxt.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverTxt.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverTxt.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

   

}
