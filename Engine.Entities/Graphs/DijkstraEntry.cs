using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities.Graphs
{
    class DijkstraEntry
    {
        public bool Known;
        public Node Via;
        public float Cost;

        public DijkstraEntry()
        {
            this.Known = false;
            this.Via = null;
            this.Cost = float.MaxValue;
        }
    }
}
