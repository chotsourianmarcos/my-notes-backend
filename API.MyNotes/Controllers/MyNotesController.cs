using Microsoft.AspNetCore.Mvc;

namespace API.MyNotes.Controllers
{
    public class MyNotesController : ControllerBase
    {
        protected const string AdminRolName = "Administrador";
        protected const string UserRolName = "Usuario";
    }
}
