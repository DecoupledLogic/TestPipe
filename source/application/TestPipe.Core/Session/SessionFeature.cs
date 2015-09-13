namespace TestPipe.Core.Session
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestPipe.Core.Interfaces;

    public class SessionFeature
    {
        public SessionFeature()
        {
            this.Scenarios = new List<SessionScenario>();
        }

        public IBrowser Browser { get; set; }

        public string Id { get; set; }

        public string KeyPrefix { get; set; }

        public string Path { get; set; }

        public ICollection<SessionScenario> Scenarios { get; set; }

        public string[] Tags { get; set; }

        public string Title { get; set; }

        public SessionScenario GetScenario(string id)
        {
            if (this.Scenarios == null)
            {
                throw new NullReferenceException("Features cannot be a null value.");
            }

            SessionScenario scenario = this.Scenarios.Where(x => x.Id == id).FirstOrDefault();
            return scenario;
        }
    }
}