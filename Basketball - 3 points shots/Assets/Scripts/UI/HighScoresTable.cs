using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class HighScoresTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    [SerializeField] private Text[] PlayersScoreArray;

    private const int numberOfScoresInTheTable = 10;


    private void sortByScore()
    {
        Text temp;
        int currentPlayersScore;
        int nextPlayerScore;
        for(int i = 0; i < numberOfScoresInTheTable - 1; i++)
        {
            for(int j = 0; j < numberOfScoresInTheTable - i - 1; j++)
            {
                currentPlayersScore = int.Parse(PlayersScoreArray[j].ToString());
                nextPlayerScore = int.Parse(PlayersScoreArray[j + 1].ToString());
                if (currentPlayersScore < nextPlayerScore)
                {
                    temp = PlayersScoreArray[j];
                    PlayersScoreArray[j] = PlayersScoreArray[j + 1];
                    PlayersScoreArray[j + 1] = PlayersScoreArray[j];
                }
            }
        }
    }

    public void UpdateHighScoresTable(int newScore)
    {
        int lastScore = int.Parse(PlayersScoreArray[numberOfScoresInTheTable - 1].ToString());
        if (newScore > lastScore)
        {
            PlayersScoreArray[numberOfScoresInTheTable - 1].text = lastScore.ToString();
            sortByScore();
        }
    }
}
