using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }


        [Required]
        public Product Product { get; set; } //Ligação de um para um

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double Quantity { get; set; }

        public decimal Value => Price * (decimal)Quantity; //Get da maneira moderna
    }
}
