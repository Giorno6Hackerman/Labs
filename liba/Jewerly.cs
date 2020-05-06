using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoneOcean
{
    [Serializable]
    public class Jewerly : ISerializable
    {
        public Jewerly()
        {
        
        }


        public Jewerly(string newName, decimal newPrice, PreciousMetal newJMetal, Stone newJStone)
        {
            Name = newName;
            Price = newPrice;
            JMetal = newJMetal;
            JStone = newJStone;
        }


        private string name;
        private decimal price;
        private PreciousMetal jMetal;
        private Stone jStone;

        [Required]
        [Display(Name = "Name")]
        public string Name
        {
            get
            {
                return name ?? "Unknown";
            }
            set
            {
                if (value != null && value.Length > 0)
                    name = value;
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

        [Required]
        [Display(Name = "Precious metal used")]
        public PreciousMetal JMetal
        {
            get
            {
                return jMetal;
            }
            set
            {
                if (value != null)
                    jMetal = value;
            }
        }

        [Required]
        [Display(Name = "Precious stone used")]
        public Stone JStone
        {
            get
            {
                return jStone;
            }
            set
            {
                if (value != null)
                    jStone = value;
            }
        }


        public Jewerly(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
            JMetal = (PreciousMetal)info.GetValue("JMetal", typeof(PreciousMetal));
            JStone = (Stone)info.GetValue("JStone", typeof(Stone));
            
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Price", Price);
            info.AddValue("JMetal", JMetal);
            info.AddValue("JStone", JStone);
        }
    }
}
