namespace DVLD.Application.Features.LocalApplication.Commands.IssueLocalApplicationCommand
{
    public class IssueLocalApplicationCommand : IRequest<Response<int?>>
    {
        public int LocalApplicationId { get; set; }
        public int userId { get; set; }
        public string Notes { get; set; }
    }
}
