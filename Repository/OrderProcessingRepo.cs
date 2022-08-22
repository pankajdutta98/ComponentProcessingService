using ComponentProcessingService.Models;
using ComponentProcessingService.Models.DTOs;
using ComponentProcessingService.Repository.IRepository;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComponentProcessingService.Repository
{

    public class OrderProcessingRepo : IOrderProcessingRepo
    {
        private readonly ApplicationDbContext _db;
        public OrderProcessingRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        static HttpClient client = new HttpClient();

        #region Private
        private int getLatestIdCount()
        {
            try
            {
                int count = 0;
                //count = _db.processedOrderData.LastOrDefault().id;
                count = _db.processedOrderData.Count();
                return count + 1;
            }
            catch (Exception ex)
            {
                throw new Exception("ComponentProcessingService.Repository.getLatestIdCount", ex);
            }
        }
        private int saveOrderDetails(OrderDataDto orderData)
        {
            try
            {
                ProcessedOrderData data = new ProcessedOrderData();
                data.RequestId = orderData.RequestId;
                data.customerName = orderData.customerName;
                data.CustContactNo = orderData.CustContactNo;
                data.ComponentType = orderData.ComponentType;
                data.ComponentName = orderData.ComponentName;
                data.Quantity = orderData.Quantity;
                data.ProcessingCharge = orderData.ProcessingCharge;
                data.PckgngAndDlvryCharge = orderData.PckgngAndDlvryCharge;
                data.DateOfDelivery = orderData.DateOfDelivery;
                data.RequestDate = DateTime.Now;

                _db.processedOrderData.Add(data);
                return _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("ComponentProcessingService.Repository.saveOrderDetails", ex);
            }

        }
        private int savePaymentDetails(OrderDataDto orderData)
        {
            try
            {
                PaymentDetails data = new PaymentDetails();
                data.RequestId = orderData.RequestId;
                data.CardNbr = orderData.CardNbr;
                data.NameOnCard = orderData.NameOnCard;
                data.ValidThru = orderData.ValidThru;
                data.TxnDate = orderData.TxnDate;

                _db.paymentDetails.Add(data);
                return _db.SaveChanges();
            }

            catch (Exception ex)
            {
                throw new Exception("ComponentProcessingService.Repository.savePaymentDetails", ex);
            }
        }
        private static async Task<double> InvokeDeliveryAndPackagingService(char ComponentType, float? Qty)
        {
            try
            {
                //string path = "https://localhost:44391/api/PackagingAndDelivery/GetPackagingDeliveryCharge?ItemType=" + ComponentType + "&Qty=" + Qty.ToString();
                //string path = "https://localhost:44343/api/PackagingAndDelivery/GetPackagingDeliveryCharge?ItemType=" + ComponentType + "&Qty=" + Qty.ToString();
                string path = "https://roms-pkganddlvrysvc.azurewebsites.net/api/PackagingAndDelivery/GetPackagingDeliveryCharge?ItemType=" + ComponentType + "&Qty=" + Qty.ToString();
                double product = 0;
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    product = await response.Content.ReadAsAsync<double>();
                }
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("ComponentProcessingService.Repository.InvokeDeliveryAndPackagingService", ex);
            }
        }
        #endregion

        #region Public
        public async Task<ProcessResponse> getProcessResponse(Char ComponentType, float? Qty)
        {
            try
            {
                ProcessResponse response = new ProcessResponse();                
                int RecordCount = 1000 + getLatestIdCount();
                if (ComponentType == 'I')
                {
                    response.RequestId = "IT-" + DateTime.Now.ToString("ddMMyyyy") + RecordCount.ToString();
                    response.ProcessingCharge = (Qty ?? 0) * 500;
                    response.DeliveryDate = DateTime.Now.AddDays(5);

                }
                else
                {
                    response.RequestId = "AC-" + DateTime.Now.ToString("ddMMyyyy") + RecordCount.ToString();
                    response.ProcessingCharge = (Qty ?? 0) * 300;
                    response.DeliveryDate = DateTime.Now.AddDays(2);
                }

                //Call API for calculating Delivery Charges
                double PackagingAndDeliveryCharges = await getDeliveryCharge(ComponentType, Qty);
                response.PackagingCharge = PackagingAndDeliveryCharges;
                return response;
            }
            catch (Exception ex)
            {

                throw new Exception("ComponentProcessingService.Repository.getProcessResponse", ex);
            }
        }

        public static async Task<double> getDeliveryCharge(Char ComponentType, float? Qty)
        {
            //Invoke Packaging and delivery charge calculator API GetPackagingDeliveryCharge
            try
            {
                double product = await InvokeDeliveryAndPackagingService(ComponentType, Qty);
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("ComponentProcessingService.Repository.getDeliveryCharge", ex);
            }
        }

        public int CompleteProcessing(OrderDataDto orderData)
        {
            try
            {
                if (orderData == null)
                {
                    return -1;
                }
                if (String.IsNullOrEmpty(orderData.RequestId) || orderData.ComponentType == ' ')
                {
                    return -1;
                }
                if (orderData.ProcessingCharge == 0 || orderData.PckgngAndDlvryCharge == '0')
                {
                    return -1;
                }

                int status = saveOrderDetails(orderData);

                if (status > 0)
                {
                    status = savePaymentDetails(orderData);
                }
                return status;
            }
            catch (Exception ex)
            {

                throw new Exception("ComponentProcessingService.Repository.CompleteProcessing", ex);
            }
        }
        #endregion

         

    }
}
