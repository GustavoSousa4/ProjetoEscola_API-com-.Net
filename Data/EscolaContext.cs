using Microsoft.EntityFrameworkCore;
using ProjetoEscola_API.Models;

namespace ProjetoEscola_API.Data
{
    public class EscolaContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public EscolaContext(IConfiguration configuration){
            
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
           // connect to sql server with connection string from app settings
           options.UseSqlServer(_configuration.GetConnectionString("StringConexaoSQLServer"));
        }

        public DbSet<Aluno> Aluno { get; set; }
        public DbSet<User> Usuario { get; set; }
    }
}