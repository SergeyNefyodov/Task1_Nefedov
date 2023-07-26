using TestTaskNefedov.ViewModels;

namespace TestTaskNefedov.Views
{
    public partial class TestTaskNefedovView
    {
        public TestTaskNefedovView(TestTaskNefedovViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}