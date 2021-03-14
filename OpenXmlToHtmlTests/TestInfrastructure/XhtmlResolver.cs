using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace OpenXmlToHtmlTests
{
    public class XhtmlResolver : XmlResolver
    {
        public override System.Net.ICredentials Credentials
        {
            set => throw new NotSupportedException();
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (ShouldAttemptResourceLoad(absoluteUri, ofObjectToReturn))
            {
                return ReadXhtmlDtdAsync();
            }
            throw new NotSupportedException("Resolving beside of whitelisted dtds not supported");
        }

        public override async Task<object> GetEntityAsync(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri == null)
            {
                throw new ArgumentNullException("absoluteUri", "Must provide a URI");
            }

            if (ShouldAttemptResourceLoad(absoluteUri, ofObjectToReturn))
            {
                return ReadXhtmlDtdAsync();
            }

            return await base.GetEntityAsync(absoluteUri, role, ofObjectToReturn);
        }

        private Stream ReadXhtmlDtdAsync()
        {
            var filename = @"../../../TestInfrastructure/xhtml11.dtd";

            var fileStream = File.Open(filename, FileMode.Open);
            return fileStream;
        }

        private bool ShouldAttemptResourceLoad(Uri absoluteUri, Type ofObjectToReturn)
        {
            return true;
        }
    }
}