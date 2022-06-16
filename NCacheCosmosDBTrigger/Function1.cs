using System;
using System.Collections.Generic;
using Alachisoft.NCache.Client;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NCacheCosmosDBTrigger
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ToDoList",
            collectionName: "Items",
            ConnectionStringSetting = "<Provide your CosmosDB Connection string>",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            ILogger log)
        {
            try
            {
                if (input != null && input.Count > 0)
                {
                  
                    Family family = null;

                    using (var cache = CacheManager.GetCache("myLocalCache"))
                    {
                        foreach (var document in input)
                        {
                            family = new Family
                            {
                                LastName = document.GetPropertyValue<string>("LastName"),
                                Id = document.GetPropertyValue<string>("Id")

                            };
                            cache.InsertAsync(family.Id, family);
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
