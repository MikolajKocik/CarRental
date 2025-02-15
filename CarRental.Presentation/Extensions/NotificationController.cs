using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Wypożyczalnia_samochodów_online.Models;

namespace Wypożyczalnia_samochodów_online.Extensions
{
    public static class NotificationController
    {
        public static void SetNotification(this Controller controller, string type, string message)
        {
            var notification = new Notification(type, message);

            // serialize object to JSON due to simple types which are allowed in TempData
            controller.TempData["Notification"] = JsonConvert.SerializeObject(notification); 
        }
    }
}
