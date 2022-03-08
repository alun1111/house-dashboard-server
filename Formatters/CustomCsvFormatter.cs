using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace house_dashboard_server.Formatters
{
    public class CustomCsvFormatter : TextOutputFormatter
    {
        public CustomCsvFormatter()
        {
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var csv = new StringBuilder();
            // Context is the outbound model
            var contextType = context.Object?.GetType();

            if (contextType == null)
                return null;
            
            // Write first line of csv as headers by getting the properties of the context
            csv.Append(string.Join("," , contextType.GetProperties().Select(p=>p.Name)));
            
            // loop through context object after
            foreach (var obj in (IEnumerable<object>)context.Object) {
                var vals = obj.GetType().GetProperties().Select(
                    pi => new
                    {
                        Value = pi.GetValue(obj, null)
                    }
                );

                List<string> values = new List<string>();
                foreach (var val in vals)
                {
                    if (val.Value != null)
                    {
                        var tmpval = val.Value.ToString();

                        //Check if the value contains a comma and place it in quotes if so
                        if (tmpval.Contains(","))
                            tmpval = string.Concat("\"", tmpval, "\"");

                        //Replace any \r or \n special characters from a new line with a space
                        tmpval = tmpval.Replace("\r", " ", StringComparison.InvariantCultureIgnoreCase);
                        tmpval = tmpval.Replace("\n", " ", StringComparison.InvariantCultureIgnoreCase);

                        values.Add(tmpval);
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }
                csv.AppendLine(string.Join(",", values));
            }
            return context.HttpContext.Response.WriteAsync(csv.ToString(), selectedEncoding);
        }
    }
}