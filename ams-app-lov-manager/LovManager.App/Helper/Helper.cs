using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace LovManager.Api.Helper
{
    public class Helper
    {
        public static void WriteCSV(
               IEnumerable data,
               TextWriter output,
               bool headerRow = false)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            const char separator = ',';
            Type type = null;

            if (data.GetType().IsGenericType)
            {
                type = data.GetType().GetGenericArguments()[0];
            }
            else
            {
                // get the type of the data item
                // perhaps there is a better way
                foreach (var item in data)
                {
                    type = item.GetType();
                    break;
                }
            }

            // only take properties with [JsonProperty] attribute defined
            //var propsInfo = type.GetProperties(BindingFlags.Public
            //                        | BindingFlags.Instance)
            //              .Where(p => p.IsDefined(typeof(JsonPropertyAttribute), true));
            var propsInfo = type.GetProperties(BindingFlags.Public
                           | BindingFlags.Instance)
                 .Where(p => p.IsDefined(typeof(JsonPropertyAttribute), true));
            if (!propsInfo.Any())
            {
                return;
            }

            // create a collection of properties to write
            var propsToWrite = new List<PropertyInfoWrapper>();

            foreach (var pi in propsInfo)
            {
                // get the [JsonProperty] attribute
                var attr = (JsonPropertyAttribute)Attribute
                     .GetCustomAttribute(pi, typeof(JsonPropertyAttribute));

                propsToWrite.Add(new PropertyInfoWrapper(pi) { Order = attr.Order });

                // write header columns
                if (headerRow)
                {
                    // use the actual property name if PropertyName attribute is null
                    string displayName = attr.PropertyName ?? pi.Name;

                    // if the display name contains a separator char
                    // enclose it in double quotes
                    if (displayName.IndexOf(separator) != -1)
                    {
                        output.Write("\"" + displayName + "\"");
                    }
                    else
                    {
                        output.Write(displayName);
                    }

                    output.Write(separator);
                }
            }

            if (headerRow)
            {
                output.WriteLine();
            }

            bool firstProperty = false;
            // sort properties by the Order property then by Property name
            var sortedProps = propsToWrite
                      .OrderBy(p => p.Order)
                      .ThenBy(p => p.PropertyInfo.Name);

            // write the data items
            foreach (var item in data)
            {
                firstProperty = true;
                foreach (var prop in sortedProps)
                {
                    // prevents a leading separator char on a row
                    if (!firstProperty)
                    {
                        output.Write(separator);
                    }
                    firstProperty = false;

                    var pi = prop.PropertyInfo;

                    object propValue = pi.GetValue(item, null);

                    if (propValue == null)
                    {
                        output.Write("");
                    }
                    else
                    {
                        // collection property types
                        // create a pipe (|) separated string for collection properties
                        if (pi.PropertyType is ICollection)
                        {
                            var sb = new StringBuilder();

                            foreach (var enumItem in (propValue as IEnumerable))
                            {
                                sb.AppendFormat("{0}| ", enumItem.ToString());
                            }

                            output.Write(sb.ToString());
                        }
                        else
                        {
                            string value = propValue.ToString();

                            // if the data contains a separator char
                            // enclose it in double quotes
                            if (value.IndexOf(separator) != -1)
                            {
                                output.Write("\"" + value + "\"");
                            }
                            else
                            {
                                output.Write(value);
                            }
                        }
                    }
                }

                output.WriteLine();
            }
        }

        // the helper class used in the WriteCSV method
        class PropertyInfoWrapper
        {
            public int Order { get; set; }

            public PropertyInfo PropertyInfo { get; private set; }

            public PropertyInfoWrapper(PropertyInfo propertyInfo)
            {
                PropertyInfo = propertyInfo;
            }
        }
    }
}