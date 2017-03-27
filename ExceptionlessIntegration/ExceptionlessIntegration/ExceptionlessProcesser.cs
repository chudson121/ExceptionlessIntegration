using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ExceptionlessIntegration
{
    internal class ExceptionlessProcesser
    {
        private ExceptionlessConfig Config { get; set; }
        private string AuthToken { get; set; }

        public ExceptionlessProcesser(ExceptionlessConfig exceptionlessConfiguration)
        {
            Config = exceptionlessConfiguration;
            if (string.IsNullOrEmpty(AuthToken))
            {
                Login();
            }
        }


        
        private void Login()
        {
            var payload = new Dictionary<string, string>
            {
                {"email", Config.AccountEmail},
                {"password", Config.AccountPassword}
            };

            string apiPathLogin = "/api/v2/auth/login";
            var verb = Method.POST;

            var response = ConfigureClient(verb, apiPathLogin, payload);
            var content = response.Content; // raw content as string
            var json = JsonConvert.DeserializeObject< Dictionary<string, string>>(content);

            AuthToken = json["token"];
            
        }

        public List<ProjectDto> GetProjects()
        {
            string apiPathProjects = $"/api/v2/organizations/{Config.OrganizationId}/projects";
            var verb = Method.GET;
            //var payload = new Dictionary<string, string>();
            var response = ConfigureClient(verb, apiPathProjects, null);
            var content = response.Content; // raw content as string
            var jsonObj = JsonConvert.DeserializeObject<List<ProjectDto>>(content);

            return jsonObj;

        }

        public ProjectDto CreateProject(ProjectDto project)
        {

            string apiPathProjects = "/api/v2/projects";
            var verb = Method.POST;
            var payload = new Dictionary<string, string>
            {
                {"organization_id", Config.OrganizationId},
                {"name", project.name},
                {"delete_bot_data_enabled", "true"}
            };

            var response = ConfigureClient(verb, apiPathProjects, payload);
            var content = response.Content; // raw content as string
            var jsonObj = JsonConvert.DeserializeObject<ProjectDto>(content);
            var token = GenerateToken(jsonObj.id);
            project.apiToken = token.id;

            return project;
            
        }


        private TokenDto GenerateToken(string projectId)
        {

            string apiPathProjects = $"api/v2/projects/{projectId}/tokens";
            var verb = Method.POST;


            var response = ConfigureClient(verb, apiPathProjects, null);
            var content = response.Content; // raw content as string
            var jsonObj = JsonConvert.DeserializeObject<TokenDto>(content);

           return jsonObj;




        }

        public void GetProjectConfig(string projectId)
        {

            string apiPathProjects = $"api/v2/projects/{projectId}/config";
            var verb = Method.GET;
          

            var response = ConfigureClient(verb, apiPathProjects, null);
            var content = response.Content; // raw content as string
            var jsonObj = JsonConvert.DeserializeObject(content);

           // return jsonObj;




        }

        private IRestResponse ConfigureClient(Method httpVerb, string resourceMethod, Dictionary<string, string> payload)
        {
            var client = new RestClient(Config.BaseUrl);
            var request = new RestRequest(resourceMethod, httpVerb);

            if (!string.IsNullOrEmpty(AuthToken))
            {
                var bearerToken = string.Format("Bearer " + AuthToken);
                request.AddParameter("Authorization", bearerToken, ParameterType.HttpHeader);
            }

            if (payload != null)
            {
                foreach (var kvp in payload)
                {
                    request.AddParameter(kvp.Key, kvp.Value);
                }
            }

            var response = client.Execute(request);
            return response;

        }
    }
}
