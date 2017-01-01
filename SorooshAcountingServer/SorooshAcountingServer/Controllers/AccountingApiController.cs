using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SorooshAcountingServer.Models;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace SorooshAcountingServer.Controllers
{
    public class AccountingApiController : ApiController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationDbContext _db;
        public ApplicationDbContext db
        {
            get
            {
                return _db ?? new ApplicationDbContext();
            }
            private set
            {
                _db = value;
            }
        }
        [HttpPost]
        public LogInfoVM endLog(int logId)
        {
            try
            {
                var log = db.Logs.Where(x => x.Log_Id == logId).First();
                var time = DateTime.Now;
                log.Log_endTime =
                    (time.Date == DateTime.Parse(log.Log_date).Date)
                    ? time.TimeOfDay.ToString()
                    : "false";
                db.SaveChanges();
                return new LogInfoVM()
                {
                    date = log.Log_date,
                    startTime = log.Log_startTime,
                    Id = log.Log_Id,
                    endTime = log.Log_endTime,
                    Respond = new Respond("success")
                };
            }
            catch (Exception ex)
            {
                return new LogInfoVM()
                {
                    Respond = new Respond(ex.Message, status.unknownError)
                };
            }
        }
        [HttpPost]
        public async Task<LogInfoVM> startLog(string username)
        {
            try
            {
                var user = await UserManager.FindByNameAsync(username);
                var time = DateTime.Now;
                var newLog = new Log()
                {
                    Log_user = user,
                    Log_date = time.Date.ToString(),
                    Log_startTime = time.TimeOfDay.ToString()
                };
                db.Logs.Add(newLog);
                db.SaveChanges();
                return new LogInfoVM()
                {
                    date = newLog.Log_date,
                    startTime = newLog.Log_startTime,
                    Id = newLog.Log_Id,
                    Respond = new Respond("success")
                };
            }
            catch (Exception ex)
            {
                return new LogInfoVM()
                {
                    Respond = new Respond(ex.Message, status.unknownError)
                };
            }
        }
        [HttpPost]
        public async Task<AuthenticatedUserVM> Authenticate(AuthenticationInfoVM vm)
        {
            var user = await UserManager.FindByNameAsync(vm.user);
            bool res = await UserManager.CheckPasswordAsync(user, vm.pass);
            if (res)
            {
                return new AuthenticatedUserVM()
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    ImageUrl = user.ImgUrl,
                    success = true
                };
            }
            return new AuthenticatedUserVM() { success = false };
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
