using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject[] Backgrounds;  // 2 copies per layer
    [SerializeField] private float[] scrollingSpeed;    // one speed per background

    private float[] backgroundWidths;

    void Start()
    {
        backgroundWidths = new float[Backgrounds.Length];

        for (int i = 0; i < Backgrounds.Length; i += 2) // each layer has 2 copies
        {
            SpriteRenderer sr = Backgrounds[i].GetComponent<SpriteRenderer>();
            float width = sr.bounds.size.x;
            backgroundWidths[i] = width;
            backgroundWidths[i + 1] = width;

            // Position the second copy exactly next to the first
            Backgrounds[i].transform.position = new Vector3(0, Backgrounds[i].transform.position.y, Backgrounds[i].transform.position.z);
            Backgrounds[i + 1].transform.position = new Vector3(width, Backgrounds[i].transform.position.y, Backgrounds[i].transform.position.z);
        }
    }


    void Update()
    {
        for (int i = 0; i < Backgrounds.Length; i++)
        {
            float width = backgroundWidths[i];
            float speed = scrollingSpeed[i / 2];

            Backgrounds[i].transform.position += Vector3.left * speed * Time.deltaTime;

            if (Backgrounds[i].transform.position.x <= -width)
            {
                Backgrounds[i].transform.position += new Vector3(width * 2f, 0f, 0f);
            }
        }
    }

}

