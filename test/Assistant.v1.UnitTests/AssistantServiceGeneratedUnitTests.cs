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


using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using IBM.Cloud.SDK.Core.Http;
using IBM.Cloud.SDK.Core.Authentication.NoAuth;
using IBM.Watson.Assistant.v1.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using IBM.Cloud.SDK.Core.Http.Exceptions;
using IBM.Cloud.SDK.Core.Model;
using System.Net;

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
            var url = Environment.GetEnvironmentVariable("ASSISTANT_SERVICE_URL");
            Environment.SetEnvironmentVariable("ASSISTANT_SERVICE_URL", null);
            AssistantService service = Substitute.For<AssistantService>("versionDate");
            Assert.IsTrue(service.ServiceUrl == "https://gateway.watsonplatform.net/assistant/api");
            Environment.SetEnvironmentVariable("ASSISTANT_SERVICE_URL", url);
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/message");
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

            var pageLimit = 1;
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
            var learningOptOut = false;
            var systemSettings = new WorkspaceSystemSettings();
            var intents = new List<CreateIntent>();
            var entities = new List<CreateEntity>();
            var dialogNodes = new List<DialogNode>();
            var counterexamples = new List<Counterexample>();

            var result = service.CreateWorkspace(name: name, description: description, language: language, metadata: metadata, learningOptOut: learningOptOut, systemSettings: systemSettings, intents: intents, entities: entities, dialogNodes: dialogNodes, counterexamples: counterexamples);

            request.Received().WithArgument("version", versionDate);
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
            var learningOptOut = false;
            var systemSettings = new WorkspaceSystemSettings();
            var intents = new List<CreateIntent>();
            var entities = new List<CreateEntity>();
            var dialogNodes = new List<DialogNode>();
            var counterexamples = new List<Counterexample>();
            var append = false;

            var result = service.UpdateWorkspace(workspaceId: workspaceId, name: name, description: description, language: language, metadata: metadata, learningOptOut: learningOptOut, systemSettings: systemSettings, intents: intents, entities: entities, dialogNodes: dialogNodes, counterexamples: counterexamples, append: append);

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}");
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
            var pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListIntents(workspaceId: workspaceId, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents");
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents");
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}");
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
            var pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListExamples(workspaceId: workspaceId, intent: intent, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}/examples");
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}/examples");
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/intents/{intent}/examples/{text}");
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
            var pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListCounterexamples(workspaceId: workspaceId, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/counterexamples");
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/counterexamples");
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/counterexamples/{text}");
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
            var pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListEntities(workspaceId: workspaceId, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities");
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
            var fuzzyMatch = false;
            var values = new List<CreateValue>();

            var result = service.CreateEntity(workspaceId: workspaceId, entity: entity, description: description, metadata: metadata, fuzzyMatch: fuzzyMatch, values: values);

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities");
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
            var newFuzzyMatch = false;
            var newValues = new List<CreateValue>();

            var result = service.UpdateEntity(workspaceId: workspaceId, entity: entity, newEntity: newEntity, newDescription: newDescription, newMetadata: newMetadata, newFuzzyMatch: newFuzzyMatch, newValues: newValues);

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}");
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
            var pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListValues(workspaceId: workspaceId, entity: entity, export: export, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values");
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
            var type = "type";
            var synonyms = new List<string>();
            var patterns = new List<string>();

            var result = service.CreateValue(workspaceId: workspaceId, entity: entity, value: value, metadata: metadata, type: type, synonyms: synonyms, patterns: patterns);

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values");
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
            var newType = "newType";
            var newSynonyms = new List<string>();
            var newPatterns = new List<string>();

            var result = service.UpdateValue(workspaceId: workspaceId, entity: entity, value: value, newValue: newValue, newMetadata: newMetadata, newType: newType, newSynonyms: newSynonyms, newPatterns: newPatterns);

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}");
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
            var pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListSynonyms(workspaceId: workspaceId, entity: entity, value: value, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms");
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms");
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms/{synonym}");
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
            var pageLimit = 1;
            var sort = "sort";
            var cursor = "cursor";
            var includeAudit = false;

            var result = service.ListDialogNodes(workspaceId: workspaceId, pageLimit: pageLimit, sort: sort, cursor: cursor, includeAudit: includeAudit);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/dialog_nodes");
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
            var metadata = new Dictionary<string, object>();
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/dialog_nodes");
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
            var newMetadata = new Dictionary<string, object>();
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

            request.Received().WithArgument("version", versionDate);
            client.Received().PostAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/dialog_nodes/{dialogNode}");
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
            var pageLimit = 1;
            var cursor = "cursor";

            var result = service.ListLogs(workspaceId: workspaceId, sort: sort, filter: filter, pageLimit: pageLimit, cursor: cursor);

            request.Received().WithArgument("version", versionDate);
            client.Received().GetAsync($"{service.ServiceUrl}/v1/workspaces/{workspaceId}/logs");
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
            var pageLimit = 1;
            var cursor = "cursor";

            var result = service.ListAllLogs(filter: filter, sort: sort, pageLimit: pageLimit, cursor: cursor);

            request.Received().WithArgument("version", versionDate);
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

    }
}
