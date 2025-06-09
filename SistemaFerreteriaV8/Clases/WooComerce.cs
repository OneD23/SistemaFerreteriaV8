using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;

namespace WooCommerce
{
    public class WooCommerce
    {
        public Order P = new Order();
        public void Main(string[] args)
        {
            // Llama el método asíncrono de manera síncrona (en .NET Framework)
            EjecutarWooCommerce().GetAwaiter().GetResult();
        }

        public async Task EjecutarWooCommerce()
        {
            var rest = new RestAPI(
                "https://imprezado.com/wp-json/wc/v3/", // Cambia por la URL de tu tienda
                "ck_c85aadb4639e99d872518130b12a3d34890ab48f",                  // Tu Consumer Key
                "cs_397e84c9527a14c5bb54104feee45d76295b3c17"                // Tu Consumer Secret
            );

            var wc = new WCObject(rest);

            try
            {
                var productos = await wc.Order.Get(4000);
                P = productos;
                foreach (var prod in productos.line_items)
                {
                    MessageBox.Show(prod.product_id.ToString() + " " +prod.name + " " + prod.price + " " + prod.ToString());
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con WooCommerce: " + ex.Message);
            }
        }
    }
}
