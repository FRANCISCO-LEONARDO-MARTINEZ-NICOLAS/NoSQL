using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.API.DTOs;
using System.Linq;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _consultaService;
        private readonly IPacienteService _pacienteService;
        private readonly IOptometristaService _optometristaService;

        public ConsultasController(
            IConsultaService consultaService,
            IPacienteService pacienteService,
            IOptometristaService optometristaService)
        {
            _consultaService = consultaService;
            _pacienteService = pacienteService;
            _optometristaService = optometristaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var consultas = await _consultaService.GetAllAsync();
            var consultasDto = await MapConsultasToDto(consultas.ToList());
            return Ok(consultasDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var consulta = await _consultaService.GetByIdAsync(id);
            if (consulta == null)
                return NotFound();

            var consultaDto = await MapConsultaToDto(consulta);
            return Ok(consultaDto);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(string patientId)
        {
            var consultas = await _consultaService.GetAllAsync();
            var patientConsultas = consultas.Where(c => c.PacienteId == patientId).ToList();
            var consultasDto = await MapConsultasToDto(patientConsultas);
            return Ok(consultasDto);
        }

        [HttpGet("optometrist/{optometristId}")]
        public async Task<IActionResult> GetByOptometrist(string optometristId)
        {
            var consultas = await _consultaService.GetAllAsync();
            var optometristConsultas = consultas.Where(c => c.OptometristaId == optometristId).ToList();
            var consultasDto = await MapConsultasToDto(optometristConsultas);
            return Ok(consultasDto);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetByDate(string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
                return BadRequest("Fecha inválida");

            var consultas = await _consultaService.GetAllAsync();
            var dateConsultas = consultas.Where(c => c.Fecha.Date == parsedDate.Date).ToList();
            var consultasDto = await MapConsultasToDto(dateConsultas);
            return Ok(consultasDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateConsultaDto createDto)
        {
            var consulta = new Consulta
            {
                Id = Guid.NewGuid().ToString(),
                PacienteId = createDto.PacienteId,
                OptometristaId = createDto.OptometristaId,
                Pacientecorreo = createDto.Pacientecorreo,
                Optometristacorreo = createDto.Optometristacorreo,
                Fecha = createDto.Fecha,
                Motivo = createDto.Motivo,
                Sintomas = createDto.Sintomas,
                Diagnostico = createDto.Diagnostico,
                Tratamiento = createDto.Tratamiento,
                Recomendaciones = createDto.Recomendaciones,
                Observaciones = createDto.Observaciones,
                AgudezaVisual = createDto.AgudezaVisual != null ? new AgudezaVisual
                {
                    OjoDerecho = createDto.AgudezaVisual.OjoDerecho,
                    OjoIzquierdo = createDto.AgudezaVisual.OjoIzquierdo
                } : null,
                Refraccion = createDto.Refraccion != null ? new Refraccion
                {
                    OjoDerecho = new OjoRefraccion
                    {
                        Esfera = createDto.Refraccion.OjoDerecho.Esfera,
                        Cilindro = createDto.Refraccion.OjoDerecho.Cilindro,
                        Eje = createDto.Refraccion.OjoDerecho.Eje
                    },
                    OjoIzquierdo = new OjoRefraccion
                    {
                        Esfera = createDto.Refraccion.OjoIzquierdo.Esfera,
                        Cilindro = createDto.Refraccion.OjoIzquierdo.Cilindro,
                        Eje = createDto.Refraccion.OjoIzquierdo.Eje
                    }
                } : null,
                FechaSeguimiento = createDto.FechaSeguimiento
            };

            var (success, message) = await _consultaService.CreateAsync(consulta);
            if (!success)
                return BadRequest(message);

            var consultaDto = await MapConsultaToDto(consulta);
            return CreatedAtAction(nameof(GetById), new { id = consulta.Id }, consultaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateConsultaDto updateDto)
        {
            var existingConsulta = await _consultaService.GetByIdAsync(id);
            if (existingConsulta == null)
                return NotFound();

            // Actualizar solo los campos proporcionados
            if (updateDto.Motivo != null)
                existingConsulta.Motivo = updateDto.Motivo;
            if (updateDto.Sintomas != null)
                existingConsulta.Sintomas = updateDto.Sintomas;
            if (updateDto.Diagnostico != null)
                existingConsulta.Diagnostico = updateDto.Diagnostico;
            if (updateDto.Tratamiento != null)
                existingConsulta.Tratamiento = updateDto.Tratamiento;
            if (updateDto.Recomendaciones != null)
                existingConsulta.Recomendaciones = updateDto.Recomendaciones;
            if (updateDto.Observaciones != null)
                existingConsulta.Observaciones = updateDto.Observaciones;
            if (updateDto.AgudezaVisual != null)
                existingConsulta.AgudezaVisual = new AgudezaVisual
                {
                    OjoDerecho = updateDto.AgudezaVisual.OjoDerecho,
                    OjoIzquierdo = updateDto.AgudezaVisual.OjoIzquierdo
                };
            if (updateDto.Refraccion != null)
                existingConsulta.Refraccion = new Refraccion
                {
                    OjoDerecho = new OjoRefraccion
                    {
                        Esfera = updateDto.Refraccion.OjoDerecho.Esfera,
                        Cilindro = updateDto.Refraccion.OjoDerecho.Cilindro,
                        Eje = updateDto.Refraccion.OjoDerecho.Eje
                    },
                    OjoIzquierdo = new OjoRefraccion
                    {
                        Esfera = updateDto.Refraccion.OjoIzquierdo.Esfera,
                        Cilindro = updateDto.Refraccion.OjoIzquierdo.Cilindro,
                        Eje = updateDto.Refraccion.OjoIzquierdo.Eje
                    }
                };
            if (updateDto.FechaSeguimiento != null)
                existingConsulta.FechaSeguimiento = updateDto.FechaSeguimiento;

            await _consultaService.UpdateAsync(id, existingConsulta);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _consultaService.DeleteAsync(id);
            return NoContent();
        }

        private async Task<ConsultaDto> MapConsultaToDto(Consulta consulta)
        {
            var paciente = await _pacienteService.GetByIdAsync(consulta.PacienteId);
            var optometrista = await _optometristaService.GetByIdAsync(consulta.OptometristaId);

            return new ConsultaDto
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                OptometristaId = consulta.OptometristaId,
                Pacientecorreo = consulta.Pacientecorreo,
                Optometristacorreo = consulta.Optometristacorreo,
                Fecha = consulta.Fecha,
                Motivo = consulta.Motivo,
                Sintomas = consulta.Sintomas,
                Diagnostico = consulta.Diagnostico,
                Tratamiento = consulta.Tratamiento,
                Recomendaciones = consulta.Recomendaciones,
                Observaciones = consulta.Observaciones,
                AgudezaVisual = consulta.AgudezaVisual != null ? new AgudezaVisualDto
                {
                    OjoDerecho = consulta.AgudezaVisual.OjoDerecho,
                    OjoIzquierdo = consulta.AgudezaVisual.OjoIzquierdo
                } : null,
                Refraccion = consulta.Refraccion != null ? new RefraccionDto
                {
                    OjoDerecho = new OjoRefraccionDto
                    {
                        Esfera = consulta.Refraccion.OjoDerecho.Esfera,
                        Cilindro = consulta.Refraccion.OjoDerecho.Cilindro,
                        Eje = consulta.Refraccion.OjoDerecho.Eje
                    },
                    OjoIzquierdo = new OjoRefraccionDto
                    {
                        Esfera = consulta.Refraccion.OjoIzquierdo.Esfera,
                        Cilindro = consulta.Refraccion.OjoIzquierdo.Cilindro,
                        Eje = consulta.Refraccion.OjoIzquierdo.Eje
                    }
                } : null,
                FechaSeguimiento = consulta.FechaSeguimiento,
                NombrePaciente = paciente?.Nombre,
                ApellidoPaciente = paciente?.Apellido,
                NombreOptometrista = optometrista?.Nombre,
                ApellidoOptometrista = optometrista?.Apellido
            };
        }

        private async Task<List<ConsultaDto>> MapConsultasToDto(List<Consulta> consultas)
        {
            var dtos = new List<ConsultaDto>();
            foreach (var consulta in consultas)
            {
                dtos.Add(await MapConsultaToDto(consulta));
            }
            return dtos;
        }
    }
}