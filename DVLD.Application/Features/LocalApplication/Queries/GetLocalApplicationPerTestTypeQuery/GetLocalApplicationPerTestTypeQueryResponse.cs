namespace DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationPerTestTypeQuery
{
    public class GetLocalApplicationPerTestTypeQueryResponse
    {
        public int LocalApplicationId { get; set; }
        public string ClassName { get; set; }
        public string FullName { get; set; }
        public int Trails { get; set; }
    }
}
