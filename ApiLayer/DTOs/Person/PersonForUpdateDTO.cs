namespace ApiLayer.DTOs.Person
{
    public class PersonForUpdateDTO : PersonForModificationDTO
    {
        public bool RemoveImage { get; set; } = false;
    }
}
