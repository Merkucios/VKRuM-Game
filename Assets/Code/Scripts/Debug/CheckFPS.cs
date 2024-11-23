using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheckFPS : MonoBehaviour
{
    public int FramesPerSec { get; protected set; }

    [SerializeField] private float frequency = 0.5f;


    public  Text counter;

    private void Start()
    {
        counter.text = "";
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);

            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
            counter.text = "FPS: " + FramesPerSec.ToString();
        }
    }
}