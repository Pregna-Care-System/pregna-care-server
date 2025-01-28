namespace PregnaCare.Common.Constants
{
    /// <summary>
    /// Messages
    /// </summary>
    public partial class Messages
    {
        /// <summary>
        /// messageId
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static string GetMessageById(string messageId)
        {
            return messageId switch
            {
                // Success
                "I00001" => "The operation was successful.",
                "I00002" => "The operation has been completed successfully.",
                "I00003" => "Your request has been processed successfully.",

                // Error
                "E00001" => "An error occurred while processing your request. Please try again later.",
                "E00002" => "Invalid input. Please check your information and try again.",
                "E00003" => "Unauthorized access. You do not have permission to perform this action.",
                "E00004" => "The requested resource could not be found. Please check your information.",
                "E00005" => "Missing required input. Please provide all necessary information.",
                "E00006" => "Invalid characters detected in input. Please use valid characters only.",
                "E00007" => "Input exceeds the maximum allowed length. Please reduce the size of your input.",
                "E00008" => "Input does not meet the minimum required length. Please provide more details.",
                "E00009" => "The input format is invalid. Please follow the expected format.",
                "E00010" => "Input error occurred. Please check the detailed errors for more information.",
                "E00011" => "The email address you entered is already registered. Please use a different email address.",
                "E00012" => "No user with this email exists.",
                "E99999" => "A system error occurred. Please contact technical support.",

                // Warning
                "W00001" => "There might be an issue with your request. Please review and try again.",
                "W00002" => "The system storage is running low. Some functionalities may be affected.",
                "W99999" => "This feature will no longer be supported in the next version. Please refer to the documentation for more details.",

                // Default message
                _ => "Unknown message ID."
            };
        }

    }
}
