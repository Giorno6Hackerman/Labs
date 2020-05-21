using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Reflection;
using System.IO;
using System.Security.Permissions;
using System.Security;
using System.Reflection.Emit;

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
            LoadPlagins();
            objects = obj;
            if(!IsSerialize)
                obj = objects;
            else
                objects = obj;

            SetWatcher();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void SetWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = plaginPath;
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Attributes;
            watcher.Filter = "*.dll";

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;

            watcher.EnableRaisingEvents = true;
        }


        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            encryptionTypeComboBox.Items.Clear();
            LoadPlagins();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            encryptionTypeComboBox.Items.Clear();
            LoadPlagins();
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

        public string plaginPath = "D:/prog/4 sem/OOTPISP/Labs/Lab_4/plagins";
        private List<Type> plagins = new List<Type>();


        [Serializable]
        private class Pass
        {
            private readonly AppDomain _domain;
            private readonly string _dllPath;
            //private ComboBox _box;
            public List<Type> _plagins = new List<Type>();

            public Pass(AppDomain domain, string dllPath)
            {
                _domain = domain;
                _dllPath = dllPath;
                //_box = box;
            }

            public void Foo()
            {
                DirectoryInfo dir = new DirectoryInfo(_dllPath);
                FileInfo[] files = dir.GetFiles("*.dll");

                foreach (FileInfo file in files)
                {
                    string name = _dllPath + "/" + file.Name;

                    AssemblyName cur = AssemblyName.GetAssemblyName(name);
                    Assembly plagin = _domain.Load(cur);


                    //Assembly plagin = Assembly.LoadFrom(file);
                    Type[] classes = plagin.GetTypes();
                    foreach (Type type in classes)
                    {
                        if (type.IsClass)
                        {
                            //_box.Items.Add(type.Name);
                            _plagins.Add(type);
                        }
                    }
                }
            }
        }


        private void LoadPlagins()
        {

            DirectoryInfo dir = new DirectoryInfo(plaginPath);
            FileInfo[] files = dir.GetFiles("*.dll");

            try
            {
                AppDomain domain = AppDomain.CreateDomain("Satan");
                //string[] files = Directory.GetFiles(plaginPath, "*.dll");
                var pass = new Pass(domain, plaginPath);
                domain.DoCallBack(pass.Foo);
                plagins = pass._plagins;
                foreach (Type type in plagins)
                {
                    encryptionTypeComboBox.Items.Add(type.Name);
                }

                AppDomain.Unload(domain);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void chooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsSerialize)
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "aes files (*.ae)|*.ae|Rij files (*.rj)|*.rj";
                if (fileDialog.ShowDialog() == true)
                {
                    fileName = fileDialog.FileName;
                }
            }
            else
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "aes files (*.ae)|*.ae|Rij files (*.rj)|*.rj";
                if (fileDialog.ShowDialog() == true)
                {
                    fileName = fileDialog.FileName;
                }
                string ext = fileName.Substring(fileName.LastIndexOf(".") + 1);
                if (ext == "rj")
                {
                    encryptionTypeComboBox.SelectedItem = "RijndaelEncryptor";
                }
                else if (ext == "ae")
                {
                    encryptionTypeComboBox.SelectedItem = "AesEncryptor";
                }
                encryptionTypeComboBox.IsEnabled = false;
            }
            fileNameTextBox.Text = fileName;
            if ((serializationTypeComboBox.SelectedItem != null) && (encryptionTypeComboBox.SelectedItem != null))
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
            try
            {
                Type serialization = serLib.GetType("CRUD." + serializationTypeComboBox.SelectedItem.ToString());
                object serializer = Activator.CreateInstance(serialization);
                MethodInfo method = serialization.GetMethod("Serialize");
                //FileStream file = File.Create(fileName);
                MemoryStream file = new MemoryStream();
                object[] param = new object[2] { file, objects.ToArray() };
                method.Invoke(serializer, param);////////

                file.Position = 0;
                Type encryption = plagins[encryptionTypeComboBox.SelectedIndex];
                object encryptor = Activator.CreateInstance(encryption);
                MethodInfo enMethod = encryption.GetMethod("Encrypt");
                object[] param2 = new object[2] { file, fileName };
                enMethod.Invoke(encryptor, param2);
                file.Close();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.DialogResult = false;
            }
        }

        private void deserializeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Type serialization = serLib.GetType("CRUD." + serializationTypeComboBox.SelectedItem.ToString());
                object serializer = Activator.CreateInstance(serialization);
                MethodInfo method = serialization.GetMethod("Deserialize");
                //MemoryStream file = new MemoryStream();

                Type decryption = plagins[encryptionTypeComboBox.SelectedIndex];
                object decryptor = Activator.CreateInstance(decryption);
                MethodInfo deMethod = decryption.GetMethod("Decrypt");
                MemoryStream file = new MemoryStream();
                object[] param2 = new object[2] { file, fileName };
                deMethod.Invoke(decryptor, param2);

                //FileStream file = File.OpenRead(fileName);
                object[] param = new object[1] { file };
                objects.AddRange((object[])method.Invoke(serializer, param));////////
                file.Close();
                rer = objects.ToArray();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.DialogResult = false;
            }
        }

        private void serializationTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((fileNameTextBox.Text != "") && (encryptionTypeComboBox.SelectedItem != null))
                if (IsSerialize)
                {
                    serializeButton.IsEnabled = true;
                }
                else
                {
                    deserializeButton.IsEnabled = true;
                }
        }

        private void encryptionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((fileNameTextBox.Text != "") && (serializationTypeComboBox.SelectedItem != null))
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
