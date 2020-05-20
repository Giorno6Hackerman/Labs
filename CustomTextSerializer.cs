using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System.Resources;

namespace CRUD
{
    public class CustomTextSerializer : ISatanSerializer
    {
        public CustomTextSerializer()
        {

        }

        public void Serialize(Stream data, object[] graph)
        {
            StreamWriter writer = new StreamWriter(data);
            foreach (object obj in graph)
            {
                object ob = obj;
                Type type = obj.GetType();
                writer.WriteLine(type.ToString());
                SaveProperties(writer, type, ref ob);
                SaveFields(writer, type, ref ob);
                writer.WriteLine();
            }
            writer.Close();
        }


        public object[] Deserialize(Stream data)
        {
            Assembly currentasm = Assembly.LoadFrom("D:/prog/4 sem/OOTPISP/Labs/StoneLibrary/bin/Debug/StoneLibrary.dll");
            Type type;
            StreamReader reader = new StreamReader(data);
            List<object> list = new List<object>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (!line.Contains(" "))
                {
                    type = currentasm.GetType(line);
                }
                else
                {
                    type = typeof(object);
                }
                object obj = Activator.CreateInstance(type);

                line = reader.ReadLine();
                while (line != "")
                {
                    string propName = line.Substring(0, line.IndexOf(" "));
                    MemberInfo curMember = GetCurrentMember(type, propName);
                    object value;
                    if ((curMember as PropertyInfo).PropertyType.IsClass && !(curMember as PropertyInfo).PropertyType.IsPrimitive && ((curMember as PropertyInfo).PropertyType != typeof(string)))
                    {
                        value = CreateSubObject(reader, (curMember as PropertyInfo).PropertyType);
                    }
                    else
                    {
                        value = line.Substring(line.IndexOf(" ") + 1);
                    }
                    PropertyInfo curProp = ConvertArgumentAsProperty(curMember, ref value);
                    curProp.SetValue(obj, value);
                    line = reader.ReadLine();
                }
                line = reader.ReadLine();
                while (line != "")
                {
                    string fieldName = line.Substring(0, line.IndexOf(" "));
                    MemberInfo curMember = GetCurrentMember(type, fieldName);
                    object value = line.Substring(line.IndexOf(" ") + 1);
                    FieldInfo curField = ConvertArgumentAsField(curMember, ref value);
                    curField.SetValue(obj, value);
                    line = reader.ReadLine();
                }
                list.Add(obj);
                reader.ReadLine();
                //reader.ReadLine();
            }

            reader.Close();
            return list.ToArray();
        }


        private object CreateSubObject(StreamReader reader, Type type)
        {
            object subObj = Activator.CreateInstance(type);
            string line = reader.ReadLine();
            while (line != "")
            {
                string propName = line.Substring(0, line.IndexOf(" "));
                MemberInfo curMember = GetCurrentMember(type, propName);
                object value;
                if ((curMember as PropertyInfo).PropertyType.IsClass && !(curMember as PropertyInfo).PropertyType.IsPrimitive && ((curMember as PropertyInfo).PropertyType != typeof(string)))
                {
                    value = CreateSubObject(reader, (curMember as PropertyInfo).PropertyType);
                }
                else
                {
                    value = line.Substring(line.IndexOf(" ") + 1);
                }
                PropertyInfo curProp = ConvertArgumentAsProperty(curMember, ref value);
                curProp.SetValue(subObj, value);
                line = reader.ReadLine();
            }
            line = reader.ReadLine();
            while (line != "")
            {
                string fieldName = line.Substring(0, line.IndexOf(" "));
                MemberInfo curMember = GetCurrentMember(type, fieldName);
                object value = line.Substring(line.IndexOf(" ") + 1);
                FieldInfo curField = ConvertArgumentAsField(curMember, ref value);
                curField.SetValue(subObj, value);
                line = reader.ReadLine();
            }
            return subObj;
        }


        public MemberInfo GetCurrentMember(Type type, string name)
        {
            MemberInfo[] members = type.GetMember(name);
            MemberInfo currentMember = members[0];
            foreach (MemberInfo member in members)
            {
                if (member.MemberType == MemberTypes.Property || member.MemberType == MemberTypes.Field)
                {
                    return member;
                }
            }

            return null;
        }


        // базовые обычные типы
        List<Type> BasicTypes = new List<Type>
        {
            typeof(Boolean),
            typeof(Char),
            typeof(Byte),
            typeof(SByte),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(Decimal),
            typeof(Double),
            typeof(Single)
        };


        // конвертирование значения поля объекта в нужный базовый тип
        private object BasicTypeConvertion(object from, Type basicType)
        {
            switch (basicType.Name)
            {
                case "Boolean":
                    return Boolean.Parse(from.ToString());
                case "Byte":
                case "SByte":
                    return Byte.Parse(from.ToString());
                case "Char":
                    return Char.Parse(from.ToString());
                case "Int16":
                case "UInt16":
                    return Int16.Parse(from.ToString());
                case "Int32":
                case "UInt32":
                    return Int32.Parse(from.ToString());
                case "Double":
                    return Double.Parse(from.ToString());
                case "Single":
                    return Single.Parse(from.ToString());
                case "Decimal":
                    return Decimal.Parse(from.ToString());
                default:
                    return Int64.Parse(from.ToString());
            }
        }


