namespace DVLD.Application.Features.LocalApplication.Commands.CancelLocalApplicationCommand
{
    public class CancelLocalApplicationCommand : IRequest<Response<bool>>
    {
        public int LocalApplicationId { get; set; }

        public CancelLocalApplicationCommand(int localApplicationId)
        {
            LocalApplicationId = localApplicationId;
        }
    }
}
