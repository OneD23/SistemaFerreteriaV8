using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8.Clases
{   
    public  class OneKeys
    {
        public Color Naranja = Color.FromArgb(240, 111, 49);
        public Color Gris = Color.FromArgb(48, 48, 45);
        public Color Negro = Color.FromArgb(255, 255, 255);
        public Color Rojo = Color.FromArgb(255, 0, 0);
        public Color Azul = Color.FromArgb(0, 0, 255);
        public Color Verde = Color.FromArgb(0, 255, 0);

   public string URI = "mongodb://localhost:27017/";
        /*ING =>*/ //public string URI = "mongodb://192.168.2.241:27017/";
        //public string URI = "mongodb://admin:1234@192.168.100.6:27017/";
        //Server Ing => 192.168.2.241zz
    }
}
