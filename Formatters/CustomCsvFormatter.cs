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
            var contextObject = context.Object as IEnumerable<string[]>;

            if (contextObject == null)
                return null;
            
            // loop through context object after (only strings)
            foreach (string[] str in contextObject) {
                List<string> values = new List<string>();
                foreach(var s in str)
                {
                    var tmpval = s;

                    if (tmpval != null){
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