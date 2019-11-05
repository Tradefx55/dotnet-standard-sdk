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
using IBM.Watson.Assistant.v1.Model;
using IBM.Cloud.SDK.Core.Model;

namespace IBM.Watson.Assistant.v1.UnitTests
{
    [TestClass]
    public class AssistantServiceUnitTests
    {
        #region Constructor
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_HttpClient_Null()
        {
            AssistantService service = new AssistantService(httpClient: null);
        }

        [TestMethod]
        public void ConstructorHttpClient()
        {
            AssistantService service = new AssistantService(new IBMHttpClient());
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ConstructorExternalConfig()
        {
            AssistantService service = Substitute.For<AssistantService>("versionDate");
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void Constructor()
        {
            AssistantService service = new AssistantService(new IBMHttpClient());
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ConstructorAuthenticator()
        {
            AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
            Assert.IsNotNull(service);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNoVersion()
        {
            AssistantService service = new AssistantService(null, new NoAuthAuthenticator());
        }

        [TestMethod]
        public void ConstructorNoUrl()
        {
            var url = System.Environment.GetEnvironmentVariable("ASSISTANT_SERVICE_URL");
            System.Environment.SetEnvironmentVariable("ASSISTANT_SERVICE_URL", null);
            AssistantService service = Substitute.For<AssistantService>("versionDate");
            Assert.IsTrue(service.ServiceUrl == "https://gateway.watsonplatform.net/assistant/api");
            System.Environment.SetEnvironmentVariable("ASSISTANT_SERVICE_URL", url);
        }
        #endregion

        [TestMethod]
        public void Message_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var input = new MessageInput();
            var intents = new List<RuntimeIntent>();
            var entities = new List<RuntimeEntity>();
            var alternateIntents = false;
            var context = new Context();
            var output = new OutputData();
            var nodesVisitedDetails = false;

            var result = service.Message(workspaceId: workspaceId, input: input, intents: intents, entities: entities, alternateIntents: alternateIntents, context: context, output: output, nodesVisitedDetails: nodesVisitedDetails);

            JObject bodyObject = new JObject();
            if (input != null)
            {
                bodyObject["input"] = JToken.FromObject(input);
            }
            if (intents != null && intents.Count > 0)
            {
                bodyObject["intents"] = JToken.FromObject(intents);
            }
            if (entities != null && entities.Count > 0)
            {
                bodyObject["entities"] = JToken.FromObject(entities);
            }
            bodyObject["alternate_intents"] = JToken.FromObject(alternateIntents);
            if (context != null)
            {
                bodyObject["context"] = JToken.FromObject(context);
            }
            if (output != null)
            {
                bodyObject["output"] = JToken.FromObject(output);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/message");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void Message_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.Message(workspaceId: workspaceId, input: input, intents: intents, entities: entities, alternateIntents: alternateIntents, context: context, output: output, nodesVisitedDetails: nodesVisitedDetails);
    }
        [TestMethod]
        public void ListWorkspaces_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            long? pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListWorkspaces(pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
        }
        [TestMethod]
        public void CreateWorkspace_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var name = "name";
            var description = "description";
            var language = "language";
            var metadata = new Dictionary<string, object>();
            metadata.Add("metadata", new object());
            var learningOptOut = false;
            var systemSettings = new WorkspaceSystemSettings();
            var intents = new List<CreateIntent>();
            var entities = new List<CreateEntity>();
            var dialogNodes = new List<DialogNode>();
            var counterexamples = new List<Counterexample>();

            var result = service.CreateWorkspace(name: name, description: description, language: language, metadata: metadata, learningOptOut: learningOptOut, systemSettings: systemSettings, intents: intents, entities: entities, dialogNodes: dialogNodes, counterexamples: counterexamples);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(name))
            {
                bodyObject["name"] = JToken.FromObject(name);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (!string.IsNullOrEmpty(language))
            {
                bodyObject["language"] = JToken.FromObject(language);
            }
            if (metadata != null)
            {
                bodyObject["metadata"] = JToken.FromObject(metadata);
            }
            bodyObject["learning_opt_out"] = JToken.FromObject(learningOptOut);
            if (systemSettings != null)
            {
                bodyObject["system_settings"] = JToken.FromObject(systemSettings);
            }
            if (intents != null && intents.Count > 0)
            {
                bodyObject["intents"] = JToken.FromObject(intents);
            }
            if (entities != null && entities.Count > 0)
            {
                bodyObject["entities"] = JToken.FromObject(entities);
            }
            if (dialogNodes != null && dialogNodes.Count > 0)
            {
                bodyObject["dialog_nodes"] = JToken.FromObject(dialogNodes);
            }
            if (counterexamples != null && counterexamples.Count > 0)
            {
                bodyObject["counterexamples"] = JToken.FromObject(counterexamples);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
        }
        [TestMethod]
        public void GetWorkspace_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var export = false;
            var includeAudit = false;
            var sort = "sort";

            var result = service.GetWorkspace(workspaceId: workspaceId, export: export, includeAudit: includeAudit, sort: sort);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetWorkspace_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.GetWorkspace(workspaceId: workspaceId, export: export, includeAudit: includeAudit, sort: sort);
    }
        [TestMethod]
        public void UpdateWorkspace_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var name = "name";
            var description = "description";
            var language = "language";
            var metadata = new Dictionary<string, object>();
            metadata.Add("metadata", new object());
            var learningOptOut = false;
            var systemSettings = new WorkspaceSystemSettings();
            var intents = new List<CreateIntent>();
            var entities = new List<CreateEntity>();
            var dialogNodes = new List<DialogNode>();
            var counterexamples = new List<Counterexample>();
            var append = false;

            var result = service.UpdateWorkspace(workspaceId: workspaceId, name: name, description: description, language: language, metadata: metadata, learningOptOut: learningOptOut, systemSettings: systemSettings, intents: intents, entities: entities, dialogNodes: dialogNodes, counterexamples: counterexamples, append: append);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(name))
            {
                bodyObject["name"] = JToken.FromObject(name);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (!string.IsNullOrEmpty(language))
            {
                bodyObject["language"] = JToken.FromObject(language);
            }
            if (metadata != null)
            {
                bodyObject["metadata"] = JToken.FromObject(metadata);
            }
            bodyObject["learning_opt_out"] = JToken.FromObject(learningOptOut);
            if (systemSettings != null)
            {
                bodyObject["system_settings"] = JToken.FromObject(systemSettings);
            }
            if (intents != null && intents.Count > 0)
            {
                bodyObject["intents"] = JToken.FromObject(intents);
            }
            if (entities != null && entities.Count > 0)
            {
                bodyObject["entities"] = JToken.FromObject(entities);
            }
            if (dialogNodes != null && dialogNodes.Count > 0)
            {
                bodyObject["dialog_nodes"] = JToken.FromObject(dialogNodes);
            }
            if (counterexamples != null && counterexamples.Count > 0)
            {
                bodyObject["counterexamples"] = JToken.FromObject(counterexamples);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateWorkspace_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.UpdateWorkspace(workspaceId: workspaceId, name: name, description: description, language: language, metadata: metadata, learningOptOut: learningOptOut, systemSettings: systemSettings, intents: intents, entities: entities, dialogNodes: dialogNodes, counterexamples: counterexamples, append: append);
    }
        [TestMethod]
        public void DeleteWorkspace_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";

            var result = service.DeleteWorkspace(workspaceId: workspaceId);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteWorkspace_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.DeleteWorkspace(workspaceId: workspaceId);
    }
        [TestMethod]
        public void ListIntents_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var export = false;
            long? pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListIntents(workspaceId: workspaceId, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListIntents_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.ListIntents(workspaceId: workspaceId, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }
        [TestMethod]
        public void CreateIntent_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var intent = "intent";
            var description = "description";
            var examples = new List<Example>();

            var result = service.CreateIntent(workspaceId: workspaceId, intent: intent, description: description, examples: examples);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(intent))
            {
                bodyObject["intent"] = JToken.FromObject(intent);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (examples != null && examples.Count > 0)
            {
                bodyObject["examples"] = JToken.FromObject(examples);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateIntent_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.CreateIntent(workspaceId: workspaceId, intent: intent, description: description, examples: examples);
    }
        [TestMethod]
        public void GetIntent_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var intent = "intent";
            var export = false;
            var includeAudit = false;

            var result = service.GetIntent(workspaceId: workspaceId, intent: intent, export: export, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetIntent_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.GetIntent(workspaceId: workspaceId, intent: intent, export: export, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetIntent_NoIntent()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var intent = null;
        var result = service.GetIntent(workspaceId: workspaceId, intent: intent, export: export, includeAudit: includeAudit);
    }
        [TestMethod]
        public void UpdateIntent_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var intent = "intent";
            var newIntent = "newIntent";
            var newDescription = "newDescription";
            var newExamples = new List<Example>();

            var result = service.UpdateIntent(workspaceId: workspaceId, intent: intent, newIntent: newIntent, newDescription: newDescription, newExamples: newExamples);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(newIntent))
            {
                bodyObject["intent"] = JToken.FromObject(newIntent);
            }
            if (!string.IsNullOrEmpty(newDescription))
            {
                bodyObject["description"] = JToken.FromObject(newDescription);
            }
            if (newExamples != null && newExamples.Count > 0)
            {
                bodyObject["examples"] = JToken.FromObject(newExamples);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateIntent_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.UpdateIntent(workspaceId: workspaceId, intent: intent, newIntent: newIntent, newDescription: newDescription, newExamples: newExamples);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateIntent_NoIntent()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var intent = null;
        var result = service.UpdateIntent(workspaceId: workspaceId, intent: intent, newIntent: newIntent, newDescription: newDescription, newExamples: newExamples);
    }
        [TestMethod]
        public void DeleteIntent_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var intent = "intent";

            var result = service.DeleteIntent(workspaceId: workspaceId, intent: intent);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteIntent_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.DeleteIntent(workspaceId: workspaceId, intent: intent);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteIntent_NoIntent()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var intent = null;
        var result = service.DeleteIntent(workspaceId: workspaceId, intent: intent);
    }
        [TestMethod]
        public void ListExamples_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var intent = "intent";
            long? pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListExamples(workspaceId: workspaceId, intent: intent, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}/examples");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListExamples_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.ListExamples(workspaceId: workspaceId, intent: intent, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListExamples_NoIntent()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var intent = null;
        var result = service.ListExamples(workspaceId: workspaceId, intent: intent, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }
        [TestMethod]
        public void CreateExample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var intent = "intent";
            var text = "text";
            var mentions = new List<Mention>();

            var result = service.CreateExample(workspaceId: workspaceId, intent: intent, text: text, mentions: mentions);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(text))
            {
                bodyObject["text"] = JToken.FromObject(text);
            }
            if (mentions != null && mentions.Count > 0)
            {
                bodyObject["mentions"] = JToken.FromObject(mentions);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}/examples");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateExample_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.CreateExample(workspaceId: workspaceId, intent: intent, text: text, mentions: mentions);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateExample_NoIntent()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var intent = null;
        var result = service.CreateExample(workspaceId: workspaceId, intent: intent, text: text, mentions: mentions);
    }
        [TestMethod]
        public void GetExample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var intent = "intent";
            var text = "text";
            var includeAudit = false;

