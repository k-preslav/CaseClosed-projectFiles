using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    public List<CardData> discoveredCards = new List<CardData>();
    public GameObject cardPrefab;

    [HideInInspector] public List<InterviewData> completedInterviews = new List<InterviewData>();

    [HideInInspector] public CardData selectedCardData = null;
    [HideInInspector] public Card convictedCard = null;

    [HideInInspector] public float caseTime = 600f; // in seconds

    [HideInInspector] public OnScreenLog ScreenLog;
    [HideInInspector] public SceneTransitionUI SceneTransitionUI;

    [HideInInspector] public CustomCursor Cursor;

    private void Awake()
    {
        //QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        SceneTransitionUI = FindAnyObjectByType<SceneTransitionUI>();
        ScreenLog = FindAnyObjectByType<OnScreenLog>();
        Cursor = FindAnyObjectByType<CustomCursor>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Update()
    {
        if (caseTime > 0)
            caseTime -= Time.deltaTime;
    }

    public void DiscoverCard(CardData card)
    {
        if (card != null && !discoveredCards.Contains(card))
        {
            // unity scriprable object bullshit
            card.positionOnBoard = new Vector2(1000, 1000);

            discoveredCards.Add(card);
            ScreenLog.Log("Discovered new card");
        }

    }
    public bool IsCardDiscovered(CardData card)
    {
        return discoveredCards.Contains(card);
    }

    public async void GoToLocation(string locationSceneName)
    {
        SceneTransitionUI.FadeIn();

        await Task.Delay(60);
        await SceneManager.LoadSceneAsync(locationSceneName);
    }
    public async void GoToBoard()
    {
        SceneTransitionUI.FadeIn();

        await Task.Delay(60);
        await SceneManager.LoadSceneAsync("Board");
    }

    public async void GoToInterview()
    {
        SceneTransitionUI.FadeIn();

        await Task.Delay(60);
        await SceneManager.LoadSceneAsync("Interview");
    }
}
