using System;
using System.IO;
using ml.ImageBlurrer.Shared;
using UnityEngine;

namespace Assets.Scripts
{
    public class Drawer : MonoBehaviour
    {
        public GameObject Brush;
        public float BrushSize = 0.1f;
        public RenderTexture RenderTexture;
        private ImageManager _imageManager;
        public GameObject ClearObject;

        private void Start()
        {
            _imageManager = new ImageManager();
        }

        private void Update()
        {
            if (!Input.GetMouseButton(0))
                return;

            var ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (ray.collider != null && ray.transform.CompareTag("Paper"))
            {
                var createdObject = Instantiate(Brush, ray.centroid, Quaternion.identity);
                createdObject.transform.parent = ClearObject.transform;
            }
        }

        public void Save()
        {
            var dataPath = $@"{Application.dataPath}\Data\";
            var fileName = "image.jpg";

            RenderTexture.active = RenderTexture;
            var texture = new Texture2D(RenderTexture.width, RenderTexture.height);
            texture.ReadPixels(new Rect(0, 0, RenderTexture.width, RenderTexture.height), 0, 0);
            texture.Apply();

            var data = texture.EncodeToJPG();
            File.WriteAllBytes($@"{dataPath}\{fileName}", data);
            _imageManager.GetImageAndSaveBlurred(fileName, $"edited{fileName}", dataPath);
        }

        public void Clear()
        {
            foreach (Transform child in ClearObject.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}