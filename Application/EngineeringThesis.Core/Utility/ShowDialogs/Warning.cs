using System;
using System.Collections.Generic;
using System.Text;
using Forge.Forms;
using Forge.Forms.Annotations;
using MaterialDesignThemes.Wpf;

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
            this.Message = message;
        }

        public Warning(string message, string title)
        {
            this.Message = message;
            this.Title = title;
        }

        public Warning(string message, string title, string positiveAction)
        {
            this.Message = message;
            this.Title = title;
            this.PositiveAction = positiveAction;
        }

        public Warning(
            string message,
            string title,
            string positiveAction,
            string negativeAction)
        {
            this.Message = message;
            this.Title = title;
            this.PositiveAction = positiveAction;
            this.NegativeAction = negativeAction;
        }
    }
}
