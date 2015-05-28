using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Http_Listener
{
    public partial class ListenerService : ServiceBase
    {
        private static HttpListener listener;

        public ListenerService()
        {
            InitializeComponent();
            // Initialize the Http listener
            listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:80/");
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        }

        protected override void OnStart(string[] args)
        {
            listener.Start();
            // Do async callback to process request
            listener.BeginGetContext(new AsyncCallback(OnRequestReceived), listener);
        }

        protected override void OnStop()
        {
            listener.Stop();
        }

        private void OnRequestReceived(IAsyncResult result)
        {
            var context = listener.EndGetContext(result);
            var response = context.Response;
            var buffer = Encoding.UTF8.GetBytes("<div></div>");
            response.ContentLength64 = buffer.Length;
            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            // Wait for next request
            listener.BeginGetContext(new AsyncCallback(OnRequestReceived), listener);
        }

        private byte[] getImageBytes()
        {
            byte[] imageBytes = null;
            Image image = Image.FromFile("Images\\mnbp.jpg");
            return imageBytes;
        }
    }
}
