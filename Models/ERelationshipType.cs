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
        RECEPTION_ON_METHOD,
        RECEPTION_ON_CONSTRUCTOR,
        INSTANTIATION_ON_CREATION,
        INSTANTIATION_ON_METHOD
    }
}
