using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GH.Utilities
{
    public class Funtions
    {
        private object GetPropValue(object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }

        private object GetColumName(object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }

        //              .OrderBy(b => GetPropValue(b, pagingParameterModel.term))

    }
}