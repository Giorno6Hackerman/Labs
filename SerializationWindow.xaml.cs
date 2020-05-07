using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Serialization;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CRUD
{
    /// <summary>
    /// Логика взаимодействия для SerializationWindow.xaml
    /// </summary>
    public partial class SerializationWindow : Window
    {
        bool IsSerialize;
        string fileName;
        string libPath = "D:/prog/4 sem/OOTPISP/Labs/Lab_3/SerializationLibrary/SerializationLibrary/bin/Debug/SerializationLibrary.dll";
        Assembly serLib;
        private Type[] classes;
        Type inter;
        public List<object> objects = new List<object>();
        public object[] rer;

        public SerializationWindow(bool ser, ref List<object> obj)
        {
            InitializeComponent();
            IsSerialize = ser;
            LoadSerializationTypes();
            objects = obj;
            if(!IsSerialize)
                obj = objects;
            else
                objects = obj;
        }

        private void LoadSerializationTypes()
        {
            serLib = Assembly.LoadFrom(libPath);
            classes = serLib.GetTypes();

            inter = serLib.GetType("CRUD.ISatanSerializer");
            foreach(Type type in classes)
            {
                if ((type != inter) && (type.GetInterface(inter.Name) != null))
                    serializationTypeComboBox.Items.Add(type.Name);
            }
        }


        

        private void chooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                fileName = fileDialog.FileName;
            }
            fileNameTextBox.Text = fileName;
            if (serializationTypeComboBox.SelectedItem != null)
                if (IsSerialize)
                {
                    serializeButton.IsEnabled = true;
                }
                else
                {
                    deserializeButton.IsEnabled = true;
                }
        }

        private void serializeButton_Click(object sender, RoutedEventArgs e)
        {
            Type serialization = serLib.GetType("CRUD." + serializationTypeComboBox.SelectedItem.ToString());
            object serializer = Activator.CreateInstance(serialization);
            MethodInfo method = serialization.GetMethod("Serialize");
            object[] param = new object[2] { fileName, objects.ToArray() };
            method.Invoke(serializer, param);////////
            this.DialogResult = true;
        }

        private void deserializeButton_Click(object sender, RoutedEventArgs e)
        {
            //objects.Clear();
            Type serialization = serLib.GetType("CRUD." + serializationTypeComboBox.SelectedItem.ToString());
            object serializer = Activator.CreateInstance(serialization);
            MethodInfo method = serialization.GetMethod("Deserialize");
            object[] param = new object[1] { fileName };
            objects.AddRange((object[])method.Invoke(serializer, param));////////
            rer = objects.ToArray();
            this.DialogResult = true;
        }

        private void serializationTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fileNameTextBox.Text != "")
                if (IsSerialize)
                {
                    serializeButton.IsEnabled = true;
                }
                else
                {
                    deserializeButton.IsEnabled = true;
                }
        }

        /*
            XMLSerializer ser = new XMLSerializer(typeof(object));
            FileStream stream = new FileStream("object.xml", FileMode.OpenOrCreate);
            try
            {

                object[] objects = ser.Deserialize(stream);
                foreach (object obj in objects)
                {
                    reflector.objectsList.Add(obj);
                    ListBoxItem item = new ListBoxItem();
                    item.Height = DefaultTextBoxHeight;
                    item.FontSize = DefaultFontSize - 2;
                    item.Selected += ObjectsListBox_Selected;
                    item.Content = obj;
                    ObjectsListBox.Items.Add(item);
                }
                
                ObjectsListBox.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                stream.Close();
            }
            */

        /*
            try
            {
                
                FileStream stream = new FileStream("object.xml", FileMode.Create);
                //object[] cur = reflector.objectsList.ToArray();
                foreach (object obj in reflector.objectsList)
                {
                    
                    XMLSerializer ser = new XMLSerializer(obj.GetType());
                    //ser.Serialize(stream, cur);
                    //object[] ob = list.ToArray();

                    //Array arr = Array.CreateInstance(obj.GetType(), 1);
                    //ser.Serialize(stream, (object[])arr);
                    ser.Serialize(stream, new object[1] { obj });
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            */


        //soap
        /*
            XMLSerializer ser = new XMLSerializer();
            FileStream stream = new FileStream("object.xml", FileMode.OpenOrCreate);
            try
            {

                object[] objects = ser.Deserialize(stream);
                foreach (object obj in objects)
                {
                    reflector.objectsList.Add(obj);
                    ListBoxItem item = new ListBoxItem();
                    item.Height = DefaultTextBoxHeight;
                    item.FontSize = DefaultFontSize - 2;
                    item.Selected += ObjectsListBox_Selected;
                    item.Content = obj;
                    ObjectsListBox.Items.Add(item);
                }
                
                ObjectsListBox.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                stream.Close();
            }
            */

        /*
            try
            {
                
                FileStream stream = new FileStream("object.xml", FileMode.Create);
                //object[] cur = reflector.objectsList.ToArray();
                //foreach (object obj in reflector.objectsList)
                //{
                    
                    XMLSerializer ser = new XMLSerializer();
                    //ser.Serialize(stream, cur);
                    object[] ob = reflector.objectsList.ToArray();

                    //Array arr = Array.CreateInstance(obj.GetType(), 1);
                    //ser.Serialize(stream, (object[])arr);
                    ser.Serialize(stream, ob);
                //}
                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            */
    }
}
