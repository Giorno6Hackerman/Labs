using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Serialization;

namespace CRUD
{
    /// <summary>
    /// Логика взаимодействия для SerializationWindow.xaml
    /// </summary>
    public partial class SerializationWindow : Window
    {
        

        public SerializationWindow()
        {
            InitializeComponent();
        }

        private void chooseFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void serializeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deserializeButton_Click(object sender, RoutedEventArgs e)
        {

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
