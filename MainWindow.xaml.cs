﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.IO;

// ДОБАВИТЬ ВОЗМОЖНОСТЬ ВЫБОРА УЖЕ СОЗДАННОГО ОБЪЕКТА ДЛЯ ПОЛЯ-ОБЪЕКТА
// НУ И ВОЗМОЖНОСТЬ ЕГО ИЗМЕНЕНИЯ ПОДКАТИТЬ


namespace CRUD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Reflector reflector;
        private int DefaultTextBoxHeight = 30;
        private int DefaultFontSize = 18;
        private int DefaultPanelWidth = 574; 
        private byte[] DefaultPanelColor = { 0xCE, 0xE1, 0xE6};


        public MainWindow()
        {
            InitializeComponent();
            
            this.Title = "CRUD";
            this.Closed += MainWindow_Closed;

            reflector = new Reflector();
            reflector.LoadClasses(ClassesComboBox);
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
        }


        // очистка списка созданных объектов
        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
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
            
            reflector.ClearObjectsList();
        }


        // отрисовка всех полей класса в PropertiesListBox
        private void ClassesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PropertiesListBox.Items.Count != 0)
            {
                PropertiesListBox.Items.Clear();
            }
            CreateObjectButton.IsEnabled = true;
            EditObjectButton.IsEnabled = false;
            DeleteObjectButton.IsEnabled = false;
            string selectedClass = "StoneOcean." + (string)ClassesComboBox.SelectedItem;
            Type currentClass = reflector.GetTypeByName(selectedClass);
            DrawAllProperties(currentClass, PropertiesListBox);
            DrawAllFields(currentClass, PropertiesListBox);
            PropertiesListBox.Visibility = Visibility.Visible;
        }


        // отрисовка конкретно ПОЛЕЙ
        private void DrawAllFields(Type currentClass, ListBox propertiesListBox)
        {
            FieldInfo[] fields = currentClass.GetFields();
            foreach (FieldInfo field in fields)
            {
                StackPanel fieldStackPanel = new StackPanel();
                fieldStackPanel.Width = DefaultPanelWidth;
                fieldStackPanel.Background = new SolidColorBrush(Color.FromRgb(DefaultPanelColor[0], DefaultPanelColor[1], DefaultPanelColor[2]));
                fieldStackPanel.Name = field.Name + "FieldStackPanel";
                fieldStackPanel.Children.Add((UIElement)CreateTextBlockForProperties(field));
                if (field.FieldType.IsEnum)
                {
                    fieldStackPanel.Children.Add((UIElement)CreateComboBoxForProperties(field));
                }
                else if (field.FieldType.IsClass && !field.FieldType.IsPrimitive && (field.FieldType != typeof(string)))
                {
                    fieldStackPanel.Children.Add((UIElement)CreateClassComboBoxForProperties(field));
                }
                else
                {
                    fieldStackPanel.Children.Add((UIElement)CreateTextBoxForProperties(field));
                }
                propertiesListBox.Items.Add(fieldStackPanel);
            }
        }


        // отрисовка конкретно СВОЙСТВ
        private void DrawAllProperties(Type currentClass, ListBox propertiesListBox)
        {
            PropertyInfo[] properties = currentClass.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                StackPanel propertyStackPanel = new StackPanel();
                propertyStackPanel.Width = DefaultPanelWidth;
                propertyStackPanel.Background = new SolidColorBrush(Color.FromRgb(DefaultPanelColor[0], DefaultPanelColor[1], DefaultPanelColor[2]));
                propertyStackPanel.Name = property.Name + "PropertyStackPanel";
                propertyStackPanel.Children.Add((UIElement)CreateTextBlockForProperties(property));
                if (property.PropertyType.IsEnum)
                {
                    propertyStackPanel.Children.Add((UIElement)CreateComboBoxForProperties(property));
                }
                else if (property.PropertyType.IsClass && !property.PropertyType.IsPrimitive && (property.PropertyType != typeof(string)))
                {
                    propertyStackPanel.Children.Add((UIElement)CreateClassComboBoxForProperties(property));
                }
                else
                {
                    propertyStackPanel.Children.Add((UIElement)CreateTextBoxForProperties(property));
                }
                propertiesListBox.Items.Add(propertyStackPanel);
            }
        }


        // достаём имя для поля из атрибута
        private object CreateTextBlockForProperties(MemberInfo member)
        {
            TextBlock fieldName = new TextBlock();
            fieldName.Height = DefaultTextBoxHeight;
            fieldName.FontSize = DefaultFontSize;
            fieldName.Name = member.Name + "TextBlock";
            DisplayAttribute natName = (DisplayAttribute)Attribute.GetCustomAttribute(member, typeof(DisplayAttribute));
            if (natName != null)
            {
                fieldName.Text = natName.Name + ":";
            }
            else
            {
                fieldName.Text = member.Name + ":";
            }
            return fieldName;
        }


        // если поле обычного типа (текстовое поле)
        private object CreateTextBoxForProperties(MemberInfo member)
        {
            TextBox fieldContent = new TextBox();
            fieldContent.Height = DefaultTextBoxHeight;
            fieldContent.FontSize = DefaultFontSize;
            fieldContent.Name = member.Name + "TextBox";

            return fieldContent;
        }


        // если поле типа enum
        private object CreateComboBoxForProperties(MemberInfo member)
        {
            ComboBox fieldList = new ComboBox();
            fieldList.Height = DefaultTextBoxHeight;
            fieldList.FontSize = DefaultFontSize;
            fieldList.Name = member.Name + "EnumComboBox";
            Type enumType;
            if (member.MemberType == MemberTypes.Property)
            {
                enumType = ((PropertyInfo)member).PropertyType;
            }
            else
            {
                enumType = ((FieldInfo)member).FieldType;
            }

            foreach (string value in enumType.GetEnumNames())
            {
                fieldList.Items.Add(value);
            }

            return fieldList;
        }


        // если поле типа класс
        private object CreateClassComboBoxForProperties(MemberInfo member)
        {
            ComboBox classList = new ComboBox();
            classList.Height = DefaultTextBoxHeight;
            classList.FontSize = DefaultFontSize;
            classList.Name = member.Name + "SubClassComboBox";
            Type classType;
            if (member.MemberType == MemberTypes.Property)
            {
                classType = ((PropertyInfo)member).PropertyType;
            }
            else
            {
                classType = ((FieldInfo)member).FieldType;
            }

            classList.Items.Add(classType.Name);

            reflector.AddChildrenClasses(classList, classType);

            AddExistingObjects(classList, classType);
            // добавить уже созданные объекты подходящих классов
            //
            //


            classList.SelectionChanged += SubClassComboBox_SelectionChanged;
            return classList;
        }


        // добавить уже созданные объекты подходящих классов
        private void AddExistingObjects(ComboBox box, Type parentType)
        {
            foreach (object obj in reflector.objectsList)
            {
                //if (obj.Equals((ObjectsListBox.SelectedItem as ListBoxItem).Content))
                  //  return;
                Type objType = reflector.GetTypeByName(obj.ToString());
                if (reflector.CheckCompatibleTypes(parentType, objType))
                {
                    box.Items.Add(reflector.objectsList.IndexOf(obj).ToString() + " " + obj.ToString());
                }

            }
        }

        // /////////////////////////////////////////////////////////////////////////
        // сделать норм реакцию на выбор объекта
        // ////////////////////////////////////////////////////////////////////////

        // выбор класса для поля-объекта
        private void SubClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StackPanel parent = (StackPanel)(((ComboBox)sender).Parent);
            ListBox subList;
            
            if (parent.Children[parent.Children.Count - 1].GetType() == typeof(ListBox))
            {
                subList = (parent.Children[parent.Children.Count - 1] as ListBox);
                if (subList.Items.Count > 0)
                {
                    subList.Items.Clear();
                    parent.Children.RemoveAt(parent.Children.Count - 1);
                    subList = new ListBox();
                }
                else
                {
                    return;
                }
            }
            else
            {
                subList = new ListBox();
            }

            ComboBox current = (ComboBox)sender;
            subList = new ListBox();
            subList.Name = current.Name.Substring(0, current.Name.IndexOf("ComboBox")) + "SubClassListBox";
            parent.Children.Add(subList);

            if (subList.Items.Count != 0)
            {
                subList.Items.Clear();
            }

            string selectedClass = current.SelectedItem.ToString();
            if (selectedClass.IndexOf(" ") >= 0)
            {
                int index = Int32.Parse(selectedClass.Substring(0, selectedClass.IndexOf(" ")));
                selectedClass = selectedClass.Remove(0, selectedClass.IndexOf(" ") + 1);
                Type currentClass = reflector.GetTypeByName(selectedClass);
                DrawAllProperties(currentClass, subList);
                DrawAllFields(currentClass, subList);
                
                FillAllProperties(currentClass, reflector.objectsList[index], ref subList);
            }
            else
            {
                selectedClass = "StoneOcean." + selectedClass; ////////////
                Type currentClass = reflector.GetTypeByName(selectedClass);
                DrawAllProperties(currentClass, subList);
                DrawAllFields(currentClass, subList);
            }
            
        }


        // создание объекта с заданными значениями полей из PropertiesListBox
        //добавление его в ObjectsListBox (имя класса : имя объекта)
        private void CreateObjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Type objClass = reflector.GetTypeByName("StoneOcean." + (string)ClassesComboBox.SelectedItem);
                object currentObject = CreateSubObject(objClass, PropertiesListBox);
                if (currentObject != null)
                {
                    reflector.objectsList.Add(currentObject);

                    ListBoxItem item = new ListBoxItem();
                    item.Height = DefaultTextBoxHeight;
                    item.FontSize = DefaultFontSize - 2;
                    item.Selected += ObjectsListBox_Selected;
                    item.Content = currentObject;
                    ObjectsListBox.Items.Add(item);
                }
                if (ObjectsListBox.Visibility == Visibility.Hidden)
                    ObjectsListBox.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            PropertiesListBox.Items.Clear();
        }

        
        // непосредственно создание объекта
        private object CreateSubObject(Type currentClass, object list)
        {
            object currentObject = Activator.CreateInstance(currentClass);
            ListBox propertiesList = list as ListBox;
            object argument;

            try
            {
                for (int index = 0; index < propertiesList.Items.Count; index++)
                {
                    string name;
                    StackPanel currentPanel = propertiesList.Items[index] as StackPanel;
                    if (currentPanel.Children[currentPanel.Children.Count - 1].GetType() == typeof(ListBox))
                    {
                        ListBox listBox = currentPanel.Children[currentPanel.Children.Count - 1] as ListBox;
                        ComboBox objClassBox = currentPanel.Children[1] as ComboBox;
                        string variantName = objClassBox.SelectedItem.ToString();
                        if (variantName.IndexOf(" ") >= 0)
                        {
                            int num = Int32.Parse(variantName.Substring(0, variantName.IndexOf(" ")));
                            argument = Activator.CreateInstance(reflector.objectsList[num].GetType());
                            argument = reflector.objectsList[num];
                        }
                        else
                        {
                            Type objectClass = reflector.GetTypeByName("StoneOcean." + variantName);
                            argument = CreateSubObject(objectClass, listBox);
                        }
                        name = objClassBox.Name.Substring(0, objClassBox.Name.IndexOf("SubClassComboBox"));
                    }
                    else
                    {
                        if (currentPanel.Children[1].GetType() == typeof(ComboBox))
                        {
                            ComboBox propertyBox = currentPanel.Children[1] as ComboBox;
                            argument = propertyBox.SelectedItem;
                            name = propertyBox.Name.Substring(0, propertyBox.Name.IndexOf("EnumComboBox"));
                        }
                        else
                        {
                            TextBox PropertyText = currentPanel.Children[1] as TextBox;
                            argument = PropertyText.Text;
                            name = PropertyText.Name.Substring(0, PropertyText.Name.IndexOf("TextBox"));
                        }
                    }

                    MemberInfo currentMember = reflector.GetCurrentMember(currentClass, name);

                    if (currentMember.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo currentProperty = reflector.ConvertArgumentAsProperty(currentMember, ref argument);
                        currentProperty.SetValue(currentObject, argument);                        
                    }
                    else
                    {
                        FieldInfo currentField = reflector.ConvertArgumentAsField(currentMember,ref argument);
                        currentField.SetValue(currentObject, argument);
                    }
                }
                return currentObject;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        
        // изменение атрибутов выбранного в ObjectsListBox объекта
        private void EditObjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ObjectsListBox.SelectedItem != null)
            {
                try
                {
                    int index = ObjectsListBox.SelectedIndex;
                    ListBoxItem currentItem = ObjectsListBox.SelectedItem as ListBoxItem;
                    object currentObject = currentItem.Content;
                    Type currentClass = reflector.GetTypeByName(currentObject.ToString());
                    object newObject = CreateSubObject(currentClass, PropertiesListBox);
                    if (newObject != null)
                    {
                        currentItem.Content = newObject;
                        reflector.objectsList[index] = newObject;
                        currentObject = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            PropertiesListBox.Items.Clear();
        }


        //удаление выбранного в ObjectsListBox объекта
        //перерисовка списка ObjectsListBox
        private void DeleteObjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ObjectsListBox.SelectedItem != null)
            {
                try
                {
                    int index = ObjectsListBox.SelectedIndex;
                    reflector.objectsList.RemoveAt(index);
                    ObjectsListBox.Items.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            PropertiesListBox.Items.Clear();
        }


        // отрисовка полей уже созданного объекта при выборе его из списка
        private void ObjectsListBox_Selected(object sender, RoutedEventArgs e)
        {
            CreateObjectButton.IsEnabled = false;
            EditObjectButton.IsEnabled = true;
            DeleteObjectButton.IsEnabled = true;
            object currentObject = (e.Source as ListBoxItem).Content;
            if (currentObject != null)
            {
                Type currentClass = reflector.GetTypeByName(currentObject.ToString());

                if (PropertiesListBox.Items.Count != 0)
                {
                    PropertiesListBox.Items.Clear();
                }

                DrawAllProperties(currentClass, PropertiesListBox);
                DrawAllFields(currentClass, PropertiesListBox);
                FillAllProperties(currentClass, currentObject, ref PropertiesListBox);
            }
            if(ObjectsListBox.Items.Count == 1)
                (e.Source as ListBoxItem).IsSelected = false;
            PropertiesListBox.Visibility = Visibility.Visible;
        }


        // заполнение всех полей
        private void FillAllProperties(Type currentClass, object currentObject, ref ListBox propertiesList)
        {
            try
            {
                FieldInfo[] fields = currentClass.GetFields();
                PropertyInfo[] properties = currentClass.GetProperties();

                for (int index = 0; index < propertiesList.Items.Count; index++)
                {
                    StackPanel currentPanel = propertiesList.Items[index] as StackPanel;
                    if (currentPanel.Name.Contains("PropertyStackPanel"))
                    {
                        FillProperty(ref currentPanel, currentClass, currentObject);                        
                    }
                    else
                    {
                        FillField(ref currentPanel, currentClass, currentObject);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // заполнение поля-класса
        private void FillClassProperty(ComboBox classBox, StackPanel panel, PropertyInfo property, object obj)
        {
            ListBox listBox = new ListBox();
            listBox.Name = classBox.Name.Substring(0, classBox.Name.IndexOf("ComboBox")) + "ListBox";
            panel.Children.Add(listBox);
            Type className = property.GetValue(obj).GetType();
            classBox.SelectedIndex = classBox.Items.IndexOf(className.Name);
            DrawAllProperties(className, listBox);
            FillAllProperties(className, property.GetValue(obj), ref listBox);
        }


        // заполнение поля-enum
        private void FillEnumProperty(ref ComboBox enumBox, PropertyInfo property, object obj)
        {
            string value = property.GetValue(obj).ToString();
            foreach (object item in enumBox.Items)
            {
                if (item.ToString() == value)
                    enumBox.SelectedItem = item;
            }
        }


        // заполнение СВОЙСТВА
        private void FillProperty(ref StackPanel panel, Type type, object obj)
        {
            PropertyInfo[] properties = type.GetProperties();
            string propertyName = panel.Name.Substring(0, panel.Name.IndexOf("PropertyStackPanel"));
            PropertyInfo currentProperty = properties[0];
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == propertyName)
                    currentProperty = property;
            }
            if (currentProperty.PropertyType.IsClass && !currentProperty.PropertyType.IsPrimitive && (currentProperty.PropertyType != typeof(string)))
            {
                ComboBox currentClassBox = panel.Children[1] as ComboBox;
                FillClassProperty(currentClassBox, panel, currentProperty, obj);
            }
            else if (currentProperty.PropertyType.IsEnum)
            {
                ComboBox currentEnumBox = panel.Children[1] as ComboBox;
                FillEnumProperty(ref currentEnumBox, currentProperty, obj);
            }
            else
            {
                TextBox currentText = panel.Children[1] as TextBox;
                currentText.Text = currentProperty.GetValue(obj).ToString();
            }
        }


        // заполнение ПОЛЯ
        private void FillField(ref StackPanel panel, Type type, object obj)
        {
            FieldInfo[] fields = type.GetFields();
            string fieldName = panel.Name.Substring(0, panel.Name.IndexOf("FieldStackPanel"));
            FieldInfo currentField = fields[0];
            foreach (FieldInfo field in fields)
            {
                if (field.Name == fieldName)
                    currentField = field;
            }

            if (currentField.FieldType.IsClass && !currentField.FieldType.IsPrimitive && (currentField.FieldType != typeof(string)))
            {
                ComboBox currentClassBox = panel.Children[1] as ComboBox;
                ListBox currentListBox = panel.Children[panel.Children.Count - 1] as ListBox;
                currentClassBox.SelectedItem = currentClassBox.FindName(currentField.GetValue(obj).ToString());
                FillAllProperties(currentField.FieldType, currentField, ref currentListBox);
            }
            else if (currentField.FieldType.IsEnum)
            {
                ComboBox currentEnumBox = panel.Children[1] as ComboBox;
                currentEnumBox.SelectedItem = currentEnumBox.FindName(currentField.GetValue(obj).ToString());
            }
            else
            {
                TextBox currentText = panel.Children[1] as TextBox;
                currentText.Text = currentField.GetValue(obj).ToString();
            }
        }
        
    }

    
}
