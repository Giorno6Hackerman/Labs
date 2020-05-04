using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using System.ComponentModel.DataAnnotations;


namespace CRUD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Assembly lib;
        //private string libPath = "D:/prog/4 sem/OOTPISP/Labs/StoneLibrary/bin/Debug/StoneLibrary.dll";
        //private Type[] classes;
        //private List<object> objectsList;
        private Reflector reflector;
        private int DefaultTextBoxHeight = 30;
        private int DefaultFontSize = 18;
        private int DefaultPanelWidth = 574; 
        private byte[] DefaultPanelColor = { 0xCE, 0xE1, 0xE6};

        public MainWindow()
        {
            InitializeComponent();
            
            this.Title = "CRUD";
            this.Closed += MainWindow_Closed;
            // загрузка всех классов в ClassesComboBox
            //GetClassesNames(out classes);

            reflector = new Reflector();
            reflector.LoadClasses(ClassesComboBox);
            /*
            objectsList = new List<object>();
            foreach (Type className in classes)
            {
                if (className.IsClass)
                    ClassesComboBox.Items.Add(className.Name);
            }
            */
        }

        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            reflector.ClearObjectsList();
        }

        /*
        public void GetClassesNames(out Type[] classes)
        {
            lib = Assembly.LoadFrom(libPath);
            classes = lib.GetTypes();
        }
        */


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
            PropertiesListBox.Visibility = Visibility.Visible;
        }


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


        private object CreateTextBoxForProperties(MemberInfo member)
        {
            TextBox fieldContent = new TextBox();
            fieldContent.Height = DefaultTextBoxHeight;
            fieldContent.FontSize = DefaultFontSize;
            fieldContent.Name = member.Name + "TextBox";

            return fieldContent;
        }


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
            //MemberInfo[] values = enumType.GetMembers();
            foreach (string value in enumType.GetEnumNames())
            {
                /*if((value.MemberType == MemberTypes.Field) && (value.Name != "value__"))
                    fieldList.Items.Add(value.Name);*/
                fieldList.Items.Add(value);
            }

            return fieldList;
        }


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

            classList.SelectionChanged += SubClassComboBox_SelectionChanged;
            return classList;
        }


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

            string selectedClass = "StoneOcean." + (string)current.SelectedItem;
            Type currentClass = reflector.GetTypeByName(selectedClass);
            DrawAllProperties(currentClass, subList);
            
        }


        // создание объекта с заданными значениями полей из PropertiesListBox
        //добавление его в ObjectsListBox (имя класса : имя объекта)
        private void CreateObjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Type objClass = lib.GetType("StoneOcean." + (string)ClassesComboBox.SelectedItem);
                object currentObject = CreateSubObject(objClass, PropertiesListBox);
                if (currentObject != null)
                {
                    objectsList.Add(currentObject);

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
        }


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
                        Type objectClass = reflector.GetTypeByName("StoneOcean." + (string)objClassBox.SelectedItem);
                        argument = CreateSubObject(objectClass, listBox);
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

                    /*
                    MemberInfo[] members = currentClass.GetMember(name);
                    MemberInfo currentMember = members[0];
                    foreach (MemberInfo member in members)
                    {
                        if (member.MemberType == MemberTypes.Property || member.MemberType == MemberTypes.Field)
                        {
                            currentMember = member;
                        }
                    }
                    */

                    MemberInfo currentMember = reflector.GetCurrentMember(currentClass, name);

                    if (currentMember.MemberType == MemberTypes.Property)
                    {
                        /*
                        PropertyInfo currentProperty = currentMember as PropertyInfo;
                        propertyType = currentProperty.PropertyType;
                        if (BasicTypes.Contains(propertyType))
                        {
                            argument = BasicTypeConvertion(argument, propertyType);
                        }
                        if (propertyType.IsEnum)
                        {
                            argument = EnumTypeConvertion(argument, propertyType);
                        }
                        */
                        PropertyInfo currentProperty = reflector.ConvertArgumentAsProperty(currentMember, argument);
                        currentProperty.SetValue(currentObject, argument);
                        
                    }
                    else
                    {
                        /*
                        FieldInfo currentField = currentMember as FieldInfo;
                        propertyType = currentField.FieldType;
                        if (BasicTypes.Contains(propertyType) || propertyType.IsEnum)
                        {
                            argument = BasicTypeConvertion(argument, propertyType);
                        }
                        if (propertyType.IsEnum)
                        {
                            argument = EnumTypeConvertion(argument, propertyType);
                        }
                        */
                        FieldInfo currentField = reflector.ConvertArgumentAsField(currentMember, argument);
                        
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
                    Type currentClass = lib.GetType(currentObject.ToString());
                    object newObject = CreateSubObject(currentClass, PropertiesListBox);
                    if (newObject != null)
                    {
                        currentItem.Content = newObject;
                        objectsList[index] = newObject;
                        currentObject = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
                    objectsList.RemoveAt(index);
                    ObjectsListBox.Items.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ObjectsListBox_Selected(object sender, RoutedEventArgs e)
        {
            CreateObjectButton.IsEnabled = false;
            EditObjectButton.IsEnabled = true;
            DeleteObjectButton.IsEnabled = true;
            object currentObject = (e.Source as ListBoxItem).Content;
            if (currentObject != null)
            {
                Type currentClass = lib.GetType(currentObject.ToString());

                if (PropertiesListBox.Items.Count != 0)
                {
                    PropertiesListBox.Items.Clear();
                }

                DrawAllProperties(currentClass, PropertiesListBox);
                FillAllProperties(currentClass, currentObject, PropertiesListBox);
            }
            if(ObjectsListBox.Items.Count == 1)
                (e.Source as ListBoxItem).IsSelected = false;
        }

        private void FillAllProperties(Type currentClass, object currentObject, ListBox propertiesList)
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
                        string propertyName = currentPanel.Name.Substring(0, currentPanel.Name.IndexOf("PropertyStackPanel"));
                        PropertyInfo currentProperty = properties[0];
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.Name == propertyName)
                                currentProperty = property;
                        }

                        if (currentProperty.PropertyType.IsClass && !currentProperty.PropertyType.IsPrimitive && (currentProperty.PropertyType != typeof(string)))
                        {
                            ComboBox currentClassBox = currentPanel.Children[1] as ComboBox;
                            ListBox currentListBox = new ListBox();
                            currentListBox.Name = currentClassBox.Name.Substring(0, currentClassBox.Name.IndexOf("ComboBox")) + "ListBox";
                            currentPanel.Children.Add(currentListBox);
                            Type className = currentProperty.GetValue(currentObject).GetType();
                            currentClassBox.SelectedIndex = currentClassBox.Items.IndexOf(className.Name);
                            DrawAllProperties(className, currentListBox);
                            FillAllProperties(className, currentProperty.GetValue(currentObject), currentListBox);
                        }
                        else if (currentProperty.PropertyType.IsEnum)
                        {
                            ComboBox currentEnumBox = currentPanel.Children[1] as ComboBox;
                            string value = currentProperty.GetValue(currentObject).ToString();
                            foreach (object item in currentEnumBox.Items)
                            {
                                if (item.ToString() == value)
                                    currentEnumBox.SelectedItem = item;
                            }
                        }
                        else
                        {
                            TextBox currentText = currentPanel.Children[1] as TextBox;
                            currentText.Text = currentProperty.GetValue(currentObject).ToString();
                        }
                    }
                    else
                    {
                        string fieldName = currentPanel.Name.Substring(0, currentPanel.Name.IndexOf("FieldStackPanel"));
                        FieldInfo currentField = fields[0];
                        foreach (FieldInfo field in fields)
                        {
                            if (field.Name == fieldName)
                                currentField = field;
                        }

                        if (currentField.FieldType.IsClass && !currentField.FieldType.IsPrimitive && (currentField.FieldType != typeof(string)))
                        {
                            ComboBox currentClassBox = currentPanel.Children[1] as ComboBox;
                            ListBox currentListBox = currentPanel.Children[currentPanel.Children.Count - 1] as ListBox;
                            currentClassBox.SelectedItem = currentClassBox.FindName(currentField.GetValue(currentObject).ToString());
                            FillAllProperties(currentField.FieldType, currentField, currentListBox);
                        }
                        else if (currentField.FieldType.IsEnum)
                        {
                            ComboBox currentEnumBox = currentPanel.Children[1] as ComboBox;
                            currentEnumBox.SelectedItem = currentEnumBox.FindName(currentField.GetValue(currentObject).ToString());
                        }
                        else
                        {
                            TextBox currentText = currentPanel.Children[1] as TextBox;
                            currentText.Text = currentField.GetValue(currentObject).ToString();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        
    }

    
}
