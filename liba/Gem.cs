using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoneOcean
{
    [Serializable]
    public class Gem : SemipreciousStone
    {
        public Gem()
        {
        
        }

        public Gem(string newName, string newColor, double newWeight, decimal newPrice, int newHardness, string newMainHue, string newAdditionalHue) 
            : base(newName, newColor, newWeight, newPrice, newHardness)
        {
            MainHue = newMainHue;
            AdditionalHue = newAdditionalHue;
        }

        
        private string mainHue;
        private string additionalHue;

        [Required]
        [Display(Name = "Main hue of the gem")]
        public string MainHue
        {
            get
            {
                return mainHue ?? "Unknown";
            }
            set
            {
                if (value != null && value.Length > 0)
                    mainHue = value;
            }
        }

        [Required]
        [Display(Name = "Additional hue of the gem")]
        public string AdditionalHue
        {
            get
            {
                return additionalHue ?? "Unknown";
            }
            set
            {
                if (value != null && value.Length > 0)
                    additionalHue = value;
            }
        }


        public Gem(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            MainHue = (string)info.GetValue("MainHue", typeof(string));
            AdditionalHue = (string)info.GetValue("AdditionalHue", typeof(string));
        }


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MainHue", MainHue);
            info.AddValue("AdditionalHue", AdditionalHue);
        }
    }
}
