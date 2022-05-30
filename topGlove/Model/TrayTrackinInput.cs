using System;
using System.ComponentModel.DataAnnotations;


namespace topGlove.Model
{
    public class TrayTrackinInput
    {
        [Key]
        public int UserId { get; set; }
        public string FormerType { get; set; }
        public string NoOfFormer { get; set; }
        public string BatchNo { get; set; }
        public DateTime DateTime { get; set; }
        public string User { get; set; }
        public string Process { get; set; }
        public string Status { get; set; }
        public string AdditionalInfo { get; set; }
        public string TrolleyNo { get; set; }
        public string Shift { get; set; }

    }
}
