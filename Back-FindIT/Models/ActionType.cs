using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_FindIT.Models
{
    public enum ActionType
    {
        Cadastrar = 1,
        Atualizar = 2,
        Remover = 3,
        Resgatar = 4
    }
}
