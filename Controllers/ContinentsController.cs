using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNetHttpClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContinentsController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public ContinentsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<dynamic> Get()
        {
            try
            {
                string url = "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso";

                string xmlSOAP = @"<Envelope xmlns=""http://www.w3.org/2003/05/soap-envelope"">
                                    <Body>
                                        <ListOfContinentsByCode xmlns=""http://www.oorsprong.org/websamples.countryinfo""/>
                                    </Body>
                                </Envelope>";

                try
                {
                    var xmlResponse = await PostSOAPRequestAsync(url, xmlSOAP);
                    return xmlToJson(RemoveAllNamespaces(xmlResponse));
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Something went wrong. Error: {ex.Message}");
            }
        }

        private async Task<string> PostSOAPRequestAsync(string url, string text)
        {
            using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = content;
                var client = _clientFactory.CreateClient();
                using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        private string xmlToJson(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var responseXml = doc.GetElementsByTagName("ListOfContinentsByCodeResult")[0];
            return JsonConvert.SerializeXmlNode(responseXml, Newtonsoft.Json.Formatting.None, true);
        }

        //Implemented based on interface, not part of algorithm
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
    }
}
