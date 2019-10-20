using System;
using System.Collections.Generic;
using System.Text;
using Forge.Forms;
using Forge.Forms.Annotations;
using MaterialDesignThemes.Wpf;

namespace EngineeringThesis.Core.Utility.ShowDialogs
{
    [Form(Mode = DefaultFields.None)]
    [Title("{Binding Title}", 6, IsVisible = "{Binding Title|IsNotEmpty}", Icon = "Information")]
    [Text("{Binding Message}", 7, IsVisible = "{Binding Message|IsNotEmpty}")]
    [Action("positive", "{Binding PositiveAction}", 8, ClosesDialog = true, Icon = "{Binding PositiveActionIcon}", IsCancel = true, IsDefault = true, IsVisible = "{Binding PositiveAction|IsNotEmpty}")]
    public sealed class Information : DialogBase
    {
        public Information()
        {
        }

        public Information(string message)
        {
            this.Message = message;
        }

        public Information(string message, string title)
        {
            this.Message = message;
            this.Title = title;
        }

        public Information(string message, string title, string positiveAction)
        {
            this.Message = message;
            this.Title = title;
            this.PositiveAction = positiveAction;
        }
    }
}
