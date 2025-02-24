CREATE DATABASE HotelDB

CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL
);

-- Room Types Table
CREATE TABLE RoomTypes (
    RoomTypeId INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(50) UNIQUE NOT NULL,
    Description NVARCHAR(255)
);

-- Rooms Table
CREATE TABLE Rooms (
    RoomId INT PRIMARY KEY IDENTITY(1,1),
    RoomNumber INT UNIQUE NOT NULL,
    Capacity INT NOT NULL,
    PricePerNight DECIMAL(10, 2) NOT NULL,
    RoomTypeId INT,
    FOREIGN KEY (RoomTypeId) REFERENCES RoomTypes(RoomTypeId)
);

-- Reservations Table
CREATE TABLE Reservations (
    ReservationId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT,
    CheckInDate DATE NOT NULL,
    CheckOutDate DATE NOT NULL,
    TotalPrice DECIMAL(10, 2),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

-- Mapping Table for many-to-many relationship between Reservations and Rooms
CREATE TABLE ReservationRooms (
    ReservationId INT,
    RoomId INT,
    PRIMARY KEY (ReservationId, RoomId),
    FOREIGN KEY (ReservationId) REFERENCES Reservations(ReservationId),
    FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);


-- Insert into Customers
INSERT INTO Customers (FullName, Email) VALUES
('Ivan Ivanov', 'ivan.ivanov@email.com'),
('Petar Petrov', 'petar.petrov@email.com'),
('Maria Georgieva', 'maria.georgieva@email.com'),
('Georgi Dimitrov', 'georgi.dimitrov@email.com'),
('Elena Stoyanova', 'elena.stoyanova@email.com');

-- Insert into RoomTypes
INSERT INTO RoomTypes (TypeName, Description) VALUES
('Single', 'A small room for one person'),
('Double', 'A room with a double bed for two people'),
('Suite', 'A luxurious suite with extra space'),
('Family', 'A room suitable for families'),
('Deluxe', 'A premium room with a beautiful view');

-- Insert into Rooms
INSERT INTO Rooms (RoomNumber, Capacity, PricePerNight, RoomTypeId) VALUES
(101, 1, 50.00, 1),
(102, 2, 75.00, 2),
(201, 4, 120.00, 4),
(301, 2, 90.00, 3),
(401, 3, 150.00, 5);

-- Insert into Reservations
INSERT INTO Reservations (CustomerId, CheckInDate, CheckOutDate, TotalPrice) VALUES
(1, '2025-03-01', '2025-03-05', 200.00),
(2, '2025-03-10', '2025-03-15', 375.00),
(3, '2025-03-20', '2025-03-22', 150.00),
(4, '2025-04-05', '2025-04-10', 450.00),
(5, '2025-04-15', '2025-04-18', 270.00);

-- Insert into ReservationRooms (Many-to-Many relationship)
INSERT INTO ReservationRooms (ReservationId, RoomId) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

