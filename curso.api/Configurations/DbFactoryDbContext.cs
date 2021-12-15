using curso.api.Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace curso.api.Configurations
{
    public class DbFactoryDbContext : IDesignTimeDbContextFactory<CursoDBContext>
    {
        public CursoDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CursoDBContext>();
            optionsBuilder.UseSqlServer("Server=localhot;Database=CURSO;user=sa;password=App@223020");

            CursoDBContext contexto = new CursoDBContext(optionsBuilder.Options);

            return contexto;
        }
    }
}
