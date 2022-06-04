using EstateOwners.App;
using EstateOwners.Domain;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EstateOwners.UnitTests
{
    public class EstatesServiceTests: TestsBase
	{
		EstatesService _service;

		[SetUp]
		public void Setup()
		{
			_service = new EstatesService(_db);
		}

		[Test]
		public async Task AddEstate_ShouldBeAdded()
		{
			var complex = new ResidentialComplex("Title", "Address");
			_db.ResidentialComplexes.Add(complex);
			await _db.SaveChangesAsync();

			var building = new Building(complex.Id, "ShortAddress", "Address");
			_db.Buildings.Add(building);
			await _db.SaveChangesAsync();

			var type = EstateType.Apartment;
			var number = "324";

			await _service.AddEstateAsync(_user.Id, new Estate(type, building.Id, number));

			var count = await _db.Estates.CountAsync();
			Assert.AreEqual(1, count);
		}
	}
}