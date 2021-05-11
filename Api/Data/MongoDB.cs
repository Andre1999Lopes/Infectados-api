using Api.Data.Collections;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data
{
	public class MongoDB
	{
		public IMongoDatabase Db { get; set; }

		public MongoDB(IConfiguration configuration)
		{
			try
			{
				var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionString"]));
				var client = new MongoClient(settings);
				Db = client.GetDatabase(configuration["NomeBanco"]);
				MapClasses();
			}
			catch (Exception ex)
			{
				throw new MongoException("Não foi possível conectar-se ao MongoDB.", ex);
			}
			
		}

		private void MapClasses()
		{
			var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
			ConventionRegistry.Register("camelCase", conventionPack, t => true);

			if (!BsonClassMap.IsClassMapRegistered(typeof(Infectado)))
			{
				BsonClassMap.RegisterClassMap<Infectado>(i => {
					i.AutoMap();
					i.SetIgnoreExtraElements(true);
				});
			}
		}
	}
}
