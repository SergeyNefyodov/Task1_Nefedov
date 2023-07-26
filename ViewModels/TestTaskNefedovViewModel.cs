using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestTaskNefedov.Resources;

namespace TestTaskNefedov.ViewModels
{
    public sealed class TestTaskNefedovViewModel : ObservableObject
    {
        public TestTaskNefedovViewModel()
        {
            SelectCommand = new RelayCommand(selectCommandExecute, canSelectCommandExecuted);
            CopyParameters = new RelayCommand(copyParametersExecute, canCopyParametersExecuted);
        }
        internal ExternalCommandData commandData { get; set; }
        internal UIDocument uidoc { get => commandData.Application.ActiveUIDocument; }
        internal Document doc { get => uidoc.Document; }
        public Element firstElement { get; set; }
        public Element secondElement { get; set; }
        public Parameter SelectedParameter { get; set; }
        private List<Parameter> parameters;
        public List<Parameter> Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
                OnPropertyChanged();
            }
        }
        internal CopyParameterProblem Problem { get; set; } = CopyParameterProblem.None;
        internal Logger Logger
        {
            get
            {
                return new Logger() { CopyParameterProblem = Problem, ParameterName = SelectedParameter.Definition.Name };
            }
        }
        public RelayCommand SelectCommand { get; set; }
        private bool canSelectCommandExecuted(object p)
        {
            return true;
        }
        public void selectCommandExecute(object p)
        {
            try
            {
                RaiseHideRequest();
                var ref0 = uidoc.Selection.PickObject(ObjectType.Element,
                    "Выберите первый элемент (из него будут скопированы значения параметров)");
                firstElement = doc.GetElement(ref0);
                var selectionFilter = new SelectionFilter()
                {
                    id = firstElement.Id,
                };
                var ref1 = uidoc.Selection.PickObject(ObjectType.Element, selectionFilter,
                    "Выберите второй элемент (в него будут скопированы значения  параметров)");
                secondElement = doc.GetElement(ref1);
                var collection = new List<Parameter>();
                var dict = new Dictionary<Parameter, bool>();
                if (firstElement != null)
                {
                    var parameters = firstElement.Parameters;
                    foreach (Parameter parameter in parameters)
                    {
                        collection.Add(parameter);
                        dict.Add(parameter, false);
                    }
                }
                Parameters = collection;
                RaiseShowRequest();
            }
            catch { }
        }

        public RelayCommand CopyParameters { get; set; }
        private bool canCopyParametersExecuted(object p)
        {
            if (firstElement != null && secondElement != null && SelectedParameter != null)
            {
                return true;
            }
            return false;
        }
        private void copyParametersExecute(object p)
        {
            using (Transaction t = new Transaction(doc, "Копирование значения параметра"))
            {
                t.Start();
                Parameter paramToCopy = FindParameter();
                if (paramToCopy != null)
                {
                    CopyValue(paramToCopy);
                }                
                t.Commit();
            }
            RaiseCloseRequest();
            Logger.ShowResults();
        }

        private void CopyValue(Parameter paramToCopy)
        {
            if (paramToCopy.IsReadOnly == true)
            {
                Problem = CopyParameterProblem.ReadOnlyError;
                return;
            }
            if (paramToCopy.StorageType == SelectedParameter.StorageType)
            {
                switch (SelectedParameter.StorageType)
                {
                    case StorageType.String:
                        paramToCopy.Set(SelectedParameter.AsString());
                        break;
                    case StorageType.Integer:
                        paramToCopy.Set(SelectedParameter.AsInteger());
                        break;
                    case StorageType.ElementId:
                        paramToCopy.Set(SelectedParameter.AsElementId());
                        break;
                    case StorageType.Double:
                        if (paramToCopy.GetTypeId() == SelectedParameter.GetTypeId())
                        {
                            paramToCopy.Set(SelectedParameter.AsDouble());
                        }
                        else
                        {
                            Problem = CopyParameterProblem.UnitTypesError;
                        }
                        break;
                }
            }
            else if (paramToCopy.StorageType == StorageType.String)
            {
                paramToCopy.Set(SelectedParameter.AsValueString());
                Problem = CopyParameterProblem.ToStringWarning;
            }
            else
            {
                Problem = CopyParameterProblem.NotFindParameter;
            }
        }

        private Parameter FindParameter()
        {
            Parameter paramToCopy;
            if (SelectedParameter.IsShared)
            {
                paramToCopy = secondElement.get_Parameter(SelectedParameter.GUID);
            }
            else if (SelectedParameter.Id.IntegerValue < 0)
            {
                paramToCopy = secondElement.get_Parameter((BuiltInParameter)SelectedParameter.Id.IntegerValue);
            }
            else
            {
                paramToCopy = secondElement.LookupParameter(SelectedParameter.Definition.Name);
            }
            if (paramToCopy == null) 
            {
                Problem = CopyParameterProblem.NotFindParameter;
            }
            return paramToCopy;
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}