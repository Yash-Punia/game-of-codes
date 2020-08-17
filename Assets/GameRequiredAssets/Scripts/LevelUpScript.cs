using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LevelUpScript : MonoBehaviour
{
    private Animator levelUpPlayer;
    public int playerLevel = 0;
    [SerializeField] TextMeshProUGUI playerLevelText;
   
    void Start()
    {
        levelUpPlayer = GetComponent<Animator>();
    }
    public void PlayerLevelUp()
    {
        levelUpPlayer.SetBool("isLevelUp",true);
        levelUpPlayer.Play("Trophy");
        playerLevel += 1;
        playerLevelText.text = playerLevel.ToString();
        StartCoroutine(setBoolFalse());
    }
    IEnumerator setBoolFalse()
    {
        yield return new WaitForSeconds(3f);
        levelUpPlayer.SetBool("isLevelUp", false);
    }


}
