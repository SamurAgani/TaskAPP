using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaskApp.Services.Abstract;

namespace TaskApp.Services.Concrete
{
    public class SamurSoft : ISamurSoft
    {
        public SamurSoft(Type type)
        {
            _type = type;
        }

        public Type _type { get ; set ; }

        private static string FindChildString(string des, string realKey)
        {
            int start = des.IndexOf(realKey + ":") + realKey.Length + 1;
            string afterStart = des.Substring(start);
            int end = FindEndOfChildString(afterStart);
            string child = des.Substring(start, end + 1);
            return child;
        }

        private static int FindEndOfChildString(string afterStart)
        {
            if (afterStart.StartsWith('{'))
                afterStart = afterStart.Remove(0, 1);
            int numberOfOpenbracket = 1;
            for (int i = 0; i < afterStart.Length; i++)
            {
                if (afterStart[i] == '{')
                    numberOfOpenbracket++;
                if (afterStart[i] == '}')
                    numberOfOpenbracket--;

                if (numberOfOpenbracket == 0)
                    return i;
            }
            throw new Exception($"Invalid text!");
        }

        private static string RemoveUselessElements(string[] arr,string mainString) 
        {
            foreach (var item in arr)
            {
                mainString = mainString.Replace(item, null);
            }
            return mainString;
        }
        public object Deserialize(string des)        {

            object obj = Activator.CreateInstance(_type);

            List<PropertyInfo> properties = _type.GetProperties().ToList();

            List<string> pairs = des.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
            string key, realKey, value;
            for (int i = 0; i < pairs.Count; i++)
            {
                string[] uselessElements = new string[] { "{", "}", "\t", "\n" ,",","'", "’", "‘","\\","\"" };
                string[] keyValue = pairs[i].Split(':');
                realKey = keyValue[0];
                key = keyValue[0];
                key = RemoveUselessElements(uselessElements, key).Trim().ToLower();
              
                if (keyValue.Length < 1 || key.Length < 1)
                    continue;

                var property = properties.FirstOrDefault(x => x.Name.ToLower() == key);
                if (property is null)
                    continue;
                if (!property.PropertyType.IsValueType && property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(string) && property.PropertyType.Name.IndexOf("Nullable") < 0)
                {
                    Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name == property.PropertyType.Name);
                    //var temp = Activator.CreateInstance(type);
                    SamurSoft childSamurSoft = new SamurSoft(type);
                    string child = FindChildString(des, realKey);
                    var tempChild = childSamurSoft.Deserialize(child);
                    List<string> childPairs = child.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                    i += childPairs.Count();
                    property.SetValue(obj, tempChild);
                    continue;
                }
                value = keyValue[1];
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                if (property is not null)
                {
                    value = RemoveUselessElements(uselessElements, value).Trim();
                    if (property.PropertyType == typeof(bool) || property.PropertyType.FullName.IndexOf("bool") >= 0)
                    {
                        property.SetValue(obj, bool.Parse(value));
                    }
                    else if (property.PropertyType == typeof(int) || property.PropertyType.FullName.IndexOf("Int32") >= 0)
                    {
                        property.SetValue(obj, int.Parse(value));
                    }
                    else if (property.PropertyType == typeof(double) || property.PropertyType.FullName.IndexOf("Double") >= 0)
                    {
                        property.SetValue(obj, double.Parse(value));
                    }
                    else if (property.PropertyType == typeof(char) || property.PropertyType.FullName.IndexOf("Char") >= 0)
                    {
                        property.SetValue(obj, char.Parse(value));
                    }
                    else if (property.PropertyType == typeof(float) || property.PropertyType.FullName.IndexOf("Single") >= 0)
                    {
                        property.SetValue(obj, float.Parse(value));
                    }
                    else if (property.PropertyType == typeof(long) || property.PropertyType.FullName.IndexOf("Int64") >= 0)
                    {
                        property.SetValue(obj, long.Parse(value));
                    }
                    else if (property.PropertyType == typeof(short) || property.PropertyType.FullName.IndexOf("Int16") >= 0)
                    {
                        property.SetValue(obj, short.Parse(value));
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(obj, value);
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime date = new DateTime();
                        DateTime.TryParse(value, out date);
                        property.SetValue(obj, date);
                    }
                    else
                    {
                        throw new Exception($"Invalid type '{property.PropertyType}'.");
                    }
                }
            }
            return obj;
        }

        public string Serialize(object graph)
        {
            string serializedText = "{";
            List<PropertyInfo> properties = _type.GetProperties().ToList();
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].PropertyType.IsValueType || properties[i].PropertyType == typeof(DateTime) || properties[i].PropertyType == typeof(string))
                {
                    var value = properties[i].GetValue(graph);
                    serializedText += String.Format("{0}:{1}", properties[i].Name, value);
                }
                else
                {
                    var property = properties[i].GetValue(graph);
                    SamurSoft a1 = new SamurSoft(property.GetType());
                    serializedText += String.Format("{0}:{1}", properties[i].Name, a1.Serialize(property));
                }
                serializedText += ",";
            }
            serializedText += "}";
            return serializedText;
        }
    }
}