using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AnimalShelter
{
  public class TypeTest : IDisposable
  {
    public TypeTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=animalShelter_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_TypesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Type.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Type firstType = new Type("Mammal");
      Type secondType = new Type("Mammal");

      //Assert
      Assert.Equal(firstType, secondType);
    }

    [Fact]
    public void Test_Save_SavesTypeToDatabase()
    {
      //Arrange
      Type testType = new Type("Mammal");
      testType.Save();

      //Act
      List<Type> result = Type.GetAll();
      List<Type> testList = new List<Type>{testType};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToTypeObject()
    {
      //Arrange
      Type testType = new Type("Mammal");
      testType.Save();

      //Act
      Type savedType = Type.GetAll()[0];

      int result = savedType.GetId();
      int testId = testType.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTypeInDatabase()
    {
      //Arrange
      Type testType = new Type("Mammal");
      testType.Save();

      //Act
      Type foundType = Type.Find(testType.GetId());

      //Assert
      Assert.Equal(testType, foundType);
    }

    [Fact]
    public void Test_GetTasks_RetrieveAllTasksWithType()
    {
      Type testType = new Type("Mammal");
      testType.Save();

      Animals firstAnimal = new Animals("Harry", "rabbit", "male", testType.GetId(), new DateTime (2016, 2, 16));
      firstAnimal.Save();

      Animals secondAnimal = new Animals("Harry", "rabbit", "male", testType.GetId(), new DateTime (2016, 2, 16));
      secondAnimal.Save();

      List<Animals> testAnimalsList = new List<Animals> {firstAnimal, secondAnimal};
      List<Animals> resultAnimalsList = testType.GetAnimals();

      // foreach (var animal in testAnimalsList)
      // {
      //   Console.WriteLine(animal.GetId());
      // }
      // foreach (var animal in resultAnimalsList)
      // {
      //   Console.WriteLine(animal.GetId());
      // }

      Assert.Equal(testAnimalsList, resultAnimalsList);
    }

    public void Dispose()
    {
      Animals.DeleteAll();
      Type.DeleteAll();
    }
  }
}
