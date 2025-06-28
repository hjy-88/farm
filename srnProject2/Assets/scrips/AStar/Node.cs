using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//创建Node类，表示地图中每个格子的三类数据，并且给出比较数据大小的逻辑
namespace MFarm.AStar
{
    public class Node : IComparable<Node>
    {
        public Vector2Int gridPosition; // 网格坐标
        public int gCost = 0; // 距离 Start 格子的距离
        public int hCost = 0; // 距离 Target 格子的距离
        public int FCost => gCost + hCost; // 当前格子的综合成本值
        public bool isObstacle = false; // 当前格子是否是障碍
        public Node parentNode;

        public Node(Vector2Int pos)
        {
            gridPosition = pos;
            parentNode = null;
        }

        public int CompareTo(Node other)
        {
            // 比较选出最低的 F 值，返回 -1, 0, 1 
            int result = FCost.CompareTo(other.FCost);
            if (result == 0)
            {
                result = hCost.CompareTo(other.hCost);
            }
            return result;
        }
    }
}