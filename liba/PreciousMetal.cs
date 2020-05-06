using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoneOcean
{
    [Serializable]
    public class PreciousMetal : ISerializable
    {
        public PreciousMetal()
        {
        
        }

        public PreciousMetal(string newType, int newFineness, double newWeight, decimal newPrice)
        {
            MType = newType;
            Fineness = newFineness;
            Weight = newWeight;
            Price = newPrice;
        }

        private string mType;
        private int fineness;
        private double weight;
        private decimal price;

        [Required]
        [Display(Name = "Type of metal")]
        public string MType
        {
            get
            {
                return mType ?? "Unknown";
            }
            set
            {
                if (value != null && value.Length > 0)
                    mType = value;
            }
        }

        [Required]
        [Display(Name = "Fineness of metal")]
        [Range(1, 999, ErrorMessage = "Please enter an integer number from 1 to 999.")]
        public int Fineness
        {
            get
            {
                return fineness;
            }
            set
            {
                if (value > 0 && value < 1000)
                    fineness = value;
            }
        }

        [Required]
        [Display(Name = "Weight")]
        [RegularExpression(@"^[0-9]*[,]?[0-9]+$", ErrorMessage = "Please enter a float number > 0.")]
        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                if (value > 0)
                    weight = value;
            }
        }

        [Required]
        [Display(Name = "Price")]
        [RegularExpression(@"^[0-9]*[,]?[0-9]+$", ErrorMessage = "Please enter a float number > 0.")]
        public decimal Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value > 0)
                    price = value;
            }
        }


        public PreciousMetal(SerializationInfo info, StreamingContext context)
        {
            MType = (string)info.GetValue("MType", typeof(string));
            Fineness = (int)info.GetValue("Fineness", typeof(int));
            Weight = (double)info.GetValue("Weight", typeof(double));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MType", MType);
            info.AddValue("Fineness", Fineness);
            info.AddValue("Weight", Weight);
            info.AddValue("Price", Price);
        }
    }
}
