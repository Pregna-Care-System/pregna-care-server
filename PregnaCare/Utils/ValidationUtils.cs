using System.Text.RegularExpressions;

namespace PregnaCare.Utils
{
    public class ValidationUtils
    {
        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }

        public static bool IsValidPassword(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$");
            return passwordRegex.IsMatch(password);
        }

        public static bool IsValidPregnancyStartDate(DateOnly pregnancyStartDate, out string errorMessage)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var tenYearsAgo = today.AddYears(-10);

            if (pregnancyStartDate > today)
            {
                errorMessage = "Pregnancy start date cannot be in the future.";
                return false;
            }

            if (pregnancyStartDate < tenYearsAgo)
            {
                errorMessage = "Pregnancy start date is too far in the past.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public static bool IsValidExpectedDueDate(DateOnly? expectedDueDate, out string errorMessage)
        {
            if (!expectedDueDate.HasValue)
            {
                errorMessage = string.Empty;
                return true;
            }

            if (expectedDueDate.Value < DateOnly.FromDateTime(DateTime.Today))
            {
                errorMessage = "Expected due date cannot be in the past.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public static bool IsValidPregnancyDates(DateOnly pregnancyStartDate, DateOnly? expectedDueDate, out string errorMessage)
        {
            if (expectedDueDate.HasValue)
            {
                if (expectedDueDate.Value < pregnancyStartDate)
                {
                    errorMessage = "Expected due date cannot be earlier than the pregnancy start date.";
                    return false;
                }

                if (expectedDueDate.Value > pregnancyStartDate.AddMonths(12)) 
                {
                    errorMessage = "Expected due date is too far in the future.";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

    }
}
