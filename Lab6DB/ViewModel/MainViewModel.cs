using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using Lab6DB.Model;
using System.Windows.Input;
using System.Reflection.Metadata;
using static System.Windows.Forms.AxHost;
using System.IO;
using System.Xml.Schema;
using Core0;

namespace Lab6DB.ViewModel
{
    public class MainViewModel : MyViewModel
    {

        private LoaderFiles Model;
        private ElementDB elementDB;
        private ViewModelRewriteTableWindow vmRewriteTable = new ViewModelRewriteTableWindow();

        private ObservableCollection<BaseItem> treeElement = new ObservableCollection<BaseItem>();
        private ObservableCollection<string> combpBoxElement = new ObservableCollection<string>();
        private string comboBoxSelectItem = "";
        private ObservableCollection<DataTable> collectionTableElement = new ObservableCollection<DataTable>();
        private DataTable tableElement = new DataTable();
        private string contentErrorWindow;
        private ObservableCollection<ElementDB> elementDBs = new ObservableCollection<ElementDB>();


        public ObservableCollection<BaseItem> TreeElement
        {
            get { return treeElement; }
            set
            {
                treeElement = value;
                OnPropertyChanged(nameof(TreeElement));
            }
        }
        public ObservableCollection<string> ComboBoxElement
        {
            get { return combpBoxElement; }
            set
            {
                combpBoxElement = value;
                OnPropertyChanged(nameof(ComboBoxElement));
            }
        }
        public string ComboBoxSelectItem
        {
            get { return comboBoxSelectItem; }
            set
            {
                comboBoxSelectItem = value;
                OnPropertyChanged(nameof(ComboBoxSelectItem));
            }
        }
        public ObservableCollection<DataTable> CollectionTableElement
        {
            get { return collectionTableElement; }
            set
            {
                collectionTableElement = value;
                OnPropertyChanged(nameof(CollectionTableElement));
            }
        }
        public DataTable TableElement
        {
            get { return tableElement; }
            set
            {
                tableElement = value;
                OnPropertyChanged(nameof(TableElement));
            }
        }
        public string ContentErrorWindow
        {
            get { return contentErrorWindow; }
            set
            {
                contentErrorWindow = value;
                OnPropertyChanged(nameof(ContentErrorWindow));
            }
        }
        public ObservableCollection<ElementDB> ElementDBs
        {
            get { return elementDBs; }
            set
            {
                elementDBs = value;
                OnPropertyChanged(nameof(ElementDBs));
            }
        }



        public ICommand AddFiles
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    DialogResult result = dialog.ShowDialog();

