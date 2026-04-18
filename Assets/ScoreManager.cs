using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI nowScoreUI;
    public int nowScore;
    public TextMeshProUGUI bestScoreUI;
    public int bestScore;
    // Start is called before the first frame update
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreUI.text = "Best Score : " + bestScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
