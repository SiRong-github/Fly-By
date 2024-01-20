using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
{   
    static float timeElapsed = 0;
    private static float score = 0;
    private static float eventsTimer = 0;
    public static float finalScore = 0;
    [SerializeField]
    private Text _time;

    [SerializeField] private float scoreRate = 100f;
    private float currScoreRate;
    [SerializeField] private float scoreAcceleration = 3f;
    [SerializeField] private float EVENT_INTERVAL = 2000;
    [SerializeField] private float currentEventInterval;

    private SceneEvent currentEvent;

    // Start is called before the first frame update
    void Start()
    {
        currentEvent = SceneEvent.Day;
        currentEventInterval = NewEventInterval();
       
        string currScene = SceneManager.GetActiveScene().name;
        if (currScene == "EndMenu") {
            if (finalScore != 0) {
                _time.text = (int) Mathf.Floor(finalScore)+"";
                GameObject.Find("BestTimeText").GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetInt("BestScore").ToString();
            }
        }

        timeElapsed = 0;
        score = 0;
        currScoreRate = scoreRate;
    }

    private float NewEventInterval()
    {
        return Random.Range(0.5f, 1.5f) * EVENT_INTERVAL;
    }

    // Update is called once per frame
    void Update()
    {
        if(finalScore == 0){
            timeElapsed += Time.deltaTime;
            
            currScoreRate += scoreAcceleration * Time.deltaTime * PlaneMovement.SpeedShieldSpeed;
            score += currScoreRate * Time.deltaTime * PlaneMovement.SpeedShieldSpeed;

            eventsTimer += currScoreRate * Time.deltaTime;

            _time.text = Mathf.Floor(score).ToString();

            if (eventsTimer >= currentEventInterval)
            {
                currentEvent = NewEvent();
                ChangeEvent(currentEvent);

                RoadSpawner1.RandomiseSelectedTile();
            }

            Fog.fogCoeff = FogFunction(eventsTimer, currentEventInterval);
        }
    }

    void ChangeEvent(SceneEvent e)
    {
        eventsTimer = 0;
        currentEventInterval = NewEventInterval();

        switch (e)
        {
            case SceneEvent.Day:
                DayTime.day = true;
                Fog.fogEnabled = false;
                break;
            case SceneEvent.Fog:
                DayTime.day = true;
                Fog.fogEnabled = true;
                break;
            case SceneEvent.Night:
                DayTime.day = false;
                Fog.fogEnabled = false;
                break;
        }

        _time.color = e == SceneEvent.Night ? new Color(0.04f, 0.045f, 0.04f) : Color.white;
    }

    SceneEvent NewEvent()
    {
        if (currentEvent != SceneEvent.Day)
            return SceneEvent.Day;
        return Random.Range(0, 2) < 1 ? SceneEvent.Night : SceneEvent.Fog;
    }

    private float FogFunction(float eventTime, float eventInterval)
    {
        float transition = eventTime / scoreRate;
        if (transition < 4)
            return 100 * Mathf.Pow(10, - transition/2);

        float endTransition = eventInterval / scoreRate;
        float endFog = 2 + score/10000f;

        float wayThrough = (transition - 4f) / (endTransition - 4f);

        return Mathf.Exp(-1f * wayThrough * endFog);
    }

    public void ResetTimer() {
        timeElapsed = 0;
        score = 0;
        _time.text = "0";
        currScoreRate = scoreRate;
        finalScore = 0;
        eventsTimer = 0;
        currentEvent = SceneEvent.Day;
    }
    public static void PauseTimer() {
        finalScore = score;
    }

    public static float Score { get { return score; } }


}

enum SceneEvent
{
    Day,
    Fog,
    Night
}