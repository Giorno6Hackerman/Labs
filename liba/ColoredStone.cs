using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoneOcean
{
    [Serializable]
    public class ColoredStone : SemipreciousStone
    {
        public ColoredStone()
        {
        
        }

        public ColoredStone(string newName, string newColor, double newWeight, decimal newPrice, int newHardness, int newOrder)
            : base(newName, newColor, newWeight, newPrice, newHardness)
        {
            Order = newOrder;
        }

        private int order;

        [Required]
        [Display(Name = "Order of the Colored stone")]
        [Range(1, 4, ErrorMessage = "Please enter an integer number from 1 to 4.")]
        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                if (value > 0 && value < 5)
                    order = value;
            }
        }


        public ColoredStone(SerializationInfo info, StreamingContext context)
         //   : base(info, context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Color = (string)info.GetValue("Color", typeof(string));
            Weight = (double)info.GetValue("Weight", typeof(double));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
            Hardness = (int)info.GetValue("Hardness", typeof(int));
            Order = (int)info.GetValue("Order", typeof(int));
        }


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Color", Color);
            info.AddValue("Weight", Weight);
            info.AddValue("Price", Price);
            info.AddValue("Hardness", Hardness);
            info.AddValue("Order", Order);
        }
    }
}
