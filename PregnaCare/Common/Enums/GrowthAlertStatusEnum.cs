namespace PregnaCare.Common.Enums
{
    public enum GrowthAlertStatusEnum
    {
        Pending,    // Alert is created but not reviewed
        Acknowledged, // User has seen the alert
        Resolved,   // Issue has been resolved
        Dismissed   // Alert is ignored or dismissed
    }
}
