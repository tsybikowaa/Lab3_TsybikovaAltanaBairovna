using IntegrationTestingTriangle;
using static IntegrationTestingTriangle.DatabaseManager;
using System.Data;
using Moq;
using System.Data.SqlClient;
using System;

namespace TestProject1
{
    [TestFixture]

    public class Tests
    {
        private const string connectionString = @"Data Source = LAPTOP-9LSSTQJA;  Initial Catalog=altana; Integrated Security=True;";
        [Test]
        public void AddData_Should_Insert_Data_Into_Database1()
        {
            // Arrange
            float length1 = 3.0f;
            float length2 = 4.0f;
            float length3 = 5.0f;
            string triangleType = "разносторонний";
            string errorMessage = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Clear existing data
                string clearQuery = "DELETE FROM Triangle";
                using (SqlCommand clearCommand = new SqlCommand(clearQuery, connection))
                {
                    clearCommand.ExecuteNonQuery();
                }
            }

            // Create an instance of DatabaseManager
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            // Act
            dbManager.AddData(length1, length2, length3, triangleType, errorMessage);

            // Assert
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM Triangle";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    int rowCount = (int)selectCommand.ExecuteScalar();
                    Assert.AreEqual(1, rowCount);
                }
            }
        }
  

        [Test]
        public void AddData_WhenCalled_ShouldInsertDataIntoDatabase()
        {
            // Arrange
            float length1 = 1.0f;
            float length2 = 2.0f;
            float length3 = 3.0f;
            string triangleType = "TestTriangle";
            string errorMessage = "TestError";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Triangle";
                int initialRowCount = (int)command.ExecuteScalar();

                // Act
                DatabaseManager databaseManager = new DatabaseManager(connectionString);
                databaseManager.AddData(length1, length2, length3, triangleType, errorMessage);

                command.CommandText = "SELECT COUNT(*) FROM Triangle";
                int finalRowCount = (int)command.ExecuteScalar();

                // Assert
                Assert.AreEqual(initialRowCount + 1, finalRowCount);
            }
        }
    
        [Test]
        public void GetTriangleType_Should_Return_Equilateral_For_Equilateral_Triangle()
        {
            // Arrange
            float sideA = 5.0f;
            float sideB = 5.0f;
            float sideC = 5.0f;

            // Act
            string triangleType = TriangleUtility.GetTriangleType(sideA, sideB, sideC);

            // Assert
            Assert.AreEqual("равносторонний", triangleType);
        }

        [Test]
        public void GetTriangleType_Should_Return_Isosceles_For_Isosceles_Triangle()
        {
            // Arrange
            float sideA = 5.0f;
            float sideB = 5.0f;
            float sideC = 6.0f;

            // Act
            string triangleType = TriangleUtility.GetTriangleType(sideA, sideB, sideC);

            // Assert
            Assert.AreEqual("равнобедренный", triangleType);
        }

        [Test]
        public void GetTriangleType_Should_Return_Scalene_For_Scalene_Triangle()
        {
            // Arrange
            float sideA = 3.0f;
            float sideB = 4.0f;
            float sideC = 5.0f;

     
            // Act
            string triangleType = TriangleUtility.GetTriangleType(sideA, sideB, sideC);

            // Assert
            Assert.AreEqual("разносторонний", triangleType);
        }

        [Test]
        public void GetTriangleType_Should_Return_NotTriangle_For_Invalid_Triangle()
        {
            // Arrange
            float sideA = 1.0f;
            float sideB = 2.0f;
            float sideC = 7.0f;

            // Act
            string triangleType = TriangleUtility.GetTriangleType(sideA, sideB, sideC);

            // Assert
            Assert.AreEqual("не треугольник", triangleType);
        }
        [Test]
        public void GetData_Should_Retrieve_Data_From_Database()
        {
            // Arrange
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Clear existing data
                string clearQuery = "DELETE FROM Triangle";
                using (SqlCommand clearCommand = new SqlCommand(clearQuery, connection))
                {
                    clearCommand.ExecuteNonQuery();
                }

                // Insert test data
                string insertQuery = "INSERT INTO Triangle (Length_1, Length_2, Length_3, Type_triangle, ErrorMessage) " +
                    "VALUES (3.0, 4.0, 5.0, 'разносторонний', '')";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.ExecuteNonQuery();
                }
            }

            // Create an instance of DatabaseManager
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            // Act
            DataTable dataTable = dbManager.GetData();

            // Assert
            Assert.AreEqual(1, dataTable.Rows.Count);
            Assert.AreEqual(3.0f, dataTable.Rows[0]["Length_1"]);
            Assert.AreEqual(4.0f, dataTable.Rows[0]["Length_2"]);
            Assert.AreEqual(5.0f, dataTable.Rows[0]["Length_3"]);
            Assert.AreEqual("разносторонний", dataTable.Rows[0]["Type_triangle"]);
            Assert.AreEqual("", dataTable.Rows[0]["ErrorMessage"]);
        }

        [Test]
        public void DeleteData_Should_Delete_Data_From_Database1()
        {
            // Arrange
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Clear existing data
                string clearQuery = "DELETE FROM Triangle";
                using (SqlCommand clearCommand = new SqlCommand(clearQuery, connection))
                {
                    clearCommand.ExecuteNonQuery();
                }

                // Insert test data
                string insertQuery = "INSERT INTO Triangle (Length_1, Length_2, Length_3, Type_triangle, ErrorMessage) " +
                    "VALUES (3.0, 4.0, 5.0, 'разносторонний', '')";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.ExecuteNonQuery();
                }
            }

            // Create an instance of DatabaseManager
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            // Act
            bool success = dbManager.DeleteData("3.0", "4.0", "5.0", "разносторонний", "");

            // Assert
            Assert.IsTrue(success);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM Triangle";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    int rowCount = (int)selectCommand.ExecuteScalar();
                    Assert.AreEqual(0, rowCount);
                }
            }
        }
        [Test]
        public void GetUserInput_Should_Display_ErrorMessage_When_Input_Is_Invalid()
        {
            // Arrange
            string length1 = "abc"; // Invalid input
            string length2 = "2.0";
            string length3 = "3.0";
            string triangleType = "разносторонний";
            string errorMessage = "";

            Mock<IUserInterface> userInterfaceMock = new Mock<IUserInterface>();
            MainWindow dbManager = new MainWindow("connectionString", null);

            // Act
            dbManager.GetUserInput(length1, length2, length3, triangleType, errorMessage, userInterfaceMock.Object);

            // Assert
            userInterfaceMock.Verify(mock => mock.DisplayMessage("Неверный формат данных."), Times.Once);
        }

        [Test]
        public void GetUserInput_Should_Add_Data_And_Call_ExternalDependency_SimulateDataTransfer_When_Input_Is_Valid()
        {
            // Arrange
            float length1 = 3.0f;
            float length2 = 4.0f;
            float length3 = 5.0f;
            string triangleType = "разносторонний";
            string errorMessage = "";

            Mock<IUserInterface> userInterfaceMock = new Mock<IUserInterface>();
            Mock<IExternalDependency> externalDependencyMock = new Mock<IExternalDependency>();
            MainWindow dbManager = new MainWindow("connectionString", externalDependencyMock.Object);

            // Act
            dbManager.GetUserInput(length1.ToString(), length2.ToString(), length3.ToString(), triangleType, errorMessage, userInterfaceMock.Object);

            // Assert
            userInterfaceMock.Verify(mock => mock.DisplayMessage("Данные успешно добавлены и переданы стороннему процессу."), Times.Once);
            externalDependencyMock.Verify(mock => mock.SimulateDataTransfer(length1, length2, length3), Times.Once);
        }
    }
}
}






