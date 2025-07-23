using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int currentExp = 0;
    public int level = 1;
    public int expToNextLevel = 5;

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log("현재 경험치: " + currentExp);

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            level++;
            expToNextLevel += 5; // 레벨업 점점 증가
            Debug.Log("레벨 업! 현재 레벨: " + level);
        }
    }
}
