using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace TestBrowserSession
{
    public class BrowserSession
    {
        private bool _isPost;
        private bool _isDownload;
        private HtmlDocument _htmlDoc;
        private string _download;

        /// <summary>
        /// System.Net.CookieCollection. Provides a collection container for instances of Cookie class 
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Provide a key-value-pair collection of form elements 
        /// </summary>
        public FormElementCollection FormElements { get; set; }

        /// <summary>
        /// Makes a HTTP GET request to the given URL
        /// </summary>
        public string Get(string url)
        {
            _isPost = false;
            CreateWebRequestObject().Load(url);
            return _htmlDoc.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// Makes a HTTP POST request to the given URL
        /// </summary>
        public string Post(string url)
        {
            _isPost = true;
            CreateWebRequestObject().Load(url, "POST");
            return _htmlDoc.DocumentNode.InnerHtml;
        }

        public string GetDownload(string url)
        {
            _isPost = false;
            _isDownload = true;
            CreateWebRequestObject().Load(url);
            return _download;
        }

        /// <summary>
        /// Creates the HtmlWeb object and initializes all event handlers. 
        /// </summary>
        private HtmlWeb CreateWebRequestObject()
        {
            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;
            web.PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest);
            web.PostResponse = new HtmlWeb.PostResponseHandler(OnAfterResponse);
            web.PreHandleDocument = new HtmlWeb.PreHandleDocumentHandler(OnPreHandleDocument);
            return web;
        }

        /// <summary>
        /// Event handler for HtmlWeb.PreRequestHandler. Occurs before an HTTP request is executed.
        /// </summary>
        protected bool OnPreRequest(HttpWebRequest request)
        {
            request.AllowAutoRedirect = false; // Location header messing up cookie handling!

            AddCookiesTo(request);               // Add cookies that were saved from previous requests
            if (_isPost) AddPostDataTo(request); // We only need to add post data on a POST request
            return true;
        }

        /// <summary>
        /// Event handler for HtmlWeb.PostResponseHandler. Occurs after a HTTP response is received
        /// </summary>
        protected void OnAfterResponse(HttpWebRequest request, HttpWebResponse response)
        {
            SaveCookiesFrom(request, response); // Save cookies for subsequent requests

            if (response != null && _isDownload)
            {
                Stream remoteStream = response.GetResponseStream();
                var sr = new StreamReader(remoteStream);
                _download = sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Event handler for HtmlWeb.PreHandleDocumentHandler. Occurs before a HTML document is handled
        /// </summary>
        protected void OnPreHandleDocument(HtmlDocument document)
        {
            SaveHtmlDocument(document);
        }

        /// <summary>
        /// Assembles the Post data and attaches to the request object
        /// </summary>
        private void AddPostDataTo(HttpWebRequest request)
        {
            string payload = FormElements.AssemblePostPayload();
            byte[] buff = Encoding.UTF8.GetBytes(payload.ToCharArray());
            request.ContentLength = buff.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            System.IO.Stream reqStream = request.GetRequestStream();
            reqStream.Write(buff, 0, buff.Length);
        }

        /// <summary>
        /// Add cookies to the request object
        /// </summary>
        private void AddCookiesTo(HttpWebRequest request)
        {
            if (Cookies != null && Cookies.Count > 0)
            {
                request.CookieContainer.Add(Cookies);
            }
        }

        /// <summary>
        /// Saves cookies from the response object to the local CookieCollection object
        /// </summary>
        private void SaveCookiesFrom(HttpWebRequest request, HttpWebResponse response)
        {
            //save the cookies ;)
            if (request.CookieContainer.Count > 0 || response.Cookies.Count > 0)
            {
                if (Cookies == null)
                {
                    Cookies = new CookieCollection();
                }

                Cookies.Add(request.CookieContainer.GetCookies(request.RequestUri));
                Cookies.Add(response.Cookies);
            }
        }

        /// <summary>
        /// Saves the form elements collection by parsing the HTML document
        /// </summary>
        private void SaveHtmlDocument(HtmlDocument document)
        {
            _htmlDoc = document;
            FormElements = new FormElementCollection(_htmlDoc);
        }
    }
}
