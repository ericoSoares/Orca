using System;
using System.Collections.Generic;
using System.Text;

namespace tcc.Models
{
    public enum ERelationshipType
    {
        ASSOCIATION = 1,
        AGGREGATION,
        INHERITANCE,
        COMPOSITION,
        IMPLEMENTATION,
        RECEPTION_IN_METHOD,
        RECEPTION_IN_CONSTRUCTOR,
        INSTANTIATION_IN_CONSTRUCTOR,
        INSTANTIATION_IN_METHOD,
        INSTANTIATION_IN_CLASS,
        DEPENDENCY
    }
}
