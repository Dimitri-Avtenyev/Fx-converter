using System.ComponentModel.DataAnnotations;

namespace Fx_converter.Models
{
    public class Currency {
        public int Id { get; set; }
        [MaxLength(10)]
		public string Symbol { get; set; }
        
    }
}
