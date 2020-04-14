using DAL.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MentoringA1_HTTP_Ramanau
{
    public class XmlOutputFormatter : OutputFormatter
    {
        public const string ContentType = "application/xml";

        public XmlOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ContentType));
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(IEnumerable<Order>) || type == typeof(List<Order>) || type == typeof(Order);
        }


        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            var responseObject = context.Object;

            XmlSerializer formatter = new XmlSerializer(responseObject.GetType());

            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, responseObject);
                var byteArray = ms.ToArray();
                await response.Body.WriteAsync(byteArray, 0, byteArray.Length);
            }
        }
    }
}
