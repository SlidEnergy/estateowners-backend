using EstateOwners.App;
using EstateOwners.Domain;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EstateOwners.UnitTests
{
    public class BuildingsServiceTests: TestsBase
    {
		BuildingsService _service;

		[SetUp]
		public void Setup()
		{
			_service = new BuildingsService(_db);
		}

		[Test]
		public async Task GetList_ShouldBeListReturned()
		{
			var complex = new ResidentialComplex("Title", "Address");
			_db.ResidentialComplexes.Add(complex);
			await _db.SaveChangesAsync();

			var building1 = new Building(complex.Id, "ShortAddress1", "Address1");
			_db.Buildings.Add(building1);
			await _db.SaveChangesAsync();

			var building2 = new Building(complex.Id, "ShortAddress2", "Address2");
			_db.Buildings.Add(building2);
			await _db.SaveChangesAsync();

			var result = await _service.GetListAsync();

			Assert.AreEqual(2, result.Count());
		}
	}
}