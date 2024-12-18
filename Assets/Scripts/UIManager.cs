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
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _spriteLives;

    private bool _isGameOver;


    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverTxt.text = "";
        _restartTxt.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isGameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _spriteLives[currentLives];

    }

    public void GameOver()
    {
        StartCoroutine(GameOverFlicker());
        _restartTxt.gameObject.SetActive(true);
        _isGameOver = true;


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
