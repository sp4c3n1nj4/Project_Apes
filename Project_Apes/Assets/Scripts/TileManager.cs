using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Towers;
    [SerializeField]
    private GameObject TileHighlight, TowerUI, TileUI;
    private const float tileSize = 1f;
    private const float tileOffset = 0.5f;

    public Vector3 GetTile(int x, int y)
    {
        Vector3 pos = Vector3.zero;
        pos.x += (tileSize * x) + tileOffset;
        pos.z += (tileSize * y) + tileOffset;
        return pos;
    }
    public Vector3 GetTile(Vector2Int tile)
    {
        Vector3 pos = Vector3.zero;
        pos.x += (tileSize * tile.x) + tileOffset;
        pos.z += (tileSize * tile.y) + tileOffset;
        return pos;
    }

    private enum Selected { none, tile, tower }
    private Selected selected;

    private Vector2Int selectedTile;
    private Vector2Int clickedTile;

    public Dictionary<Vector2Int, GameObject> ActiveTowers = new();
    public List<Vector2Int> EnemyPath;

    private void Update()
    {
        TileMouseOver();

        switch (selected)
        {
            case Selected.none:                
                TileInteract();
                break;
            case Selected.tile:
                TileMenu();
                break;
            case Selected.tower:
                TowerMenu();
                break;
        }      
    }

    private void TileMouseOver()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, LayerMask.GetMask("Tiles")))
        {
            Vector2Int hitPoint = new Vector2Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));

            if (selectedTile == hitPoint) return;
            selectedTile = hitPoint;

            if (selected != Selected.none) return;
            TileHighlight.transform.position = GetTile(selectedTile);
        }
        else
        {
            selectedTile = new Vector2Int(-1, -1);
            TileHighlight.transform.position = GetTile(selectedTile);
        }
    }

    private void TileInteract()
    {
        if (selectedTile == new Vector2Int(-1, -1))
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (EnemyPath.Contains(selectedTile))
                return;
            if (ActiveTowers.Count > 0)
            {
                if (ActiveTowers.ContainsKey(selectedTile))
                    TowerInteract();
            }
            else
                EmptyTileInteract();
        }
    }
    private void EmptyTileInteract()
    {
        clickedTile = selectedTile;
        selected = Selected.tile;
        TileUI.transform.position = GetTile(clickedTile);
        TileUI.SetActive(true);
    }
    private void TowerInteract()
    {
        clickedTile = selectedTile;
        selected = Selected.tower;
        TowerUI.transform.position = GetTile(clickedTile);
        TowerUI.SetActive(true);
    }

    public void DisableMenu()
    {
        clickedTile = new Vector2Int(-1, -1);
        selected = Selected.none;
        TileUI.SetActive(false);
        TowerUI.SetActive(false);
        TileHighlight.transform.position = GetTile(selectedTile);
    }

    private void TowerMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            DisableMenu();

        if (Input.GetKeyDown(KeyCode.R))
            RotateTower();

        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
            DestroyTower();
    }
    public void RotateTower()
    {
        ActiveTowers[clickedTile].transform.Rotate(Vector3.up * 90);
    }
    public void DestroyTower()
    {
        ActiveTowers[clickedTile].GetComponent<Tower>().DestroyTower();
        ActiveTowers.Remove(clickedTile);
    }

    private void TileMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            DisableMenu();          
    }
    public void SpawnTower(int i)
    {
        GameObject tower = Instantiate<GameObject>(Towers[i]);
        tower.transform.position = GetTile(clickedTile);

        ActiveTowers.Add(clickedTile, tower);

        selected = Selected.none;
    }

    
}
