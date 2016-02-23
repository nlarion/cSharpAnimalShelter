using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace AnimalShelter
{
  public class Type
  {
    private int _id;
    private string _name;

    public Type(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherType)
    {
        if (!(otherType is Type))
        {
          return false;
        }
        else
        {
          Type newType = (Type) otherType;
          bool idEquality = this.GetId() == newType.GetId();
          bool nameEquality = this.GetName() == newType.GetName();
          return (idEquality && nameEquality);
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
    public static List<Type> GetAll()
    {
      List<Type> allTypes = new List<Type>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM types;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int typeId = rdr.GetInt32(0);
        string typeName = rdr.GetString(1);
        Type newType = new Type(typeName, typeId);
        allTypes.Add(newType);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allTypes;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO types (name) OUTPUT INSERTED.id VALUES (@TypesName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@TypesName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM types;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Type Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM types WHERE id = @TypesId;", conn);
      SqlParameter typeIdParameter = new SqlParameter();
      typeIdParameter.ParameterName = "@TypesId";
      typeIdParameter.Value = id.ToString();
      cmd.Parameters.Add(typeIdParameter);
      rdr = cmd.ExecuteReader();

      int foundTypesId = 0;
      string foundTypesName = null;

      while(rdr.Read())
      {
        foundTypesId = rdr.GetInt32(0);
        foundTypesName = rdr.GetString(1);
      }
      Type foundCategory = new Type(foundTypesName, foundTypesId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCategory;
    }

    public List<Animals> GetAnimals()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals WHERE typeId = @TypesId ORDER BY breed DESC;", conn);
      SqlParameter typeIdParameter = new SqlParameter();
      typeIdParameter.ParameterName = "@TypesId";
      typeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(typeIdParameter);
      rdr = cmd.ExecuteReader();

      List<Animals> animals = new List<Animals> {};
      while (rdr.Read())
      {
        int animalsId = rdr.GetInt32(0);
        string animalsName = rdr.GetString(1);
        string animalsBreed = rdr.GetString(2);
        string animalsGender = rdr.GetString(3);
        int animalsTypesId = rdr.GetInt32(4);
        DateTime animalsDate = rdr.GetDateTime(5);
        Animals newAnimals = new Animals(animalsName, animalsBreed, animalsGender, animalsTypesId, animalsDate, animalsId);
        animals.Add(newAnimals);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        rdr.Close();
      }
      return animals;
    }
  }
}
