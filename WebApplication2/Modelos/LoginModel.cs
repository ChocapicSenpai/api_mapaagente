using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Modelos
{
    public class LoginModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
