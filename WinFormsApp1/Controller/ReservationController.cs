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
            // Check if fields are empty
            if (string.IsNullOrWhiteSpace(res.Name) || string.IsNullOrWhiteSpace(res.LabRoom))
            {
                System.Windows.Forms.MessageBox.Show("Please fill in all required fields.", "Validation Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            // CHECK IF THE RESERVATION IS FOR TODAY ONLY
            if (res.Date.Date != DateTime.Today)
            {
                System.Windows.Forms.MessageBox.Show("Reservations are strictly for same-day booking only. You cannot reserve for past or future dates.", "Invalid Date", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            // CHECK THE 3-HOUR LIMIT AND VALID TIME
            DateTime parsedStartTime, parsedEndTime;

            if (DateTime.TryParse(res.StartTime, out parsedStartTime) && DateTime.TryParse(res.EndTime, out parsedEndTime))
            {
                //Prevent booking a time that has already passed today
                if (parsedStartTime <= DateTime.Now)
                {
                    System.Windows.Forms.MessageBox.Show("You cannot reserve a time slot that has already passed today. Please choose a future time.", "Invalid Time", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }
                TimeSpan duration = parsedEndTime - parsedStartTime;

                if (duration.TotalMinutes <= 0)
                {
                    System.Windows.Forms.MessageBox.Show("Invalid time. The End Time must be later than the Start Time.", "Time Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }

                if (duration.TotalHours > 3)
                {
                    System.Windows.Forms.MessageBox.Show("Reservations cannot exceed a maximum of 3 hours. Please adjust your schedule.", "Time Limit Exceeded", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }
            }

            // CHECK IF USER ALREADY BOOKED
            if (repository.HasUserAlreadyBooked(res.Name))
            {
                System.Windows.Forms.MessageBox.Show("Sorry, this name has already booked a reservation. Only one reservation per person is allowed.", "Limit Reached", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            // CHECK FOR DOUBLE BOOKING
            if (repository.IsTimeSlotTaken(res))
            {
                System.Windows.Forms.MessageBox.Show("Lab Reserved choose another time or Room", "Schedule Conflict", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }

            return repository.AddReservation(res);
        }

        public void AutoCleanUp()
        {
            repository.DeletePastReservations();
        }

        public Reservation GetReservationForUpdate(string name)
        {
            return repository.GetReservationByName(name);
        }

        public bool UpdateReservation(string originalName, Reservation updatedRes)
        {
            if (string.IsNullOrWhiteSpace(updatedRes.Name) || string.IsNullOrWhiteSpace(updatedRes.LabRoom))
            {
                System.Windows.Forms.MessageBox.Show("Please fill in all required fields.", "Validation Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            return repository.UpdateReservation(originalName, updatedRes);
        }

        public bool DeleteReservation(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            return repository.DeleteReservationByName(name);
        }

        public DataTable GetAllReservations()
        {
            return repository.GetAllReservations();
        }
    }
}