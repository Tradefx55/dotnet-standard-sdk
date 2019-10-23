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

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using IBM.Cloud.SDK.Core.Authentication;
using IBM.Cloud.SDK.Core.Http;
using IBM.Cloud.SDK.Core.Http.Extensions;
using IBM.Cloud.SDK.Core.Service;
using IBM.Watson.VisualRecognition.v3.Model;
using System;

namespace IBM.Watson.VisualRecognition.v3
{
    public partial class VisualRecognitionService : IBMService, IVisualRecognitionService
    {
        const string serviceName = "visual_recognition";
        private const string defaultServiceUrl = "https://gateway.watsonplatform.net/visual-recognition/api";
        public string VersionDate { get; set; }

        public VisualRecognitionService(string versionDate) : this(versionDate, ConfigBasedAuthenticatorFactory.GetAuthenticator(serviceName)) { }
        public VisualRecognitionService(IClient httpClient) : base(serviceName, httpClient) { }

        public VisualRecognitionService(string versionDate, IAuthenticator authenticator) : base(serviceName, authenticator)
        {
            if (string.IsNullOrEmpty(versionDate))
            {
                throw new ArgumentNullException("versionDate cannot be null.");
            }
            
            VersionDate = versionDate;

            if (string.IsNullOrEmpty(ServiceUrl))
            {
                SetServiceUrl(defaultServiceUrl);
            }
        }

