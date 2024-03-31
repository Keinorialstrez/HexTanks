using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HexCell : MonoBehaviour
{

    public HexCoordinates coordinates;

    [SerializeField]
    HexCell[] neighbors;

    private int distance;

    public int Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
        }
    }

    public HexCell PathFrom { get; set; }

    public HexCell GetNeighbor(Direction direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(Direction direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }


}

[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinatesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HexCoordinates coordinates = new HexCoordinates(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue);

        GUI.Label(position, coordinates.ToString());
    }
}