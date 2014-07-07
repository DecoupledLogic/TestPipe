namespace TestPipe.Core.Specs.Session
{
	using System;
	using System.Collections.Generic;
	using System.Dynamic;
	using System.Linq;
	using KellermanSoftware.CompareNetObjects;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using TestPipe.Core.Session;
	using Xunit;

	public class DeserializeFact
	{
		[Fact]
		public void Can_Deserialize_Suite()
		{
			string json = this.GetSuiteJson();
			SessionSuite expected = this.GetSuite();

			var e = JsonConvert.DeserializeObject<SessionSuite>(json);

			SessionSuite suite = (SessionSuite)e;

			CompareLogic compareLogic = new CompareLogic();
			ComparisonResult result = compareLogic.Compare(expected, suite);

			Assert.True(result.AreEqual, result.DifferencesString);
		}

		[Fact]
		public void Can_Deserialize_Feature()
		{
			string json = this.GetFeatureJson();
			SessionFeature expected = this.GetFeature();

			var e = JsonConvert.DeserializeObject<SessionFeature>(json);

			SessionFeature feature = (SessionFeature)e;

			Assert.Equal(expected.Id, feature.Id);
			Assert.Equal(expected.Path, feature.Path);
			Assert.Equal(expected.Title, feature.Title);
			Assert.Equal(expected.Scenarios.FirstOrDefault().Data.UserName, feature.Scenarios.FirstOrDefault().Data.UserName);
			Assert.Equal(expected.Scenarios.FirstOrDefault().Data.Password, feature.Scenarios.FirstOrDefault().Data.Password);
		}

		private string GetSuiteJson()
		{
			return @"{
									'Name': 'Google',
									'Browser': 'Firefox',
									'ApplicationKey': 'C5851267-AD37-482B-9762-A3FE5DF017EE',
									'Environment': 'Prod',
									'BaseURL': 'http://www.bing.com/',
									'LogoutURL': '',
									'Timeout': 60,
									'PageTimeout': 60,
									'LoginUrl': '',
									'WaitTime': 60,
									'DbConnections': [
										{
											'Name': 'Main',	
											'ConnectionString': 'Data Source=localhost;Initial Catalog=Demo;Integrated Security=True'
										},
										{
											'Name': 'Security', 
											'ConnectionString': 'Data Source=localhost;Initial Catalog=Demo;Integrated Security=True'
										}
									],
									'Features': [
										{
											'Title': 'Search',
											'Id': '1',
											'Path': 'Feature_Search.json'
										},
										{
											'Title': 'Result',
											'Id': '2',
											'Path': 'Feature_Result.json'
										}
									]
								}";
		}

		private SessionSuite GetSuite()
		{
			SessionSuite suite = new SessionSuite();
			suite.Name = "Google";
			suite.Browser = "Firefox";
			suite.ApplicationKey = "C5851267-AD37-482B-9762-A3FE5DF017EE";
			suite.Environment = "Prod";
			suite.BaseUrl = "http://www.bing.com/";
			suite.LogoutUrl = string.Empty;
			suite.Timeout = 60;
			suite.LoginUrl = string.Empty;

			ICollection<DbConnection> dbConnections = new List<DbConnection>();

			DbConnection connection = new DbConnection();

			connection.Name = "Main";
			connection.ConnectionString = "Data Source=localhost;Initial Catalog=Demo;Integrated Security=True";
			dbConnections.Add(connection);

			connection = new DbConnection();
			connection.Name = "Security";
			connection.ConnectionString = "Data Source=localhost;Initial Catalog=Demo;Integrated Security=True";
			dbConnections.Add(connection);

			suite.DbConnections = dbConnections;

			ICollection<SessionFeature> features = new List<SessionFeature>();

			SessionFeature feature = new SessionFeature();

			feature.Title = "Search";
			feature.Id = "1";
			feature.Path = "Feature_Search.json";
			features.Add(feature);

			feature = new SessionFeature();
			feature.Title = "Result";
			feature.Id = "2";
			feature.Path = "Feature_Result.json";
			features.Add(feature);

			suite.Features = features;

			return suite;
		}

		private string GetFeatureJson()
		{
			return @"{
									'Id': '1',
									'Path': 'Feature_Search.json',
									'Title': 'Search',
									'Scenarios': [
										{
											'Id': '1',
											'Data': {
												'UserName':'testuser1', 
												'Password':'Abcd1234'
											}
										}
									]
								}";
		}

		private SessionFeature GetFeature()
		{
			SessionFeature feature = new SessionFeature();

			feature.Id = "1";
			feature.Path = "Feature_Search.json";
			feature.Title = "Search";

			ICollection<SessionScenario> scenarios = new List<SessionScenario>();

			SessionScenario scenario = new SessionScenario();
			scenario.Id = "1";

			dynamic data = new JObject();

			data.UserName = "testuser1";
			data.Password = "Abcd1234";

			scenario.Data = data;

			scenarios.Add(scenario);

			feature.Scenarios = scenarios;

			return feature;
		}
	}
}
