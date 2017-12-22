using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System;
using System.Diagnostics;
using System.IO;

namespace MovieAPI.Logger
{
    public class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestInfo = string.Format("{0}\r\t{1}\r\t{2}", request.Method, request.RequestUri, request.Headers);

            var requestMessage = await request.Content.ReadAsByteArrayAsync();

            //writes to debug output window
            Debug.WriteLine(string.Format("Request:{0}\r\t{1}\r\t{2}\t", DateTime.Now, requestInfo, Encoding.UTF8.GetString(requestMessage)) + Environment.NewLine);

            // Writes to a local file.
            //File.AppendAllText(@"D:\ApiRequestLogs\logs.txt", string.Format("Request:{0}\r\t{1}\r\t{2}\t", DateTime.Now, requestInfo, Encoding.UTF8.GetString(requestMessage)) + Environment.NewLine);
            var response = await base.SendAsync(request, cancellationToken);

            byte[] responseMessage;

            if (response.IsSuccessStatusCode)
                responseMessage = await response.Content.ReadAsByteArrayAsync();
            else
                responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
            return response;
        }
    }
}