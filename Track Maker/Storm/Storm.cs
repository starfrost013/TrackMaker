using TrackMaker.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Track_Maker
{
    //REMOVED V1.0 STORMTYPE!!!!
    //public enum StormType { Tropical, Subtropical, Extratropical, InvestPTC, PolarLow }

    /// <summary>
    /// MOVE ALL STORM APIS HERE - VERSION 2!
    /// </summary>
    public class Storm
    {
        public DateTime FormationDate { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Node> NodeList { get; set; }
        public List<Node> NodeList_Deleted { get; set; } // Deprecated - Dano stores basin copies in a History now.
        
        /* As of Priscilla build 512, only nodes store their storm types,
         * as it's redundant and easier for the user to only set the storm type per node. */
        //public StormType2 StormType { get; set; }

        public Storm()
        {
            NodeList = new List<Node>();
            NodeList_Deleted = new List<Node>(); 
        }

        /// <summary>
        /// Add a node to this storm.
        /// </summary>
        public void AddNode(int Intensity, string Type, Point Pos) // String in this case bad but swnabhfabg
        {
            Node NewNode = new Node();

            // Determine intensity.
            NewNode.Intensity = Intensity;

#if DANO
            StormTypeManager ST2Manager = GlobalState.GetST2Manager();
#else
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            StormTypeManager ST2Manager = MnWindow.ST2Manager;
#endif
            // Get node type.
            NewNode.NodeType = ST2Manager.GetStormTypeWithName(Type); 

            // Get id.
            NewNode.Id = NodeList.Count;

            // Set node position.
            NewNode.Position = Pos;
            Logging.Log($"Adding node");
            // Add.
            NodeList.Add(NewNode);

        }

        /// <summary>
        /// Add a node to the Storm with a direct node object.
        /// </summary>
        /// <param name="Nod"></param>
        public void AddNode(Node Nod) => NodeList.Add(Nod); 
        public DateTime GetDissipationDate()
        {
            DateTime _t = FormationDate; // Create a temporary date/time. 

            foreach (Node Node in NodeList)
            {
                // Add six hours for each point
                _t = _t.AddHours(6);
            }

            return _t; 
        }

        public Category GetNodeCategory(Node Node, CategorySystem CatSystem)
        {
            foreach (Category Category in CatSystem.Categories)
            {
                if (Node.Intensity >= Category.LowerBound && Node.Intensity <= Category.HigherBound)
                {
                    return Category; 
                }
            }

            return null; 
        }

        public DateTime GetNodeDate(int NodeId)
        {
            DateTime _t = FormationDate;

            _t = _t.AddHours(6 * NodeId);

            return _t; 
        }

        /// <summary>
        /// Get the peak category for a current storm in the current category system. Dano-style api
        /// </summary>
        /// <returns></returns>
        public Category GetPeakCategory(Storm Storm, CategorySystem CatSystem)
        {
            // get the first category.
            Category Category = CatSystem.Categories[0];
            
            // V2: move category to node. Iterate through all of the nodes. 
            foreach (Node XNode in Storm.NodeList)
            {
                // Check that we are above the first category
                if (XNode.Intensity > Category.HigherBound)
                {
                    // Get the category
                    foreach (Category Cat2 in CatSystem.Categories)
                    {
                        // check it
                        if (XNode.Intensity > Cat2.LowerBound && XNode.Intensity < Cat2.HigherBound)
                        {
                            Category = Cat2;
                        }
                    }
                }
            }

            return Category; 
        }

        public void Redo()
        {
            // restore the last node. 

            if (NodeList_Deleted.Count == 0) return; 

            Node Reincarnated = NodeList_Deleted[NodeList_Deleted.Count - 1];
            NodeList.Add(Reincarnated);
            NodeList_Deleted.Remove(Reincarnated); 
        }

        public void Undo()
        {
            // remove the last node

            if (NodeList.Count == 0) return; 

            Node Victim = NodeList[NodeList.Count - 1];
            NodeList_Deleted.Add(Victim);
            NodeList.Remove(Victim); 
        }

        /// <summary>
        /// Get the peak intensity of this storm.
        /// </summary>
        /// <returns></returns>
        public int GetPeakIntensity()
        {
            int PeakIntensity = 0;
            
            foreach (Node Nod in NodeList)
            {
                if (Nod.Intensity > PeakIntensity) PeakIntensity = Nod.Intensity;
            }

            return PeakIntensity;
        }

        public List<Node> GetNodeList() => NodeList;
        public List<Node> GetDeletedNodeList() => NodeList_Deleted;


    }

}
