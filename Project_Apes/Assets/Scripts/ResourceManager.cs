using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    //make ui to visualize
    public int gold;

    [SerializeField]
    private TextMeshProUGUI goldUI;

    private void Start()
    {
        goldUI.text = gold.ToString() + " Gold";
    }

    public void AddGold(int i)
    {
        gold += i;
        gold = Mathf.Clamp(gold, 0, 10000);

        goldUI.text = gold.ToString() + " Gold";
    }
}
