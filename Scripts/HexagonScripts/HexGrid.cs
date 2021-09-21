using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{

    public int width = 6;

    public int height = 6;

    public HexCell cellPrefab;

    public Text cellLabelPrefab;

    private Canvas gridCanvas;

    private HexMesh hexMesh;

    private MapGeneration generator;

    private HexCell[] cells;

    public Transform cellsMassive;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();

        generator = GetComponent<MapGeneration>();

        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }

    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    public HexCell[] GetHexCells()
    {
        if (cells.Length > 0)
        {
            return cells;
        }
        else return null;
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetric.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetric.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(cellsMassive, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        if (x > 0)
        {
            cell.SetNeighbor(Direction.W_LEFT, cells[i - 1]);                                // Setting West Neighbor
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(Direction.SE_RIGHT_DOWN, cells[i - width]);                // Setting South-East Neighbor for even cells 

                if (x > 0)
                {
                    cell.SetNeighbor(Direction.SW_LEFT_DOWN, cells[i - width - 1]);         // Setting South-West Neighbor for even cells 
                }
            }
            else
            {
                cell.SetNeighbor(Direction.SW_LEFT_DOWN, cells[i - width]);                 // Setting South-West Neighbor for odd cells 
                if (x < width - 1)
                {
                    cell.SetNeighbor(Direction.SE_RIGHT_DOWN, cells[i - width + 1]);        // Setting South-East Neighbor for odd cells
                }
            }
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }



}

