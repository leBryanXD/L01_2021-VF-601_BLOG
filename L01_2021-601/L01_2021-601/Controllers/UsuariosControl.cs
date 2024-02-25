using L01_2021_601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2021_601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosControl : ControllerBase
    {
        private readonly blogContext _userContext;

        public UsuariosControl(blogContext contextUser)
        {
            _userContext = contextUser;
        }
       
        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            List<Usuarios> listadoUsuarios = (from e in _userContext.usuarios
                                              select e).ToList();

            if (listadoUsuarios.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoUsuarios);
        }

        [HttpPost]
        [Route("Post")]
        public IActionResult GuardarUsuario([FromBody] Usuarios usuarios)
        {
            try
            {
                _userContext.usuarios.Add(usuarios);
                _userContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       

        [HttpPut]
        [Route("Put")]
        public IActionResult ActualizarUsuario(int id, [FromBody] Usuarios usuarioAmodificar)
        {
            try
            {
                Usuarios equipoActual = _userContext.usuarios.FirstOrDefault(e => e.usuarioId == id);

                if (equipoActual == null)
                {
                    return NotFound(); // Devolver HTTP 404 si no se encuentra el usuario
                }

                equipoActual.rolId = usuarioAmodificar.rolId;
                equipoActual.nombreUsuario = usuarioAmodificar.nombreUsuario;
                equipoActual.clave = usuarioAmodificar.clave;
                equipoActual.nombre = usuarioAmodificar.nombre;
                equipoActual.apellido = usuarioAmodificar.apellido;

 
                _userContext.Entry(equipoActual).State = EntityState.Modified;

                // Guardar los cambios en la base de datos
                _userContext.SaveChanges();

                // Devolver HTTP 200 OK junto con el usuario actualizado
                return Ok(equipoActual);
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
        public IActionResult EliminarUsuario(int id)
        {
            Usuarios? usuarios = (from e in _userContext.usuarios
                                  where e.usuarioId == id
                                  select e).FirstOrDefault();

            ///Verificamos que exista el registro segun su ID
            if (usuarios == null)
            { return NotFound(); }

            _userContext.usuarios.Attach(usuarios);
            _userContext.usuarios.Remove(usuarios);
            _userContext.SaveChanges();

            return Ok(usuarios);

        }
        [HttpGet]
        [Route("BUSCANDO POR ROL ID")]
        public IActionResult Get(int id)
        {
            Usuarios? equipo = (from e in _userContext.usuarios
                                where e.rolId == id
                                select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        [HttpGet]
        [Route("BUSCAR POR NOMBRE Y APELLIDO")]
        public IActionResult FindByName(string filtro)
        {
            var usuarios = _userContext.usuarios
                            .Where(e => e.nombre.Contains(filtro) || e.apellido.Contains(filtro))
                            .ToList();

            if (usuarios.Count == 0)
            {
                return NotFound();
            }
            return Ok(usuarios);
        }
    }
}
