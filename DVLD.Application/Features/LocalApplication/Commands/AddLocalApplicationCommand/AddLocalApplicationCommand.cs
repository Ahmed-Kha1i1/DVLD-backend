namespace DVLD.Application.Features.LocalApplication.Commands.AddLocalApplicationCommand
{
    public class AddLocalApplicationCommand : IRequest<Response<int?>>
    {
        public int LicenseClassId { get; set; }
        public int PersonId { get; set; }
        public int UserId { get; set; }
    }
}
