/**
* (C) Copyright IBM Corp. 2018, 2019.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using NSubstitute;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using IBM.Cloud.SDK.Core.Http;
using IBM.Cloud.SDK.Core.Http.Exceptions;
using IBM.Cloud.SDK.Core.Authentication.NoAuth;
using IBM.Watson.Discovery.v1.Model;
using IBM.Cloud.SDK.Core.Model;
using Environment = IBM.Watson.Discovery.v1.Model.Environment;

namespace IBM.Watson.Discovery.v1.UnitTests
{
    [TestClass]
    public class DiscoveryServiceUnitTests
    {
        #region Constructor
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_HttpClient_Null()
        {
            DiscoveryService service = new DiscoveryService(httpClient: null);
        }

        [TestMethod]
        public void ConstructorHttpClient()
        {
            DiscoveryService service = new DiscoveryService(new IBMHttpClient());
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ConstructorExternalConfig()
        {
            DiscoveryService service = Substitute.For<DiscoveryService>("versionDate");
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void Constructor()
        {
            DiscoveryService service = new DiscoveryService(new IBMHttpClient());
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ConstructorAuthenticator()
        {
            DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
            Assert.IsNotNull(service);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNoVersion()
        {
            DiscoveryService service = new DiscoveryService(null, new NoAuthAuthenticator());
        }

        [TestMethod]
        public void ConstructorNoUrl()
        {
            var url = System.Environment.GetEnvironmentVariable("DISCOVERY_SERVICE_URL");
            System.Environment.SetEnvironmentVariable("DISCOVERY_SERVICE_URL", null);
            DiscoveryService service = Substitute.For<DiscoveryService>("versionDate");
            Assert.IsTrue(service.ServiceUrl == "https://gateway.watsonplatform.net/discovery/api");
            System.Environment.SetEnvironmentVariable("DISCOVERY_SERVICE_URL", url);
        }
        #endregion

        [TestMethod]
        public void CreateEnvironment_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var name = "name";
            var description = "description";
            var size = "size";

            var result = service.CreateEnvironment(name: name, description: description, size: size);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(name))
            {
                bodyObject["name"] = JToken.FromObject(name);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (!string.IsNullOrEmpty(size))
            {
                bodyObject["size"] = JToken.FromObject(size);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
        }
        [TestMethod]
        public void ListEnvironments_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var name = "name";

            var result = service.ListEnvironments(name: name);

            request.Received().WithArgument("version", versionDate);
        }
        [TestMethod]
        public void GetEnvironment_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";

            var result = service.GetEnvironment(environmentId: environmentId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetEnvironment_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetEnvironment(environmentId: environmentId);
    }
        [TestMethod]
        public void UpdateEnvironment_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PutAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var name = "name";
            var description = "description";
            var size = "size";

            var result = service.UpdateEnvironment(environmentId: environmentId, name: name, description: description, size: size);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(name))
            {
                bodyObject["name"] = JToken.FromObject(name);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (!string.IsNullOrEmpty(size))
            {
                bodyObject["size"] = JToken.FromObject(size);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PutAsync($"{service.ServiceUrl}/v1/environments/{environmentId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateEnvironment_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.UpdateEnvironment(environmentId: environmentId, name: name, description: description, size: size);
    }
        [TestMethod]
        public void DeleteEnvironment_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";

            var result = service.DeleteEnvironment(environmentId: environmentId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteEnvironment_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteEnvironment(environmentId: environmentId);
    }
        [TestMethod]
        public void ListFields_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionIds = new List<string>() { "collectionIds0", "collectionIds1" };

            var result = service.ListFields(environmentId: environmentId, collectionIds: collectionIds);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/fields");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListFields_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.ListFields(environmentId: environmentId, collectionIds: collectionIds);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListFields_NoCollectionIds()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        List<string> collectionIds = null;
        var result = service.ListFields(environmentId: environmentId, collectionIds: collectionIds);
    }
        [TestMethod]
        public void CreateConfiguration_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var name = "name";
            var description = "description";
            var conversions = new Conversions();
            var enrichments = new List<Enrichment>();
            var normalizations = new List<NormalizationOperation>();
            var source = new Source();

            var result = service.CreateConfiguration(environmentId: environmentId, name: name, description: description, conversions: conversions, enrichments: enrichments, normalizations: normalizations, source: source);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(name))
            {
                bodyObject["name"] = JToken.FromObject(name);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (conversions != null)
            {
                bodyObject["conversions"] = JToken.FromObject(conversions);
            }
            if (enrichments != null && enrichments.Count > 0)
            {
                bodyObject["enrichments"] = JToken.FromObject(enrichments);
            }
            if (normalizations != null && normalizations.Count > 0)
            {
                bodyObject["normalizations"] = JToken.FromObject(normalizations);
            }
            if (source != null)
            {
                bodyObject["source"] = JToken.FromObject(source);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/configurations");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateConfiguration_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.CreateConfiguration(environmentId: environmentId, name: name, description: description, conversions: conversions, enrichments: enrichments, normalizations: normalizations, source: source);
    }
        [TestMethod]
        public void ListConfigurations_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var name = "name";

            var result = service.ListConfigurations(environmentId: environmentId, name: name);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/configurations");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListConfigurations_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.ListConfigurations(environmentId: environmentId, name: name);
    }
        [TestMethod]
        public void GetConfiguration_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var configurationId = "configurationId";

            var result = service.GetConfiguration(environmentId: environmentId, configurationId: configurationId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/configurations/{configurationId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetConfiguration_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetConfiguration(environmentId: environmentId, configurationId: configurationId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetConfiguration_NoConfigurationId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var configurationId = null;
        var result = service.GetConfiguration(environmentId: environmentId, configurationId: configurationId);
    }
        [TestMethod]
        public void UpdateConfiguration_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PutAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var configurationId = "configurationId";
            var name = "name";
            var description = "description";
            var conversions = new Conversions();
            var enrichments = new List<Enrichment>();
            var normalizations = new List<NormalizationOperation>();
            var source = new Source();

            var result = service.UpdateConfiguration(environmentId: environmentId, configurationId: configurationId, name: name, description: description, conversions: conversions, enrichments: enrichments, normalizations: normalizations, source: source);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(name))
            {
                bodyObject["name"] = JToken.FromObject(name);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (conversions != null)
            {
                bodyObject["conversions"] = JToken.FromObject(conversions);
            }
            if (enrichments != null && enrichments.Count > 0)
            {
                bodyObject["enrichments"] = JToken.FromObject(enrichments);
            }
            if (normalizations != null && normalizations.Count > 0)
            {
                bodyObject["normalizations"] = JToken.FromObject(normalizations);
            }
            if (source != null)
            {
                bodyObject["source"] = JToken.FromObject(source);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PutAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/configurations/{configurationId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateConfiguration_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.UpdateConfiguration(environmentId: environmentId, configurationId: configurationId, name: name, description: description, conversions: conversions, enrichments: enrichments, normalizations: normalizations, source: source);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateConfiguration_NoConfigurationId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var configurationId = null;
        var result = service.UpdateConfiguration(environmentId: environmentId, configurationId: configurationId, name: name, description: description, conversions: conversions, enrichments: enrichments, normalizations: normalizations, source: source);
    }
        [TestMethod]
        public void DeleteConfiguration_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var configurationId = "configurationId";

            var result = service.DeleteConfiguration(environmentId: environmentId, configurationId: configurationId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/configurations/{configurationId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteConfiguration_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteConfiguration(environmentId: environmentId, configurationId: configurationId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteConfiguration_NoConfigurationId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var configurationId = null;
        var result = service.DeleteConfiguration(environmentId: environmentId, configurationId: configurationId);
    }
        [TestMethod]
        public void CreateCollection_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var name = "name";
            var description = "description";
            var configurationId = "configurationId";
            var language = "language";

            var result = service.CreateCollection(environmentId: environmentId, name: name, description: description, configurationId: configurationId, language: language);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(name))
            {
                bodyObject["name"] = JToken.FromObject(name);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (!string.IsNullOrEmpty(configurationId))
            {
                bodyObject["configuration_id"] = JToken.FromObject(configurationId);
            }
            if (!string.IsNullOrEmpty(language))
            {
                bodyObject["language"] = JToken.FromObject(language);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateCollection_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.CreateCollection(environmentId: environmentId, name: name, description: description, configurationId: configurationId, language: language);
    }
        [TestMethod]
        public void ListCollections_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var name = "name";

            var result = service.ListCollections(environmentId: environmentId, name: name);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListCollections_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.ListCollections(environmentId: environmentId, name: name);
    }
        [TestMethod]
        public void GetCollection_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.GetCollection(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetCollection_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetCollection(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetCollection_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.GetCollection(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void UpdateCollection_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PutAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var name = "name";
            var description = "description";
            var configurationId = "configurationId";

            var result = service.UpdateCollection(environmentId: environmentId, collectionId: collectionId, name: name, description: description, configurationId: configurationId);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(name))
            {
                bodyObject["name"] = JToken.FromObject(name);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (!string.IsNullOrEmpty(configurationId))
            {
                bodyObject["configuration_id"] = JToken.FromObject(configurationId);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PutAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateCollection_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.UpdateCollection(environmentId: environmentId, collectionId: collectionId, name: name, description: description, configurationId: configurationId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateCollection_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.UpdateCollection(environmentId: environmentId, collectionId: collectionId, name: name, description: description, configurationId: configurationId);
    }
        [TestMethod]
        public void DeleteCollection_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.DeleteCollection(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteCollection_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteCollection(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteCollection_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.DeleteCollection(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void ListCollectionFields_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.ListCollectionFields(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/fields");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListCollectionFields_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.ListCollectionFields(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListCollectionFields_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.ListCollectionFields(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void ListExpansions_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.ListExpansions(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/expansions");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListExpansions_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.ListExpansions(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListExpansions_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.ListExpansions(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void CreateExpansions_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var expansions = new List<Expansion>();

            var result = service.CreateExpansions(environmentId: environmentId, collectionId: collectionId, expansions: expansions);

            JObject bodyObject = new JObject();
            if (expansions != null && expansions.Count > 0)
            {
                bodyObject["expansions"] = JToken.FromObject(expansions);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/expansions");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateExpansions_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.CreateExpansions(environmentId: environmentId, collectionId: collectionId, expansions: expansions);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateExpansions_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.CreateExpansions(environmentId: environmentId, collectionId: collectionId, expansions: expansions);
    }
        [TestMethod]
        public void DeleteExpansions_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.DeleteExpansions(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/expansions");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteExpansions_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteExpansions(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteExpansions_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.DeleteExpansions(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void GetTokenizationDictionaryStatus_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.GetTokenizationDictionaryStatus(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/word_lists/tokenization_dictionary");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetTokenizationDictionaryStatus_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetTokenizationDictionaryStatus(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetTokenizationDictionaryStatus_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.GetTokenizationDictionaryStatus(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void CreateTokenizationDictionary_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var tokenizationRules = new List<TokenDictRule>();

            var result = service.CreateTokenizationDictionary(environmentId: environmentId, collectionId: collectionId, tokenizationRules: tokenizationRules);

            JObject bodyObject = new JObject();
            if (tokenizationRules != null && tokenizationRules.Count > 0)
            {
                bodyObject["tokenization_rules"] = JToken.FromObject(tokenizationRules);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/word_lists/tokenization_dictionary");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateTokenizationDictionary_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.CreateTokenizationDictionary(environmentId: environmentId, collectionId: collectionId, tokenizationRules: tokenizationRules);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateTokenizationDictionary_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.CreateTokenizationDictionary(environmentId: environmentId, collectionId: collectionId, tokenizationRules: tokenizationRules);
    }
        [TestMethod]
        public void DeleteTokenizationDictionary_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.DeleteTokenizationDictionary(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/word_lists/tokenization_dictionary");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteTokenizationDictionary_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteTokenizationDictionary(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteTokenizationDictionary_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.DeleteTokenizationDictionary(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void GetStopwordListStatus_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.GetStopwordListStatus(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/word_lists/stopwords");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetStopwordListStatus_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetStopwordListStatus(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetStopwordListStatus_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.GetStopwordListStatus(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void CreateStopwordList_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var stopwordFile = new MemoryStream();
            var stopwordFilename = "stopwordFilename";

            var result = service.CreateStopwordList(environmentId: environmentId, collectionId: collectionId, stopwordFile: stopwordFile, stopwordFilename: stopwordFilename);

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/word_lists/stopwords");

        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateStopwordList_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.CreateStopwordList(environmentId: environmentId, collectionId: collectionId, stopwordFile: stopwordFile, stopwordFilename: stopwordFilename);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateStopwordList_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.CreateStopwordList(environmentId: environmentId, collectionId: collectionId, stopwordFile: stopwordFile, stopwordFilename: stopwordFilename);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateStopwordList_NoStopwordFile()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        System.IO.MemoryStream stopwordFile = null;
        var result = service.CreateStopwordList(environmentId: environmentId, collectionId: collectionId, stopwordFile: stopwordFile, stopwordFilename: stopwordFilename);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateStopwordList_NoStopwordFilename()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        string stopwordFilename = null;
        var result = service.CreateStopwordList(environmentId: environmentId, collectionId: collectionId, stopwordFile: stopwordFile, stopwordFilename: stopwordFilename);
    }
        [TestMethod]
        public void DeleteStopwordList_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.DeleteStopwordList(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/word_lists/stopwords");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteStopwordList_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteStopwordList(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteStopwordList_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.DeleteStopwordList(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void AddDocument_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var file = new MemoryStream();
            var filename = "filename";
            var fileContentType = "fileContentType";
            var metadata = "metadata";

            var result = service.AddDocument(environmentId: environmentId, collectionId: collectionId, file: file, filename: filename, fileContentType: fileContentType, metadata: metadata);

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/documents");

        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void AddDocument_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.AddDocument(environmentId: environmentId, collectionId: collectionId, file: file, filename: filename, fileContentType: fileContentType, metadata: metadata);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void AddDocument_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.AddDocument(environmentId: environmentId, collectionId: collectionId, file: file, filename: filename, fileContentType: fileContentType, metadata: metadata);
    }
        [TestMethod]
        public void GetDocumentStatus_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var documentId = "documentId";

            var result = service.GetDocumentStatus(environmentId: environmentId, collectionId: collectionId, documentId: documentId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/documents/{documentId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetDocumentStatus_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetDocumentStatus(environmentId: environmentId, collectionId: collectionId, documentId: documentId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetDocumentStatus_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.GetDocumentStatus(environmentId: environmentId, collectionId: collectionId, documentId: documentId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetDocumentStatus_NoDocumentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var documentId = null;
        var result = service.GetDocumentStatus(environmentId: environmentId, collectionId: collectionId, documentId: documentId);
    }
        [TestMethod]
        public void UpdateDocument_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var documentId = "documentId";
            var file = new MemoryStream();
            var filename = "filename";
            var fileContentType = "fileContentType";
            var metadata = "metadata";

            var result = service.UpdateDocument(environmentId: environmentId, collectionId: collectionId, documentId: documentId, file: file, filename: filename, fileContentType: fileContentType, metadata: metadata);

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/documents/{documentId}");

        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateDocument_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.UpdateDocument(environmentId: environmentId, collectionId: collectionId, documentId: documentId, file: file, filename: filename, fileContentType: fileContentType, metadata: metadata);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateDocument_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.UpdateDocument(environmentId: environmentId, collectionId: collectionId, documentId: documentId, file: file, filename: filename, fileContentType: fileContentType, metadata: metadata);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateDocument_NoDocumentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var documentId = null;
        var result = service.UpdateDocument(environmentId: environmentId, collectionId: collectionId, documentId: documentId, file: file, filename: filename, fileContentType: fileContentType, metadata: metadata);
    }
        [TestMethod]
        public void DeleteDocument_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var documentId = "documentId";

            var result = service.DeleteDocument(environmentId: environmentId, collectionId: collectionId, documentId: documentId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/documents/{documentId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteDocument_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteDocument(environmentId: environmentId, collectionId: collectionId, documentId: documentId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteDocument_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.DeleteDocument(environmentId: environmentId, collectionId: collectionId, documentId: documentId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteDocument_NoDocumentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var documentId = null;
        var result = service.DeleteDocument(environmentId: environmentId, collectionId: collectionId, documentId: documentId);
    }
        [TestMethod]
        public void Query_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var filter = "filter";
            var query = "query";
            var naturalLanguageQuery = "naturalLanguageQuery";
            var passages = false;
            var aggregation = "aggregation";
            var count = 1;
            var _return = "_return";
            var offset = 1;
            var sort = "sort";
            var highlight = false;
            var passagesFields = "passagesFields";
            var passagesCount = 1;
            var passagesCharacters = 1;
            var deduplicate = false;
            var deduplicateField = "deduplicateField";
            var similar = false;
            var similarDocumentIds = "similarDocumentIds";
            var similarFields = "similarFields";
            var bias = "bias";
            var spellingSuggestions = false;
            var xWatsonLoggingOptOut = false;

            var result = service.Query(environmentId: environmentId, collectionId: collectionId, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, passages: passages, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, passagesFields: passagesFields, passagesCount: passagesCount, passagesCharacters: passagesCharacters, deduplicate: deduplicate, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields, bias: bias, spellingSuggestions: spellingSuggestions, xWatsonLoggingOptOut: xWatsonLoggingOptOut);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(filter))
            {
                bodyObject["filter"] = JToken.FromObject(filter);
            }
            if (!string.IsNullOrEmpty(query))
            {
                bodyObject["query"] = JToken.FromObject(query);
            }
            if (!string.IsNullOrEmpty(naturalLanguageQuery))
            {
                bodyObject["natural_language_query"] = JToken.FromObject(naturalLanguageQuery);
            }
            bodyObject["passages"] = JToken.FromObject(passages);
            if (!string.IsNullOrEmpty(aggregation))
            {
                bodyObject["aggregation"] = JToken.FromObject(aggregation);
            }
            if (count != null)
            {
                bodyObject["count"] = JToken.FromObject(count);
            }
            if (!string.IsNullOrEmpty(_return))
            {
                bodyObject["return"] = JToken.FromObject(_return);
            }
            if (offset != null)
            {
                bodyObject["offset"] = JToken.FromObject(offset);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                bodyObject["sort"] = JToken.FromObject(sort);
            }
            bodyObject["highlight"] = JToken.FromObject(highlight);
            if (!string.IsNullOrEmpty(passagesFields))
            {
                bodyObject["passages.fields"] = JToken.FromObject(passagesFields);
            }
            if (passagesCount != null)
            {
                bodyObject["passages.count"] = JToken.FromObject(passagesCount);
            }
            if (passagesCharacters != null)
            {
                bodyObject["passages.characters"] = JToken.FromObject(passagesCharacters);
            }
            bodyObject["deduplicate"] = JToken.FromObject(deduplicate);
            if (!string.IsNullOrEmpty(deduplicateField))
            {
                bodyObject["deduplicate.field"] = JToken.FromObject(deduplicateField);
            }
            bodyObject["similar"] = JToken.FromObject(similar);
            if (!string.IsNullOrEmpty(similarDocumentIds))
            {
                bodyObject["similar.document_ids"] = JToken.FromObject(similarDocumentIds);
            }
            if (!string.IsNullOrEmpty(similarFields))
            {
                bodyObject["similar.fields"] = JToken.FromObject(similarFields);
            }
            if (!string.IsNullOrEmpty(bias))
            {
                bodyObject["bias"] = JToken.FromObject(bias);
            }
            bodyObject["spelling_suggestions"] = JToken.FromObject(spellingSuggestions);
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/query");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void Query_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.Query(environmentId: environmentId, collectionId: collectionId, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, passages: passages, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, passagesFields: passagesFields, passagesCount: passagesCount, passagesCharacters: passagesCharacters, deduplicate: deduplicate, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields, bias: bias, spellingSuggestions: spellingSuggestions, xWatsonLoggingOptOut: xWatsonLoggingOptOut);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void Query_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.Query(environmentId: environmentId, collectionId: collectionId, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, passages: passages, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, passagesFields: passagesFields, passagesCount: passagesCount, passagesCharacters: passagesCharacters, deduplicate: deduplicate, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields, bias: bias, spellingSuggestions: spellingSuggestions, xWatsonLoggingOptOut: xWatsonLoggingOptOut);
    }
        [TestMethod]
        public void QueryNotices_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var filter = "filter";
            var query = "query";
            var naturalLanguageQuery = "naturalLanguageQuery";
            var passages = false;
            var aggregation = "aggregation";
            long? count = 1;
            var _return = new List<string>() { "_return0", "_return1" };
            long? offset = 1;
            var sort = new List<string>() { "sort0", "sort1" };
            var highlight = false;
            var passagesFields = new List<string>() { "passagesFields0", "passagesFields1" };
            long? passagesCount = 1;
            long? passagesCharacters = 1;
            var deduplicateField = "deduplicateField";
            var similar = false;
            var similarDocumentIds = new List<string>() { "similarDocumentIds0", "similarDocumentIds1" };
            var similarFields = new List<string>() { "similarFields0", "similarFields1" };

            var result = service.QueryNotices(environmentId: environmentId, collectionId: collectionId, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, passages: passages, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, passagesFields: passagesFields, passagesCount: passagesCount, passagesCharacters: passagesCharacters, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/notices");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void QueryNotices_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.QueryNotices(environmentId: environmentId, collectionId: collectionId, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, passages: passages, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, passagesFields: passagesFields, passagesCount: passagesCount, passagesCharacters: passagesCharacters, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void QueryNotices_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.QueryNotices(environmentId: environmentId, collectionId: collectionId, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, passages: passages, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, passagesFields: passagesFields, passagesCount: passagesCount, passagesCharacters: passagesCharacters, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields);
    }
        [TestMethod]
        public void FederatedQuery_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionIds = "collectionIds";
            var filter = "filter";
            var query = "query";
            var naturalLanguageQuery = "naturalLanguageQuery";
            var passages = false;
            var aggregation = "aggregation";
            var count = 1;
            var _return = "_return";
            var offset = 1;
            var sort = "sort";
            var highlight = false;
            var passagesFields = "passagesFields";
            var passagesCount = 1;
            var passagesCharacters = 1;
            var deduplicate = false;
            var deduplicateField = "deduplicateField";
            var similar = false;
            var similarDocumentIds = "similarDocumentIds";
            var similarFields = "similarFields";
            var bias = "bias";
            var xWatsonLoggingOptOut = false;

            var result = service.FederatedQuery(environmentId: environmentId, collectionIds: collectionIds, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, passages: passages, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, passagesFields: passagesFields, passagesCount: passagesCount, passagesCharacters: passagesCharacters, deduplicate: deduplicate, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields, bias: bias, xWatsonLoggingOptOut: xWatsonLoggingOptOut);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(collectionIds))
            {
                bodyObject["collection_ids"] = JToken.FromObject(collectionIds);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                bodyObject["filter"] = JToken.FromObject(filter);
            }
            if (!string.IsNullOrEmpty(query))
            {
                bodyObject["query"] = JToken.FromObject(query);
            }
            if (!string.IsNullOrEmpty(naturalLanguageQuery))
            {
                bodyObject["natural_language_query"] = JToken.FromObject(naturalLanguageQuery);
            }
            bodyObject["passages"] = JToken.FromObject(passages);
            if (!string.IsNullOrEmpty(aggregation))
            {
                bodyObject["aggregation"] = JToken.FromObject(aggregation);
            }
            if (count != null)
            {
                bodyObject["count"] = JToken.FromObject(count);
            }
            if (!string.IsNullOrEmpty(_return))
            {
                bodyObject["return"] = JToken.FromObject(_return);
            }
            if (offset != null)
            {
                bodyObject["offset"] = JToken.FromObject(offset);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                bodyObject["sort"] = JToken.FromObject(sort);
            }
            bodyObject["highlight"] = JToken.FromObject(highlight);
            if (!string.IsNullOrEmpty(passagesFields))
            {
                bodyObject["passages.fields"] = JToken.FromObject(passagesFields);
            }
            if (passagesCount != null)
            {
                bodyObject["passages.count"] = JToken.FromObject(passagesCount);
            }
            if (passagesCharacters != null)
            {
                bodyObject["passages.characters"] = JToken.FromObject(passagesCharacters);
            }
            bodyObject["deduplicate"] = JToken.FromObject(deduplicate);
            if (!string.IsNullOrEmpty(deduplicateField))
            {
                bodyObject["deduplicate.field"] = JToken.FromObject(deduplicateField);
            }
            bodyObject["similar"] = JToken.FromObject(similar);
            if (!string.IsNullOrEmpty(similarDocumentIds))
            {
                bodyObject["similar.document_ids"] = JToken.FromObject(similarDocumentIds);
            }
            if (!string.IsNullOrEmpty(similarFields))
            {
                bodyObject["similar.fields"] = JToken.FromObject(similarFields);
            }
            if (!string.IsNullOrEmpty(bias))
            {
                bodyObject["bias"] = JToken.FromObject(bias);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/query");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void FederatedQuery_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.FederatedQuery(environmentId: environmentId, collectionIds: collectionIds, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, passages: passages, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, passagesFields: passagesFields, passagesCount: passagesCount, passagesCharacters: passagesCharacters, deduplicate: deduplicate, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields, bias: bias, xWatsonLoggingOptOut: xWatsonLoggingOptOut);
    }
        [TestMethod]
        public void FederatedQueryNotices_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionIds = new List<string>() { "collectionIds0", "collectionIds1" };
            var filter = "filter";
            var query = "query";
            var naturalLanguageQuery = "naturalLanguageQuery";
            var aggregation = "aggregation";
            long? count = 1;
            var _return = new List<string>() { "_return0", "_return1" };
            long? offset = 1;
            var sort = new List<string>() { "sort0", "sort1" };
            var highlight = false;
            var deduplicateField = "deduplicateField";
            var similar = false;
            var similarDocumentIds = new List<string>() { "similarDocumentIds0", "similarDocumentIds1" };
            var similarFields = new List<string>() { "similarFields0", "similarFields1" };

            var result = service.FederatedQueryNotices(environmentId: environmentId, collectionIds: collectionIds, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/notices");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void FederatedQueryNotices_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.FederatedQueryNotices(environmentId: environmentId, collectionIds: collectionIds, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void FederatedQueryNotices_NoCollectionIds()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        List<string> collectionIds = null;
        var result = service.FederatedQueryNotices(environmentId: environmentId, collectionIds: collectionIds, filter: filter, query: query, naturalLanguageQuery: naturalLanguageQuery, aggregation: aggregation, count: count, _return: _return, offset: offset, sort: sort, highlight: highlight, deduplicateField: deduplicateField, similar: similar, similarDocumentIds: similarDocumentIds, similarFields: similarFields);
    }
        [TestMethod]
        public void GetAutocompletion_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var prefix = "prefix";
            var field = "field";
            long? count = 1;

            var result = service.GetAutocompletion(environmentId: environmentId, collectionId: collectionId, prefix: prefix, field: field, count: count);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/autocompletion");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetAutocompletion_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetAutocompletion(environmentId: environmentId, collectionId: collectionId, prefix: prefix, field: field, count: count);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetAutocompletion_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.GetAutocompletion(environmentId: environmentId, collectionId: collectionId, prefix: prefix, field: field, count: count);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetAutocompletion_NoPrefix()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        string prefix = null;
        var result = service.GetAutocompletion(environmentId: environmentId, collectionId: collectionId, prefix: prefix, field: field, count: count);
    }
        [TestMethod]
        public void ListTrainingData_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.ListTrainingData(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListTrainingData_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.ListTrainingData(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListTrainingData_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.ListTrainingData(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void AddTrainingData_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var naturalLanguageQuery = "naturalLanguageQuery";
            var filter = "filter";
            var examples = new List<TrainingExample>();

            var result = service.AddTrainingData(environmentId: environmentId, collectionId: collectionId, naturalLanguageQuery: naturalLanguageQuery, filter: filter, examples: examples);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(naturalLanguageQuery))
            {
                bodyObject["natural_language_query"] = JToken.FromObject(naturalLanguageQuery);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                bodyObject["filter"] = JToken.FromObject(filter);
            }
            if (examples != null && examples.Count > 0)
            {
                bodyObject["examples"] = JToken.FromObject(examples);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void AddTrainingData_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.AddTrainingData(environmentId: environmentId, collectionId: collectionId, naturalLanguageQuery: naturalLanguageQuery, filter: filter, examples: examples);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void AddTrainingData_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.AddTrainingData(environmentId: environmentId, collectionId: collectionId, naturalLanguageQuery: naturalLanguageQuery, filter: filter, examples: examples);
    }
        [TestMethod]
        public void DeleteAllTrainingData_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";

            var result = service.DeleteAllTrainingData(environmentId: environmentId, collectionId: collectionId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteAllTrainingData_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteAllTrainingData(environmentId: environmentId, collectionId: collectionId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteAllTrainingData_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.DeleteAllTrainingData(environmentId: environmentId, collectionId: collectionId);
    }
        [TestMethod]
        public void GetTrainingData_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var queryId = "queryId";

            var result = service.GetTrainingData(environmentId: environmentId, collectionId: collectionId, queryId: queryId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data/{queryId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetTrainingData_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetTrainingData(environmentId: environmentId, collectionId: collectionId, queryId: queryId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetTrainingData_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.GetTrainingData(environmentId: environmentId, collectionId: collectionId, queryId: queryId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetTrainingData_NoQueryId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var queryId = null;
        var result = service.GetTrainingData(environmentId: environmentId, collectionId: collectionId, queryId: queryId);
    }
        [TestMethod]
        public void DeleteTrainingData_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var queryId = "queryId";

            var result = service.DeleteTrainingData(environmentId: environmentId, collectionId: collectionId, queryId: queryId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data/{queryId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteTrainingData_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteTrainingData(environmentId: environmentId, collectionId: collectionId, queryId: queryId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteTrainingData_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.DeleteTrainingData(environmentId: environmentId, collectionId: collectionId, queryId: queryId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteTrainingData_NoQueryId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var queryId = null;
        var result = service.DeleteTrainingData(environmentId: environmentId, collectionId: collectionId, queryId: queryId);
    }
        [TestMethod]
        public void ListTrainingExamples_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var queryId = "queryId";

            var result = service.ListTrainingExamples(environmentId: environmentId, collectionId: collectionId, queryId: queryId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data/{queryId}/examples");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListTrainingExamples_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.ListTrainingExamples(environmentId: environmentId, collectionId: collectionId, queryId: queryId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListTrainingExamples_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.ListTrainingExamples(environmentId: environmentId, collectionId: collectionId, queryId: queryId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListTrainingExamples_NoQueryId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var queryId = null;
        var result = service.ListTrainingExamples(environmentId: environmentId, collectionId: collectionId, queryId: queryId);
    }
        [TestMethod]
        public void CreateTrainingExample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var queryId = "queryId";
            var documentId = "documentId";
            var crossReference = "crossReference";
            var relevance = 1;

            var result = service.CreateTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, documentId: documentId, crossReference: crossReference, relevance: relevance);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(documentId))
            {
                bodyObject["document_id"] = JToken.FromObject(documentId);
            }
            if (!string.IsNullOrEmpty(crossReference))
            {
                bodyObject["cross_reference"] = JToken.FromObject(crossReference);
            }
            if (relevance != null)
            {
                bodyObject["relevance"] = JToken.FromObject(relevance);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data/{queryId}/examples");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateTrainingExample_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.CreateTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, documentId: documentId, crossReference: crossReference, relevance: relevance);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateTrainingExample_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.CreateTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, documentId: documentId, crossReference: crossReference, relevance: relevance);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateTrainingExample_NoQueryId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var queryId = null;
        var result = service.CreateTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, documentId: documentId, crossReference: crossReference, relevance: relevance);
    }
        [TestMethod]
        public void DeleteTrainingExample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var queryId = "queryId";
            var exampleId = "exampleId";

            var result = service.DeleteTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data/{queryId}/examples/{exampleId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteTrainingExample_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteTrainingExample_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.DeleteTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteTrainingExample_NoQueryId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var queryId = null;
        var result = service.DeleteTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteTrainingExample_NoExampleId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var exampleId = null;
        var result = service.DeleteTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);
    }
        [TestMethod]
        public void UpdateTrainingExample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PutAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var queryId = "queryId";
            var exampleId = "exampleId";
            var crossReference = "crossReference";
            var relevance = 1;

            var result = service.UpdateTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId, crossReference: crossReference, relevance: relevance);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(crossReference))
            {
                bodyObject["cross_reference"] = JToken.FromObject(crossReference);
            }
            if (relevance != null)
            {
                bodyObject["relevance"] = JToken.FromObject(relevance);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PutAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data/{queryId}/examples/{exampleId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateTrainingExample_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.UpdateTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId, crossReference: crossReference, relevance: relevance);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateTrainingExample_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.UpdateTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId, crossReference: crossReference, relevance: relevance);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateTrainingExample_NoQueryId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var queryId = null;
        var result = service.UpdateTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId, crossReference: crossReference, relevance: relevance);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateTrainingExample_NoExampleId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var exampleId = null;
        var result = service.UpdateTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId, crossReference: crossReference, relevance: relevance);
    }
        [TestMethod]
        public void GetTrainingExample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var collectionId = "collectionId";
            var queryId = "queryId";
            var exampleId = "exampleId";

            var result = service.GetTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/collections/{collectionId}/training_data/{queryId}/examples/{exampleId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetTrainingExample_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetTrainingExample_NoCollectionId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var collectionId = null;
        var result = service.GetTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetTrainingExample_NoQueryId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var queryId = null;
        var result = service.GetTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetTrainingExample_NoExampleId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var exampleId = null;
        var result = service.GetTrainingExample(environmentId: environmentId, collectionId: collectionId, queryId: queryId, exampleId: exampleId);
    }
        [TestMethod]
        public void DeleteUserData_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var customerId = "customerId";

            var result = service.DeleteUserData(customerId: customerId);

            request.Received().WithArgument("version", versionDate);
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteUserData_NoCustomerId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        string customerId = null;
        var result = service.DeleteUserData(customerId: customerId);
    }
        [TestMethod]
        public void CreateEvent_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var type = "type";
            var data = new EventData();

            var result = service.CreateEvent(type: type, data: data);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(type))
            {
                bodyObject["type"] = JToken.FromObject(type);
            }
            if (data != null)
            {
                bodyObject["data"] = JToken.FromObject(data);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
        }
        [TestMethod]
        public void QueryLog_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var filter = "filter";
            var query = "query";
            long? count = 1;
            long? offset = 1;
            var sort = new List<string>() { "sort0", "sort1" };

            var result = service.QueryLog(filter: filter, query: query, count: count, offset: offset, sort: sort);

            request.Received().WithArgument("version", versionDate);
        }
        [TestMethod]
        public void GetMetricsQuery_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            DateTime? startTime = DateTime.MaxValue;
            DateTime? endTime = DateTime.MaxValue;
            var resultType = "resultType";

            var result = service.GetMetricsQuery(startTime: startTime, endTime: endTime, resultType: resultType);

            request.Received().WithArgument("version", versionDate);
        }
        [TestMethod]
        public void GetMetricsQueryEvent_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            DateTime? startTime = DateTime.MaxValue;
            DateTime? endTime = DateTime.MaxValue;
            var resultType = "resultType";

            var result = service.GetMetricsQueryEvent(startTime: startTime, endTime: endTime, resultType: resultType);

            request.Received().WithArgument("version", versionDate);
        }
        [TestMethod]
        public void GetMetricsQueryNoResults_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            DateTime? startTime = DateTime.MaxValue;
            DateTime? endTime = DateTime.MaxValue;
            var resultType = "resultType";

            var result = service.GetMetricsQueryNoResults(startTime: startTime, endTime: endTime, resultType: resultType);

            request.Received().WithArgument("version", versionDate);
        }
        [TestMethod]
        public void GetMetricsEventRate_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            DateTime? startTime = DateTime.MaxValue;
            DateTime? endTime = DateTime.MaxValue;
            var resultType = "resultType";

            var result = service.GetMetricsEventRate(startTime: startTime, endTime: endTime, resultType: resultType);

            request.Received().WithArgument("version", versionDate);
        }
        [TestMethod]
        public void GetMetricsQueryTokenEvent_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            long? count = 1;

            var result = service.GetMetricsQueryTokenEvent(count: count);

            request.Received().WithArgument("version", versionDate);
        }
        [TestMethod]
        public void ListCredentials_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";

            var result = service.ListCredentials(environmentId: environmentId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/credentials");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListCredentials_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.ListCredentials(environmentId: environmentId);
    }
        [TestMethod]
        public void CreateCredentials_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var sourceType = "sourceType";
            var credentialDetails = new CredentialDetails();
            var status = "status";

            var result = service.CreateCredentials(environmentId: environmentId, sourceType: sourceType, credentialDetails: credentialDetails, status: status);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(sourceType))
            {
                bodyObject["source_type"] = JToken.FromObject(sourceType);
            }
            if (credentialDetails != null)
            {
                bodyObject["credential_details"] = JToken.FromObject(credentialDetails);
            }
            if (!string.IsNullOrEmpty(status))
            {
                bodyObject["status"] = JToken.FromObject(status);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/credentials");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateCredentials_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.CreateCredentials(environmentId: environmentId, sourceType: sourceType, credentialDetails: credentialDetails, status: status);
    }
        [TestMethod]
        public void GetCredentials_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var credentialId = "credentialId";

            var result = service.GetCredentials(environmentId: environmentId, credentialId: credentialId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/credentials/{credentialId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetCredentials_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetCredentials(environmentId: environmentId, credentialId: credentialId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetCredentials_NoCredentialId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var credentialId = null;
        var result = service.GetCredentials(environmentId: environmentId, credentialId: credentialId);
    }
        [TestMethod]
        public void UpdateCredentials_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PutAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var credentialId = "credentialId";
            var sourceType = "sourceType";
            var credentialDetails = new CredentialDetails();
            var status = "status";

            var result = service.UpdateCredentials(environmentId: environmentId, credentialId: credentialId, sourceType: sourceType, credentialDetails: credentialDetails, status: status);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(sourceType))
            {
                bodyObject["source_type"] = JToken.FromObject(sourceType);
            }
            if (credentialDetails != null)
            {
                bodyObject["credential_details"] = JToken.FromObject(credentialDetails);
            }
            if (!string.IsNullOrEmpty(status))
            {
                bodyObject["status"] = JToken.FromObject(status);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PutAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/credentials/{credentialId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateCredentials_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.UpdateCredentials(environmentId: environmentId, credentialId: credentialId, sourceType: sourceType, credentialDetails: credentialDetails, status: status);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateCredentials_NoCredentialId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var credentialId = null;
        var result = service.UpdateCredentials(environmentId: environmentId, credentialId: credentialId, sourceType: sourceType, credentialDetails: credentialDetails, status: status);
    }
        [TestMethod]
        public void DeleteCredentials_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var credentialId = "credentialId";

            var result = service.DeleteCredentials(environmentId: environmentId, credentialId: credentialId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/credentials/{credentialId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteCredentials_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteCredentials(environmentId: environmentId, credentialId: credentialId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteCredentials_NoCredentialId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var credentialId = null;
        var result = service.DeleteCredentials(environmentId: environmentId, credentialId: credentialId);
    }
        [TestMethod]
        public void ListGateways_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";

            var result = service.ListGateways(environmentId: environmentId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/gateways");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListGateways_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.ListGateways(environmentId: environmentId);
    }
        [TestMethod]
        public void CreateGateway_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var name = "name";

            var result = service.CreateGateway(environmentId: environmentId, name: name);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(name))
            {
                bodyObject["name"] = JToken.FromObject(name);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/gateways");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateGateway_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.CreateGateway(environmentId: environmentId, name: name);
    }
        [TestMethod]
        public void GetGateway_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var gatewayId = "gatewayId";

            var result = service.GetGateway(environmentId: environmentId, gatewayId: gatewayId);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/gateways/{gatewayId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetGateway_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.GetGateway(environmentId: environmentId, gatewayId: gatewayId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetGateway_NoGatewayId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var gatewayId = null;
        var result = service.GetGateway(environmentId: environmentId, gatewayId: gatewayId);
    }
        [TestMethod]
        public void DeleteGateway_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            DiscoveryService service = new DiscoveryService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var environmentId = "environmentId";
            var gatewayId = "gatewayId";

            var result = service.DeleteGateway(environmentId: environmentId, gatewayId: gatewayId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/environments/{environmentId}/gateways/{gatewayId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteGateway_NoEnvironmentId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var environmentId = null;
        var result = service.DeleteGateway(environmentId: environmentId, gatewayId: gatewayId);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteGateway_NoGatewayId()
    {
        DiscoveryService service = new DiscoveryService("versionDate", new NoAuthAuthenticator());
        var gatewayId = null;
        var result = service.DeleteGateway(environmentId: environmentId, gatewayId: gatewayId);
    }
    }
}
