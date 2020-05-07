using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoneOcean
{
    [Serializable]
    public class SemipreciousStone : Stone
    {
        public SemipreciousStone()
        {
        
        }

        public SemipreciousStone(string newName, string newColor, double newWeight, decimal newPrice, int newHardness) 
            : base(newName, newColor, newWeight, newPrice)
        {
            Hardness = newHardness;
        }

        protected int hardness;

        [Required]
        [Display(Name = "Hardness of the semiprecious stone")]
        [Range(1, 7, ErrorMessage = "Please enter an integer number from 1 to 7.")]
        public int Hardness
        {
            get
            {
                return hardness;
            }
            set
            {
                if (value > 0 && value < 8)
                    hardness = value;
            }
        }

        public SemipreciousStone(SerializationInfo info, StreamingContext context)
        //    :base(info, context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Color = (string)info.GetValue("Color", typeof(string));
            Weight = (double)info.GetValue("Weight", typeof(double));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
            Hardness = (int)info.GetValue("Hardness", typeof(int));
        }


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Color", Color);
            info.AddValue("Weight", Weight);
            info.AddValue("Price", Price);
            info.AddValue("Hardness", Hardness);
        }
    }
}
