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
    public class AlunoController : ControllerBase
    {

        private EscolaContext _context;
        public AlunoController(EscolaContext context)
        {
            // construtor
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Aluno>> GetAll()
        {
            return _context.Aluno.ToList();
        }
        [HttpGet("{AlunoId}")]
        public ActionResult<Aluno> Get(int AlunoId)
        {
            try
            {
                var result = _context.Aluno.Find(AlunoId);
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
        public async Task<ActionResult> Post(Aluno model)
        {
            var userRa = _context.Aluno.Select(x => x.ra == model.ra);
            try
            {
                if (userRa != null)
                {
                    _context.Aluno.Add(model);
                    if (await _context.SaveChangesAsync() == 1)
                    {
                        //return Ok();
                        return Created($"/api/aluno/{model.ra}", model);
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
        [HttpDelete("{AlunoId}")]
        public async Task<ActionResult> Delete(int AlunoId)
        {
            try
            {
                //verifica se existe aluno a ser excluído
                var aluno = await _context.Aluno.FindAsync(AlunoId);
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
        [HttpPut("{AlunoId}")]
        public async Task<IActionResult> Put(int AlunoId, Aluno dadosCurso)
        {
            try
            {
                //verifica se existe aluno a ser alterado
                var result = await _context.Aluno.FindAsync(AlunoId);
                if (AlunoId != result.Id)
                {
                    return BadRequest();
                }
                result.ra = dadosCurso.ra;
                result.nome = dadosCurso.nome;
                result.codCurso  = dadosCurso.codCurso;
                await _context.SaveChangesAsync();
                return Created($"/api/aluno/{dadosCurso.Id}", dadosCurso);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
    }
}