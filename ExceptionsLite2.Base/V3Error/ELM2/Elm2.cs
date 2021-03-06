﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionsLite2.Base
{
    /// <summary>
    /// ExceptionsLite Manager 2
    /// 
    /// ExceptionsLite © 2019,2020 starfrost.
    /// 
    /// Version 2.0.0
    /// 
    /// V2 improvements:
    /// - Uses .NET exceptions 
    /// - Uses custom delegate-based error handling
    /// - Can load error information from a Diffusion XML file (Tiralen / TrackMaker 2.1+)
    /// </summary>
    public class ExceptionsLite2
    {
        private List<V3Error> Errors { get; set; }

        public void Elm2Init()
        {
            Errors.Add(new V3Error { ErrorName = "Elm2 Internal Error", ErrorId = 0x1111DEAD, ErrorMessage = "Internal Elm2 Error" }); 
        }
        public void Elm2RegisterException(V3Error Err)
        {
            if (Err.ErrorName == null) throw new Exception($"Error {Err.ErrorId} must have a name.");

            Errors.Add(Err); 
#if DEBUG
            throw new Exception(); 
#endif
        }

        public void Elm2RegisterExceptionXml()
        {

        }
    }
}
