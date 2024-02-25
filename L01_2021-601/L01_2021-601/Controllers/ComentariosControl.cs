
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using L01_2021_601.Models;

namespace L01_2021_601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosControl : ControllerBase
    {
        private readonly blogContext _blogContexto;

        public ComentariosControl(blogContext ComentariosContexto)
        {
            _blogContexto = ComentariosContexto;
        }
        
        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            List<Comentarios> AllComents = (from e in _blogContexto.Comentarios
                                                 select e).ToList();

            if (AllComents.Count == 0)
            {
                return NotFound();
            }
            return Ok(AllComents);
        }

        [HttpPost]
        [Route("Post")]
        public IActionResult GuardarComentario([FromBody] Comentarios comentarios)
        {
            try
            {
                _blogContexto.Comentarios.Add(comentarios);
                _blogContexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      

        [HttpPut]
        [Route("Put")]
        public IActionResult ActualizarComentario(int id, [FromBody] Comentarios usuarioAmodificar)
        {
            try
            {
                
                Comentarios equipoActual = _blogContexto.Comentarios.FirstOrDefault(e => e.cometarioId == id);

                
                if (equipoActual == null)
                {
                    return NotFound(); 
                }

               
                equipoActual.publicacionId = usuarioAmodificar.publicacionId;
                equipoActual.comentario = usuarioAmodificar.comentario;
                equipoActual.usuarioId = usuarioAmodificar.usuarioId;

               
                _blogContexto.Entry(equipoActual).State = EntityState.Modified;

                _blogContexto.SaveChanges();

                return Ok(equipoActual);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, $"Ocurrió un error al actualizar el comentario: {ex.Message}");
            }
        }


        /// EndPoint que ELIMANR los registros de una tablas
        /// 

        [HttpDelete]
        [Route("Delete")]
        public IActionResult EliminarEqipo(int id)
        {
            Comentarios? comentarios = (from e in _blogContexto.Comentarios
                                     where e.cometarioId == id
                                     select e).FirstOrDefault();

            
            if (comentarios == null)
            { return NotFound(); }


            ///Ejecutamos la accion de elimnar el registro

            _blogContexto.Comentarios.Attach(comentarios);
            _blogContexto.Comentarios.Remove(comentarios);
            _blogContexto.SaveChanges();

            return Ok(comentarios);

        }


        [HttpGet("Usuario/{id}")]
        public async Task<ActionResult<IEnumerable<Comentarios>>> ObtenerComentariosPorUsuario(int id)
        {
            var comentarios = await _blogContexto.Comentarios
                .Where(c => c.usuarioId == id)
                .ToListAsync();

            if (comentarios == null || comentarios.Count == 0)
            {
                return NotFound();
            }

            return comentarios;
        }
    }
}
