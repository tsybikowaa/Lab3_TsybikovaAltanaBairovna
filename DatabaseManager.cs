using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTestingTriangle
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddData(float length1, float length2, float length3, string triangleType, string errorMessage)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Triangle (Length_1, Length_2, Length_3, Type_triangle, ErrorMessage) " +
                               "VALUES (@Length_1, @Length_2, @Length_3, @Type_triangle, @ErrorMessage)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Length_1", length1);
                    command.Parameters.AddWithValue("@Length_2", length2);
                    command.Parameters.AddWithValue("@Length_3", length3);
                    command.Parameters.AddWithValue("@Type_triangle", triangleType);
                    command.Parameters.AddWithValue("@ErrorMessage", errorMessage);

                    command.ExecuteNonQuery();
                }
            }
        }
        public interface IUserInterface
        {
            void GetUserInput();
            void DisplayMessage(string message);
        }

      

        public class ExternalDependencySimulator : IExternalDependency
        {
            public void SimulateDataTransfer(float length1, float length2, float length3)
            {
                // Simulate data transfer to external process
                Console.WriteLine($"Данные переданы: {length1}, {length2}, {length3}");
            }
        }

        public interface IExternalDependency
        {
            void SimulateDataTransfer(float length1, float length2, float length3);
        }
        public DataTable GetData()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Triangle";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return dataTable;
        }
        public bool DeleteData(string length1, string length2, string length3, string triangleType, string errorMessage)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Triangle WHERE Length_1 = @Length_1 AND Length_2 = @Length_2 AND Length_3 = @Length_3 AND @Type_triangle = @Type_triangle AND @ErrorMessage = @ErrorMessage";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Length_1", length1);
                        command.Parameters.AddWithValue("@Length_2", length2);
                        command.Parameters.AddWithValue("@Length_3", length3);
                        command.Parameters.AddWithValue("@Type_triangle", triangleType);
                        command.Parameters.AddWithValue("@ErrorMessage", errorMessage);

                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

    }
}
