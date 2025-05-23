﻿namespace PregnaCare.Core.DTOs
{
    public class AccountDTO
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public decimal? PlanPrice { get; set; }
        public DateTime? PlanCreated { get; set; }
        public int remainingDate { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string Address { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsFeedback { get; set; }

        public bool? IsActive { get; set; }


    }
}
