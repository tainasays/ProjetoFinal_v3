using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFinal_v2.Data;
using PFinal_v2.Models;


public class LoginService
{
    private readonly PFinal_v2Context _dbContext;

    public LoginService(PFinal_v2Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Usuario> FindByEmailAndPasswordAsync(string email, string senha)
    {
        return await _dbContext.Usuario
            .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
    }
}
