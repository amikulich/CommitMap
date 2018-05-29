namespace CommitMap.Services
{
    public interface ICommitScanner
    {
        string[] GetModifiedDocuments(string commitHash);
    }

    public class CommitScanner : ICommitScanner
    {
        public string[] GetModifiedDocuments(string commitHash)
        {
            return new string[]
            {
                //"LaunchAllConfirmationReason.cs",
                //"NameAndValue.cs",
                "ICommentService.cs",
                "SearchRequestData.cs",
                //"DashboardFilterAppService.cs",
                //"VDSPAttributeModel.cs",
                //"Campaign.cs",
            };
        }
    }
}
