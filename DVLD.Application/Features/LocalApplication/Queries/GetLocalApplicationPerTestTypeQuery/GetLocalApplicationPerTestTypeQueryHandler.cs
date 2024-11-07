namespace DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationPerTestTypeQuery
{
    public class GetLocalApplicationPerTestTypeQueryHandler(ILocalApplicationRepository localApplicationRepository) : ResponseHandler, IRequestHandler<GetLocalApplicationPerTestTypeQuery, Response<GetLocalApplicationPerTestTypeQueryResponse>>
    {
        public async Task<Response<GetLocalApplicationPerTestTypeQueryResponse>> Handle(GetLocalApplicationPerTestTypeQuery request, CancellationToken cancellationToken)
        {
            var testAppointment = await localApplicationRepository.GetPerTestTypeAsync(request.LocalApplicationId, request.TestTypeId);
            if (testAppointment is null)
            {
                return NotFound<GetLocalApplicationPerTestTypeQueryResponse>("Local application not found");
            }

            return Success(testAppointment);
        }
    }
}
