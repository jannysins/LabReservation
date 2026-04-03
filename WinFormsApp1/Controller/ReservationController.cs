using LabReservation.model;
using LabReservation.model;
using LabReservation.repository;
using LabReservation.repository;
using System;
using System.Data;

namespace LabReservation.controller
{
    public class ReservationController
    {
        private ReservationRepository repository = new ReservationRepository();

        public bool SaveReservation(Reservation res)
        {
            // 1. Simple validation: Check if fields are empty
            if (string.IsNullOrWhiteSpace(res.Name) || string.IsNullOrWhiteSpace(res.LabRoom))
            {
                System.Windows.Forms.MessageBox.Show("Please fill in all required fields.", "Validation Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            // 2. CHECK THE 3-HOUR LIMIT AND VALID TIME (Your new feature!)
            // Convert natin yung text mo na "6:21 PM" into actual time objects para ma-compute
            DateTime parsedStartTime, parsedEndTime;

            if (DateTime.TryParse(res.StartTime, out parsedStartTime) && DateTime.TryParse(res.EndTime, out parsedEndTime))
            {
                // TimeSpan measures the distance between two times
                TimeSpan duration = parsedEndTime - parsedStartTime;

                // Safety Check A: Is the end time earlier than the start time?
                if (duration.TotalMinutes <= 0)
                {
                    System.Windows.Forms.MessageBox.Show("Invalid time. The End Time must be later than the Start Time.", "Time Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }

                // Safety Check B: Did they exceed 3 hours?
                if (duration.TotalHours > 3)
                {
                    System.Windows.Forms.MessageBox.Show("Reservations cannot exceed a maximum of 3 hours. Please adjust your schedule.", "Time Limit Exceeded", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }
            }

            // 3. CHECK IF USER ALREADY BOOKED
            if (repository.HasUserAlreadyBooked(res.Name))
            {
                System.Windows.Forms.MessageBox.Show("Sorry, this name has already booked a reservation. Only one reservation per person is allowed.", "Limit Reached", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            // 4. CHECK FOR DOUBLE BOOKING
            if (repository.IsTimeSlotTaken(res))
            {
                System.Windows.Forms.MessageBox.Show("Lab Reserved choose another time or Room", "Schedule Conflict", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }

            // 5. If everything is clear, pass data to the repository to save
            return repository.AddReservation(res);
        }

        public void AutoCleanUp()
        {
            // Tells the repository to do the housekeeping
            repository.DeletePastReservations();
        }

        public Reservation GetReservationForUpdate(string name)
        {
            return repository.GetReservationByName(name);
        }

        public bool UpdateReservation(string originalName, Reservation updatedRes)
        {
            // Simple validation
            if (string.IsNullOrWhiteSpace(updatedRes.Name) || string.IsNullOrWhiteSpace(updatedRes.LabRoom))
            {
                System.Windows.Forms.MessageBox.Show("Please fill in all required fields.", "Validation Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            // Pass to repository to update
            return repository.UpdateReservation(originalName, updatedRes);
        }

        public bool DeleteReservation(string name)
        {
            // A quick safety check
            if (string.IsNullOrWhiteSpace(name)) return false;

            // Pass the command to the repository
            return repository.DeleteReservationByName(name);
        }

        public DataTable GetAllReservations()
        {
            return repository.GetAllReservations();
        }
    }
}