using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CubeSpone cubeSpawner;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private GameObject bottom;
    private bool isGameStart = false;
    private int currentScore = 0;

    private AudioSource start;
    public void Awake()
    {
        Screen.SetResolution(1080, 2180, true);

        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true);
        start = GetComponent<AudioSource>();

        Color color = new Color(Random.value, Random.value, Random.value);
        camera.backgroundColor = color;

        Color color2 = new Color(Random.value, Random.value, Random.value);
        color2.a = 125;
        bottom.GetComponent<MeshRenderer>().material.color = color2;
    }
    private IEnumerator Start()
    {
        while (true)
        {
            // Perfect 테스트용
            /*
            if (Input.GetMouseButtonDown(1))
            {
                if (cubeSpawner.CurrentCube != null)
                {
                    cubeSpawner.CurrentCube.transform.position = cubeSpawner.LastCube.position + Vector3.up * 0.1f;
                    cubeSpawner.CurrentCube.Arrangement();
                    currentScore++;
                    uiController.UpdateScore(currentScore);
                }
                cameraController.MoveOneStep();
                cubeSpawner.SpawnCube();
            }
            */
            if (Input.GetMouseButtonDown(0))
            {
                if (isGameStart == false)
                {
                    isGameStart = true;
                    start.Play();
                    uiController.GameStart();
                }

                if (cubeSpawner.CurrentCube != null)
                {
                    bool isGameOver = cubeSpawner.CurrentCube.Arrangement();
                    if(isGameOver == true)
                    {
                        cameraController.GameOverAnimation(cubeSpawner.LastCube.position.y, OnGameOver);

                        yield break;
                    }
                    currentScore++;
                    uiController.UpdateScore(currentScore);
                }
                cameraController.MoveOneStep();
                cubeSpawner.SpawnCube();
            }
            yield return null;
        }
    }

    private void OnGameOver()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            uiController.GameOver(true);
        }
        else
        {
            uiController.GameOver(false);
        }

    }

    /*
    private IEnumerator AfterGameOver()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {

                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }

            yield return null;
        }
    }
    */
}
