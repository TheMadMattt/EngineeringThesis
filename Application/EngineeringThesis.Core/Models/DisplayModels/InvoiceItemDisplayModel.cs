using System.ComponentModel.DataAnnotations;

namespace EngineeringThesis.Core.Models.DisplayModels
{
    public class InvoiceItemDisplayModel: BaseModel
    {
        private string _name;

        [Required(ErrorMessageResourceType = typeof(Resources.InvoiceItemDisplayModel), ErrorMessageResourceName = "NotEmptyError")]
        public string Name
        {
            get => _name;
            set
            {
                ValidateProperty(value, "Name");
                SetProperty(ref _name, value);
            }
        }
        private string _pkwiu;

        public string PKWiU
        {
            get => _pkwiu;
            set
            {
                ValidateProperty(value, "PKWiU");
                SetProperty(ref _pkwiu, value);
            }
        }
        private string _unit;

        public string Unit
        {
            get => _unit;
            set
            {
                ValidateProperty(value, "Unit");
                SetProperty(ref _unit, value);
            }
        }
        private string _netPrice;
        [Required(ErrorMessageResourceType = typeof(Resources.InvoiceItemDisplayModel), ErrorMessageResourceName = "NotEmptyError")]
        public string NetPrice
        {
            get => _netPrice;
            set
            {
                ValidateProperty(value, "NetPrice");
                SetProperty(ref _netPrice, value);
            }
        }
        private int _amount;
        public int Amount
        {
            get => _amount;
            set
            {
                ValidateProperty(value, "Amount");
                SetProperty(ref _amount, value);
            }
        }
        private string _vat;
        [RegularExpression("^[0-9]{1,2}$", ErrorMessageResourceType = typeof(Resources.InvoiceItemDisplayModel), ErrorMessageResourceName = "VATerror")]
        public string VAT
        {
            get => _vat;
            set
            {
                ValidateProperty(value, "VAT");
                SetProperty(ref _vat, value);
            }
        }
        private string _vatSum;

        public string VATSum
        {
            get => _vatSum;
            set
            {
                ValidateProperty(value, "VATSum");
                SetProperty(ref _vatSum, value);
            }
        }
        private string _netSum;

        public string NetSum
        {
            get => _netSum;
            set
            {
                ValidateProperty(value, "NetSum");
                SetProperty(ref _netSum, value);
            }
        }
        private string _grossSum;

        public string GrossSum
        {
            get => _grossSum;
            set
            {
                ValidateProperty(value, "GrossSum");
                SetProperty(ref _grossSum, value);
            }
        }
        private string _comments;

        public string Comments
        {
            get => _comments;
            set
            {
                ValidateProperty(value, "Comments");
                SetProperty(ref _comments, value);
            }
        }

        public Invoice Invoice { get; set; }
        public int InvoiceId { get; set; }
    }
}
