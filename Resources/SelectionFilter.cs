using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskNefedov.Resources
{
    public class SelectionFilter : ISelectionFilter
    {
        public ElementId id { get; set; }
        public bool AllowElement(Element elem)
        {
            if (elem.Id.IntegerValue == id.IntegerValue)
            {
                return false;
            }
            return true;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
