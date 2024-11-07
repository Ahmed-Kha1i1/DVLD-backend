using AutoMapper;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class TestSqlProfile : Profile
    {
        public TestSqlProfile()
        {
            CreateMap<IDataRecord, Test>();
        }
    }
}
