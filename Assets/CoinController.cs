using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private int coins;

    // Start is called before the first frame update
    void Start()
    {
        coins = 300;
    }

    // Update is called once per frame
    void Update()
    {
        //coinText.SetText(coins.ToString());
    }

    public void IncreaseCoins(int amount)
    {
        coins += amount;
    }

    public void DecreaseCoins(int amount)
    {
        coins -= amount;
    }

    public int GetCoins()
    {
        return coins;
    }
}
