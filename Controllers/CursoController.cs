using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetoEscola_API.Data;
using ProjetoEscola_API.Models;

namespace ProjetoEscola_API.Controllers
{
    [Route("api/[controller]")]
    public class CursoController : ControllerBase   
    {
        private EscolaContext _context;

        public CursoController(EscolaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public  ActionResult<List<Curso>> GetAllCursos(){
            return _context.Curso.ToList();
        }

        [HttpGet("{Id}")]
        public ActionResult<Curso> Get(int Id)
        {
            try
            {
                var result = _context.Curso.Find(Id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Curso model)
        {
            var userRa = _context.Curso.Select(x => x.Id == model.Id);
            try
            {
                if (userRa != null)
                {
                    _context.Curso.Add(model);
                    if (await _context.SaveChangesAsync() == 1)
                    {
                        //return Ok();
                        return Created($"/api/curso/{model.Id}", model);
                    }
                }           
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                //verifica se existe aluno a ser excluído
                var curso = await _context.Curso.FindAsync(Id);
                if (curso == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(curso);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, Curso dadosAlunoAlt)
        {
            try
            {
                //verifica se existe aluno a ser alterado
                var result = await _context.Curso.FindAsync(Id);
                if (Id != result.Id)
                {
                    return BadRequest();
                }
                result.CodCurso = dadosAlunoAlt.CodCurso;
                result.NomeCurso = dadosAlunoAlt.NomeCurso;
                await _context.SaveChangesAsync();
                return Created($"/api/curso/{dadosAlunoAlt.Id}", dadosAlunoAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
    }
}