using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace RestSharpTest
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        RestClient Client;
        [TestInitialize]
        public void SetUp()
        {
            Client = new RestClient("http://localhost:4000");
        }
        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);
            IRestResponse response = Client.Execute(request);
            return response;
        }
        [TestMethod]
        public void OnCallingList_ReturnEmployeeList()
        {

            IRestResponse response = getEmployeeList();
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(5, dataResponse.Count);
            foreach(Employee e in dataResponse)
            {
                System.Console.WriteLine("id: " + e.id + " Name: " + e.name + " Salary " + e.salary);
            }
        }

        [TestMethod]
        public void givenEmployee_OnPost_ShouldResturnAddEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObject = new JObject();
            jObject.Add("name", "Clark");
            jObject.Add("Salary", "5000");

            request.AddParameter("application/json", jObject, ParameterType.RequestBody);
            IRestResponse response = Client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);

            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual("5000", dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }

    }
}
