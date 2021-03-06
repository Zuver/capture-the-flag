﻿using System.Collections.Generic;
using System.Linq;

namespace Engine.Entities.Graphs.Internal
{
    internal static class GraphTraversalHelper
    {
        /// <summary>
        /// Uses Dijkstra's algorithm to map the best path to take if you are coming from any Node
        /// </summary>
        /// <param name="goalNode"></param>
        /// <returns></returns>
        public static Dictionary<Node, Node> BuildShortestPathDictionary(Node goalNode)
        {
            // Run Dijkstra's algorithm
            Dictionary<Node, DijkstraEntry> dijkstrasAlgorithmResult = ExecuteDijkstrasAlgorithm(goalNode);

            // The key Node of the resulting dictionary represents a starting location.
            // The value Node of the dictionary represents the "best" Node to take in order to eventually arrive at the goal Node.
            
            // For example, let's say a bot is trying to get from Node A to the goal Node.
            // If we look up Node A in this dictionary we'll be able to determine the next Node
            // the bot should visit in order to minimize the distance to the goal Node.
            Dictionary<Node, Node> result = new Dictionary<Node, Node>();

            foreach (Node key in dijkstrasAlgorithmResult.Keys)
            {
                result.Add(key, dijkstrasAlgorithmResult[key].Via);
            }

            return result;
        }

        /// <summary>
        /// Execute Dijkstra's algorithm
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private static Dictionary<Node, DijkstraEntry> ExecuteDijkstrasAlgorithm(Node start)
        {
            Dictionary<Node, DijkstraEntry> result = new Dictionary<Node, DijkstraEntry>
            {
                [start] = new DijkstraEntry {Via = start, Cost = 0f, Known = true}
            };

            Node currentNode = start;

            while (currentNode != null)
            {
                // The best path to get to the current node is definitely known
                result[currentNode].Known = true;

                foreach (Node neighbor in currentNode.Neighbors)
                {
                    // Check if it's a Node we've already analyzed
                    if (result.ContainsKey(neighbor) && result[neighbor].Known)
                        continue;

                    // Calculate distance from the current (known) Node
                    float distanceFromCurrentNode = (currentNode.Position - neighbor.Position).LengthSquared();

                    if (result.ContainsKey(neighbor))
                    {
                        DijkstraEntry existingEntry = result[neighbor];

                        // Check if a better path is available
                        if (result[currentNode].Cost + distanceFromCurrentNode < existingEntry.Cost)
                        {
                            existingEntry.Via = currentNode;
                            existingEntry.Cost = result[currentNode].Cost + distanceFromCurrentNode;
                        }
                    }
                    else
                    {
                        result[neighbor] = new DijkstraEntry
                        {
                            Via = currentNode,
                            Cost = result[currentNode].Cost + distanceFromCurrentNode,
                            Known = false
                        };
                    }
                }

                // Search for the next node to analyze
                currentNode = null;

                // In the set of unknown Nodes, find the one that has the least "cost" and set Known = true
                KeyValuePair<Node, DijkstraEntry> nextKnownNode = result
                    .Where(n => !n.Value.Known)
                    .OrderBy(n => n.Value.Cost)
                    .FirstOrDefault();

                if (nextKnownNode.Key != null)
                    currentNode = nextKnownNode.Key;
            }

            return result;
        }
    }
}
