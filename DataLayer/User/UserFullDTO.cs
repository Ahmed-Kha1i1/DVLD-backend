using DataLayerCore.Person;

namespace DataLayerCore.User
{
    public class UserFullDTO : UserPrefDTO
    {
        public PersonFullDTO Person { get; set; }
    }

}