            var result = service.GetExample(workspaceId: workspaceId, intent: intent, text: text, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}/examples/{text}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetExample_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.GetExample(workspaceId: workspaceId, intent: intent, text: text, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetExample_NoIntent()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var intent = null;
        var result = service.GetExample(workspaceId: workspaceId, intent: intent, text: text, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetExample_NoText()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var text = null;
        var result = service.GetExample(workspaceId: workspaceId, intent: intent, text: text, includeAudit: includeAudit);
    }
        [TestMethod]
        public void UpdateExample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var intent = "intent";
            var text = "text";
            var newText = "newText";
            var newMentions = new List<Mention>();

            var result = service.UpdateExample(workspaceId: workspaceId, intent: intent, text: text, newText: newText, newMentions: newMentions);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(newText))
            {
                bodyObject["text"] = JToken.FromObject(newText);
            }
            if (newMentions != null && newMentions.Count > 0)
            {
                bodyObject["mentions"] = JToken.FromObject(newMentions);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}/examples/{text}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateExample_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.UpdateExample(workspaceId: workspaceId, intent: intent, text: text, newText: newText, newMentions: newMentions);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateExample_NoIntent()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var intent = null;
        var result = service.UpdateExample(workspaceId: workspaceId, intent: intent, text: text, newText: newText, newMentions: newMentions);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateExample_NoText()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var text = null;
        var result = service.UpdateExample(workspaceId: workspaceId, intent: intent, text: text, newText: newText, newMentions: newMentions);
    }
        [TestMethod]
        public void DeleteExample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var intent = "intent";
            var text = "text";

