using System;
using System.Collections.Generic;
using System.Linq;

public class CorridorGenerator
{
    private List<RoomNode> allNodesCollections;
    private int corridorWidht;

    public CorridorGenerator(List<RoomNode> allNodesCollections, int corridorWidht)
    {
        this.allNodesCollections = allNodesCollections;
        this.corridorWidht = corridorWidht;
    }

    internal List<Node> CreateCorridor()
    {
        List<Node> corridorList =new List<Node>();
        Queue<RoomNode> structuresFromCheck= new Queue<RoomNode>(
        allNodesCollections.OrderByDescending(node =>node.TreeLayerIndex).ToList());
        while (structuresFromCheck.Count > 0) {
            var node = structuresFromCheck.Dequeue();
            if (node.ChildrenNodeList.Count == 0) 
            {
                continue;           
            }
            CorrridorNode corridor = new CorrridorNode(node.ChildrenNodeList[0], node.ChildrenNodeList[1],corridorWidht);
            corridorList.Add(corridor);

        }
        return corridorList;
    }
}