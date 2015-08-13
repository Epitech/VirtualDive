using UnityEngine;
using UnityEditor;

public class ScoreController
{
    public static float GetScore(float speed, float time, float finalScore, float xp)
    {
        return (finalScore + xp);
    }
}