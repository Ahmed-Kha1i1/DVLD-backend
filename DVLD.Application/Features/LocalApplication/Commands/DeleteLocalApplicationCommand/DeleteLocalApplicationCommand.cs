namespace DVLD.Application.Features.LocalApplication.Commands.DeleteLocalApplicationCommand
{
    public class DeleteLocalApplicationCommand : IRequest<Response<bool>>
    {
        public int LocalApplicationId { get; set; }

        public DeleteLocalApplicationCommand(int localApplicationId)
        {
            LocalApplicationId = localApplicationId;
        }
    }
}
