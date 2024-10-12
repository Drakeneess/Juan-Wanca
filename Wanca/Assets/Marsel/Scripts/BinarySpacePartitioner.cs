using System;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using Random = UnityEngine.Random;
public class BinarySpacePartitioner
{
    RoomNode rootNode;

    public RoomNode RootNode { get => rootNode; }
    public BinarySpacePartitioner(int roomWidth, int roomLenght)
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(roomWidth, roomLenght), null, 0);
    }

    public List<RoomNode> PrepareNodesCollection(int maxIterations, int roomWidthMin, int roomLenghtMin)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);
        int iterations = 0;
        while (iterations < maxIterations && graph.Count > 0)
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if (currentNode.width >= roomWidthMin * 2 || currentNode.lenght >= roomLenghtMin * 2)
            {
                SplitTheSpace(currentNode, listToReturn, roomLenghtMin, roomWidthMin, graph);
            }
        }
        return listToReturn;
    }

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLenghtMin, int roomWidthMin, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(
            currentNode.BottomLeftAreaCorner,
            currentNode.TopRightAreaCorner,
            roomWidthMin,
            roomLenghtMin);
        RoomNode node1, node2;
        if(line.Orientation== Orientation.horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(currentNode.TopRightAreaCorner.x,line.Coordinates.y),
                currentNode,currentNode.TreeLayerIndex+1 );
            node2 = new RoomNode(
                new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinates.y),
                currentNode.TopRightAreaCorner,
                currentNode, currentNode.TreeLayerIndex + 1);
        }
        else
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(line.Coordinates.x,currentNode.TopRightAreaCorner.y),
                currentNode, currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode( new Vector2Int(line.Coordinates.x,currentNode.BottomLeftAreaCorner.y),
                currentNode.TopRightAreaCorner,
                currentNode, currentNode.TreeLayerIndex + 1);
        }
        AddNewNodeToCollections(listToReturn, graph, node1);
        AddNewNodeToCollections(listToReturn, graph, node2);
    }

    private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLenghtMin)
    {
        Orientation orientation;
        bool lenghtStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * roomLenghtMin;
        bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * roomWidthMin;
        if (lenghtStatus && widthStatus)
        {
            orientation = (Orientation)(Random.Range(0, 2));
        }
        else if (widthStatus)
        {

            orientation = Orientation.vertical;

        }
        else 
        {
            orientation = Orientation.horizontal;              
        }
        return new Line(orientation, GetCoordinatesForOrientation
            (orientation,
            bottomLeftAreaCorner,
            topRightAreaCorner,
            roomWidthMin,
            roomLenghtMin

            ));
    }

    private Vector2Int GetCoordinatesForOrientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLenghtMin)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if (orientation == Orientation.horizontal)
        {

            coordinates = new Vector2Int(0, Random.Range(
                (bottomLeftAreaCorner.y + roomLenghtMin), (topRightAreaCorner.y - roomLenghtMin)));
        }
        else {
            coordinates = new Vector2Int( Random.Range(
              (bottomLeftAreaCorner.x + roomWidthMin), (topRightAreaCorner.x - roomWidthMin)),0);
        }
        return coordinates;
    }
}