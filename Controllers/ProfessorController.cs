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
    [ApiController]
    public class ProfessorController : ControllerBase
    {

        private EscolaContext _context;
        public ProfessorController(EscolaContext context)
        {
            // construtor
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Professor>> GetAll()
        {
            return _context.Professor.ToList();
        }
        [HttpGet("{ProfessorId}")]
        public ActionResult<Professor> Get(int ProfessorId)
        {
            try
            {
                var result = _context.Professor.Find(ProfessorId);
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
        public async Task<ActionResult> Post(Professor model)
        {
            var user = _context.Professor.Select(x => x.Id == model.Id);
            try
            {
                if (user != null)
                {
                    _context.Professor.Add(model);
                    if (await _context.SaveChangesAsync() == 1)
                    {
                        //return Ok();
                        return Created($"/api/aluno/{model.Id}", model);
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
        [HttpDelete("{ProfessorId}")]
        public async Task<ActionResult> Delete(int ProfessorId)
        {
            try
            {
                //verifica se existe aluno a ser excluído
                var aluno = await _context.Professor.FindAsync(ProfessorId);
                if (aluno == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(aluno);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
        [HttpPut("{ProfessorId}")]
        public async Task<IActionResult> Put(int ProfessorId, Professor dados)
        {
            try
            {
                //verifica se existe aluno a ser alterado
                var result = await _context.Professor.FindAsync(ProfessorId);
                if (ProfessorId != result.Id)
                {
                    return BadRequest();
                }
                result.email = dados.email;
                result.nome = dados.nome;
                result.codCurso  = dados.codCurso;
                await _context.SaveChangesAsync();
                return Created($"/api/aluno/{dados.Id}", dados);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
    }
}