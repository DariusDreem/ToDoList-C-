using Framework.Migrations;
using Framework.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class UserModel
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }

        public UserModel(string name)
        {

            this.name = name;
        }
    }
}