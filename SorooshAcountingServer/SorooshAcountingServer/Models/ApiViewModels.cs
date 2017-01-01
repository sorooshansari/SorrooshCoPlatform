using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SorooshAcountingServer.Models
{
    public class ApiViewModels
    {
    }
    public enum status
    {
        success = 0,
        unknownError = 1,
        forgotStoping = 2
    }
    public class Respond
    {
        public Respond(string message = "", status status = status.success)
        {
            this.status = status;
            this.message = message;
        }
        public status status { get; set; }
        public string message { get; set; }
    }
    public class LogInfoVM
    {
        public int Id { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string date { get; set; }
        public Respond Respond { get; set; }

    }
    public class AuthenticationInfoVM
    {
        public string user { get; set; }
        public string pass { get; set; }
    }
    public class AuthenticatedUserVM
    {
        public bool success { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
    }
}