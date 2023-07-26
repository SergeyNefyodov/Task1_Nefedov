using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskNefedov.Resources
{
    internal class Logger
    {
        internal CopyParameterProblem CopyParameterProblem { get; set; }
        internal string ParameterName { get; set; }
        internal void ShowResults()
        {
            string result = "";
            if (CopyParameterProblem == CopyParameterProblem.NotFindParameter)
            {
                result = "Параметр \"" + ParameterName + "\" не найден у целевого элемента";
            }
            else if (CopyParameterProblem == CopyParameterProblem.UnitTypesError)
            {
                result = "Копирование значения параметра \"" + ParameterName + "\" не проведено, так как единицы измерения в выбранных элементах отличаются.";
            }
            else if (CopyParameterProblem == CopyParameterProblem.ReadOnlyError)
            {
                result = "Копирование значения параметра \"" + ParameterName + "\" не проведено, так как он доступен только для чтения в целевом элементе.";
            }
            else if (CopyParameterProblem == CopyParameterProblem.ToStringWarning)
            {
                result = "Параметр \"" + ParameterName + "\" имеет тип данных, отличный от строки у первого элемента, и строковый тип у второго элемента. Значение было скопировано в виде строки.";
            }
            else
            {
                result = "Копирование значения параметра \"" + ParameterName + "\" успешно проведено.";
            }
            TaskDialog dialog = new TaskDialog("Результаты работы");
            dialog.TitleAutoPrefix = false;
            dialog.MainInstruction = result;
            dialog.Show();
        }
    }
}
