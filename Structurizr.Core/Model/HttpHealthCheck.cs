using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{

    /// <summary>
    /// Represents a HTTP-based health check.
    /// </summary>
    public sealed class HttpHealthCheck : IEquatable<HttpHealthCheck>
    {

        /// <summary>
        /// A name for the health check.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = true)]
        public string Name { get; internal set; }

        /// <summary>
        /// The health check URL/endpoint.
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = true)]
        public string Url { get; internal set; }

        private Dictionary<string, string> _headers = new Dictionary<string, string>();

        /// <summary>
        /// The headers that should be sent in the HTTP request.
        /// </summary>
        [DataMember(Name = "headers", EmitDefaultValue = true)]
        public Dictionary<string, string> Headers
        {
            get
            {
                return new Dictionary<string, string>(_headers);
            }

            internal set
            {
                _headers = value;
            }
        }

        /// <summary>
        /// The polling interval, in seconds.
        /// </summary>
        [DataMember(Name = "interval", EmitDefaultValue = true)]
        public int Interval { get; internal set; }

        /// <summary>
        /// The timeout after which a health check is deemed as failed, in milliseconds.
        /// </summary>
        [DataMember(Name = "timeout", EmitDefaultValue = true)]
        public long Timeout { get; internal set; }

        internal HttpHealthCheck()
        {
        }

        internal HttpHealthCheck(string name, string url, int interval, long timeout)
        {
            this.Name = name;
            this.Url = url;
            this.Interval = interval;
            this.Timeout = timeout;
        }

        /// <summary>
        /// Adds a HTTP header, which will be sent with the HTTP request to the health check URL.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The header value.</param>
        public void AddHeader(string name, string value)
        {
            if (name == null || name.Trim().Length == 0)
            {
                throw new ArgumentException("The header name must not be null or empty.");
            }

            if (value == null)
            {
                throw new ArgumentException("The header value must not be null.");
            }

            _headers.Add(name, value);
        }

        public bool Equals(HttpHealthCheck other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Url, other.Url);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is HttpHealthCheck && Equals((HttpHealthCheck) obj);
        }

        public override int GetHashCode()
        {
            if (Url != null)
            {
                return Url.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

    }
}