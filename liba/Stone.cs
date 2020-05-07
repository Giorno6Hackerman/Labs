using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoneOcean
{
    [Serializable]
    public class Stone : ISerializable
    {
        public Stone()
        {
        
        }

        public Stone(string newName, string newColor, double newWeight, decimal newPrice)
        {
            Name = newName;
            Color = newColor;
            Weight = newWeight;
            Price = newPrice;
        }

        protected string name;
        protected string color;
        protected double weight;
        protected decimal price;

        [Required]
        [Display(Name = "Name of the stone")]
        public string Name
        {
            get 
            { 
                return name ?? "Unknown"; 
            }
            set 
            {
                if (value != null && value.Length != 0)
                    name = value;
                
            }
        }

        [Required]
        [Display(Name = "Color of the stone")]
        public string Color
        {
            get 
            { 
                return color ?? "Unknown"; 
            }
            set
            {
                if (value != null && value.Length != 0)
                    color = value;
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


        public Stone(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Color = (string)info.GetValue("Color", typeof(string));
            Weight = (double)info.GetValue("Weight", typeof(double));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
        }


        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Color", Color);
            info.AddValue("Weight", Weight);
            info.AddValue("Price", Price);
        }
    }
}
