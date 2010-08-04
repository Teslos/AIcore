using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Collections;

namespace AIcore
{
 
    /// <summary>
    /// Node class contains primitive info 
    /// about the nodes in the graph.
    /// </summary>
    class Node
    {
        public int Id;
        public Location location;
    }
    /// <summary>
    /// This graph represents the game level
    /// </summary>
    class Graph
    {
        ///<summary>
        /// returns an array of connections (class Connection)
        /// outgoing from the given node
        /// </summary>
        public System.Collections.ArrayList getConnections(Node node)
        {
            ArrayList connections = new ArrayList();
            // find node id and all connection to neighbours of this node
            // return all neighbours 
            return connections;
        }
    }
    /// <summary>
    /// This class represents the connection in the graph class.
    /// </summary>
    class Connection
    {
        float cost;
        Node fromNode;
        Node toNode;

        /// <summary>
        /// Connection constructor
        /// </summary>
        /// <param name="mcost">Cost of connection</param>
        /// <param name="mfromNode">Node from</param>
        /// <param name="mtoNode">Node to</param>
        public Connection(float mcost, Node mfromNode, Node mtoNode)
        {
            cost = mcost;
            fromNode = mfromNode;
            toNode = mtoNode;
        }

        /// <summary>
        /// returns the cost of the connection
        /// </summary>
        public float Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        
        /// <summary>
        /// sets and returns the node that this connection came from
        /// </summary>
        public Node FromNode
        {
            get { return fromNode; }
            set { fromNode = value; }
        }

        /// <summary>
        /// returns the node that this connection leads to
        /// </summary>
        public Node ToNode
        {
            get { return toNode; }
            set { toNode = value; }
        }
    }
    
    /// <summary>
    /// This class implements path finding 
    /// using Dijkstra algorithm
    /// </summary>
    class Pathfinding
    {
        List<NodeRecord> open;
        List<NodeRecord> closed;


        // This structure is used to keep track of the
        // information we need for each record
        public struct NodeRecord
        {
            public Node node;
            public Connection connection;
            public float costSoFar;
            public float estimateTotalCost;
        }

        List<NodeRecord> PathfindingList()
        {
            List<NodeRecord> pathList = new List<NodeRecord>();
            return pathList;
        }

        /// <summary>
        /// This function implements path finding on the graph
        /// using Dijkstra algorithm. This is not good algorithm
        /// for finding path in the game, but it can be use to
        /// analyze different paths on the complex maps.
        /// </summary>
        /// <param name="graph">graph which represent game level</param>
        /// <param name="start">starting node in graph</param>
        /// <param name="goal">goal node in graph</param>
        public List<NodeRecord> pathfindDijkstra(Graph graph, Node start, Node goal)
        {
            // Initialize the record for the start node
            NodeRecord endNodeRecord;
            NodeRecord startRecord = new NodeRecord();
            startRecord.node = start;
            startRecord.connection = null;
            startRecord.costSoFar = 0.0f;

            float endNodeCost = 0.0f;

            // Initialize the open and closed lists
            open = PathfindingList();
            open.Add(startRecord);
            closed = PathfindingList();

            // Iterate through processing each node
            while (open.Count > 0)
            {
                // Find the smallest element in the list
                NodeRecord current = open.Find(smallest);

                // If it is the goal node, then terminate
                if (current.node == goal) break;

                // Otherwise get its outgoing connections
                ArrayList connections = graph.getConnections(current.node);

                // Loop through each connection in turn
                foreach (Connection connection in connections)
                {
                    // Get the cost estimate in turn
                    NodeRecord endNode = ((Connection)connection).ToNode;
                    endNodeCost = current.costSoFar + connection.Cost;

                    // Skip if the node is closed
                    if (closed.Contains(endNode))
                    {
                        continue;
                    }
                    // .. or if it is open and we've found a worse
                    // route
                    else if (open.Contains(endNode))
                    {
                        // Here we find the record in the open list
                        // corresponding to the endNode.
                        // endNodeRecord = open.Find(endNode);

                        if (endNode.costSoFar <= endNodeCost)
                            continue;

                    }
                    else
                    {
                        endNodeRecord = new NodeRecord();
                        endNodeRecord.node = endNode;

                        // We're here if we need to update the node
                        // update the cost and connection
                        endNodeRecord.costSoFar = endNodeCost;
                        endNodeRecord.connection = connection;

                        // And add if to the the open list
                        if (!open.Contains(endNode))
                        {
                            open.Add(endNodeRecord);
                        }
                    }
                    // We've finished looking at the connections for
                    // the current node, so add it to the closed list
                    // and remove it from the open list
                    open.Remove(current);
                    closed.Add(current);
                }
                // We've here if we've either found the goal, or
                // if we've no more nodes to search, find which.
                if (current.node != goal)
                {
                    // We've run out of nodes without finding the 
                    // goal so there is no solution
                    return null;
                }
                else
                {
                    // compile the list of connections in the path
                    List<NodeRecord> path = new List<NodeRecord>();
                    // Work back along the path, accumulating
                    // connections
                    while (current.node != start)
                    {
                        path.Add(current);
                        current = current.connection.FromNode;
                    }

                    // Reverse the path, and return it
                    return path.Reverse();
                }
            }

        }


