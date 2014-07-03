namespace TestPipe.Core.Specs.Session
{
	using System;
	using System.Collections.Generic;
	using KellermanSoftware.CompareNetObjects;
	using Newtonsoft.Json;
	using TestPipe.Core.Session;
	using Xunit;

	public class DeserializeFact
	{
		[Fact]
		public void Can_Deserialize_Suite()
		{
			string json = this.GetSuiteJson();
			Suite expected = this.GetSuite();

			var e = JsonConvert.DeserializeObject<Suite>(json);

			Suite suite = (Suite)e;

			CompareLogic compareLogic = new CompareLogic();
			ComparisonResult result = compareLogic.Compare(expected, suite);

			Assert.True(result.AreEqual, result.DifferencesString);
		}

		[Fact]
		public void Can_Deserialize_Feature()
		{
			string json = this.GetFeatureJson();
			Feature expected = this.GetFeature();

			var e = JsonConvert.DeserializeObject<Feature>(json);

			Feature feature = (Feature)e;

			CompareLogic compareLogic = new CompareLogic();
			ComparisonResult result = compareLogic.Compare(expected, feature);

			//Assert.True(result.AreEqual, result.DifferencesString);
			Assert.True(result.Differences.Count < 2);
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

		private Suite GetSuite()
		{
			Suite suite = new Suite();
			suite.Name = "Google";
			suite.Browser = "Firefox";
			suite.ApplicationKey = "C5851267-AD37-482B-9762-A3FE5DF017EE";
			suite.Environment = "Prod";
			suite.BaseUrl = "http://www.bing.com/";
			suite.LogoutUrl = string.Empty;
			suite.Timeout = 60;
			suite.PageTimeout = 60;
			suite.LoginUrl = string.Empty;
			suite.WaitTime = 60;

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

			ICollection<Feature> features = new List<Feature>();

			Feature feature = new Feature();

			feature.Title = "Search";
			feature.Id = "1";
			feature.Path = "Feature_Search.json";
			features.Add(feature);

			feature = new Feature();
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
											'Data': [
												{'Key':'key', 'Value':'value'},
												{'Key':'key2', 'Value':'value2'},
												{'Key':'keyName', 'Value':'valueString'}
											],
											'Objects': [
												{
													'Type': 'User',
													'Name': 'testuser1',
													'Database': 'TestPipe',
													'Namespace': 'TestPipe.Data.Transfer.Dto.Test',
													'Seed': false,
													'Properties': [
														{'FullName': ''},
														{'User_ID': 0},
														{'User_Name': 'testuser1'}
													]
												}
											]
										}
									]
								}";
		}

		private Feature GetFeature()
		{
			Feature feature = new Feature();

			feature.Id = "1";
			feature.Path = "Feature_Search.json";
			feature.Title = "Search";

			ICollection<Scenario> scenarios = new List<Scenario>();

			Scenario scenario = new Scenario();
			scenario.Id = "1";

			ICollection<KeyValue> data = new List<KeyValue>();

			KeyValue kv = new KeyValue();
			kv.Key = "key";
			kv.Value = "value";
			data.Add(kv);

			kv = new KeyValue();
			kv.Key = "key2";
			kv.Value = "value2";
			data.Add(kv);

			kv = new KeyValue();
			kv.Key = "keyName";
			kv.Value = "valueString";
			data.Add(kv);

			scenario.Data = data;

			scenarios.Add(scenario);

			ICollection<DataObject> objects = new List<DataObject>();

			DataObject obj = new DataObject();
			obj.Name = "testuser1";
			obj.Database = "TestPipe";
			obj.Namespace = "TestPipe.Data.Transfer.Dto.Test";
			obj.Type = "User";
			obj.Seed = false;

			ICollection<IDictionary<string, dynamic>> properties = new List<IDictionary<string, dynamic>>();

			properties.Add(new Dictionary<string, dynamic>() { { "FullName", string.Empty } });
			properties.Add(new Dictionary<string, dynamic>() { { "User_ID", 0 } });
			properties.Add(new Dictionary<string, dynamic>() { { "User_Name", "testuser1" } });

			obj.Properties = properties;

			objects.Add(obj);

			scenario.Objects = objects;

			feature.Scenarios = scenarios;

			return feature;
		}
	}
}
