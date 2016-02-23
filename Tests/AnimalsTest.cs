using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Xunit;

namespace AnimalShelter
{
  public class AnimalsTest : IDisposable
  {
    public AnimalsTest()
    {
      DBConfiguration.ConnectionString = "Data Source = (localdb)\\mssqllocaldb;Initial Catalog=animalShelter_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Animals.GetAll().Count;
      Assert.Equal(0,result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionAreTheSame()
    {
      Animals firstAnimal = new Animals("Snakey", "snake", "female", 1, new DateTime (2016, 2, 18));
      Animals secondCoin = new Animals("Snakey", "snake", "female", 1, new DateTime (2016, 2, 18));

      Assert.Equal(firstAnimal, secondCoin);
    }
    [Fact]
    public void Test_Save_SavestoDatabase()
    {
      Animals testAnimals = new Animals("Snakey", "snake", "female", 1, new DateTime (2016, 2, 18));
      testAnimals.Save();
      List<Animals> result = Animals.GetAll();
      List<Animals> testList = new List<Animals> {testAnimals};

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObjects()
    {
      Animals testAnimals = new Animals("Snakey", "snake", "female", 1, new DateTime (2016, 2, 18));
      testAnimals.Save();

      Animals savedAnimals = Animals.GetAll()[0];

      int result = savedAnimals.GetId();
      int testId = testAnimals.GetId();
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsAnimalsInDatabase()
    {
      Animals testAnimals = new Animals("Snakey", "snake", "female", 1, new DateTime (2016, 2, 18));
      testAnimals.Save();

      Animals foundAnimals = Animals.Find(testAnimals.GetId());
      Assert.Equal(testAnimals, foundAnimals);
    }

    public void Dispose()
    {
      Animals.DeleteAll();
    }
  }
}
