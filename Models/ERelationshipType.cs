using System;
using System.Collections.Generic;
using System.Text;

namespace tcc.Models
{
    enum ERelationshipType
    {
        ASSOCIATION = 1,
        AGGREGATION,
        INHERITANCE,
        COMPOSITION,
        IMPLEMENTATION,
        RECEIVED_ON_METHOD,
        RECEIVED_ON_CONSTRUCTOR,
        INSTANTIATED_ON_CREATION,
        INSTANTIATED_ON_METHOD
    }
}