                    string folder = "";
                    if (result == DialogResult.OK)
                    {
                        folder = dialog.SelectedPath;
                    }
                    Model = new LoaderFiles(folder);
                    if(Model.State != null)
                    { 
                        ContentErrorWindow = Model.State;
                        if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
                            CreateViewModel();
                    }
                });

            }
        }

        private void CreateViewModel()
        {
            foreach (AdditionalDataPattern addPattern in Model.Patterns.Values)
            {
                PatternObjectDB pattern = addPattern.Pattern;
                DataTable table = new DataTable(pattern.Name);
                BaseItem item = new BaseItem(pattern.Name);
                ObservableCollection<ItemTextBox> columns = new ObservableCollection<ItemTextBox>();

                foreach (PatternPropertyDB property in pattern.Properties.Values)
                {
                    BaseItem subitem = new BaseItem(property.Name);
                    item.Children.Add(subitem);
                    DataColumn column = new DataColumn(property.Name);
                    table.Columns.Add(column);
                    ItemTextBox itemColumn = new ItemTextBox(property.Name, property.Type);
                    columns.Add(itemColumn);
                }
                TreeElement.Add(item);
                ComboBoxElement.Add(pattern.Name);
                elementDB = new ElementDB();
                table = CreateRowTable(pattern.Name, table);
                
                elementDB.WayJson = addPattern.WayJson;
                elementDB.Pattern = pattern;
                elementDB.Columns = columns;
                elementDB.Table = table;
                ElementDBs.Add(elementDB);
                CollectionTableElement.Add(table);

            }
        }
        private DataTable CreateRowTable(string nameObject, DataTable table)
        {
            foreach (string nameObjectDB in Model.Objects.Keys)
            {
                if (nameObjectDB.Contains(nameObject))
                {
                    elementDB.WayCSV = Model.Objects[nameObject].WayCSV;
                    foreach (ObjectDB objectDB in Model.Objects[nameObject].DBObject)
                    {
                        DataRow row = table.NewRow();
                        int nuberCell = 0;
                        foreach (string text in objectDB.PropertyDB.Values)
                        {
                            row[nuberCell] = text;
                            nuberCell++;
                        }
                        table.Rows.Add(row);
                    }
                }
            }
            return table;
        }

        public ICommand Update
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    TableElement = new DataTable();
                    for(int i = 0; i < ElementDBs.Count; i++ )
                    {
                        if (ElementDBs[i].Table.TableName.CompareTo(ComboBoxSelectItem) == 0 && vmRewriteTable.Element != null)
                        {
                            ElementDBs[i].Table = vmRewriteTable.Element.Table;
                            TreeElement[i] = RewriteBaseItem(ElementDBs[i].Table);
                            ComboBoxElement[i] = ElementDBs[i].Table.TableName;
                            CollectionTableElement[i] = vmRewriteTable.Element.Table;
                        }
                    }
                });
            }
        }

        private BaseItem RewriteBaseItem(DataTable table)
        {
            BaseItem item = new BaseItem(table.TableName);
            foreach(DataColumn column in table.Columns)
            {
                BaseItem subItem = new BaseItem(column.ToString());
                item.Children.Add(subItem);
            }
            return item;
        }


        public ICommand OutputTable
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    string namePattern = ComboBoxSelectItem;
                    foreach (DataTable table in CollectionTableElement)
                    {
                        if (table.TableName.CompareTo(namePattern) == 0)
                        {
                            TableElement = table;
                        }
                    }
                });
            }
        }

        public ICommand CreateDB
        {
            get 
            { 
                return new CommandDelegate(parameter => 
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    DialogResult result = dialog.ShowDialog();

                    string folder = "";
                    if (result == DialogResult.OK)
                    {
                        folder = dialog.SelectedPath;
                    }
                    //File.Create(folder + "\\ok.txt");//CreateDirectory для папок
                    ViewModelCreateWindow vmCreateWindow = new ViewModelCreateWindow();
                    CreateWindow createWindow = new CreateWindow();
                    createWindow.DataContext = vmCreateWindow;
                    vmCreateWindow.FullFolderPath = folder;
                    vmCreateWindow.CreateWindow = createWindow;
                    createWindow.Show();
                });
            }
        }
        public ICommand Clear
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    TreeElement.Clear();
                    CollectionTableElement.Clear();
                    ComboBoxElement.Clear();
                    TableElement = new DataTable();
                });
            }
        }
        public ICommand RewriteTable
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    string namePattern = ComboBoxSelectItem;
                    foreach (ElementDB element in ElementDBs)
                    {
                        DataTable table = element.Table; 
                        if (table.TableName.CompareTo(namePattern) == 0)
                        {
                            if (table.Rows.Count == 0)
                            {
                                
                                RewriteTableWindow rewriteTableWindow = new RewriteTableWindow();
                                rewriteTableWindow.DataContext = vmRewriteTable;

                                vmRewriteTable.NameTable = namePattern;
                                vmRewriteTable.Columns = element.Columns;
                                vmRewriteTable.OldColumns = RewriteFormatCollectionList(element.Columns);
                                vmRewriteTable.NameColumns = CreateCollectionNameColumns(element);
                                vmRewriteTable.Element = element;

                                rewriteTableWindow.Show();
                            }
                            else
                            {
                                ContentErrorWindow = "Нельзя редактировать таблицу, если в ней есть данные.";
                            }
                        }
                    }
                });
            }
        }

        private ObservableCollection<ItemTextBox> RewriteFormatCollectionList(ObservableCollection<ItemTextBox> items)
        {
            ObservableCollection<ItemTextBox> copyCollection = new ObservableCollection<ItemTextBox>();
            foreach (ItemTextBox item in items) 
            {
                ItemTextBox copyItem = new ItemTextBox(item.Name, item.SelectedElementComboBoxType);
                copyCollection.Add(copyItem);
            }
            return copyCollection;
        }

        private ObservableCollection<string> CreateCollectionNameColumns(ElementDB element)
        { 
            ObservableCollection<string> names = new ObservableCollection<string>();
            foreach(ItemTextBox item in element.Columns)
            {
                names.Add(item.Name);
            }
            return names;
        }
        public ICommand RewriteData
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    string namePattern = ComboBoxSelectItem;
                    foreach (ElementDB element in ElementDBs)
                    {
                        DataTable table = element.Table;
                        if (table.TableName.CompareTo(namePattern) == 0)
                        {
                            ViewModelRewriteDataWindow vmRewriteData = new ViewModelRewriteDataWindow();
                            RewriteDataWindow rewriteDataWindow = new RewriteDataWindow();
                            rewriteDataWindow.DataContext = vmRewriteData;

                            vmRewriteData.NameTable = namePattern;
                            vmRewriteData.SelectedTable = table;
                            vmRewriteData.Element = element;
                            vmRewriteData.CollectDelete = CreateCollectionDelete(element);


                            rewriteDataWindow.Show();
                        }
                    }
                });
            }
        }
        private ObservableCollection<string> CreateCollectionDelete(ElementDB element)
        {
            ObservableCollection<string> collect = new ObservableCollection<string>();
            foreach(DataRow row in element.Table.Rows)
            {
                collect.Add(row[0].ToString());
            }
            return collect;
        }
    }
}
