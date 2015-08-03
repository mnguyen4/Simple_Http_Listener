﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        private static HostsFileManager hostsFileManager;
        private static bool useHostsFile;

        public ListenerService(string[] hostNames, bool useHosts)
        {
            useHostsFile = useHosts;
            hostsFileManager = new HostsFileManager();
            InitializeComponent();
            // Initialize the Http listener
            listener = new HttpListener();
            foreach (string name in hostNames) {
                listener.Prefixes.Add("http://" + name + ":80/");
                listener.Prefixes.Add("https://" + name + ":443/");
            }
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        }

        protected override void OnStart(string[] args)
        {
            if (useHostsFile)
            {
                hostsFileManager.applyHostsFile();
            }
            LogUtils.writeLog("Starting service...");
            listener.Start();
            // Do async callback to process request
            listener.BeginGetContext(new AsyncCallback(OnRequestReceived), listener);
        }

        protected override void OnStop()
        {
            LogUtils.writeLog("Stopping service...");
            listener.Stop();
            if (useHostsFile)
            {
                hostsFileManager.restoreHostsFile();
            }
        }

        private void OnRequestReceived(IAsyncResult result)
        {
            var context = listener.EndGetContext(result);
            var request = context.Request;
            var response = context.Response;
            try
            {
                if (request.Url.ToString().Contains(".js") || request.AcceptTypes.Contains("text/javascript"))
                {
                    sendJavaScriptResponse(response);
                }
                else if (request.Url.ToString().Contains(".html") || request.AcceptTypes.Contains("text/html"))
                {
                    sendHTMLResponse(response);
                }
                else
                {
                    sendImageResponse(response);
                }
            }
            catch (Exception e)
            {
                LogUtils.writeLog(e.StackTrace);
            }
            finally
            {
                // Wait for next request
                listener.BeginGetContext(new AsyncCallback(OnRequestReceived), listener);
            }
        }

        private byte[] getImageBytes()
        {
            byte[] imageBytes = null;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images\\mnbp.jpg");
            Image image = Image.FromFile(filePath);
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            imageBytes = ms.ToArray();
            return imageBytes;
        }

        private byte[] getFileBytes(string file)
        {
            byte[] fileByte = null;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            string jsText = "";
            try
            {
                jsText = File.ReadAllText(filePath);
            }
            catch (IOException e)
            {
                jsText = e.Message;
            }
            fileByte = Encoding.UTF8.GetBytes(jsText);
            return fileByte;
        }

        private void sendImageResponse(HttpListenerResponse response)
        {
            byte[] buffer = getImageBytes();
            response.ContentType = "image/jpeg";
            response.ContentLength64 = buffer.Length;
            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }

        private void sendJavaScriptResponse(HttpListenerResponse response)
        {
            byte[] buffer = getFileBytes("Content\\mnbp.js");
            response.ContentType = "text/javascript";
            response.ContentLength64 = buffer.Length;
            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }

        private void sendHTMLResponse(HttpListenerResponse response)
        {
            byte[] buffer = getFileBytes("Content\\mnbp.html");
            response.ContentType = "text/html";
            response.ContentLength64 = buffer.Length;
            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
