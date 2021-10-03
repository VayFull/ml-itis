using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using ml.ImageBlurrer.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuperPixel : MonoBehaviour
{
    public GameObject PreviewImage;
    private Image _spriteRenderer;

    public GameObject ResultImage;
    private Image _resultSpriteRenderer;
    
    public InputField CoefField;
    public InputField NumberOfClustersField;

    private SuperPixelManager _superPixelManager;

    private bool _coroutineStarted = false;

    public Text AwaitingText;

    public void Start()
    {
        AwaitingText.gameObject.SetActive(false);
        CoefField.text = "1,2";
        NumberOfClustersField.text = "2";
        _superPixelManager = new SuperPixelManager();

        _spriteRenderer = PreviewImage.GetComponent<Image>();
        _resultSpriteRenderer = ResultImage.GetComponent<Image>();

        var dataPath = $@"{Application.dataPath}\Data\";
        var fileName = "image.jpg";

        var file = File.ReadAllBytes($@"{dataPath}\{fileName}");

        var texture = new Texture2D(1920, 1080);
        texture.LoadImage(file);
        texture.Apply();

        _spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, 1920, 1080), new Vector2(0, 0));
        
        UpdateOutputImage();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void Apply()
    {
        var hasCoef = float.TryParse(CoefField.text, out var coef);
        if (hasCoef && !_coroutineStarted)
        {
            const string imageName = "image.jpg";
            const string editedSuperPixelImageName = "superPixelOutput.jpg";
            var dataPath = $@"{Application.dataPath}\Data\";
            AwaitingText.gameObject.SetActive(true);
            StartCoroutine(GetPixelized(imageName, editedSuperPixelImageName, dataPath, coef, int.Parse(NumberOfClustersField.text)));
        }
    }

    public IEnumerator GetPixelized(string imageName, string editedSuperPixelImageName, string dataPath, float coef,
        int numberOfClusters)
    {
        _coroutineStarted = true;
        yield return new WaitForSeconds(0.1f);
        _superPixelManager.GetImageAndSaveSuperPixelized(imageName, editedSuperPixelImageName, dataPath, coef,
            numberOfClusters);
        _coroutineStarted = false;
        AwaitingText.gameObject.SetActive(false);
        UpdateOutputImage();
        yield return null;
    }

    private void UpdateOutputImage()
    {
        var dataPath = $@"{Application.dataPath}\Data\";
        var fileName = "superPixelOutput.jpg";

        var file = File.ReadAllBytes($@"{dataPath}\{fileName}");

        var texture = new Texture2D(1920, 1080);
        texture.LoadImage(file);
        texture.Apply();

        _resultSpriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, 1920, 1080), new Vector2(0, 0));
    }
}