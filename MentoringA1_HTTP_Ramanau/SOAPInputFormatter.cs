using DAL.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using Microsoft.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Soap;
using System.Threading.Tasks;

namespace MentoringA1_HTTP_Ramanau
{
    public class SOAPInputFormatter : InputFormatter
    {
        public const string ContentType = "application/soap";

        public SOAPInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ContentType));
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var memoryStream = new MemoryStream();
            context.HttpContext.Request.Body.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            SoapFormatter formatter = new SoapFormatter();
            var content = formatter.Deserialize(memoryStream);
            return InputFormatterResult.SuccessAsync(content);
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(Order);
        }
    }
}
