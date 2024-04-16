using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum OrderStatus
    {
        Shipped =0,
        Delivered=1,
        Processing=2,
        Cancelled=3,
        Placed,
        PendingPayment,
        Returned
    }
    //public enum OrderStatusAr
    //{
    //    تم_الشحن = 0,
    //    تم_التسليم  = 1,
    //    جاري_الشحن = 2,
    //    ملغي = 3,
    //    وضعت,
    //    في_انتظار_الدفع,
    //    مرتجع
    //}
}
