using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.astar
{
    public class Node
    {
        // Change this depending on what the desired size is for each element in the grid
        public static float NODE_SIZE = 1f;
        public Node Parent;
        public Vector3 Position;
        public Vector3 Center
        {
            get
            {
                return new Vector3(Position.x + NODE_SIZE / 2f, Position.y + NODE_SIZE / 2f);
            }
        }
        public float DistanceToTarget;
        public float Cost;
        public float Weight;
        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }
        public bool Walkable;

        public Node(Vector3 pos, bool walkable, float weight = 1)
        {
            Parent = null;
            Position = pos;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
            Walkable = walkable;
        }
    }

    public class Astar
    {
        Dictionary< Vector3Int ,Node > Grid;
        public Vector3Int GridBoundaryMax;
        public Vector3Int GridBoundaryMin;
        

        public Astar(Dictionary<Vector3Int, Node> grid)
        {
            Grid = grid;
        }

        public Stack<Node> FindPath(Vector3 Start, Vector3 End)
        {
            Node start = new Node(Start, true);
            Node end = new Node(End, true);

            Stack<Node> Path = new Stack<Node>();
            List<Node> OpenList = new List<Node>();
            List<Node> ClosedList = new List<Node>();
            List<Node> adjacencies;
            Node current = start;
           
            // add start node to Open List
            OpenList.Add(start);

            while(OpenList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList[0];
                OpenList.Remove(current);
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

 
                foreach(Node n in adjacencies)
                {
                    if (!ClosedList.Contains(n) && n.Walkable)
                    {
                        if (!OpenList.Contains(n))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.x - end.Position.x) + Math.Abs(n.Position.y - end.Position.y);
                            n.Cost = n.Weight + n.Parent.Cost;
                            OpenList.Add(n);
                            OpenList = OpenList.OrderBy(node => node.F).ToList<Node>();
                        }
                    }
                }
            }
            
            // construct path, if end was not closed return null
            if(!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            Node temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null) ;
            return Path;
        }

        private List<Node> GetAdjacentNodes(Node n)
        {
            List<Node> temp = new List<Node>();

            int row = (int)n.Position.y;
            int col = (int)n.Position.x;

            Vector3Int key;
            if (row + 1 < GridBoundaryMax.y)
            {
                key = new Vector3Int(col, row + 1, 0);
                if (Grid.ContainsKey(key))
                    temp.Add(Grid[key]);
            }
            if (row - 1 >= GridBoundaryMin.y)
            {
                key = new Vector3Int(col, row - 1, 0);
                if (Grid.ContainsKey(key))
                    temp.Add(Grid[key]);
            }
            if (col - 1 >= GridBoundaryMin.x)
            {
                key = new Vector3Int(col - 1, row, 0);
                if (Grid.ContainsKey(key))
                    temp.Add(Grid[key]);
            }
            if (col + 1 < GridBoundaryMax.x)
            {
                key = new Vector3Int(col + 1, row, 0);
                if (Grid.ContainsKey(key))
                    temp.Add(Grid[key]);
            }

            return temp;
        }
    }
}
