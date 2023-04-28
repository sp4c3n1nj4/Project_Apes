using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Towers;
    [SerializeField]
    private GameObject TileHighlight, TowerUI, TileUI;
    private const float tileSize = 1f;
    private const float tileOffset = 0.5f;

    public Vector2Int gridSize = new Vector2Int(20, 20);
    public Node[,] grid;

    public UnityEvent GridObstacleChange;

    private void Awake()
    {
        if (GridObstacleChange == null)
            GridObstacleChange = new UnityEvent();

        CreateGrid();  
    }
    private void CreateGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int gridPoint = new Vector2Int(x, y);
                grid[x, y] = new Node(true, gridPoint);
            }
        }
    }

    public Vector3 GetTile(Vector2Int tile)
    {
        Vector3 worldBottomLeft = Vector3.zero - Vector3.right * gridSize.x/2 - Vector3.forward * gridSize.y/2;
        Vector3 pos = worldBottomLeft;
        pos.x += (tileSize * tile.x) + tileOffset;
        pos.z += (tileSize * tile.y) + tileOffset;
        return pos;
    }
    public Vector3 GetTile(Node node)
    {
        Vector3 worldBottomLeft = Vector3.zero - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        Vector3 pos = worldBottomLeft;
        pos.x += (tileSize * node.gridPos.x) + tileOffset;
        pos.z += (tileSize * node.gridPos.y) + tileOffset;
        return pos;
    }

    public Vector2Int TileFromWorldPoint(Vector3 worldPos)
    {
        float X = worldPos.x + gridSize.x/2;
        float Y = worldPos.z + gridSize.y/2;

        int x = Mathf.FloorToInt(X);
        int y = Mathf.FloorToInt(Y);

        return grid[x,y].gridPos;
    }
    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float X = worldPos.x + gridSize.x / 2;
        float Y = worldPos.z + gridSize.y / 2;

        int x = Mathf.FloorToInt(X);
        int y = Mathf.FloorToInt(Y);

        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y))
                    continue;

                int checkX = node.gridPos.x + x;
                int checkY = node.gridPos.y + y;

                if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                {
                    neighbours.Add(grid[checkX,checkY]);
                }
            }
        }

        return neighbours;
    }

    public int maxSize()
    {
        return gridSize.x * gridSize.y;
    }

    private enum Selected { none, tile, tower }
    private Selected selected;

    private Vector2Int selectedTile;
    private Vector2Int clickedTile;

    public Dictionary<Vector2Int, GameObject> ActiveTowers = new();

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
            Vector2Int hitPoint = TileFromWorldPoint(hit.point);

            if (selectedTile == hitPoint) return;
            selectedTile = hitPoint;

            if (selected != Selected.none) return;
            TileHighlight.transform.position = GetTile(selectedTile);
        }
        else
        {
            selectedTile = new Vector2Int(-1, -1);

            if (selected != Selected.none) return;
            TileHighlight.transform.position = GetTile(selectedTile);
        }
    }

    private void TileInteract()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}

        if (selectedTile == new Vector2Int(-1, -1))
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {          
            print(selectedTile.ToString() + "\n" + GetTile(selectedTile).ToString());

            if (ActiveTowers.Count > 0)
            {
                if (ActiveTowers.ContainsKey(selectedTile))
                    TowerInteract();
                else
                    EmptyTileInteract();
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
        grid[clickedTile.x, clickedTile.y].walkable = true;

        GridObstacleChange.Invoke();
        DisableMenu();
    }

    private void TileMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            DisableMenu();          
    }
    public void SpawnTower(int i)
    {
        GameObject tower = Instantiate<GameObject>(Towers[i]);
        tower.transform.position = GetTile(clickedTile) + Vector3.up * tower.GetComponent<Tower>().spawnOffset;

        ActiveTowers.Add(clickedTile, tower);
        grid[clickedTile.x, clickedTile.y].walkable = false;

        GridObstacleChange.Invoke();

        selected = Selected.none;
        DisableMenu();
    }   
}