        // конвертирование значения поля объекта в нужный тип-перечисление
        private object EnumTypeConvertion(object from, Type basicType)
        {
            foreach (string t in basicType.GetEnumNames())
            {
                if (from.ToString() == t)
                    return Enum.Parse(basicType, t);
            }
            return basicType.GetEnumNames()[0];
        }


        // конвертирование значения СВОЙСТВА
        public PropertyInfo ConvertArgumentAsProperty(MemberInfo member, ref object argument)
        {
            PropertyInfo currentProperty = member as PropertyInfo;
            Type type = currentProperty.PropertyType;
            if (BasicTypes.Contains(type))
            {
                argument = BasicTypeConvertion(argument, type);
            }
            if (type.IsEnum)
            {
                argument = EnumTypeConvertion(argument, type);
            }
            return currentProperty;
        }


        // конвертирование значения ПОЛЯ
        public FieldInfo ConvertArgumentAsField(MemberInfo member, ref object argument)
        {
            FieldInfo currentField = member as FieldInfo;
            Type type = currentField.FieldType;
            if (BasicTypes.Contains(type)/* || type.IsEnum*/)
            {
                argument = BasicTypeConvertion(argument, type);
            }
            if (type.IsEnum)
            {
                argument = EnumTypeConvertion(argument, type);
            }
            return currentField;
        }


        private void SaveProperties(StreamWriter writer, Type type, ref object obj)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string str = property.Name + " " + property.GetValue(obj).ToString();
                writer.WriteLine(str);
                if (property.PropertyType.IsClass && !property.PropertyType.IsPrimitive && (property.PropertyType != typeof(string)))
                {
                    object subObj = property.GetValue(obj);
                    SaveProperties(writer, property.PropertyType, ref subObj);
                    SaveFields(writer, property.PropertyType, ref subObj);
                }
            }
            writer.WriteLine();
        }


        private void SaveFields(StreamWriter writer, Type type, ref object obj)
        {
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                string str = field.Name + " " + field.GetValue(obj).ToString();
                writer.WriteLine(str);
                if (field.FieldType.IsClass && !field.FieldType.IsPrimitive && (field.FieldType != typeof(string)))
                {
                    object subObj = field.GetValue(obj);
                    SaveProperties(writer, field.FieldType, ref subObj);
                    SaveFields(writer, field.FieldType, ref subObj);
                }
            }
            writer.WriteLine();
        }

        /*
        private void SaveSubProperties(StreamWriter writer, PropertyInfo info, ref object obj)
        {
            Type type = info.PropertyType;
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string str = property.Name + " " + property.GetValue(obj).ToString();
                writer.WriteLine(str);
                if (property.PropertyType.IsClass && !property.PropertyType.IsPrimitive && (property.PropertyType != typeof(string)))
                {
                    object subObj = Activator.CreateInstance(property.PropertyType);
                    SaveSubProperties(writer, property, ref subObj);
                    SaveSubFields(writer, property, ref subObj);
                }
            }
            writer.WriteLine();
        }


        private void SaveSubProperties(StreamWriter writer, FieldInfo info, ref object obj)
        {
            Type type = info.FieldType;
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                string str = field.Name + " " + field.GetValue(obj).ToString();
                writer.WriteLine(str);
                if (field.FieldType.IsClass && !field.FieldType.IsPrimitive && (field.FieldType != typeof(string)))
                {
                    object subObj = Activator.CreateInstance(field.FieldType);
                    SaveSubProperties(writer, field, ref subObj);
                    SaveSubFields(writer, field, ref subObj);
                }
            }
            writer.WriteLine();
        }


        private void SaveSubFields(StreamWriter writer, PropertyInfo info, ref object obj)
        {
            Type type = info.PropertyType;
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string str = property.Name + " " + property.GetValue(obj).ToString();
                writer.WriteLine(str);
                if (property.PropertyType.IsClass && !property.PropertyType.IsPrimitive && (property.PropertyType != typeof(string)))
                {
                    object subObj = Activator.CreateInstance(property.PropertyType);
                    SaveSubProperties(writer, property, ref subObj);
                    SaveSubFields(writer, property, ref subObj);
                }
            }
            writer.WriteLine();
        }


        private void SaveSubFields(StreamWriter writer, FieldInfo info, ref object obj)
        {
            Type type = info.FieldType;
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                string str = field.Name + " " + field.GetValue(obj).ToString();
                writer.WriteLine(str);
                if (field.FieldType.IsClass && !field.FieldType.IsPrimitive && (field.FieldType != typeof(string)))
                {
                    object subObj = Activator.CreateInstance(field.FieldType);
                    SaveSubProperties(writer, field, ref subObj);
                    SaveSubFields(writer, field, ref subObj);
                }
            }
            writer.WriteLine();
        }
        */

       
    }
}
