using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskNefedov.Resources
{
    internal enum CopyParameterProblem
    {
        None = 0,
        NotFindParameter = 1,
        UnitTypesError = 2,
        ReadOnlyError = 3,
        ToStringWarning = 4
    }
}
