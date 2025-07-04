using System.Collections;
using System.Collections.Generic;
//using MFarm.Map;
using UnityEngine;

namespace MFarm.AStar
{
    public class AStar : MonoBehaviour
    {
        private GridNodes gridNodes;
        private Node startNode;
        private Node targetNode;
        private int gridWidth;
        private int gridHeight;
        private int originX;
        private int originY;

        private List<Node> openNodeList;    //当前选中Node周围的8个点
        private HashSet<Node> closedNodeList;  //所有被选中的点

        private bool pathFound;


        public void BuildPath(string sceneName, Vector2Int startPos, Vector2Int endPos, Stack<MovementStep> npcMovementStack)
        {
            pathFound = false;

            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                // 查找最短路径
                if (FindShortestPath())
                {
                    // 构建NPC移动路径
                    UpdatePathOnMovementStepStack(sceneName, npcMovementStack);
                }
            }
        }

        private bool GenerateGridNodes(string sceneName, Vector2Int startPos, Vector2Int endPos)
        {
            //if (GridMapManager.Instance.GetGridDimensions(sceneName, out Vector2Int gridDimensions, out Vector2Int gridOrigin))
            //{
            //    // 按瓦片地图范围构建网格移动节点范围数组
            //    gridNodes = new GridNodes(gridDimensions.x, gridDimensions.y);
            //    gridWidth = gridDimensions.x;
            //    gridHeight = gridDimensions.y;
            //    originX = gridOrigin.x;
            //    originY = gridOrigin.y;

            //    openNodeList = new List<Node>();

            //    closedNodeList = new HashSet<Node>();
            //}
            //else
            //    return false;

            //// gridNodes 的范围是从0,0开始所以需要减去原点坐标得到实际位置
            //startNode = gridNodes.GetGridNode(startPos.x - originX, startPos.y - originY);
            //targetNode = gridNodes.GetGridNode(endPos.x - originX, endPos.y - originY);

            //for (int x = 0; x < gridWidth; x++)
            //{
            //    for (int y = 0; y < gridHeight; y++)
            //    {
            //        Vector3Int tilePos = new Vector3Int(x + originX, y + originY, 0);

            //        TileDetails tile = GridMapManager.Instance.GetTileDetailsOnMousePosition(tilePos);

            //        if (tile != null)
            //        {
            //            Node node = gridNodes.GetGridNode(x, y);

            //            if (tile.isNPCObstacle)
            //                node.isObstacle = true;
            //        }
            //    }
            //}

            return true;
        }

        private bool FindShortestPath()
        {
            // 添加起点
            openNodeList.Add(startNode);

            while (openNodeList.Count > 0)
            {
                // 节点排序，Node内涵比较函数
                openNodeList.Sort();

                Node closeNode = openNodeList[0];

                openNodeList.RemoveAt(0);
                closedNodeList.Add(closeNode);

                if (closeNode == targetNode)
                {
                    pathFound = true;
                    break;
                }

                // 计算周围8个Node补充到OpenList
                EvaluateNeighbourNodes(closeNode);
            }

            return pathFound;
        }

        private void EvaluateNeighbourNodes(Node currentNode)
        {
            Vector2Int currentNodePos = currentNode.gridPosition;
            Node validNeighbourNode;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    validNeighbourNode = GetValidNeighbourNode(currentNodePos.x + x, currentNodePos.y + y);

                    if (validNeighbourNode != null)
                    {
                        if (!openNodeList.Contains(validNeighbourNode))
                        {
                            validNeighbourNode.gCost = currentNode.gCost + GetDistance(currentNode, validNeighbourNode);
                            validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);
                            //链接父节点
                            validNeighbourNode.parentNode = currentNode;
                            openNodeList.Add(validNeighbourNode);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 找到有效的Node，非障碍，非已选择
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Node GetValidNeighbourNode(int x, int y)
        {
            if (x >= gridWidth || y >= gridHeight || x < 0 || y < 0)
                return null;

            Node neighbourNode = gridNodes.GetGridNode(x, y);

            if (neighbourNode.isObstacle || closedNodeList.Contains(neighbourNode))
                return null;
            else
                return neighbourNode;
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int xDistance = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
            int yDistance = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

            if (xDistance > yDistance)
            {
                return 14 * yDistance + 10 * (xDistance - yDistance);
            }
            return 14 * xDistance + 10 * (yDistance - xDistance);
        }

        /// <summary>
        /// 更新路径每一步的坐标和场景名字
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="npcMovementStep"></param>
        private void UpdatePathOnMovementStepStack(string sceneName, Stack<MovementStep> npcMovementStep)
        {
            Node nextNode = targetNode;

            while (nextNode != null)
            {
                MovementStep newStep = new MovementStep();
                newStep.sceneName = sceneName;
                newStep.gridCoordinate = new Vector2Int(nextNode.gridPosition.x + originX, nextNode.gridPosition.y + originY);
                //压入堆栈
                npcMovementStep.Push(newStep);
                nextNode = nextNode.parentNode;
            }
        }
    }
}