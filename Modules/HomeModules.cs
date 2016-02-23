using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace AnimalShelter
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Type> AllType = Type.GetAll();
        return View["index.cshtml", AllType];
      };

      Get["/animals"] = _ => {
        List<Animals> AllAnimals = Animals.GetAll();
        return View["animals.cshtml", AllAnimals];
      };
      Get["/types"] = _ => {
        List<Type> AllType = Type.GetAll();
        return View["types.cshtml", AllType];
      };

      Get["/types/new"] = _ => {
        return View["types_form.cshtml"];
      };
      Post["/types/new"] = _ => {
        Type newType = new Type(Request.Form["type-name"]);
        newType.Save();
        return View["success.cshtml"];
      };
      Get["/animals/new"] = _ => {
        List<Type> AllType = Type.GetAll();
        return View["animals_form.cshtml", AllType];
      };
      Post["/animals/new"] = _ => {
        DateTime dt = Convert.ToDateTime((string)Request.Form["dateTime"]);
        // DateTime newDateTime = new DateTime (Convert.ToDateTime(Request.Form["dateTime"]+" 00:00:00.00"));
        Console.WriteLine((string)Request.Form["dateTime"]);
        Animals newAnimals = new Animals(Request.Form["animals-name"], Request.Form["animals-breed"], Request.Form["animals-gender"],  Request.Form["type-id"], dt);
        newAnimals.Save();
        return View["success.cshtml"];
      };
      Post["/animals/delete"] = _ => {
        Animals.DeleteAll();
        return View["cleared.cshtml"];
      };
      Get["/types/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedType = Type.Find(parameters.id);
        var TypeAnimals = SelectedType.GetAnimals();
        model.Add("type", SelectedType);
        model.Add("animals", TypeAnimals);
        return View["type.cshtml", model];
      };
    }
  }
}
