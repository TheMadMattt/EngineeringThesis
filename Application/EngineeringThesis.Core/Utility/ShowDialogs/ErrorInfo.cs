using Forge.Forms;
using Forge.Forms.Annotations;

namespace EngineeringThesis.Core.Utility.ShowDialogs
{
    [Form(Mode = DefaultFields.None)]
    [Title("{Binding Title}", 6, IsVisible = "{Binding Title|IsNotEmpty}", Icon = "Error")]
    [Text("{Binding Message}", 7, IsVisible = "{Binding Message|IsNotEmpty}")]
    [Action("positive", "{Binding PositiveAction}", 8, ClosesDialog = true, Icon = "{Binding PositiveActionIcon}", IsCancel = true, IsDefault = true, IsVisible = "{Binding PositiveAction|IsNotEmpty}")]
    public sealed class ErrorInfo : DialogBase
    {
        public ErrorInfo()
        {
        }

        public ErrorInfo(string message)
        {
            Message = message;
        }

        public ErrorInfo(string message, string title)
        {
            Message = message;
            Title = title;
        }

        public ErrorInfo(string message, string title, string positiveAction)
        {
            Message = message;
            Title = title;
            PositiveAction = positiveAction;
        }
    }
}
