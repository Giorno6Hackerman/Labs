using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoneOcean
{
    [Serializable]
    public enum Origin { Natural, Synthetic }

    [Serializable]
    public class Jewel : Stone
    {
        public Jewel()
        {
        
        }

        public Jewel(string newName, string newColor, double newWeight, decimal newPrice, int newJClass, Origin newJType) 
            : base(newName, newColor, newWeight, newPrice)
        {
            JClass = newJClass;
            JType = newJType;
        }

        private int jClass;

        [Required]
        [Display(Name = "Type of jewel")]
        public Origin JType { set; get; } = Origin.Natural;

        [Required]
        [Display(Name = "Jewel class")]
        [Range(1, 3, ErrorMessage = "Please enter an integer number from 1 to 3.")]
        public int JClass
        {
            get
            {
                return jClass;
            }
            set
            {
                if (value > 0 && value < 4)
                    jClass = value;
            }
        }


        public Jewel(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Color = (string)info.GetValue("Color", typeof(string));
            Weight = (double)info.GetValue("Weight", typeof(double));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
            JType = (Origin)info.GetValue("JType", typeof(Origin));
            JClass = (int)info.GetValue("JClass", typeof(int));
        }


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Color", Color);
            info.AddValue("Weight", Weight);
            info.AddValue("Price", Price);
            info.AddValue("JType", JType);
            info.AddValue("JClass", JClass);
        }
    }
}
