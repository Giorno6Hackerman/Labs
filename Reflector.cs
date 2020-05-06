using System;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.Generic;

namespace CRUD
{
    public class Reflector
    {
        public Assembly lib;
        private string libPath = "D:/prog/4 sem/OOTPISP/Labs/StoneLibrary/bin/Debug/StoneLibrary.dll";
        private Type[] classes;
        public List<object> objectsList;


        // загрузка всех классов из библиотеки
        public void LoadClasses(ComboBox box)
        {
            lib = Assembly.LoadFrom(libPath);
            classes = lib.GetTypes();

            objectsList = new List<object>();
            foreach (Type className in classes)
            {
                if (className.IsClass)
                    box.Items.Add(className.Name);
            }
        }


        // очистка списка созданных объектов
        public void ClearObjectsList()
        {
            objectsList.Clear();
        }


        // получение типа по его имени
        public Type GetTypeByName(string name)
        {
            return lib.GetType(name);
        }


        // получение всех дочерних классов
        public void AddChildrenClasses(ComboBox box, Type classType)
        {
            foreach (Type child in classes)
            {
                Type childType = child;
                while (childType != typeof(Object))
                {
                    if ((childType.BaseType == classType) && (!box.Items.Contains(childType.Name)))
                    {
                        box.Items.Add(child.Name);
                        break;
                    }
                    else
                    {
                        childType = childType.BaseType;
                    }
                }

            }
        }


        // проверка типов на совместимость
        public bool CheckCompatibleTypes(Type parent, Type child)
        {
            if (child == parent)
                return true;
            while (child != typeof(object))
            {
                if (child.BaseType == parent)
                {
                    return true;
                }
                else
                {
                    child = child.BaseType;
                }
            }

            return false;
        }


        // получение нужного поля/свойства
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
        public FieldInfo ConvertArgumentAsField(MemberInfo member,ref object argument)
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
    }
}
