namespace Engine.Entities.Graphs.Internal
{
    /// <summary>
    /// This class models the best Node to visit in order to arrive at some destination Node
    /// </summary>
    internal class DijkstraEntry
    {
        public bool Known { get; set; }
        public Node Via { get; set; }
        public float Cost { get; set; }

        public DijkstraEntry()
        {
            Known = false;
            Via = null;
            Cost = float.MaxValue;
        }
    }
}
