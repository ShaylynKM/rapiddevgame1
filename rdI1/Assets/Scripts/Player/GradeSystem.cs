using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradeSystem : MonoBehaviour
{
    public int hitCounter = 0;
    public WinMenu winMenu;

    public static Dictionary<string, string> gradeToImage = new Dictionary<string, string>
    {
        { "S", "S_Grade" },
        { "A", "A_Grade" },
        { "B", "B_Grade" },
        { "C", "C_Grade" },
        { "D", "D_Grade" },
        { "F", "F_Grade" }
    };

    public void Start()
    {
        string playerGrade = CalculateGrade();
        winMenu.UpdateImage(playerGrade);
    }

    public void CounterTakeHit()
    {
        hitCounter++;
        Debug.Log($"Hit counter incremented. New value: {hitCounter}");
        string playerGrade = CalculateGrade();
        winMenu.UpdateImage(playerGrade);
    }

    string CalculateGrade()
    {
        int[] gradeThresholds = { 0, 1, 5, 8, 10 }; 
        string[] letterGrades = { "S", "A", "B", "C", "D", "F" };

        for (int i = 0; i < gradeThresholds.Length; i++)
        {
            Debug.Log($"Checking grade {letterGrades[i]} for threshold {gradeThresholds[i]}. Current hits: {hitCounter}");
            if (hitCounter <= gradeThresholds[i])
            {
                Debug.Log($"Selected grade: {letterGrades[i]}");
                return letterGrades[i];
            }
        }
        return letterGrades[letterGrades.Length - 1];
    }
}