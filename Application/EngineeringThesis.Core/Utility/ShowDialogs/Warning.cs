using Forge.Forms;
using Forge.Forms.Annotations;

namespace EngineeringThesis.Core.Utility.ShowDialogs
{
    [Form(Mode = DefaultFields.None)]
    [Title("{Binding Title}", 6, IsVisible = "{Binding Title|IsNotEmpty}", Icon = "Warning")]
    [Text("{Binding Message}", 7, IsVisible = "{Binding Message|IsNotEmpty}")]
    [Action("positive", "{Binding PositiveAction}", 13, ClosesDialog = true, Icon = "{Binding PositiveActionIcon}", IsDefault = true, IsVisible = "{Binding PositiveAction|IsNotEmpty}")]
    [Action("negative", "{Binding NegativeAction}", 8, ClosesDialog = true, Icon = "{Binding NegativeActionIcon}", IsCancel = true, IsVisible = "{Binding NegativeAction|IsNotEmpty}")]
    public sealed class Warning : DialogBase
    {
        public Warning()
        {
        }

        public Warning(string message)
        {
            Message = message;
        }

        public Warning(string message, string title)
        {
            Message = message;
            Title = title;
        }

        public Warning(string message, string title, string positiveAction)
        {
            Message = message;
            Title = title;
            PositiveAction = positiveAction;
        }

        public Warning(
            string message,
            string title,
            string positiveAction,
            string negativeAction)
        {
            Message = message;
            Title = title;
            PositiveAction = positiveAction;
            NegativeAction = negativeAction;
        }
    }
}
