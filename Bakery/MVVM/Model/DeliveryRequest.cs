using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bakery.MVVM.Model
{
    public class DeliveryRequest
    {
        public static List<DeliveryRequest> Collection { get; set; }

        public int ID { get; set; }
        public List<Product> Products { get; set; }
        public eRequestStatus Status { get; set; }

        public string Name => "Заявка №" + ID;
        public string TProductsCount => "Кол-во продуктов: " + Products.Count + " шт.";
        public string StatusName
        {
            get
            {
                string result = "";

                switch (Status)
                {
                    case eRequestStatus.Consideration:
                        result = "На рассмотрении";
                        break;
                    case eRequestStatus.Confirmed:
                        result = "Подтверждена";
                        break;
                    case eRequestStatus.Completed:
                        result = "Выполнена";
                        break;
                    case eRequestStatus.Denied:
                        result = "Отклонена";
                        break;
                }

                return result;
            }
        }

        public Command COM_Denied => new Command(c =>
        {
            if (Status == eRequestStatus.Denied)
            {
                new MessageView("Заявка уже отклонена.");
                return;
            }
            if (Status == eRequestStatus.Completed)
            {
                new MessageView("Невозможно отклонить. Завка уже выполнена.");
                return;
            }

            Status = eRequestStatus.Denied;

            DataContextExtracter<ViewModel.ManageRequest>.Extract().NewRequestStatusName = StatusName;
            new MessageView("Установлен статус: " + StatusName);
        });
        public Command COM_Consideration => new Command(c =>
        {
            Status = eRequestStatus.Consideration;

            DataContextExtracter<ViewModel.ManageRequest>.Extract().NewRequestStatusName = StatusName;
            new MessageView("Установлен статус: " + StatusName);
        });
        public Command COM_Confirmed => new Command(c =>
        {
            if (Status != eRequestStatus.Consideration)
            {
                new MessageView("Можно подтвердить только заявки на рассмотрении.");
                return;
            }

            Status = eRequestStatus.Confirmed;

            DataContextExtracter<ViewModel.ManageRequest>.Extract().NewRequestStatusName = StatusName;
            new MessageView("Установлен статус: " + StatusName);
        });

        public Command COM_Manage => new Command(c =>
        {
            AppManager.OpenWindow(new View.ManageRequest(), new ViewModel.ManageRequest(this), true);
        });
        public Command COM_OpenInfo => new Command(c =>
        {
            AppManager.OpenWindow(new View.OpenRequest(), new ViewModel.OpenRequest(this), false);
        });

        public DeliveryRequest()
        {

        }
        
        public DeliveryRequest(List<Product> productList)
        {
            Products = productList;
            Status = eRequestStatus.Consideration;
        }

        #region SQL

        public static void Fill()
        {
            Collection = new List<DeliveryRequest>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM deliveryrequests", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        Collection.Add(new DeliveryRequest()
                        {
                            ID = dr.GetInt32("ID"),
                            Products = Product.GetProductsForDeliveryRequest(dr.GetInt32("ID")),
                            Status = (eRequestStatus)dr.GetInt32("Status"),
                        });
                db.CloseConnection();
            }

            AppManager.UpdateSearchTrigger();
        }

        public static List<DeliveryRequest> GetConfirmedRequests() =>
            Collection.FindAll(s => s.Status == eRequestStatus.Confirmed);
        
        public static List<DeliveryRequest> GetActiveRequests() =>
            Collection.FindAll(s => s.Status == eRequestStatus.Consideration || s.Status == eRequestStatus.Confirmed);

        public void Add()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO deliveryrequests " +
                    $"( ID, Status ) " +
                    $"VALUES " +
                    $"( 0, {(int)Status} );";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                Fill();
                ID = Collection.Last().ID;

                foreach (var product in Products)
                    product.AddProductToDeliveryRequest(this);

                Fill();
            }
        }

        public void Edit()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "UPDATE deliveryrequests SET " +
                    $"Status =  {(int)Status} " +
                    $"WHERE ID = {ID};";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                Product.DeleteProductsFromDeliveryRequest(this);
                foreach (var product in Products)
                    product.AddProductToDeliveryRequest(this);

                Fill();
            }
        }

        #endregion
    }

    public enum eRequestStatus
    {
        Consideration,
        Confirmed,
        Completed,
        Denied,
    }
}
