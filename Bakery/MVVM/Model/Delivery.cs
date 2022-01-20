using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.Model
{
    public class Delivery
    {
        public static List<Delivery> Collection { get; set; }

        public int ID { get; set; }
        public List<Product> Products { get; set; }
        public Provider Provider { get; set; }
        public DateTime DeliveryDate { get; set; }

        public string Name => "Поставка №" + ID;
        public string TDeliveryDate => $"Дата поставки {DeliveryDate}";
        public string TTotalCount => "Всего продуктов: " + Products.Count;
        public string FullName => Name + " | " + TDeliveryDate;

        public Command COM_OpenInfo => new Command(c =>
        {
            AppManager.OpenWindow(new View.OpenDelivery(), new ViewModel.OpenDelivery(this));
        });

        public Delivery()
        {

        }
        
        public Delivery(DeliveryRequest request)
        {
            Products = request.Products;
        }

        #region SQL

        public static void Fill()
        {
            Collection = new List<Delivery>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM deliveries", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        Collection.Add(new Delivery()
                        {
                            ID = dr.GetInt32("ID"),
                            Provider = Provider.Collection.Find(s => s.ID == dr.GetInt32("ProviderID")),
                            Products = Product.GetProductsForDelivery(dr.GetInt32("ID")),
                            DeliveryDate = dr.GetDateTime("DeliveryDate"),
                        });
                db.CloseConnection();
            }

            AppManager.UpdateSearchTrigger();
        }

        public void Add()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO deliveries " +
                    $"( ID, DeliveryDate, ProviderID ) " +
                    $"VALUES " +
                    $"( 0, @DeliveryDate, {Provider.ID} );";

                MySqlParameter[] parames = new[]
                {
                        new MySqlParameter("DeliveryDate", MySqlDbType.DateTime) { Value = DeliveryDate },
                };

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.Parameters.AddRange(parames);
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                Fill();
                ID = Collection.Last().ID;

                foreach (var product in Products)
                {
                    product.AddProductToDelivery(this);

                    var storageProd = Product.Collection.Find(s => s.ID == product.ID);
                    storageProd.Weight += product.Weight;
                    storageProd.Edit();
                }

                Fill();
            }
        }

        public void Edit()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "UPDATE deliveries SET " +
                    $"DeliveryDate =  @DeliveryDate, " +
                    $"ProviderID =  {Provider.ID} " +
                    $"WHERE ID = {ID};";

                MySqlParameter[] parames = new[]
                {
                        new MySqlParameter("DeliveryDate", MySqlDbType.DateTime) { Value = DeliveryDate },
                };

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.Parameters.AddRange(parames);
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                Product.DeleteProductsFromDelivery(this);
                foreach (var product in Products)
                    product.AddProductToDelivery(this);

                Fill();
            }
        }

        #endregion
    }
}
