using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static IntegrationTestingTriangle.DatabaseManager;

namespace IntegrationTestingTriangle
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseManager dbManager;
        private IExternalDependency externalDependency;
        public MainWindow()
        {
            InitializeComponent();
          
            dbManager = new DatabaseManager(connectionString);
            externalDependency = new ExternalDependencySimulator();
           
       
        }
        string connectionString = @"Data Source = LAPTOP-9LSSTQJA;  Initial Catalog=altana; Integrated Security=True;";
       
        public void GetUserInput()
        {
            float length1, length2, length3;

            if (float.TryParse(Length1TextBox.Text, out length1) &&
                float.TryParse(Length2TextBox.Text, out length2) &&
                float.TryParse(Length3TextBox.Text, out length3))
            {
                string triangleType = TriangleUtility.GetTriangleType(length1, length2, length3);
                string errorMessage = ErrorMessageTextBox.Text;

                dbManager.AddData(length1, length2, length3, triangleType, errorMessage);
                externalDependency.SimulateDataTransfer(length1, length2, length3);

                MessageBox.Show("Данные успешно добавлены и переданы стороннему процессу.");
            }
            else
            {
                MessageBox.Show("Неверный формат данных.");
            }
        }

        public void DisplayMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {
            GetUserInput();
        }
       

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)DG.SelectedItem;
            if (selectedRow != null)
            {
                string length1 = selectedRow.Row["Length_1"].ToString();
                string length2 = selectedRow.Row["Length_2"].ToString();
                string length3 = selectedRow.Row["Length_3"].ToString();
                string triangleType = selectedRow.Row["Type_triangle"].ToString();
                string errorMessage = selectedRow.Row["ErrorMessage"].ToString();


                bool success = dbManager.DeleteData(length1, length2, length3, triangleType, errorMessage);
                if (success)
                {
                    MessageBox.Show("Данные успешно удалены.");
                  
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении данных из базы данных.");
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dataTable = dbManager.GetData();
                DG.ItemsSource = dataTable.DefaultView;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка при загрузке данных из базы данных: " + ex.Message);
            }
        }
    }

}

  


   