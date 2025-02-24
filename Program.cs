using Microsoft.Data.SqlClient;


class Program
{
    private static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    static void Main(string[] args)
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Hotel Reservation System");
            Console.WriteLine("1. View Customers");
            Console.WriteLine("2. View Room Types");
            Console.WriteLine("3. View Rooms");
            Console.WriteLine("4. View Reservations");
            Console.WriteLine("5. View Reservations by Customer ID");
            Console.WriteLine("6. View Available Rooms for a Date Range");
            Console.WriteLine("7. Calculate Total Income");
            Console.WriteLine("8. Find Most Popular Room Type");
            Console.WriteLine("9. Find Reservations by Date");
            Console.WriteLine("10. Search Customer by Name");
            Console.WriteLine("11. Check Room Availability by Number");
            Console.WriteLine("12. Exit");
            Console.WriteLine("------------------------------------------");
            Console.Write("Select an option: ");


            switch (Console.ReadLine())
            {
                case "1":
                    ViewCustomers();
                    break;
                case "2":
                    ViewRoomTypes();
                    break;
                case "3":
                    ViewRooms();
                    break;
                case "4":
                    ViewReservations();
                    break;
                case "5":
                    ViewReservationsByCustomerId();
                    break;
                case "6":
                    ViewAvailableRooms();
                    break;
                case "7":
                    CalculateTotalIncome();
                    break;
                case "8":
                    FindMostPopularRoomType();
                    break;
                case "9":
                    FindReservationsByDate();
                    break;
                case "10":
                    SearchCustomerByName();
                    break;
                case "11":
                    CheckRoomAvailability();
                    break;
                case "12":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ViewCustomers()
    {
        Console.Clear();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT CustomerId, FullName, Email FROM Customers";
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["CustomerId"]}, Name: {reader["FullName"]}, Email: {reader["Email"]}");
                }
            }
        }
        Console.ReadKey();
    }

    static void ViewRoomTypes()
    {
        Console.Clear();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT RoomTypeId, TypeName, Description FROM RoomTypes";
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["RoomTypeId"]}, Type: {reader["TypeName"]}, Description: {reader["Description"]}");
                }
            }
        }
        Console.ReadKey();
    }

    static void ViewRooms()
    {
        Console.Clear();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT RoomId, RoomNumber, Capacity, PricePerNight, RoomTypeId FROM Rooms";
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["RoomId"]}, Room Number: {reader["RoomNumber"]}, Capacity: {reader["Capacity"]}, Price: {reader["PricePerNight"]}, Room Type ID: {reader["RoomTypeId"]}");
                }
            }
        }
        Console.ReadKey();
    }

    static void ViewReservations()
    {
        Console.Clear();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT ReservationId, CustomerId, CheckInDate, CheckOutDate, TotalPrice FROM Reservations";
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["ReservationId"]}, Customer ID: {reader["CustomerId"]}, Check-in: {reader["CheckInDate"]}, Check-out: {reader["CheckOutDate"]}, Total Price: {reader["TotalPrice"]}");
                }
            }
        }
        Console.ReadKey();
    }


    static void ViewReservationsByCustomerId()
    {
        Console.Clear();
        Console.Write("Enter Customer ID: ");
        string customerId = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Reservations WHERE CustomerId = @CustomerId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerId", customerId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Reservation ID: {reader["ReservationId"]}, Check-in: {reader["CheckInDate"]}, Check-out: {reader["CheckOutDate"]}, Total Price: {reader["TotalPrice"]}");
                    }
                }
            }
        }
        Console.ReadKey();
    }

    static void ViewAvailableRooms()
    {
        Console.Clear();
        Console.Write("Enter Check-in Date (yyyy-mm-dd): ");
        string checkIn = Console.ReadLine();
        Console.Write("Enter Check-out Date (yyyy-mm-dd): ");
        string checkOut = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT * FROM Rooms WHERE RoomId NOT IN (SELECT RoomId FROM ReservationRooms JOIN Reservations ON ReservationRooms.ReservationId = Reservations.ReservationId WHERE CheckInDate < @CheckOut AND CheckOutDate > @CheckIn)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CheckIn", checkIn);
                command.Parameters.AddWithValue("@CheckOut", checkOut);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Room ID: {reader["RoomId"]}, Room Number: {reader["RoomNumber"]}, Capacity: {reader["Capacity"]}, Price: {reader["PricePerNight"]}");
                    }
                }
            }
        }
        Console.ReadKey();
    }

    static void CalculateTotalIncome()
    {
        Console.Clear();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT SUM(TotalPrice) AS TotalIncome FROM Reservations";
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"Total Income: {reader["TotalIncome"]}");
                }
            }
        }
        Console.ReadKey();
    }

    static void FindMostPopularRoomType()
    {
        Console.Clear();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT TOP 1 RT.TypeName, COUNT(RR.RoomId) AS ReservationCount FROM RoomTypes RT JOIN Rooms R ON RT.RoomTypeId = R.RoomTypeId JOIN ReservationRooms RR ON R.RoomId = RR.RoomId GROUP BY RT.TypeName ORDER BY ReservationCount DESC";
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"Most Popular Room Type: {reader["TypeName"]} with {reader["ReservationCount"]} reservations");
                }
            }
        }
        Console.ReadKey();
    }
    static void FindReservationsByDate()
    {
        Console.Clear();
        Console.Write("Enter Date (yyyy-mm-dd): ");
        string date = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Reservations WHERE CheckInDate = @Date OR CheckOutDate = @Date";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Date", date);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Reservation ID: {reader["ReservationId"]}, Customer ID: {reader["CustomerId"]}, Check-in: {reader["CheckInDate"]}, Check-out: {reader["CheckOutDate"]}, Total Price: {reader["TotalPrice"]}");
                    }
                }
            }
        }
        Console.ReadKey();
    }

    static void SearchCustomerByName()
    {
        Console.Clear();
        Console.Write("Enter Customer Name: ");
        string name = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Customers WHERE FullName LIKE @Name";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", $"%{name}%");
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Customer ID: {reader["CustomerId"]}, Name: {reader["FullName"]}, Email: {reader["Email"]}");
                    }
                }
            }
        }
        Console.ReadKey();
    }

    static void CheckRoomAvailability()
    {
        Console.Clear();
        Console.Write("Enter Room Number: ");
        int roomNumber = int.Parse(Console.ReadLine());

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Rooms WHERE RoomNumber = @RoomNumber";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"Room ID: {reader["RoomId"]}, Room Number: {reader["RoomNumber"]}, Capacity: {reader["Capacity"]}, Price: {reader["PricePerNight"]}");
                    }
                    else
                    {
                        Console.WriteLine("Room not found.");
                    }
                }
            }
        }
        Console.ReadKey();
    }

}