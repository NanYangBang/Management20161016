using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NYB.DeviceManagementSystem.View
{
    public class SinaDateTimeConverter : DateTimeConverterBase
    {
        private readonly string _cultureInfoName = "en-US";
        private readonly string _timeFormat = "ddd MMM dd HH:mm:ss zz00 yyyy";

        #region ReadJson
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return DateTime.MinValue;
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception();
            }

            CultureInfo provider = new CultureInfo(_cultureInfoName);
            string tm = reader.Value.ToString();

            try
            {
                return DateTime.ParseExact(tm, _timeFormat, provider);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }
        #endregion

        #region WriteJson
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime)
            {
                DateTime tm = (DateTime)value;
                CultureInfo provider = new CultureInfo(_cultureInfoName);
                var result = tm.ToString(_timeFormat, provider);
                writer.WriteValue(result);
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
        }
        #endregion
    }
}