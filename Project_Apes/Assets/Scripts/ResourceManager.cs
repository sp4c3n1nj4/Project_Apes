using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public enum ResourceType
{
    gold,
    shard,
    ingot
}

[Serializable]
public class Resource
{
    public int amount;
    public ResourceType drop;

    public Resource(ResourceType _drop, int _amount)
    {
        drop = _drop;
        amount = _amount;
    }
}

public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI resourceText;

    public Dictionary<ResourceType, int> Resources;

    private void Awake()
    {
        Resources = Enum.GetValues(typeof(ResourceType))
               .Cast<ResourceType>()
               .ToDictionary(t => t, t => 0);

        foreach (var item in Resources)
        {
            print(item);
        }
    }

    private void Start()
    {
        AddResource(ResourceType.gold, 10);
        UpdateResourceUI();
    }

    public bool TryPayCost(ResourceType r, int amount)
    {
        bool canPay = false;

        if (Resources[r] >= amount)
        {
            canPay = true;
            Resources[r] -= amount;
        }
        UpdateResourceUI();
        return canPay;
    }
    public bool TryPayCost(List<Resource> resources)
    {
        bool canPay = true;

        Resource[] _resources = resources.ToArray();
        for (int i = 0; i < _resources.Length; i++)
        {
            if (Resources[_resources[i].drop] < _resources[i].amount)
            {
                canPay = false;
                return canPay;
            }           
        }

        for (int i = 0; i < _resources.Length; i++)
        {
            Resources[_resources[i].drop] -= _resources[i].amount;
        }
        UpdateResourceUI();
        return canPay;
    }
    public bool TryPayCost(Resource[] resources)
    {
        bool canPay = true;

        for (int i = 0; i < resources.Length; i++)
        {
            if (Resources[resources[i].drop] < resources[i].amount)
            {
                canPay = false;
                return canPay;
            }
        }

        for (int i = 0; i < resources.Length; i++)
        {
            Resources[resources[i].drop] -= resources[i].amount;
        }
        UpdateResourceUI();
        return canPay;
    }
    public void AddResources(List<Resource> sends)
    {
        Resource[] drops = sends.ToArray();
        for (int i = 0; i < drops.Length; i++)
        {
            Resources[drops[i].drop] += drops[i].amount;
            Mathf.Clamp(Resources[drops[i].drop],0,9999);
        }
        UpdateResourceUI();
    }
    public void AddResources(Resource[] drops)
    {
        for (int i = 0; i < drops.Length; i++)
        {
            Resources[drops[i].drop] += drops[i].amount;
            Mathf.Clamp(Resources[drops[i].drop], 0, 9999);
        }
        UpdateResourceUI();
    }

    public void AddResource(ResourceType r, int amount)
    {
        Resources[r] += amount;
        Mathf.Clamp(Resources[r], 0, 9999);
        UpdateResourceUI();
    }

    private void UpdateResourceUI()
    {
        string s = "";
        foreach (var i in Resources)
        {
            if(i.Value <= 0)
            {
                continue;
            }
            s += i.Key.ToString() + ": " + i.Value.ToString() + "\n";
        }

        resourceText.text = s;
    }
}
