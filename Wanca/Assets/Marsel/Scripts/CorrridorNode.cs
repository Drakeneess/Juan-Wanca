using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

internal class CorrridorNode:Node
{
    private Node Structure1;
    private Node Structure2;
    private int corridorWidht;
    private int modifierDistanceFromWall=1;

    public CorrridorNode(Node node1, Node node2, int corridorWidht): base (null)
    {
        this.Structure1 = node1;
        this.Structure2 = node2;
        this.corridorWidht = corridorWidht;
        GenerateCorridor();
    }

    private void GenerateCorridor()
    {
        
        var relativePositionOfStructure2 = CheckPositionStructure1AgaisntStructure2();
        switch (relativePositionOfStructure2)
        {
            case RelativePosition.Up:
                ProcesSRoomInRelationUpOrDown(this.Structure1,this.Structure2);
                break;
                case RelativePosition.Down:
                ProcesSRoomInRelationUpOrDown(this.Structure2, this.Structure1);

                break;
                case RelativePosition.Right:
                ProcesSRoomInRelationRightOrLeft(this.Structure1, this.Structure2);

                break;
                case RelativePosition.Left:
                ProcesSRoomInRelationRightOrLeft(this.Structure2, this.Structure1);

                break;
        }
    }

    private void ProcesSRoomInRelationRightOrLeft(Node structure1, Node structure2)
    {
        Node leftStructure = null;
        List<Node> leftStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure1);
        Node rightStructure = null;
        List<Node> rightStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure2);
        var sortedLeftStructure = leftStructureChildren.OrderByDescending(child => child.TopRightAreaCorner.x).ToList();
        if (sortedLeftStructure.Count == 1)
        {
            leftStructure =sortedLeftStructure[0];
        }
        else
        {
            int maxX = sortedLeftStructure[0].TopRightAreaCorner.x;
            sortedLeftStructure = sortedLeftStructure.Where(children => Math.Abs(maxX - children.TopRightAreaCorner.x) < 10).ToList();
            int index = UnityEngine.Random.Range(0,sortedLeftStructure.Count);
            leftStructure = sortedLeftStructure[index];
        }
        var possibleNeighboursInRightStructureList = rightStructureChildren.Where(
            child=> GetValidNeighbourLeftRight(
                leftStructure.TopRightAreaCorner,
                leftStructure.BottomRightAreaCorner,
                child.TopLefAreaCorner,
                child.BottomLeftAreaCorner

                )!=1
            ).OrderBy(child=> child.BottomRightAreaCorner.x).ToList();
        if (possibleNeighboursInRightStructureList.Count <= 0)
        {
            rightStructure = structure2;
        }
        else
        {
            rightStructure = possibleNeighboursInRightStructureList[0];
        }
        int y = GetValidNeighbourLeftRight(leftStructure.TopLefAreaCorner,leftStructure.BottomRightAreaCorner
            ,rightStructure.TopLefAreaCorner,rightStructure.BottomLeftAreaCorner );
        while (y == -1&& sortedLeftStructure.Count>0)
        {
            sortedLeftStructure = sortedLeftStructure.Where(child => child.TopLefAreaCorner.y != leftStructure.TopLefAreaCorner.y ).ToList();
            leftStructure= sortedLeftStructure[0];
            y = GetValidNeighbourLeftRight(leftStructure.TopLefAreaCorner, leftStructure.BottomRightAreaCorner
            , rightStructure.TopLefAreaCorner, rightStructure.BottomLeftAreaCorner);
        }
        BottomLeftAreaCorner = new Vector2Int(leftStructure.BottomRightAreaCorner.x, y);
        TopRightAreaCorner = new Vector2Int(rightStructure.TopLefAreaCorner.x, y + this.corridorWidht);
    }

    private int GetValidNeighbourLeftRight(Vector2Int leftNodeUp, Vector2Int leftNodeDown, Vector2Int rightNodeUp, Vector2Int RightNodeDown)
    {
        if (rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= RightNodeDown.y)
        {
            return StructureHelper.CalculatemiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidht)).y;
        }
        if (rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= RightNodeDown.y) 
        {
            return StructureHelper.CalculatemiddlePoint(
              RightNodeDown+new Vector2Int(0,modifierDistanceFromWall),
              rightNodeUp- new Vector2Int (0,modifierDistanceFromWall+this.corridorWidht)).y;
        }
        if (leftNodeUp.y>=RightNodeDown.y&& leftNodeUp.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculatemiddlePoint(
               RightNodeDown+new Vector2Int(0,modifierDistanceFromWall),
               leftNodeUp-new Vector2Int(0,modifierDistanceFromWall)).y;
        }
        if(leftNodeDown.y >=RightNodeDown.y&& leftNodeDown.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculatemiddlePoint(
              leftNodeDown+new Vector2Int(0,modifierDistanceFromWall),
              rightNodeUp- new Vector2Int(0,modifierDistanceFromWall+this.corridorWidht)
               ).y;
        }
        return -1;
    }

    private void ProcesSRoomInRelationUpOrDown(Node structure1, Node structure2)
    {
        Node BottomStructure = null;
        List<Node> structureBottomChildren= StructureHelper.TraverseGraphToExtractLowestLeafes(structure1);
        Node topStructure = null;
        List<Node> structurAbvoveChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure2);
        var sortedBottomStructure = structureBottomChildren.OrderByDescending(child=> child.TopRightAreaCorner.y).ToList();
        if (sortedBottomStructure.Count == 1)
        {
            BottomStructure= structureBottomChildren[0];
        }
        else
        {
            int maxY = sortedBottomStructure[0].TopLefAreaCorner.y;
            sortedBottomStructure = sortedBottomStructure.Where(child => Mathf.Abs(maxY - child.TopLefAreaCorner.y) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedBottomStructure.Count);
            BottomStructure = sortedBottomStructure [index];
        }
        var possibleNeighbourInTopStructure = structurAbvoveChildren.Where(
            child => GetValidXForNeighbourUpDown(
                BottomStructure.TopLefAreaCorner,
                BottomStructure.TopRightAreaCorner,
                child.BottomLeftAreaCorner,
                child.BottomRightAreaCorner
                ) != 1).OrderBy(child=> child.BottomRightAreaCorner.y).ToList();
        if (possibleNeighbourInTopStructure.Count == 0)
        {
            topStructure = structure2;
        }
        else
        {
            topStructure= possibleNeighbourInTopStructure[0];
        }
        int x = GetValidXForNeighbourUpDown(
                BottomStructure.TopLefAreaCorner,
                BottomStructure.TopRightAreaCorner,
        topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner
                );
        while (x == -1 && sortedBottomStructure.Count > 1) 
        {
            sortedBottomStructure = sortedBottomStructure.Where(child => child.TopLefAreaCorner.x != topStructure.TopLefAreaCorner.x).ToList();
             x = GetValidXForNeighbourUpDown(
                BottomStructure.TopLefAreaCorner,
                BottomStructure.TopRightAreaCorner,
        topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner
                );
        }
        BottomLeftAreaCorner = new Vector2Int(x, BottomStructure.TopLefAreaCorner.y);
        TopRightAreaCorner = new Vector2Int(x+this.corridorWidht,topStructure.BottomRightAreaCorner.y);


    }

    private int GetValidXForNeighbourUpDown(Vector2Int bottomNodeLeft, 
        Vector2Int bottomNodeRight, Vector2Int topNodeLeft,
        Vector2Int topNodeRight)
    {
        if(topNodeLeft.x <bottomNodeLeft.x && bottomNodeRight.x < topNodeRight.x)
        {
            return StructureHelper.CalculatemiddlePoint(
                bottomNodeLeft+new Vector2Int ( modifierDistanceFromWall,0),
                bottomNodeRight - new Vector2Int(this.corridorWidht + modifierDistanceFromWall,0)
                ).x;
        }
        if(topNodeLeft.x>=bottomNodeLeft.x && bottomNodeRight.x>= topNodeRight.x)
        {
            return StructureHelper.CalculatemiddlePoint(
               topNodeLeft +new Vector2Int (modifierDistanceFromWall,0),
                topNodeRight- new Vector2Int (this.corridorWidht+modifierDistanceFromWall,0)
                ).x;
        }
        if( bottomNodeLeft.x >= (topNodeLeft.x) && bottomNodeLeft.x<= topNodeRight.x)
        {
            return StructureHelper.CalculatemiddlePoint(
                bottomNodeLeft+ new Vector2Int(modifierDistanceFromWall,0),
                topNodeRight - new Vector2Int(this.corridorWidht+ modifierDistanceFromWall,0)
                ).x;
        }
        if(bottomNodeRight.x <= topNodeRight.x &&bottomNodeRight.x >= topNodeLeft.x)
        {
            return StructureHelper.CalculatemiddlePoint(
                topNodeLeft+new Vector2Int(modifierDistanceFromWall,0),
                bottomNodeRight -new Vector2Int(this.corridorWidht+modifierDistanceFromWall,0)
                ).x;
        }
        return -1;
    }

    private RelativePosition CheckPositionStructure1AgaisntStructure2()
    {
        Vector2 midlePointStructure1Temp = ((Vector2)Structure1.TopRightAreaCorner + Structure1.BottomLeftAreaCorner) / 2;
        Vector2 midlePointStructure2Temp = ((Vector2)Structure2.TopRightAreaCorner + Structure2.BottomLeftAreaCorner) / 2;
        float angel = CalculateAngle(midlePointStructure1Temp,midlePointStructure2Temp);
        if((angel < 45 && angel>= 0)||(angel >- 45&& angel < 0))
        {
            return RelativePosition.Right;
        }
        else if (angel >45 && angel< 135)
        {
            return RelativePosition.Up;
        }
        else if (angel>- 135 && angel < -45)
        {
            return RelativePosition.Down;
        }
        else
        {
            return RelativePosition.Left;
        }
    }

    private float CalculateAngle(Vector2 midlePointStructure1Temp, Vector2 midlePointStructure2Temp)
    {
        return Mathf.Atan2(midlePointStructure2Temp.y - midlePointStructure1Temp.y, midlePointStructure2Temp.x - midlePointStructure1Temp.x) * Mathf.Rad2Deg;
    }
}