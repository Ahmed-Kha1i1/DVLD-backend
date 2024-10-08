namespace DataLayerCore.User
{
    public abstract class UserForModificationDTO
    {
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}
