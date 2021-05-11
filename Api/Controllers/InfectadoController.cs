using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class InfectadoController : ControllerBase
	{
		Data.MongoDB _mongoDB;
		IMongoCollection<Infectado> _infectados;

		public InfectadoController(Data.MongoDB mongoDB)
		{
			_mongoDB = mongoDB;
			_infectados = _mongoDB.Db.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
		}

		[HttpGet]
		public ActionResult Get()
		{
			var infectados = _infectados.Find(Builders<Infectado>.Filter.Empty).ToList();
			return Ok(infectados);
		}

		[HttpPost]
		public ActionResult Post([FromBody] InfectadoDto dto)
		{
			var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
			_infectados.InsertOne(infectado);
			return StatusCode(201, "Infectado salvo com sucesso");
		}

		[HttpPut]
		public ActionResult Put([FromBody] InfectadoDto dto)
		{
			var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
			_infectados.UpdateOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("sexo", dto.Sexo));
			return Ok("Atualizado com sucesso");
		}

		[HttpDelete("{dataNasc}")]
		public ActionResult Delete(string dataNasc)
		{
			_infectados.DeleteOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == DateTime.Parse(dataNasc)));
			return Ok("Deletado com sucesso");
		}
	}
}
