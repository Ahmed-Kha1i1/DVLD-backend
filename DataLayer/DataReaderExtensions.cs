using Microsoft.Data.SqlClient;
using System.Reflection;

namespace DVLD.Persistence
{
    internal static class DataReaderExtensions
    {
        public static T MapTo<T>(this SqlDataReader reader) where T : class, new()
        {
            var obj = new T();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {

                var value = reader[property.Name];

                if (value == DBNull.Value)
                {
                    property.SetValue(obj, null);
                    continue;
                }

                if (property.PropertyType.IsEnum)
                {
                    var enumValue = Enum.ToObject(property.PropertyType, value);
                    property.SetValue(obj, enumValue);
                }
                else
                {
                    var TargetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var convertedValue = Convert.ChangeType(value, TargetType);
                    property.SetValue(obj, convertedValue);
                }
            }

            return obj;
        }

    }
}
