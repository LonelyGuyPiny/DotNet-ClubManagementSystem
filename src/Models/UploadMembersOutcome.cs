namespace ClubManagementSystem.Models
{
    public enum UploadMembersOutcome
    {
        Success,
        FileNotFound,
        EmptyFile,
        MissingRequiredColumns,
        InvalidFile,
        InsertFailed
    }
}
