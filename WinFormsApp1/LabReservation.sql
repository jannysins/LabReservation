CREATE DATABASE appsdev

USE appsdev

CREATE TABLE reservations (
    id INT AUTO_INCREMENT PRIMARY KEY,
    lab_room VARCHAR(50),
    reservation_date DATE,
    start_time VARCHAR(20),
    end_time VARCHAR(20),
    reserver_name VARCHAR(100)
);

SELECT * FROM reservations;
