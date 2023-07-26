using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using TestTaskNefedov.ViewModels;
using TestTaskNefedov.Views;

namespace TestTaskNefedov.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class Command : ExternalCommand
    {
        public override void Execute()
        {
            var viewModel = new TestTaskNefedovViewModel();
            viewModel.commandData = this.ExternalCommandData;
            viewModel.selectCommandExecute(null);
            var view = new TestTaskNefedovView(viewModel);
            viewModel.CloseRequest += (s, e) => view.Close();
            viewModel.HideRequest += (s, e) => view.Hide();
            viewModel.ShowRequest += (s, e) => view.ShowDialog();            
            view.ShowDialog();            
        }
    }
}