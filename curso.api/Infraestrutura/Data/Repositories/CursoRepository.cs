using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace curso.api.Infraestrutura.Data.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly CursoDBContext _contexto;

        public CursoRepository(CursoDBContext contexto)
        {
            _contexto = contexto;
        }

        public void Adicionar(Curso curso)
        {
            _contexto.Curso.Add(curso);
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        public IList<Curso> ObterPorUsuario(int codigoUsuario)
        {
            return _contexto.Curso.Include(i => i.Usuario).Where(w => w.CodigoUsuario == codigoUsuario).ToList();
        }
    }
}
