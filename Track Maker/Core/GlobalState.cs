﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// Track Maker Globalstate (Priscilla v449)
    /// </summary>
    /// 
    
    /// IMPLEMENTATION VERSION 0.0 
    public class GlobalState
    {
        public CategoryManager CategoryManager { get; set; }
        public List<Basin> LoadedBasins { get; set; }
        public Project CurrentProject { get; set; }
        
        public void Init()
        {

        }
    }
}
