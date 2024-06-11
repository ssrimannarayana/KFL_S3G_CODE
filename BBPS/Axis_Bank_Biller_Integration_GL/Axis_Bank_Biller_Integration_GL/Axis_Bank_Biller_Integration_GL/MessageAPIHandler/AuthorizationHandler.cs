using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;



    public class AuthorizationHandler : DelegatingHandler
    {
        public string Key { get; set; }
        public AuthorizationHandler()
        {
            Key = "8epy4BeVEYPqMZJ9Xv2u9Ry8XN/VE6wlIqDfgUw2sME=";
            this.Key = Key;
        }
        protected override Task<HttpResponseMessage> SendAsync(
               HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!ValidateKey(request))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
            return base.SendAsync(request, cancellationToken);
        }

        private bool ValidateKey(HttpRequestMessage message)
        {
            var query = message.RequestUri.ParseQueryString();

            string key = query["key"];
            return (key == Key);
        }
    }
