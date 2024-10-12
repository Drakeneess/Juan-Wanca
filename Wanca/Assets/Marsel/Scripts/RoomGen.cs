using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGen
{
    private int maxIterations;
    private int roomLenghtMin;
    private int roomWidthMin;

    public RoomGen(int maxIterations, int roomLenghtMin, int roomWidthMin)
    {
        this.maxIterations = maxIterations;
        this.roomLenghtMin = roomLenghtMin;
        this.roomWidthMin = roomWidthMin;
    }

    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();
        foreach (var space in roomSpaces)
        {
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner,roomBottomCornerModifier,roomOffset
                
                );
            Vector2Int newTopRoghtPoint = StructureHelper.GenerateTopRightCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomTopCornerModifier, roomOffset);
            space.BottomLeftAreaCorner= newBottomLeftPoint;
            space.TopRightAreaCorner= newTopRoghtPoint;
            space.BottomRightAreaCorner= new Vector2Int (newTopRoghtPoint.x,newBottomLeftPoint.y);
            space.TopLefAreaCorner= new Vector2Int (newBottomLeftPoint.x,newTopRoghtPoint.y);
            listToReturn.Add((RoomNode)space);
        }
        return listToReturn;
    }
}