            var result = service.DeleteExample(workspaceId: workspaceId, intent: intent, text: text);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}/examples/{text}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteExample_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.DeleteExample(workspaceId: workspaceId, intent: intent, text: text);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteExample_NoIntent()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var intent = null;
        var result = service.DeleteExample(workspaceId: workspaceId, intent: intent, text: text);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteExample_NoText()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var text = null;
        var result = service.DeleteExample(workspaceId: workspaceId, intent: intent, text: text);
    }
        [TestMethod]
        public void ListCounterexamples_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            long? pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListCounterexamples(workspaceId: workspaceId, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/counterexamples");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListCounterexamples_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.ListCounterexamples(workspaceId: workspaceId, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }
        [TestMethod]
        public void CreateCounterexample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var text = "text";

            var result = service.CreateCounterexample(workspaceId: workspaceId, text: text);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(text))
            {
                bodyObject["text"] = JToken.FromObject(text);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/counterexamples");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateCounterexample_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.CreateCounterexample(workspaceId: workspaceId, text: text);
    }
        [TestMethod]
        public void GetCounterexample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var text = "text";
            var includeAudit = false;

            var result = service.GetCounterexample(workspaceId: workspaceId, text: text, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/counterexamples/{text}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetCounterexample_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.GetCounterexample(workspaceId: workspaceId, text: text, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetCounterexample_NoText()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var text = null;
        var result = service.GetCounterexample(workspaceId: workspaceId, text: text, includeAudit: includeAudit);
    }
        [TestMethod]
        public void UpdateCounterexample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var text = "text";
            var newText = "newText";

            var result = service.UpdateCounterexample(workspaceId: workspaceId, text: text, newText: newText);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(newText))
            {
                bodyObject["text"] = JToken.FromObject(newText);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/counterexamples/{text}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateCounterexample_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.UpdateCounterexample(workspaceId: workspaceId, text: text, newText: newText);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateCounterexample_NoText()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var text = null;
        var result = service.UpdateCounterexample(workspaceId: workspaceId, text: text, newText: newText);
    }
        [TestMethod]
        public void DeleteCounterexample_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var text = "text";

            var result = service.DeleteCounterexample(workspaceId: workspaceId, text: text);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/counterexamples/{text}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteCounterexample_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.DeleteCounterexample(workspaceId: workspaceId, text: text);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteCounterexample_NoText()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var text = null;
        var result = service.DeleteCounterexample(workspaceId: workspaceId, text: text);
    }
        [TestMethod]
        public void ListEntities_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var export = false;
            long? pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListEntities(workspaceId: workspaceId, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListEntities_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.ListEntities(workspaceId: workspaceId, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }
        [TestMethod]
        public void CreateEntity_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var description = "description";
            var metadata = new Dictionary<string, object>();
            metadata.Add("metadata", new object());
            var fuzzyMatch = false;
            var values = new List<CreateValue>();

            var result = service.CreateEntity(workspaceId: workspaceId, entity: entity, description: description, metadata: metadata, fuzzyMatch: fuzzyMatch, values: values);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(entity))
            {
                bodyObject["entity"] = JToken.FromObject(entity);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (metadata != null)
            {
                bodyObject["metadata"] = JToken.FromObject(metadata);
            }
            bodyObject["fuzzy_match"] = JToken.FromObject(fuzzyMatch);
            if (values != null && values.Count > 0)
            {
                bodyObject["values"] = JToken.FromObject(values);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateEntity_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.CreateEntity(workspaceId: workspaceId, entity: entity, description: description, metadata: metadata, fuzzyMatch: fuzzyMatch, values: values);
    }
        [TestMethod]
        public void GetEntity_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var export = false;
            var includeAudit = false;

            var result = service.GetEntity(workspaceId: workspaceId, entity: entity, export: export, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetEntity_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.GetEntity(workspaceId: workspaceId, entity: entity, export: export, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetEntity_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.GetEntity(workspaceId: workspaceId, entity: entity, export: export, includeAudit: includeAudit);
    }
        [TestMethod]
        public void UpdateEntity_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var newEntity = "newEntity";
            var newDescription = "newDescription";
            var newMetadata = new Dictionary<string, object>();
            newMetadata.Add("newMetadata", new object());
            var newFuzzyMatch = false;
            var newValues = new List<CreateValue>();

            var result = service.UpdateEntity(workspaceId: workspaceId, entity: entity, newEntity: newEntity, newDescription: newDescription, newMetadata: newMetadata, newFuzzyMatch: newFuzzyMatch, newValues: newValues);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(newEntity))
            {
                bodyObject["entity"] = JToken.FromObject(newEntity);
            }
            if (!string.IsNullOrEmpty(newDescription))
            {
                bodyObject["description"] = JToken.FromObject(newDescription);
            }
            if (newMetadata != null)
            {
                bodyObject["metadata"] = JToken.FromObject(newMetadata);
            }
            bodyObject["fuzzy_match"] = JToken.FromObject(newFuzzyMatch);
            if (newValues != null && newValues.Count > 0)
            {
                bodyObject["values"] = JToken.FromObject(newValues);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateEntity_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.UpdateEntity(workspaceId: workspaceId, entity: entity, newEntity: newEntity, newDescription: newDescription, newMetadata: newMetadata, newFuzzyMatch: newFuzzyMatch, newValues: newValues);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateEntity_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.UpdateEntity(workspaceId: workspaceId, entity: entity, newEntity: newEntity, newDescription: newDescription, newMetadata: newMetadata, newFuzzyMatch: newFuzzyMatch, newValues: newValues);
    }
        [TestMethod]
        public void DeleteEntity_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";

            var result = service.DeleteEntity(workspaceId: workspaceId, entity: entity);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteEntity_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.DeleteEntity(workspaceId: workspaceId, entity: entity);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteEntity_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.DeleteEntity(workspaceId: workspaceId, entity: entity);
    }
        [TestMethod]
        public void ListMentions_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var export = false;
            var includeAudit = false;

            var result = service.ListMentions(workspaceId: workspaceId, entity: entity, export: export, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/mentions");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListMentions_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.ListMentions(workspaceId: workspaceId, entity: entity, export: export, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListMentions_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.ListMentions(workspaceId: workspaceId, entity: entity, export: export, includeAudit: includeAudit);
    }
        [TestMethod]
        public void ListValues_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var export = false;
            long? pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListValues(workspaceId: workspaceId, entity: entity, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListValues_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.ListValues(workspaceId: workspaceId, entity: entity, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListValues_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.ListValues(workspaceId: workspaceId, entity: entity, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }
        [TestMethod]
        public void CreateValue_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var value = "value";
            var metadata = new Dictionary<string, object>();
            metadata.Add("metadata", new object());
            var type = "type";
            var synonyms = new List<string>();
            var patterns = new List<string>();

            var result = service.CreateValue(workspaceId: workspaceId, entity: entity, value: value, metadata: metadata, type: type, synonyms: synonyms, patterns: patterns);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(value))
            {
                bodyObject["value"] = JToken.FromObject(value);
            }
            if (metadata != null)
            {
                bodyObject["metadata"] = JToken.FromObject(metadata);
            }
            if (!string.IsNullOrEmpty(type))
            {
                bodyObject["type"] = JToken.FromObject(type);
            }
            if (synonyms != null && synonyms.Count > 0)
            {
                bodyObject["synonyms"] = JToken.FromObject(synonyms);
            }
            if (patterns != null && patterns.Count > 0)
            {
                bodyObject["patterns"] = JToken.FromObject(patterns);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateValue_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.CreateValue(workspaceId: workspaceId, entity: entity, value: value, metadata: metadata, type: type, synonyms: synonyms, patterns: patterns);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateValue_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.CreateValue(workspaceId: workspaceId, entity: entity, value: value, metadata: metadata, type: type, synonyms: synonyms, patterns: patterns);
    }
        [TestMethod]
        public void GetValue_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var value = "value";
            var export = false;
            var includeAudit = false;

            var result = service.GetValue(workspaceId: workspaceId, entity: entity, value: value, export: export, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetValue_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.GetValue(workspaceId: workspaceId, entity: entity, value: value, export: export, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetValue_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.GetValue(workspaceId: workspaceId, entity: entity, value: value, export: export, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetValue_NoValue()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var value = null;
        var result = service.GetValue(workspaceId: workspaceId, entity: entity, value: value, export: export, includeAudit: includeAudit);
    }
        [TestMethod]
        public void UpdateValue_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var value = "value";
            var newValue = "newValue";
            var newMetadata = new Dictionary<string, object>();
            newMetadata.Add("newMetadata", new object());
            var newType = "newType";
            var newSynonyms = new List<string>();
            var newPatterns = new List<string>();

            var result = service.UpdateValue(workspaceId: workspaceId, entity: entity, value: value, newValue: newValue, newMetadata: newMetadata, newType: newType, newSynonyms: newSynonyms, newPatterns: newPatterns);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(newValue))
            {
                bodyObject["value"] = JToken.FromObject(newValue);
            }
            if (newMetadata != null)
            {
                bodyObject["metadata"] = JToken.FromObject(newMetadata);
            }
            if (!string.IsNullOrEmpty(newType))
            {
                bodyObject["type"] = JToken.FromObject(newType);
            }
            if (newSynonyms != null && newSynonyms.Count > 0)
            {
                bodyObject["synonyms"] = JToken.FromObject(newSynonyms);
            }
            if (newPatterns != null && newPatterns.Count > 0)
            {
                bodyObject["patterns"] = JToken.FromObject(newPatterns);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateValue_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.UpdateValue(workspaceId: workspaceId, entity: entity, value: value, newValue: newValue, newMetadata: newMetadata, newType: newType, newSynonyms: newSynonyms, newPatterns: newPatterns);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateValue_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.UpdateValue(workspaceId: workspaceId, entity: entity, value: value, newValue: newValue, newMetadata: newMetadata, newType: newType, newSynonyms: newSynonyms, newPatterns: newPatterns);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateValue_NoValue()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var value = null;
        var result = service.UpdateValue(workspaceId: workspaceId, entity: entity, value: value, newValue: newValue, newMetadata: newMetadata, newType: newType, newSynonyms: newSynonyms, newPatterns: newPatterns);
    }
        [TestMethod]
        public void DeleteValue_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var value = "value";

            var result = service.DeleteValue(workspaceId: workspaceId, entity: entity, value: value);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteValue_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.DeleteValue(workspaceId: workspaceId, entity: entity, value: value);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteValue_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.DeleteValue(workspaceId: workspaceId, entity: entity, value: value);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteValue_NoValue()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var value = null;
        var result = service.DeleteValue(workspaceId: workspaceId, entity: entity, value: value);
    }
        [TestMethod]
        public void ListSynonyms_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var value = "value";
            long? pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListSynonyms(workspaceId: workspaceId, entity: entity, value: value, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListSynonyms_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.ListSynonyms(workspaceId: workspaceId, entity: entity, value: value, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListSynonyms_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.ListSynonyms(workspaceId: workspaceId, entity: entity, value: value, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListSynonyms_NoValue()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var value = null;
        var result = service.ListSynonyms(workspaceId: workspaceId, entity: entity, value: value, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }
        [TestMethod]
        public void CreateSynonym_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var value = "value";
            var synonym = "synonym";

            var result = service.CreateSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(synonym))
            {
                bodyObject["synonym"] = JToken.FromObject(synonym);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateSynonym_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.CreateSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateSynonym_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.CreateSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateSynonym_NoValue()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var value = null;
        var result = service.CreateSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym);
    }
        [TestMethod]
        public void GetSynonym_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var value = "value";
            var synonym = "synonym";
            var includeAudit = false;

            var result = service.GetSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms/{synonym}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetSynonym_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.GetSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetSynonym_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.GetSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetSynonym_NoValue()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var value = null;
        var result = service.GetSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetSynonym_NoSynonym()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var synonym = null;
        var result = service.GetSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, includeAudit: includeAudit);
    }
        [TestMethod]
        public void UpdateSynonym_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var value = "value";
            var synonym = "synonym";
            var newSynonym = "newSynonym";

            var result = service.UpdateSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, newSynonym: newSynonym);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(newSynonym))
            {
                bodyObject["synonym"] = JToken.FromObject(newSynonym);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms/{synonym}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateSynonym_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.UpdateSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, newSynonym: newSynonym);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateSynonym_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.UpdateSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, newSynonym: newSynonym);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateSynonym_NoValue()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var value = null;
        var result = service.UpdateSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, newSynonym: newSynonym);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateSynonym_NoSynonym()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var synonym = null;
        var result = service.UpdateSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym, newSynonym: newSynonym);
    }
        [TestMethod]
        public void DeleteSynonym_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var entity = "entity";
            var value = "value";
            var synonym = "synonym";

            var result = service.DeleteSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms/{synonym}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteSynonym_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.DeleteSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteSynonym_NoEntity()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var entity = null;
        var result = service.DeleteSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteSynonym_NoValue()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var value = null;
        var result = service.DeleteSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteSynonym_NoSynonym()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var synonym = null;
        var result = service.DeleteSynonym(workspaceId: workspaceId, entity: entity, value: value, synonym: synonym);
    }
        [TestMethod]
        public void ListDialogNodes_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            long? pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListDialogNodes(workspaceId: workspaceId, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/dialog_nodes");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListDialogNodes_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.ListDialogNodes(workspaceId: workspaceId, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);
    }
        [TestMethod]
        public void CreateDialogNode_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var dialogNode = "dialogNode";
            var description = "description";
            var conditions = "conditions";
            var parent = "parent";
            var previousSibling = "previousSibling";
            var output = new DialogNodeOutput();
            var context = new Dictionary<string, object>();
            context.Add("context", new object());
            var metadata = new Dictionary<string, object>();
            metadata.Add("metadata", new object());
            var nextStep = new DialogNodeNextStep();
            var title = "title";
            var type = "type";
            var eventName = "eventName";
            var variable = "variable";
            var actions = new List<DialogNodeAction>();
            var digressIn = "digressIn";
            var digressOut = "digressOut";
            var digressOutSlots = "digressOutSlots";
            var userLabel = "userLabel";

            var result = service.CreateDialogNode(workspaceId: workspaceId, dialogNode: dialogNode, description: description, conditions: conditions, parent: parent, previousSibling: previousSibling, output: output, context: context, metadata: metadata, nextStep: nextStep, title: title, type: type, eventName: eventName, variable: variable, actions: actions, digressIn: digressIn, digressOut: digressOut, digressOutSlots: digressOutSlots, userLabel: userLabel);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(dialogNode))
            {
                bodyObject["dialog_node"] = JToken.FromObject(dialogNode);
            }
            if (!string.IsNullOrEmpty(description))
            {
                bodyObject["description"] = JToken.FromObject(description);
            }
            if (!string.IsNullOrEmpty(conditions))
            {
                bodyObject["conditions"] = JToken.FromObject(conditions);
            }
            if (!string.IsNullOrEmpty(parent))
            {
                bodyObject["parent"] = JToken.FromObject(parent);
            }
            if (!string.IsNullOrEmpty(previousSibling))
            {
                bodyObject["previous_sibling"] = JToken.FromObject(previousSibling);
            }
            if (output != null)
            {
                bodyObject["output"] = JToken.FromObject(output);
            }
            if (context != null)
            {
                bodyObject["context"] = JToken.FromObject(context);
            }
            if (metadata != null)
            {
                bodyObject["metadata"] = JToken.FromObject(metadata);
            }
            if (nextStep != null)
            {
                bodyObject["next_step"] = JToken.FromObject(nextStep);
            }
            if (!string.IsNullOrEmpty(title))
            {
                bodyObject["title"] = JToken.FromObject(title);
            }
            if (!string.IsNullOrEmpty(type))
            {
                bodyObject["type"] = JToken.FromObject(type);
            }
            if (!string.IsNullOrEmpty(eventName))
            {
                bodyObject["event_name"] = JToken.FromObject(eventName);
            }
            if (!string.IsNullOrEmpty(variable))
            {
                bodyObject["variable"] = JToken.FromObject(variable);
            }
            if (actions != null && actions.Count > 0)
            {
                bodyObject["actions"] = JToken.FromObject(actions);
            }
            if (!string.IsNullOrEmpty(digressIn))
            {
                bodyObject["digress_in"] = JToken.FromObject(digressIn);
            }
            if (!string.IsNullOrEmpty(digressOut))
            {
                bodyObject["digress_out"] = JToken.FromObject(digressOut);
            }
            if (!string.IsNullOrEmpty(digressOutSlots))
            {
                bodyObject["digress_out_slots"] = JToken.FromObject(digressOutSlots);
            }
            if (!string.IsNullOrEmpty(userLabel))
            {
                bodyObject["user_label"] = JToken.FromObject(userLabel);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/dialog_nodes");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void CreateDialogNode_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.CreateDialogNode(workspaceId: workspaceId, dialogNode: dialogNode, description: description, conditions: conditions, parent: parent, previousSibling: previousSibling, output: output, context: context, metadata: metadata, nextStep: nextStep, title: title, type: type, eventName: eventName, variable: variable, actions: actions, digressIn: digressIn, digressOut: digressOut, digressOutSlots: digressOutSlots, userLabel: userLabel);
    }
        [TestMethod]
        public void GetDialogNode_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var dialogNode = "dialogNode";
            var includeAudit = false;

            var result = service.GetDialogNode(workspaceId: workspaceId, dialogNode: dialogNode, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/dialog_nodes/{dialogNode}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetDialogNode_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.GetDialogNode(workspaceId: workspaceId, dialogNode: dialogNode, includeAudit: includeAudit);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void GetDialogNode_NoDialogNode()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var dialogNode = null;
        var result = service.GetDialogNode(workspaceId: workspaceId, dialogNode: dialogNode, includeAudit: includeAudit);
    }
        [TestMethod]
        public void UpdateDialogNode_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.PostAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var dialogNode = "dialogNode";
            var newDialogNode = "newDialogNode";
            var newDescription = "newDescription";
            var newConditions = "newConditions";
            var newParent = "newParent";
            var newPreviousSibling = "newPreviousSibling";
            var newOutput = new DialogNodeOutput();
            var newContext = new Dictionary<string, object>();
            newContext.Add("newContext", new object());
            var newMetadata = new Dictionary<string, object>();
            newMetadata.Add("newMetadata", new object());
            var newNextStep = new DialogNodeNextStep();
            var newTitle = "newTitle";
            var newType = "newType";
            var newEventName = "newEventName";
            var newVariable = "newVariable";
            var newActions = new List<DialogNodeAction>();
            var newDigressIn = "newDigressIn";
            var newDigressOut = "newDigressOut";
            var newDigressOutSlots = "newDigressOutSlots";
            var newUserLabel = "newUserLabel";

            var result = service.UpdateDialogNode(workspaceId: workspaceId, dialogNode: dialogNode, newDialogNode: newDialogNode, newDescription: newDescription, newConditions: newConditions, newParent: newParent, newPreviousSibling: newPreviousSibling, newOutput: newOutput, newContext: newContext, newMetadata: newMetadata, newNextStep: newNextStep, newTitle: newTitle, newType: newType, newEventName: newEventName, newVariable: newVariable, newActions: newActions, newDigressIn: newDigressIn, newDigressOut: newDigressOut, newDigressOutSlots: newDigressOutSlots, newUserLabel: newUserLabel);

            JObject bodyObject = new JObject();
            if (!string.IsNullOrEmpty(newDialogNode))
            {
                bodyObject["dialog_node"] = JToken.FromObject(newDialogNode);
            }
            if (!string.IsNullOrEmpty(newDescription))
            {
                bodyObject["description"] = JToken.FromObject(newDescription);
            }
            if (!string.IsNullOrEmpty(newConditions))
            {
                bodyObject["conditions"] = JToken.FromObject(newConditions);
            }
            if (!string.IsNullOrEmpty(newParent))
            {
                bodyObject["parent"] = JToken.FromObject(newParent);
            }
            if (!string.IsNullOrEmpty(newPreviousSibling))
            {
                bodyObject["previous_sibling"] = JToken.FromObject(newPreviousSibling);
            }
            if (newOutput != null)
            {
                bodyObject["output"] = JToken.FromObject(newOutput);
            }
            if (newContext != null)
            {
                bodyObject["context"] = JToken.FromObject(newContext);
            }
            if (newMetadata != null)
            {
                bodyObject["metadata"] = JToken.FromObject(newMetadata);
            }
            if (newNextStep != null)
            {
                bodyObject["next_step"] = JToken.FromObject(newNextStep);
            }
            if (!string.IsNullOrEmpty(newTitle))
            {
                bodyObject["title"] = JToken.FromObject(newTitle);
            }
            if (!string.IsNullOrEmpty(newType))
            {
                bodyObject["type"] = JToken.FromObject(newType);
            }
            if (!string.IsNullOrEmpty(newEventName))
            {
                bodyObject["event_name"] = JToken.FromObject(newEventName);
            }
            if (!string.IsNullOrEmpty(newVariable))
            {
                bodyObject["variable"] = JToken.FromObject(newVariable);
            }
            if (newActions != null && newActions.Count > 0)
            {
                bodyObject["actions"] = JToken.FromObject(newActions);
            }
            if (!string.IsNullOrEmpty(newDigressIn))
            {
                bodyObject["digress_in"] = JToken.FromObject(newDigressIn);
            }
            if (!string.IsNullOrEmpty(newDigressOut))
            {
                bodyObject["digress_out"] = JToken.FromObject(newDigressOut);
            }
            if (!string.IsNullOrEmpty(newDigressOutSlots))
            {
                bodyObject["digress_out_slots"] = JToken.FromObject(newDigressOutSlots);
            }
            if (!string.IsNullOrEmpty(newUserLabel))
            {
                bodyObject["user_label"] = JToken.FromObject(newUserLabel);
            }
            var json = JsonConvert.SerializeObject(bodyObject);
            request.Received().WithArgument("version", versionDate);
            request.Received().WithBodyContent(Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result.Equals(json)));
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/dialog_nodes/{dialogNode}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateDialogNode_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.UpdateDialogNode(workspaceId: workspaceId, dialogNode: dialogNode, newDialogNode: newDialogNode, newDescription: newDescription, newConditions: newConditions, newParent: newParent, newPreviousSibling: newPreviousSibling, newOutput: newOutput, newContext: newContext, newMetadata: newMetadata, newNextStep: newNextStep, newTitle: newTitle, newType: newType, newEventName: newEventName, newVariable: newVariable, newActions: newActions, newDigressIn: newDigressIn, newDigressOut: newDigressOut, newDigressOutSlots: newDigressOutSlots, newUserLabel: newUserLabel);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void UpdateDialogNode_NoDialogNode()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var dialogNode = null;
        var result = service.UpdateDialogNode(workspaceId: workspaceId, dialogNode: dialogNode, newDialogNode: newDialogNode, newDescription: newDescription, newConditions: newConditions, newParent: newParent, newPreviousSibling: newPreviousSibling, newOutput: newOutput, newContext: newContext, newMetadata: newMetadata, newNextStep: newNextStep, newTitle: newTitle, newType: newType, newEventName: newEventName, newVariable: newVariable, newActions: newActions, newDigressIn: newDigressIn, newDigressOut: newDigressOut, newDigressOutSlots: newDigressOutSlots, newUserLabel: newUserLabel);
    }
        [TestMethod]
        public void DeleteDialogNode_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var dialogNode = "dialogNode";

            var result = service.DeleteDialogNode(workspaceId: workspaceId, dialogNode: dialogNode);

            request.Received().WithArgument("version", versionDate);
            client.Received().DeleteAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/dialog_nodes/{dialogNode}");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteDialogNode_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.DeleteDialogNode(workspaceId: workspaceId, dialogNode: dialogNode);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteDialogNode_NoDialogNode()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var dialogNode = null;
        var result = service.DeleteDialogNode(workspaceId: workspaceId, dialogNode: dialogNode);
    }
        [TestMethod]
        public void ListLogs_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var workspaceId = "workspaceId";
            var sort = "sort";
            var filter = "filter";
            long? pageLimit = 1;
            var cursor = "cursor";

            var result = service.ListLogs(workspaceId: workspaceId, sort: sort, filter: filter, pageLimit: pageLimit, cursor: cursor);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/logs");
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListLogs_NoWorkspaceId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        var workspaceId = null;
        var result = service.ListLogs(workspaceId: workspaceId, sort: sort, filter: filter, pageLimit: pageLimit, cursor: cursor);
    }
        [TestMethod]
        public void ListAllLogs_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.GetAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var filter = "filter";
            var sort = "sort";
            long? pageLimit = 1;
            var cursor = "cursor";

            var result = service.ListAllLogs(filter: filter, sort: sort, pageLimit: pageLimit, cursor: cursor);

            request.Received().WithArgument("version", versionDate);
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ListAllLogs_NoFilter()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        string filter = null;
        var result = service.ListAllLogs(filter: filter, sort: sort, pageLimit: pageLimit, cursor: cursor);
    }
        [TestMethod]
        public void DeleteUserData_Success()
        {
            IClient client = Substitute.For<IClient>();
            IRequest request = Substitute.For<IRequest>();
            client.DeleteAsync(Arg.Any<string>())
                .Returns(request);

            AssistantService service = new AssistantService(client);
            var versionDate = "versionDate";
            service.VersionDate = versionDate;

            var customerId = "customerId";

            var result = service.DeleteUserData(customerId: customerId);

            request.Received().WithArgument("version", versionDate);
        }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void DeleteUserData_NoCustomerId()
    {
        AssistantService service = new AssistantService("versionDate", new NoAuthAuthenticator());
        string customerId = null;
        var result = service.DeleteUserData(customerId: customerId);
    }
    }
}
