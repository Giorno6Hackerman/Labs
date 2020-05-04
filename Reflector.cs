using System;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.Generic;

namespace CRUD
{
    class Reflector
    {
        private Assembly lib;
        private string libPath = "D:/prog/4 sem/OOTPISP/Labs/StoneLibrary/bin/Debug/StoneLibrary.dll";
        private Type[] classes;
        private List<object> objectsList;

        private void LoadClasses(ComboBox box)
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

    }
}
