using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace AnimalShelter
{
  public class Animals
  {
    private int _id;
    private string _name;
    private string _breed;
    private string _gender;
    private int _typeId;
    private DateTime _dateTime;

    public Animals(string name, string breed, string gender, int typeId, DateTime dateTime, int Id =0)
    {
      _id = Id;
      _name = name;
      _breed = breed;
      _gender = gender;
      _typeId = typeId;
      _dateTime = dateTime;
    }

    public override bool Equals(System.Object otherAnimals)
    {
      if (!(otherAnimals is Animals)) {
        return false;
      }
      else
      {
        Animals newAnimals = (Animals) otherAnimals;
        bool idEquality = this.GetId() == newAnimals.GetId();
        bool nameEquality = this.GetName() == newAnimals.GetName();
        bool breedEquality = this.GetBreed() == newAnimals.GetBreed();
        bool genderEquality = this.GetGender() == newAnimals.GetGender();
        bool typeEquality = this.GetTypeId() == newAnimals.GetTypeId();
        bool dateEquality = this.GetDate() == newAnimals.GetDate();
        return (idEquality && nameEquality && breedEquality && genderEquality && typeEquality && dateEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

    public string GetBreed()
    {
      return _breed;
    }

    public void SetBreed(string newBreed)
    {
      _breed = newBreed;
    }

    public string GetGender()
    {
      return _gender;
    }

    public void SetGender(string newGender)
    {
      _gender = newGender;
    }


    public int GetTypeId()
    {
      return _typeId;
    }

    public void SetTypeId(int newTypeId)
    {
      _typeId = newTypeId;
    }

    public DateTime GetDate()
    {
      return _dateTime;
    }

    public void SetDate(DateTime newDateTimes)
    {
      _dateTime = newDateTimes;
    }

    public static List<Animals> GetAll()
    {
      List<Animals> allAnimals = new List<Animals>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("Select * FROM  animals ORDER BY breed DESC;",conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int animalId = rdr.GetInt32(0);
        string animalName = rdr.GetString(1);
        string animalBreed = rdr.GetString(2);
        string animalGender = rdr.GetString(3);
        int animalTypeId = rdr.GetInt32(4);
        DateTime animalDate = rdr.GetDateTime(5);
        Animals newAnimals = new Animals(animalName, animalBreed, animalGender, animalTypeId, animalDate, animalId);
        allAnimals.Add(newAnimals);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allAnimals;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("Insert INTO Animals (name, breed, gender, typeId, time) OUTPUT INSERTED.id VALUES (@AnimalsName, @AnimalsBreed, @AnimalsGender, @AnimalsTypeId, @AnimalsDate);",conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@AnimalsName";
      nameParameter.Value = this.GetName();

      SqlParameter breedParameter = new SqlParameter();
      breedParameter.ParameterName = "@AnimalsBreed";
      breedParameter.Value = this.GetBreed();

      SqlParameter genderParameter = new SqlParameter();
      genderParameter.ParameterName = "@AnimalsGender";
      genderParameter.Value = this.GetGender();

      SqlParameter typeIdParameter = new SqlParameter();
      typeIdParameter.ParameterName = "@AnimalsTypeId";
      typeIdParameter.Value = this.GetTypeId();

      SqlParameter dateParameter = new SqlParameter();
      dateParameter.ParameterName = "@AnimalsDate";
      dateParameter.Value = this.GetDate();

      cmd.Parameters.Add(typeIdParameter);
      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(breedParameter);
      cmd.Parameters.Add(genderParameter);
      cmd.Parameters.Add(dateParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM animals;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Animals Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals WHERE id= @AnimalsId;", conn);
      SqlParameter AnimalsIdParameter = new SqlParameter();
      AnimalsIdParameter.ParameterName = "@AnimalsId";
      AnimalsIdParameter.Value = id.ToString();
      cmd.Parameters.Add(AnimalsIdParameter);
      rdr = cmd.ExecuteReader();

      int foundAnimalsId = 0;
      string foundAnimalsName = null;
      string foundAnimalsBreed = null;
      string foundAnimalsGender = null;
      int foundAnimalsTypeId = 0;
      DateTime foundAnimalsDate = new DateTime (2016-02-23);

      while(rdr.Read())
      {
        foundAnimalsId = rdr.GetInt32(0);
        foundAnimalsName = rdr.GetString(1);
        foundAnimalsBreed = rdr.GetString(2);
        foundAnimalsGender = rdr.GetString(3);
        foundAnimalsTypeId = rdr.GetInt32(4);
        foundAnimalsDate = rdr.GetDateTime(5);
      }
      Animals foundAnimals = new Animals(foundAnimalsName, foundAnimalsBreed, foundAnimalsGender, foundAnimalsTypeId, foundAnimalsDate, foundAnimalsId);

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return foundAnimals;
    }
  }
}
