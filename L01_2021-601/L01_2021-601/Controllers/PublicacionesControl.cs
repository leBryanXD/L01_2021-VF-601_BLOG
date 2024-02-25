using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2021_601.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2021_601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionesControl : ControllerBase
    {
        private readonly blogContext _publicacionContext;

        public PublicacionesControl(blogContext contextPublicacion)
        {
            _publicacionContext = contextPublicacion;
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            List<Publicaciones> listadoPublicaciones = (from e in _publicacionContext.Publicaciones
                                              select e).ToList();

            if (listadoPublicaciones.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoPublicaciones);
        }

        [HttpPost]
        [Route("Post")]
        public IActionResult GuardarPublicacion([FromBody] Publicaciones publicaciones)
        {
            try
            {
                _publicacionContext.Publicaciones.Add(publicaciones);
                _publicacionContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut]
        [Route("Put")]
        public IActionResult ActualizarPublicacion(int id, [FromBody] Publicaciones publicacionModificar)
        {
            try
            {
                Publicaciones publicaionActual = _publicacionContext.Publicaciones.FirstOrDefault(e => e.publicacionId == id);

                if (publicaionActual == null)
                {
                    return NotFound(); // Devolver HTTP 404 si no se encuentra el usuario
                }

                publicaionActual.publicacionId = publicacionModificar.publicacionId;
                publicaionActual.titulo = publicacionModificar.titulo;
                publicaionActual.descripcion = publicacionModificar.descripcion;
                publicaionActual.usuarioId = publicacionModificar.usuarioId;


                _publicacionContext.Entry(publicaionActual).State = EntityState.Modified;

                // Guardar los cambios en la base de datos
                _publicacionContext.SaveChanges();

                // Devolver HTTP 200 OK junto con el usuario actualizado
                return Ok(publicaionActual);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar el usuario: {ex.Message}");
            }
        }


        /// EndPoint que ELIMANR los registros de una tablas
        /// 

        [HttpDelete]
        [Route("Delete")]
        public IActionResult EliminarPublicacion(int id)
        {
            Publicaciones? publi = (from e in _publicacionContext.Publicaciones
                                  where e.publicacionId == id
                                  select e).FirstOrDefault();

            ///Verificamos que exista el registro segun su ID
            if (publi == null)
            { return NotFound(); }

            _publicacionContext.Publicaciones.Attach(publi);
            _publicacionContext.Publicaciones.Remove(publi);
            _publicacionContext.SaveChanges();

            return Ok(publi);

        }
        [HttpGet]
        [Route("BUSCAR POR USUARIO ESPECIFICO")]
        public IActionResult FindByUser(int usuarioId)
        {
            var publicaciones = _publicacionContext.Publicaciones
                                .Where(p => p.usuarioId == usuarioId)
                                .ToList();

            if (publicaciones.Count == 0)
            {
                return NotFound();
            }
            return Ok(publicaciones);
        }
    }
}
