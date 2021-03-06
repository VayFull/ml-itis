using System.IO;
using System.Linq;
using ml.ImageBlurrer.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Drawer : MonoBehaviour
    {
        public GameObject Brush;
        private SpriteRenderer _brushSpriteRenderer;
        public RenderTexture RenderTexture;
        private ImageManager _imageManager;
        public GameObject ClearObject;
        public GameObject ToggleGameObject;
        private Toggle _toggle;
        public GameObject PreviewImage;
        private SpriteRenderer _spriteRenderer;
        public GameObject Paper;
        public GameObject SliderGameObject;
        private Slider _slider;
        public GameObject SliderImageGameObject;
        private Image _sliderImage;
        public GameObject SliderBackgroundGameObject;
        private Image _sliderBackgroundImage;
        public GameObject SizeSliderGameObject;
        private Slider _sizeSlider;
        public GameObject SprayToggleGameObject;
        private Toggle _sprayToggle;
        public GameObject ParamsGameObject;
        private Text _paramsText;
        public GameObject ParamsTextGameObject;

        private void Start()
        {
            _imageManager = new ImageManager();
            _brushSpriteRenderer = Brush.GetComponent<SpriteRenderer>();
            _toggle = ToggleGameObject.GetComponent<Toggle>();
            _spriteRenderer = PreviewImage.GetComponent<SpriteRenderer>();
            _slider = SliderGameObject.GetComponent<Slider>();
            _sliderImage = SliderImageGameObject.GetComponent<Image>();
            _sliderBackgroundImage = SliderBackgroundGameObject.GetComponent<Image>();
            _slider.onValueChanged.AddListener(delegate { SliderValueChanged(); });
            _slider.value = 0;
            _sizeSlider = SizeSliderGameObject.GetComponent<Slider>();
            _sizeSlider.onValueChanged.AddListener(delegate { SizeSliderValueChanged(); });
            _sizeSlider.value = 1;
            _sprayToggle = SprayToggleGameObject.GetComponent<Toggle>();
            _paramsText = ParamsGameObject.GetComponent<Text>();

            Color[] colors =
            {
                new Color(1, 0, 0),
                new Color(1, 1, 0),
                new Color(0, 1, 0),
                new Color(0, 1, 1),
                new Color(0, 0, 1),
                new Color(1, 0, 1),
                new Color(1, 0, 0)
            };
            var hueTex = new Texture2D(colors.Length, 1);
            hueTex.SetPixels(colors);
            hueTex.Apply();
            _sliderBackgroundImage.sprite = Sprite.Create(hueTex, new Rect(Vector2.zero, new Vector2(colors.Length, 1)),
                Vector2.one * 0.5f);
        }

        public void SliderValueChanged()
        {
            var color = Color.HSVToRGB(_slider.value, 1, 1);
            _sliderImage.color = color;
            _brushSpriteRenderer.color = color;
        }

        public void SizeSliderValueChanged()
        {
            var sliderValue = _sizeSlider.value;
            Brush.transform.localScale = new Vector3(sliderValue, sliderValue, sliderValue);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene(1);
            }
            
            if (!Input.GetMouseButton(0))
                return;

            var ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (ray.collider != null && ray.transform.CompareTag("Paper"))
            {
                if (_sprayToggle.isOn)
                {
                    var modifiedObjectScale = Brush.transform.localScale;
                    var changeScale = Random.Range(modifiedObjectScale.x - 1.5f, modifiedObjectScale.x - 2f);
                    if (changeScale < 0.05f)
                    {
                        changeScale = 0.05f;
                    }
                    var changedScale = new Vector3(changeScale, changeScale, changeScale);
                    var createdObject = Instantiate(Brush, ray.centroid, Quaternion.identity);
                    createdObject.transform.localScale = changedScale;
                    var deltaX = Random.Range(-0.1f * modifiedObjectScale.x, 0.1f * modifiedObjectScale.x);
                    var deltaY = Random.Range(-0.1f * modifiedObjectScale.x, 0.1f * modifiedObjectScale.x);
                    createdObject.transform.position =
                        new Vector3(ray.centroid.x + deltaX, ray.centroid.y + deltaY);
                    createdObject.transform.parent = ClearObject.transform;
                }
                else
                {
                    var createdObject = Instantiate(Brush, ray.centroid, Quaternion.identity);
                    createdObject.transform.parent = ClearObject.transform;
                }
            }
        }

        public void Save()
        {
            if (_toggle.isOn)
            {
                return;
            }

            var dataPath = $@"{Application.dataPath}\Data\";
            var fileName = "image.jpg";

            RenderTexture.active = RenderTexture;
            var texture = new Texture2D(RenderTexture.width, RenderTexture.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, RenderTexture.width, RenderTexture.height), 0, 0);
            texture.Apply();

            var data = texture.EncodeToJPG();
            File.WriteAllBytes($@"{dataPath}\{fileName}", data);

            if (!string.IsNullOrEmpty(_paramsText.text))
            {
                var text = _paramsText.text;
                var fragments = text
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();

                if (fragments.Count != 5)
                {
                    _imageManager.GetImageAndSaveBlurred(fileName, $"edited{fileName}", dataPath);
                }

                var description = new BlurDescription(fragments[0], fragments[1], fragments[2], fragments[3],
                    fragments[4]);
                
                _imageManager.GetImageAndSaveBlurred(fileName, $"edited{fileName}", dataPath, description);
            }
            else
            {
                _imageManager.GetImageAndSaveBlurred(fileName, $"edited{fileName}", dataPath);
            }
        }

        public void Clear()
        {
            foreach (Transform child in ClearObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void OnValueChanged()
        {
            if (!_toggle.isOn)
            {
                _spriteRenderer.sprite = null;
                Paper.SetActive(true);
                ClearObject.SetActive(true);
                Brush.SetActive(true);
                ParamsTextGameObject.SetActive(true);
            }
            else
            {
                var dataPath = $@"{Application.dataPath}\Data\";
                var fileName = "editedimage.jpg";

                var file = File.ReadAllBytes($@"{dataPath}\{fileName}");

                var texture = new Texture2D(1920, 1080);
                texture.LoadImage(file);
                texture.Apply();

                _spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, 1920, 1080), new Vector2(0, 0));
                Paper.SetActive(false);
                ClearObject.SetActive(false);
                Brush.SetActive(false);
                ParamsTextGameObject.SetActive(false);
            }
        }
    }
}