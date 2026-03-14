using System;
using System.ComponentModel.DataAnnotations;

namespace Web_Programming_Project.Models
{
    public class Question
    {
        [Key]
        public int ID { get; set; }

        public string Text { get; set; }       
        public string Answer { get; set; }    
        public string AskerName { get; set; }  
        public DateTime Date { get; set; }     
    }
}