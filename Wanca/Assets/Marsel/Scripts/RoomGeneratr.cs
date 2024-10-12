using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class RoomGeneratr
{
    RoomNode rootNode;
    List<RoomNode> AllNodesCollections = new List<RoomNode>();
    private int roomWidth;
    private int roomLenght;

    public RoomGeneratr(int roomWidth, int roomLenght)
    {
        this.roomWidth = roomWidth;
        this.roomLenght = roomLenght;
    }
 public List<Node> CalculateRooms(int maxIterations, int roomWidthMin, int roomLenghtMin, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidht)
    {
        BinarySpacePartitioner bsp =new BinarySpacePartitioner(roomWidth,roomLenght);
        AllNodesCollections= bsp.PrepareNodesCollection(maxIterations,roomWidthMin,roomLenghtMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);

        RoomGen roomGeneratr = new RoomGen(maxIterations,roomLenghtMin,roomWidthMin);
        List<RoomNode> roomList = roomGeneratr.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);
        CorridorGenerator  corridorGenerator= new CorridorGenerator(AllNodesCollections,corridorWidht);
        var corridorList = corridorGenerator.CreateCorridor();
        return new List<Node>(roomList).Concat(corridorList).ToList();

    }
}