        /// <summary>
        /// Classify images.
        ///
        /// Classify images with built-in or custom classifiers.
        /// </summary>
        /// <param name="imagesFile">An image file (.gif, .jpg, .png, .tif) or .zip file with images. Maximum image size
        /// is 10 MB. Include no more than 20 images and limit the .zip file to 100 MB. Encode the image and .zip file
        /// names in UTF-8 if they contain non-ASCII characters. The service assumes UTF-8 encoding if it encounters
        /// non-ASCII characters.
        ///
        /// You can also include an image with the **url** parameter. (optional)</param>
        /// <param name="imagesFilename">The filename for imagesFile. (optional)</param>
        /// <param name="imagesFileContentType">The content type of imagesFile. (optional)</param>
        /// <param name="url">The URL of an image (.gif, .jpg, .png, .tif) to analyze. The minimum recommended pixel
        /// density is 32X32 pixels, but the service tends to perform better with images that are at least 224 x 224
        /// pixels. The maximum image size is 10 MB.
        ///
        /// You can also include images with the **images_file** parameter. (optional)</param>
        /// <param name="threshold">The minimum score a class must have to be displayed in the response. Set the
        /// threshold to `0.0` to return all identified classes. (optional)</param>
        /// <param name="owners">The categories of classifiers to apply. The **classifier_ids** parameter overrides
        /// **owners**, so make sure that **classifier_ids** is empty.
        /// - Use `IBM` to classify against the `default` general classifier. You get the same result if both
        /// **classifier_ids** and **owners** parameters are empty.
        /// - Use `me` to classify against all your custom classifiers. However, for better performance use
        /// **classifier_ids** to specify the specific custom classifiers to apply.
        /// - Use both `IBM` and `me` to analyze the image against both classifier categories. (optional)</param>
        /// <param name="classifierIds">Which classifiers to apply. Overrides the **owners** parameter. You can specify
        /// both custom and built-in classifier IDs. The built-in `default` classifier is used if both
        /// **classifier_ids** and **owners** parameters are empty.
        ///
        /// The following built-in classifier IDs require no training:
        /// - `default`: Returns classes from thousands of general tags.
        /// - `food`: Enhances specificity and accuracy for images of food items.
        /// - `explicit`: Evaluates whether the image might be pornographic. (optional)</param>
        /// <param name="acceptLanguage">The desired language of parts of the response. See the response for details.
        /// (optional, default to en)</param>
        /// <returns><see cref="ClassifiedImages" />ClassifiedImages</returns>
        public DetailedResponse<ClassifiedImages> Classify(System.IO.MemoryStream imagesFile = null, string imagesFilename = null, string imagesFileContentType = null, string url = null, float? threshold = null, List<string> owners = null, List<string> classifierIds = null, string acceptLanguage = null)
        {

            if (string.IsNullOrEmpty(VersionDate))
            {
                throw new ArgumentNullException("versionDate cannot be null.");
            }

            DetailedResponse<ClassifiedImages> result = null;

            try
            {
                var formData = new MultipartFormDataContent();

                if (imagesFile != null)
                {
                    var imagesFileContent = new ByteArrayContent(imagesFile.ToArray());
                    System.Net.Http.Headers.MediaTypeHeaderValue contentType;
                    System.Net.Http.Headers.MediaTypeHeaderValue.TryParse(imagesFileContentType, out contentType);
                    imagesFileContent.Headers.ContentType = contentType;
                    formData.Add(imagesFileContent, "images_file", imagesFilename);
                }

                if (url != null)
                {
                    var urlContent = new StringContent(url, Encoding.UTF8, HttpMediaType.TEXT_PLAIN);
                    urlContent.Headers.ContentType = null;
                    formData.Add(urlContent, "url");
                }

                if (threshold != null)
                {
                    var thresholdContent = new StringContent(threshold.ToString(), Encoding.UTF8, HttpMediaType.TEXT_PLAIN);
                    thresholdContent.Headers.ContentType = null;
                    formData.Add(thresholdContent, "threshold");
                }

                if (owners != null)
                {
                    foreach (string item in owners)
                    {
                        var ownersContent = new StringContent(item, Encoding.UTF8, HttpMediaType.TEXT_PLAIN);
                        ownersContent.Headers.ContentType = null;
                        formData.Add(ownersContent, "owners");
                    }
                }

                if (classifierIds != null)
                {
                    foreach (string item in classifierIds)
                    {
                        var classifierIdsContent = new StringContent(item, Encoding.UTF8, HttpMediaType.TEXT_PLAIN);
                        classifierIdsContent.Headers.ContentType = null;
                        formData.Add(classifierIdsContent, "classifier_ids");
                    }
                }

                IClient client = this.Client;
                SetAuthentication();

                var restRequest = client.PostAsync($"{this.Endpoint}/v3/classify");

                restRequest.WithArgument("version", VersionDate);
                restRequest.WithHeader("Accept", "application/json");

                if (!string.IsNullOrEmpty(acceptLanguage))
                {
                    restRequest.WithHeader("Accept-Language", acceptLanguage);
                }
                restRequest.WithBodyContent(formData);

                restRequest.WithHeaders(Common.GetSdkHeaders("watson_vision_combined", "v3", "Classify"));
                restRequest.WithHeaders(customRequestHeaders);
                ClearCustomRequestHeaders();

                result = restRequest.As<ClassifiedImages>().Result;
                if (result == null)
                {
                    result = new DetailedResponse<ClassifiedImages>();
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Create a classifier.
        ///
        /// Train a new multi-faceted classifier on the uploaded image data. Create your custom classifier with positive
        /// or negative example training images. Include at least two sets of examples, either two positive example
        /// files or one positive and one negative file. You can upload a maximum of 256 MB per call.
        ///
        /// **Tips when creating:**
        ///
        /// - If you set the **X-Watson-Learning-Opt-Out** header parameter to `true` when you create a classifier, the
        /// example training images are not stored. Save your training images locally. For more information, see [Data
        /// collection](#data-collection).
        ///
        /// - Encode all names in UTF-8 if they contain non-ASCII characters (.zip and image file names, and classifier
        /// and class names). The service assumes UTF-8 encoding if it encounters non-ASCII characters.
        /// </summary>
        /// <param name="name">The name of the new classifier. Encode special characters in UTF-8.</param>
        /// <param name="positiveExamples">A dictionary that contains the value for each classname. The value is a .zip
        /// file of images that depict the visual subject of a class in the new classifier. You can include more than
        /// one positive example file in a call.
        ///
        /// Specify the parameter name by appending `_positive_examples` to the class name. For example,
        /// `goldenretriever_positive_examples` creates the class **goldenretriever**.
        ///
        /// Include at least 10 images in .jpg or .png format. The minimum recommended image resolution is 32X32 pixels.
        /// The maximum number of images is 10,000 images or 100 MB per .zip file.
        ///
        /// Encode special characters in the file name in UTF-8.</param>
        /// <param name="negativeExamples">A .zip file of images that do not depict the visual subject of any of the
        /// classes of the new classifier. Must contain a minimum of 10 images.
        ///
        /// Encode special characters in the file name in UTF-8. (optional)</param>
        /// <param name="negativeExamplesFilename">The filename for negativeExamples. (optional)</param>
        /// <returns><see cref="Classifier" />Classifier</returns>
        public DetailedResponse<Classifier> CreateClassifier(string name, Dictionary<string, System.IO.MemoryStream> positiveExamples, System.IO.MemoryStream negativeExamples = null, string negativeExamplesFilename = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("`name` is required for `CreateClassifier`");
            }
            if (positiveExamples == null)
            {
                throw new ArgumentNullException("`positiveExamples` is required for `CreateClassifier`");
            }
            if (positiveExamples.Count == 0)
            {
                throw new ArgumentException("`positiveExamples` must contain at least one dictionary entry");
            }

            if (string.IsNullOrEmpty(VersionDate))
            {
                throw new ArgumentNullException("versionDate cannot be null.");
            }

            DetailedResponse<Classifier> result = null;

            try
            {
                var formData = new MultipartFormDataContent();

                if (name != null)
                {
                    var nameContent = new StringContent(name, Encoding.UTF8, HttpMediaType.TEXT_PLAIN);
                    nameContent.Headers.ContentType = null;
                    formData.Add(nameContent, "name");
                }

                if (positiveExamples != null && positiveExamples.Count > 0)
                {
                    foreach (KeyValuePair<string, System.IO.MemoryStream> entry in positiveExamples)
                    {
                        var partName = string.Format("{0}_positive_examples", entry.Key);
                        var partContent = new ByteArrayContent(entry.Value.ToArray());
                        System.Net.Http.Headers.MediaTypeHeaderValue contentType;
                        System.Net.Http.Headers.MediaTypeHeaderValue.TryParse("application/octet-stream", out contentType);
                        partContent.Headers.ContentType = contentType;
                        formData.Add(partContent, partName, entry.Key + ".zip");
                    }
                }

                if (negativeExamples != null)
                {
                    var negativeExamplesContent = new ByteArrayContent(negativeExamples.ToArray());
                    System.Net.Http.Headers.MediaTypeHeaderValue contentType;
                    System.Net.Http.Headers.MediaTypeHeaderValue.TryParse("application/octet-stream", out contentType);
                    negativeExamplesContent.Headers.ContentType = contentType;
                    formData.Add(negativeExamplesContent, "negative_examples", negativeExamplesFilename);
                }

                IClient client = this.Client;
                SetAuthentication();

                var restRequest = client.PostAsync($"{this.Endpoint}/v3/classifiers");

                restRequest.WithArgument("version", VersionDate);
                restRequest.WithHeader("Accept", "application/json");
                restRequest.WithBodyContent(formData);

                restRequest.WithHeaders(Common.GetSdkHeaders("watson_vision_combined", "v3", "CreateClassifier"));
                restRequest.WithHeaders(customRequestHeaders);
                ClearCustomRequestHeaders();

                result = restRequest.As<Classifier>().Result;
                if (result == null)
                {
                    result = new DetailedResponse<Classifier>();
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Retrieve a list of classifiers.
        /// </summary>
        /// <param name="verbose">Specify `true` to return details about the classifiers. Omit this parameter to return
        /// a brief list of classifiers. (optional)</param>
        /// <returns><see cref="Classifiers" />Classifiers</returns>
        public DetailedResponse<Classifiers> ListClassifiers(bool? verbose = null)
        {

            if (string.IsNullOrEmpty(VersionDate))
            {
                throw new ArgumentNullException("versionDate cannot be null.");
            }

            DetailedResponse<Classifiers> result = null;

            try
            {
                IClient client = this.Client;
                SetAuthentication();

                var restRequest = client.GetAsync($"{this.Endpoint}/v3/classifiers");

                restRequest.WithArgument("version", VersionDate);
                restRequest.WithHeader("Accept", "application/json");
                if (verbose != null)
                {
                    restRequest.WithArgument("verbose", verbose);
                }

                restRequest.WithHeaders(Common.GetSdkHeaders("watson_vision_combined", "v3", "ListClassifiers"));
                restRequest.WithHeaders(customRequestHeaders);
                ClearCustomRequestHeaders();

                result = restRequest.As<Classifiers>().Result;
                if (result == null)
                {
                    result = new DetailedResponse<Classifiers>();
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Retrieve classifier details.
        ///
        /// Retrieve information about a custom classifier.
        /// </summary>
        /// <param name="classifierId">The ID of the classifier.</param>
        /// <returns><see cref="Classifier" />Classifier</returns>
        public DetailedResponse<Classifier> GetClassifier(string classifierId)
        {
            if (string.IsNullOrEmpty(classifierId))
            {
                throw new ArgumentNullException("`classifierId` is required for `GetClassifier`");
            }
            else
            {
                classifierId = Uri.EscapeDataString(classifierId);
            }

            if (string.IsNullOrEmpty(VersionDate))
            {
                throw new ArgumentNullException("versionDate cannot be null.");
            }

            DetailedResponse<Classifier> result = null;

            try
            {
                IClient client = this.Client;
                SetAuthentication();

                var restRequest = client.GetAsync($"{this.Endpoint}/v3/classifiers/{classifierId}");

                restRequest.WithArgument("version", VersionDate);
                restRequest.WithHeader("Accept", "application/json");

                restRequest.WithHeaders(Common.GetSdkHeaders("watson_vision_combined", "v3", "GetClassifier"));
                restRequest.WithHeaders(customRequestHeaders);
                ClearCustomRequestHeaders();

                result = restRequest.As<Classifier>().Result;
                if (result == null)
                {
                    result = new DetailedResponse<Classifier>();
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Update a classifier.
        ///
        /// Update a custom classifier by adding new positive or negative classes or by adding new images to existing
        /// classes. You must supply at least one set of positive or negative examples. For details, see [Updating
        /// custom
        /// classifiers](https://cloud.ibm.com/docs/services/visual-recognition?topic=visual-recognition-customizing#updating-custom-classifiers).
        ///
        /// Encode all names in UTF-8 if they contain non-ASCII characters (.zip and image file names, and classifier
        /// and class names). The service assumes UTF-8 encoding if it encounters non-ASCII characters.
        ///
        /// **Tips about retraining:**
        ///
        /// - You can't update the classifier if the **X-Watson-Learning-Opt-Out** header parameter was set to `true`
        /// when the classifier was created. Training images are not stored in that case. Instead, create another
        /// classifier. For more information, see [Data collection](#data-collection).
        ///
        /// - Don't make retraining calls on a classifier until the status is ready. When you submit retraining requests
        /// in parallel, the last request overwrites the previous requests. The `retrained` property shows the last time
        /// the classifier retraining finished.
        /// </summary>
        /// <param name="classifierId">The ID of the classifier.</param>
        /// <param name="positiveExamples">A dictionary that contains the value for each classname. The value is a .zip
        /// file of images that depict the visual subject of a class in the classifier. The positive examples create or
        /// update classes in the classifier. You can include more than one positive example file in a call.
        ///
        /// Specify the parameter name by appending `_positive_examples` to the class name. For example,
        /// `goldenretriever_positive_examples` creates the class `goldenretriever`.
        ///
        /// Include at least 10 images in .jpg or .png format. The minimum recommended image resolution is 32X32 pixels.
        /// The maximum number of images is 10,000 images or 100 MB per .zip file.
        ///
        /// Encode special characters in the file name in UTF-8. (optional)</param>
        /// <param name="negativeExamples">A .zip file of images that do not depict the visual subject of any of the
        /// classes of the new classifier. Must contain a minimum of 10 images.
        ///
        /// Encode special characters in the file name in UTF-8. (optional)</param>
        /// <param name="negativeExamplesFilename">The filename for negativeExamples. (optional)</param>
        /// <returns><see cref="Classifier" />Classifier</returns>
        public DetailedResponse<Classifier> UpdateClassifier(string classifierId, Dictionary<string, System.IO.MemoryStream> positiveExamples = null, System.IO.MemoryStream negativeExamples = null, string negativeExamplesFilename = null)
        {
            if (string.IsNullOrEmpty(classifierId))
            {
                throw new ArgumentNullException("`classifierId` is required for `UpdateClassifier`");
            }
            else
            {
                classifierId = Uri.EscapeDataString(classifierId);
            }

            if (string.IsNullOrEmpty(VersionDate))
            {
                throw new ArgumentNullException("versionDate cannot be null.");
            }

            DetailedResponse<Classifier> result = null;

            try
            {
                var formData = new MultipartFormDataContent();

                if (positiveExamples != null && positiveExamples.Count > 0)
                {
                    foreach (KeyValuePair<string, System.IO.MemoryStream> entry in positiveExamples)
                    {
                        var partName = string.Format("{0}_positive_examples", entry.Key);
                        var partContent = new ByteArrayContent(entry.Value.ToArray());
                        System.Net.Http.Headers.MediaTypeHeaderValue contentType;
                        System.Net.Http.Headers.MediaTypeHeaderValue.TryParse("application/octet-stream", out contentType);
                        partContent.Headers.ContentType = contentType;
                        formData.Add(partContent, partName, entry.Key + ".zip");
                    }
                }

                if (negativeExamples != null)
                {
                    var negativeExamplesContent = new ByteArrayContent(negativeExamples.ToArray());
                    System.Net.Http.Headers.MediaTypeHeaderValue contentType;
                    System.Net.Http.Headers.MediaTypeHeaderValue.TryParse("application/octet-stream", out contentType);
                    negativeExamplesContent.Headers.ContentType = contentType;
                    formData.Add(negativeExamplesContent, "negative_examples", negativeExamplesFilename);
                }

                IClient client = this.Client;
                SetAuthentication();

                var restRequest = client.PostAsync($"{this.Endpoint}/v3/classifiers/{classifierId}");

                restRequest.WithArgument("version", VersionDate);
                restRequest.WithHeader("Accept", "application/json");
                restRequest.WithBodyContent(formData);

                restRequest.WithHeaders(Common.GetSdkHeaders("watson_vision_combined", "v3", "UpdateClassifier"));
                restRequest.WithHeaders(customRequestHeaders);
                ClearCustomRequestHeaders();

                result = restRequest.As<Classifier>().Result;
                if (result == null)
                {
                    result = new DetailedResponse<Classifier>();
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Delete a classifier.
        /// </summary>
        /// <param name="classifierId">The ID of the classifier.</param>
        /// <returns><see cref="object" />object</returns>
        public DetailedResponse<object> DeleteClassifier(string classifierId)
        {
            if (string.IsNullOrEmpty(classifierId))
            {
                throw new ArgumentNullException("`classifierId` is required for `DeleteClassifier`");
            }
            else
            {
                classifierId = Uri.EscapeDataString(classifierId);
            }

            if (string.IsNullOrEmpty(VersionDate))
            {
                throw new ArgumentNullException("versionDate cannot be null.");
            }

            DetailedResponse<object> result = null;

            try
            {
                IClient client = this.Client;
                SetAuthentication();

                var restRequest = client.DeleteAsync($"{this.Endpoint}/v3/classifiers/{classifierId}");

                restRequest.WithArgument("version", VersionDate);
                restRequest.WithHeader("Accept", "application/json");

                restRequest.WithHeaders(Common.GetSdkHeaders("watson_vision_combined", "v3", "DeleteClassifier"));
                restRequest.WithHeaders(customRequestHeaders);
                ClearCustomRequestHeaders();

                result = restRequest.As<object>().Result;
                if (result == null)
                {
                    result = new DetailedResponse<object>();
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Retrieve a Core ML model of a classifier.
        ///
        /// Download a Core ML model file (.mlmodel) of a custom classifier that returns <tt>"core_ml_enabled":
        /// true</tt> in the classifier details.
        /// </summary>
        /// <param name="classifierId">The ID of the classifier.</param>
        /// <returns><see cref="byte[]" />byte[]</returns>
        public DetailedResponse<System.IO.MemoryStream> GetCoreMlModel(string classifierId)
        {
            if (string.IsNullOrEmpty(classifierId))
            {
                throw new ArgumentNullException("`classifierId` is required for `GetCoreMlModel`");
            }
            else
            {
                classifierId = Uri.EscapeDataString(classifierId);
            }

            if (string.IsNullOrEmpty(VersionDate))
            {
                throw new ArgumentNullException("versionDate cannot be null.");
            }

            DetailedResponse<System.IO.MemoryStream> result = null;

            try
            {
                IClient client = this.Client;
                SetAuthentication();

                var restRequest = client.GetAsync($"{this.Endpoint}/v3/classifiers/{classifierId}/core_ml_model");

                restRequest.WithArgument("version", VersionDate);
                restRequest.WithHeader("Accept", "application/octet-stream");

                restRequest.WithHeaders(Common.GetSdkHeaders("watson_vision_combined", "v3", "GetCoreMlModel"));
                restRequest.WithHeaders(customRequestHeaders);
                ClearCustomRequestHeaders();

                result = new DetailedResponse<System.IO.MemoryStream>();
                result.Result = new System.IO.MemoryStream(restRequest.AsByteArray().Result);
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Delete labeled data.
        ///
        /// Deletes all data associated with a specified customer ID. The method has no effect if no data is associated
        /// with the customer ID.
        ///
        /// You associate a customer ID with data by passing the `X-Watson-Metadata` header with a request that passes
        /// data. For more information about personal data and customer IDs, see [Information
        /// security](https://cloud.ibm.com/docs/services/visual-recognition?topic=visual-recognition-information-security).
        /// </summary>
        /// <param name="customerId">The customer ID for which all data is to be deleted.</param>
        /// <returns><see cref="object" />object</returns>
        public DetailedResponse<object> DeleteUserData(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("`customerId` is required for `DeleteUserData`");
            }

            if (string.IsNullOrEmpty(VersionDate))
            {
                throw new ArgumentNullException("versionDate cannot be null.");
            }

            DetailedResponse<object> result = null;

            try
            {
                IClient client = this.Client;
                SetAuthentication();

                var restRequest = client.DeleteAsync($"{this.Endpoint}/v3/user_data");

                restRequest.WithArgument("version", VersionDate);
                restRequest.WithHeader("Accept", "application/json");
                if (!string.IsNullOrEmpty(customerId))
                {
                    restRequest.WithArgument("customer_id", customerId);
                }

                restRequest.WithHeaders(Common.GetSdkHeaders("watson_vision_combined", "v3", "DeleteUserData"));
                restRequest.WithHeaders(customRequestHeaders);
                ClearCustomRequestHeaders();

                result = restRequest.As<object>().Result;
                if (result == null)
                {
                    result = new DetailedResponse<object>();
                }
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
    }
}
