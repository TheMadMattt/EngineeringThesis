using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EngineeringThesis.Core.Models.DisplayModels
{
    public class CustomerDisplayModel: BaseModel
    {
        private string _name;
        [Required(ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "NotEmptyError")]
        public string Name
        {
            get => _name;
            set
            {
                ValidateProperty(value, "Name");
                SetProperty(ref _name, value);
            }
        }
        private string _zipCode;
        [AllowNull]
        [RegularExpression("^[0-9]{2}\\-[0-9]{3}$", ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "NotEnoughNumbers")]
        public string ZipCode
        {
            get => _zipCode;
            set
            {
                ValidateProperty(value, "ZipCode");
                SetProperty(ref _zipCode, value);
            }
        }
        private string _city;
        [Required(ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "NotEmptyError")]
        public string City
        {
            get => _city;
            set
            {
                ValidateProperty(value, "City");
                SetProperty(ref _city, value);
            }
        }
        private string _street;
        [Required(ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "NotEmptyError")]
        public string Street
        {
            get => _street;
            set
            {
                ValidateProperty(value, "Street");
                SetProperty(ref _street, value);
            }
        }
        private string _streetNumber;
        [Required(ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "NotEmptyError")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "WrongFormat")]
        public string StreetNumber
        {
            get => _streetNumber;
            set
            {
                ValidateProperty(value, "StreetNumber");
                SetProperty(ref _streetNumber, value);
            }
        }
        private string _flatNumber;
        [AllowNull]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "WrongFormat")]
        public string FlatNumber
        {
            get => _flatNumber;
            set
            {
                ValidateProperty(value, "FlatNumber");
                SetProperty(ref _flatNumber, value);
            }
        }
        private string _phoneNumber;
        [AllowNull]
        [RegularExpression("^(\\d{9})$", ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "NotEnoughNumbers")]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                ValidateProperty(value, "PhoneNumber");
                SetProperty(ref _phoneNumber, value);
            }
        }
        private string _nip;
        [RegularExpression("^(\\d{3}\\-\\d{2}\\-\\d{2}\\-\\d{3})$", ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "NotEnoughNumbers")]
        [AllowNull]
        public string NIP
        {
            get => _nip;
            set
            {
                ValidateProperty(value, "NIP");
                SetProperty(ref _nip, value);
            }
        }
        private string _regon;
        [RegularExpression("^(\\d{3}\\-\\d{2}\\-\\d{2}\\-\\d{2})$", ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "NotEnoughNumbers")]
        [AllowNull]
        public string REGON
        {
            get => _regon;
            set
            {
                ValidateProperty(value, "REGON");
                SetProperty(ref _regon, value);
            }
        }
        private string _bankAccountNumber;
        [RegularExpression("^(\\d{26})$", ErrorMessageResourceType = typeof(Resources.CustomerDisplayModel), ErrorMessageResourceName = "NotEnoughNumbers")]
        [AllowNull]
        public string BankAccountNumber
        {
            get => _bankAccountNumber;
            set
            {
                ValidateProperty(value, "BankAccountNumber");
                SetProperty(ref _bankAccountNumber, value);
            }
        }
        private string _comments;
        [AllowNull]
        public string Comments
        {
            get => _comments;
            set
            {
                ValidateProperty(value, "Comments");
                SetProperty(ref _comments, value);
            }
        }
        public int CustomerTypeId { get; set; }
        public CustomerType CustomerType { get; set; }

    }
}
