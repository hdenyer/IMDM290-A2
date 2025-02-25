using UnityEngine;
using System.Collections;

public class HeartGenerator : MonoBehaviour
{
    public int points = 1000; // More points for smoother heart
    public float scale = 25f; // Overall heart size
    public float baseSize = 5f; // Diameter of each sphere
    public float maxSize = 20f;
    public float pulseSpeed = 20f; // Speed of the pulse effect
    public float pulseFrequency = 2f; // Controls how quickly it spreads downward

    public float pRed = 1f;
    public float pGre = 0.6f;
    public float pBlu = 0.8f;

    public float rRed = 1f;
    public float rGre = 0f;
    public float rBlu = 0f;

    private GameObject[] spheres; // Store spheres for updating animations
    private float[] distances; // Store distances from the top crest

    void Start()
    {
        spheres = new GameObject[points]; // Initialize array
        distances = new float[points]; // Store distances for animation control
        GenerateHeart();
    }

    void Update()
    {
        AnimateHeart();
    }

    void GenerateHeart()
    {
        int valleyIndex = 0; // Assume the first point is the valley

        for (int i = 0; i < points; i++)
        {
            float t = Mathf.PI * 2 * i / points; // Spread points evenly
            Vector3 position = GetHeartPosition(t) * scale;

            // Create the sphere
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].transform.position = position;
            spheres[i].transform.localScale = Vector3.one * baseSize;
        }

        // Compute distances outward from the valley
        for (int i = 0; i < points; i++)
        {
            distances[i] = Mathf.Abs(i - valleyIndex); // Distance from valley
            if (distances[i] > points / 2) distances[i] = points - distances[i]; // Wrap around
        }
    }

    void AnimateHeart()
    {
        float timeOffset = Time.time * pulseSpeed; // Controls animation flow

        for (int i = 0; i < points; i++)
        {
            // Compute pulsing effect outward from top crest
            float waveProgress = Mathf.Sin((distances[i] / (points / 2) * Mathf.PI * pulseFrequency) - timeOffset);
            waveProgress = Mathf.Clamp01((waveProgress + 1) / 2); // Normalize to 0-1

            // Adjust size based on wave
            float newSize = Mathf.Lerp(baseSize, maxSize, waveProgress);
            spheres[i].transform.localScale = Vector3.one * newSize;

            // Adjust color (Pink to Red to Pink)
            Color sphereColor = Color.Lerp(new Color(pRed, pGre, pBlu), new Color(rRed, rGre, rBlu), waveProgress);
            spheres[i].GetComponent<Renderer>().material.color = sphereColor;
        }
    }

    Vector3 GetHeartPosition(float t)
    {
        float x = Mathf.Sqrt(2) * Mathf.Pow(Mathf.Sin(t), 3);
        float y = -(Mathf.Pow(Mathf.Cos(t), 3)) - (Mathf.Pow(Mathf.Cos(t), 2)) + 2 * Mathf.Cos(t);
        return new Vector3(x, y, 0);
    }
}
