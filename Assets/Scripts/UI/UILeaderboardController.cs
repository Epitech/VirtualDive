using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILeaderboardController : MonoBehaviour {

    public GameObject scoreEntryTemplate;
    public GameObject scoreEntries;
    public GameObject scoreLoading;

    public GameObject worldsList;
    public GameObject typesList;

    public GameController linkedGameController;

    public int selectedWorld;
    public float yOffset;
    public int worldsCount;
    public LeaderboardFilterType selectedType;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LeaderboardsInit()
    {
        GameObject bp = worldsList.transform.Find("ValuesTpl").gameObject;
        int count = 0;

        foreach (WorldGenerationPossibility wd in linkedGameController.generator.worlds)
        {
            GameObject newObj = Instantiate(bp);

            newObj.name = count.ToString();
            newObj.GetComponent<Text>().text = wd.blueprint.name;
            if (count == 0)
                newObj.SetActive(true);
            else
                newObj.SetActive(false);
            newObj.transform.SetParent(worldsList.transform);
            newObj.transform.position = bp.transform.position;
            newObj.transform.rotation = bp.transform.rotation;
            newObj.transform.localScale = bp.transform.localScale;
            ++count;
        }
        worldsCount = count;
        Destroy(bp);
    }
    
    public void Show()
    {
        UpdateAllLeaderboardsUI();
    }

    public void UpdateAllLeaderboardsUI()
    {
        UpdateLeaderboardsUI(worldsList.transform, selectedWorld);
        UpdateLeaderboardsUI(typesList.transform, (int)selectedType);
    }

    public void OnWorldChange()
    {
        selectedWorld++;
        if (selectedWorld >= worldsCount)
            selectedWorld = 0;
        UpdateLeaderboardsUI(worldsList.transform, selectedWorld);
        linkedGameController.RefreshLeaderboardsData(selectedWorld, selectedType);
    }

    public void OnTypeChange()
    {
        selectedType++;
        if (selectedType >= LeaderboardFilterType.MAX)
            selectedType = 0;
        UpdateLeaderboardsUI(typesList.transform, (int)selectedType);
        linkedGameController.RefreshLeaderboardsData(selectedWorld, selectedType);
    }

    public void ShowScores()
    {
        scoreEntries.SetActive(true);
        scoreLoading.SetActive(false);
    }

    public void SetAsLoading()
    {
        scoreLoading.SetActive(true);
        scoreEntries.SetActive(false);
    }

    public void ClearScoresList()
    {
        foreach (Transform child in scoreEntries.transform)
        {
            Destroy(child.gameObject);
        }
        yOffset = 0.0f;
    }

    public void PushScore(int rank, string user, float speed, float time)
    {
        GameObject newEntry = Instantiate(scoreEntryTemplate);

        newEntry.transform.SetParent(scoreEntries.transform);
        newEntry.transform.position = scoreEntryTemplate.transform.position + new Vector3(0.0f, yOffset, 0.0f);
        newEntry.transform.rotation = scoreEntryTemplate.transform.rotation;
        newEntry.transform.localScale = scoreEntryTemplate.transform.localScale;
        if (rank != -1)
        {
            newEntry.transform.Find("Rank").GetComponent<Text>().text = rank.ToString() + ".";
            newEntry.transform.Find("User").GetComponent<Text>().text = user;
            newEntry.transform.Find("Speed").GetComponent<Text>().text = speed.ToString() + " MPH";
            newEntry.transform.Find("Time").GetComponent<Text>().text = time.ToString();
        }
        else
        {
            newEntry.transform.Find("Rank").GetComponent<Text>().text = "";
            newEntry.transform.Find("User").GetComponent<Text>().text = user;
            newEntry.transform.Find("Speed").GetComponent<Text>().text = "";
            newEntry.transform.Find("Time").GetComponent<Text>().text = "";
        }
        newEntry.SetActive(true);
        yOffset += 20.0f;
    }

    void UpdateLeaderboardsUI(Transform values, int val)
    {
        foreach (Transform tr in values)
        {
            if (int.Parse(tr.name) == val)
            {
                tr.gameObject.SetActive(true);
            }
            else
            {
                tr.gameObject.SetActive(false);
            }
        }
    }
}

