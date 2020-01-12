using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace AttendingEngine.Managers
{
    public static class IpManager
    {

        public static bool IsIpAllowed(this HttpRequestMessage request)
        {

            if(!request.GetRequestContext().IsLocal) {
                var ipAddress = request.GetOwinContext().Request.RemoteIpAddress;
                var whiteList = ConfigurationManager.AppSettings["WhiteListOfIpAddresses"];

                if (string.IsNullOrEmpty(whiteList))
                {
                    return true;
                }

                var allowedIpAddresses = whiteList.Split(',').ToList();
                var ipIsAllowed =
                    allowedIpAddresses
                        .Any(a => a.Trim()
                            .Equals(ipAddress, StringComparison.InvariantCultureIgnoreCase));

                return ipIsAllowed;
            }

            return true;
        }
    }
}