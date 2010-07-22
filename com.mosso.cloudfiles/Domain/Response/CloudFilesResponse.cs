///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using com.mosso.cloudfiles.domain.response.Interfaces;
using com.mosso.cloudfiles.utils;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// Represents the response information from a CloudFiles request
    /// </summary>
    public class CloudFilesResponse : ICloudFilesResponse
    {
        private readonly HttpWebResponse _webResponse;
        private readonly IList<string> _contentbody = new List<string>();
        private readonly MemoryStream memstream = new MemoryStream( );
        private Stream Getstream()
        {
            memstream.Seek(0, 0);
            return memstream;
        }
        public CloudFilesResponse(HttpWebResponse webResponse)
        {
            _webResponse = webResponse;
            CopyToMemory(_webResponse.GetResponseStream(), memstream);
            if (HasTextBody())
            try
            {
                GetBody(Getstream());
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            
        }

        private bool HasTextBody()
        {
            return (
                _webResponse.ContentType.Contains("application/json") ||
                _webResponse.ContentType == "application/xml" ||
                _webResponse.ContentType == "application/xml; charset=utf-8" ||
                _webResponse.ContentType.Contains("text/plain") && _webResponse.ContentLength == -1) ||
                //_webResponse.ContentType == "text/plain; charset=UTF-8" // asw: replaced
                _webResponse.ContentType.Contains("text/plain")
                ;

        }

        private void CopyToMemory(Stream input, Stream output)
        {
            var buffer = new byte[32768];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    output.Seek(0, 0);
                    return;
                }
                output.Write(buffer, 0, read);
            }
            
        }
        private void GetBody(Stream stream)
        {
             
            using(var reader = new StreamReader(stream))
            {
                string line;
                while((line = reader.ReadLine())!= null)
                {
                    _contentbody.Add(line);
                }
            }
        }
        /// <summary>
        /// A property representing the HTTP Status code returned from cloudfiles
        /// </summary>
        public HttpStatusCode Status { get { return _webResponse.StatusCode; } }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the create container request
        /// </summary>
        public WebHeaderCollection Headers { get { return _webResponse.Headers; } }

        public void Close()
        {
            _webResponse.Close();
        }

        /// <summary>
        /// dictionary of meta tags assigned to this storage item
        /// </summary>
        public Dictionary<string, string> Metadata
        {
            get
            {
                var tags = new Dictionary<string, string>();
                foreach (string s in _webResponse.Headers.Keys)
                {
                    if (s.IndexOf(Constants.META_DATA_HEADER) == -1) continue;
                    var metaKeyStart = s.LastIndexOf("-");
                    tags.Add(s.Substring(metaKeyStart + 1), _webResponse.Headers[s]);
                }
                return tags;
            }
        }

        public string Method
        {
            get { return _webResponse.Method; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _webResponse.StatusCode; }
        }

        public string StatusDescription
        {
            get { return _webResponse.StatusDescription; }
        }

        public IList<string> ContentBody
        {
            get
            {
                return _contentbody;
            }
        }

        public string ContentType
        {
            get { return 
                _webResponse.ContentType; }
        }

        public string ETag
        {
            get { return _webResponse.Headers[Constants.ETAG]; }
            set { _webResponse.Headers[Constants.ETAG] = value; }
        }

        public long ContentLength
        {
            get { return _webResponse.ContentLength; }
        }

        public DateTime LastModified
        {
            get { return _webResponse.LastModified; }
        }

        public Stream GetResponseStream()
        {
            
            return  Getstream();
        }

        public void Dispose()
        {
            memstream.Close();
            _webResponse.Close();
        }

        public event Connection.ProgressCallback Progress;
    }
}