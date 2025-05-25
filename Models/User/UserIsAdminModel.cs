using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class UserIsAdminModel
{

    public int Id { get; set; }
    
    [Display(Name = "Rol")]
    public IList<string>? SelectedRole { get; set; }


}