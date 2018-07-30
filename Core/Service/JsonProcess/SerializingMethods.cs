
namespace Core.Service.JsonProcess
{
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SerializingMethods
	{
	   public static IList<T> ParsePartialJson<T>(IList<JToken> resultsPath)
		{		    
		        IList<T> elements = new List<T>();

		        foreach (JToken element in resultsPath)
		        {
		            T searchResult = JsonConvert.DeserializeObject<T>(element.ToString());
		            elements.Add(searchResult);
		        }

		        return elements;		    
		}
	}
}