        /// <summary>
        /// This function search the optimal path in graph using
        /// Astar algorithm.
        /// </summary>
        /// <param name="graph">Graph to search</param>
        /// <param name="start">Starting node for search</param>
        /// <param name="end">Goal node</param>
        /// <param name="heuristic">Heuristic use for search</param>
        public void pathfindAStar(Graph graph, Node start, Node end)
        {
            // Initialize the record for the start node
            NodeRecord startRecord = new NodeRecord();
            startRecord.node = start;
            startRecord.connection = null;
            startRecord.costSoFar = 0;
            startRecord.estimateTotalCost = Heuristic.Estimate(start);

            // Initialize the open and closed lists
            open = PathfindingList();
            open.Add(startRecord);
            closed = PathfindingList();

            // Iterate through processing each node
            while (open.Count > 0)
            {
                // Find the smallest element in the open list
                // using (estimatedTotalCost)
                current = open.Find(smallest);

                // if it is the goal node, then terminate
                if (current.node == end) { break; }
                // Otherwise get its outgoing connections
                ArrayList connections = graph.getConnections(current);

                // Loop through each connection in turn
                foreach (Object conn in connections)
                {
                    Connection connection = ((Connection)conn);
                    // get the cost estimate for the end node
                    endNode = connection.getToNode;
                    endNodeCost = current.costSoFar + 
                        connection.Cost;

                    // if the node is closed we may have to
                    // skip, or remove it from the closed list.
                    if (closed.Contains(endNode))
                    {
                        // Here we find the record in the closed list
                        // corresponding to the endNode.
                        NodeRecord endNodeRecord = closed.Find(endNode);

                        // if we didn't find a shorter route, skip
                        if (endNodeRecord.costSoFar <= endNodeCost)
                        {
                            continue;
                        }

                        // otherwise remove it from the closed list
                        closed.Remove(endNodeRecord);

                        // We can use the node's old cost values
                        // to calculate its heuristic without calling
                        // the possibly expensive heuristic function
                        endNodeHeuristic = endNodeRecord.cost - endNodeRecord.costSoFar;

                        // Skip if the node is open and we've not
                        // found a better route
                    }
                    else if (open.Contains(endNode)) 
                    {
                        // Here we find the record in the open list
                        // corresponding to the endNode.
                        endNodeRecord = open.Find(endNode);

                        // if our route is no better, then skip
                        if (endNodeRecord.costSoFar <= endNodeCost)
                        {
                            continue;
                        }
                        // We can use the node's old cost values
                        // to calculate its heuristic without calling
                        // the possibly expensive heuristic function.
                        endNodeHeuristic = endNodeRecord.cost - endNodeRecord.costSoFar;
                        // Otherwise we know we've got an unvisited
                        // node, so make a record for it.
                    }
                    else
                    {
                        endNodeRecord = new NodeRecord();
                        endNodeRecord.node = endNode;

                        // We'll need to calculated the heuristic value
                        // using the function, since we don't have an
                        // existing record to use
                        endNodeHeuristic = heuristic.estimate(endNode);
                    }
                    // We're here if we need to update the node
                    // update the cost, estimate and connection
                    endNodeRecord.cost = endNodeCost;
                    endNodeRecord.connection = conn;
                    endNodeRecord.estimatedTotalCost = 
                        endNodeCost + endNodeHeuristic;

                    // And add it to the open list
                    if (!open.Contains(endNode))
                    {
                        open.Add(endNodeRecord);
                    }
                    // We have finished looking at the connections for
                    // current node, so add it to the closed list
                    // and remove it from the open list
                    open.Remove(current);
                    closed.Add(current);
                }
                // We're here if we've either found the goal, or
                // if we've no more nodes to search, find which.
                if (current.node != goal) 
                {
                    // we've run out of nodes without finding the 
                    // goal, so there is no solution
                    return null;
                }
                else
                {
                    // compile the list of connections in the path
                    List<NodeRecord> path = new List<NodeRecord>();

                    // work along the path, accumulating
                    // connections
                    while (current.node != start)
                    {
                        path.Add(current);
                        current = current.connection.getFromNode();
                    }
                    // reverse the path and return it
                    return path.Reverse();
                }
            }
        }
        /// <summary>
        /// This function implements ant-algorithm.
        /// We start from node and let ants in all directions
        /// if ant comes at empty field it is his. We measure then 
        /// how far the ant is from start node.
        /// </summary>
        /// <param name="graph">Graph to search</param>
        /// <param name="start">starting point</param>
        /// <param name="end">ending point</param>
        public void pathfindAnt(Graph graph, Node start, Node end)
        {
            List<NodeRecord> actualList;
            List<NodeRecord> newList;

            // Initialize
            NodeRecord currentRecord = new NodeRecord();
            NodeRecord startRecord = new NodeRecord();
            startRecord.node = start;
            startRecord.costSoFar = 0.0f;
            startRecord.connection = null;

            NodeRecord endRecord = new NodeRecord();
            endRecord.node = end;

            // add start record to current list
            actualList.Add(startRecord);

            // Iterate through processing each node
            while (actualList.Count > 0)
            {
                for (int nr; nr < current.Count; nr++)
                {
                    currentRecord = actualList[nr];
                    // get all neighbours from currentRecord
                    ArrayList connections = graph.getConnections(current);
                    foreach (Connection connection in connections)
                    {
                        float cost1 = CostFromTo(currentRecord, connection);
                        float cost2 = currentRecord.costSoFar;
                        if (connection.getCost > cost1 + cost2)
                        {
                            NodeRecord neigh = new NodeRecord();
                            neigh.costSoFar = cost1 + cost2;
                            neigh.connection.FromNode = currentRecord;
                            //((NodeRecord)connection.getToNode()).costSoFar = cost1 + cost2;
                            //connection.getFromNode = currentRecord;
                            // add neigh. to new list
                            newList.Add(neigh);
                        }
                    }
                }
            }
            current = newList;
            
            // construct the real path
            if (endRecord.costSoFar < float.PositiveInfinity)
            {
                nr = 0;
                List<NodeRecord> path = new List<NodeRecord>();
                path.Add(endRecord);
                while (currentRecord.node != start)
                {
                    nr++;
                    path.Add(currentRecord);
                    currentRecord = currentRecord.connection.getFromNode();
                }
            }
        }

        /// <summary>
        /// Returns cost from record to next connection
        /// </summary>
        /// <param name="record">current record</param>
        /// <param name="connection">follow connection</param>
        /// <returns></returns>
        private float CostFromTo(NodeRecord record, Connection connection)
        {
            return 0;
        }
    }
}
