using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class RoomGeneratr
{
    RoomNode rootNode;
    List<RoomNode> allSpaceNodes = new List<RoomNode>();
    private int roomWidth;
    private int roomLenght;

    public RoomGeneratr(int roomWidth, int roomLenght)
    {
        this.roomWidth = roomWidth;
        this.roomLenght = roomLenght;
    }
 public List<Node> CalculateRooms(int maxIterations, int roomWidthMin, int roomLenghtMin, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset)
    {
        BinarySpacePartitioner bsp =new BinarySpacePartitioner(roomWidth,roomLenght);
        allSpaceNodes= bsp.PrepareNodesCollection(maxIterations,roomWidthMin,roomLenghtMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);

        RoomGen roomGeneratr = new RoomGen(maxIterations,roomLenghtMin,roomWidthMin);
        List<RoomNode> roomList = roomGeneratr.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);
        return new List<Node>(roomList);

    }
}