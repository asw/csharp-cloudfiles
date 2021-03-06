using System;
using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response.Interfaces;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.GetStorageItemSpecs
{
    [TestFixture]
    public class When_getting_a_storage_object : TestBase
    {
        [Test]
        public void Should_return_ok_if_the_object_exists()
        {
            
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }

        [Test]
        public void Should_return_ok_if_the_object_exists_and_valid_content_type_of_image_gif()
        {
            
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemNameGif);
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemNameGif);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(getStorageItemResponse.Headers["Content-Type"], Is.EqualTo("image/gif"));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer(Constants.StorageItemNameGif);
                }
            }
        }

        [Test]
        public void Should_return_ok_if_the_object_exists_and_valid_content_type_of_image_jpeg()
        {
            
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemNameJpg);
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(getStorageItemResponse.Headers["Content-Type"], Is.EqualTo("image/jpeg"));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer(Constants.StorageItemNameJpg);
                }
            }
        }

        [Test]
        public void Should_return_not_found_if_the_object_does_not_exist()
        {
            
            using (new TestHelper(authToken, storageUrl))
            {
                var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME,Constants.StorageItemName);
                var exceptionWasThrown = false;

                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                }
                catch (WebException ex)
                {
                    exceptionWasThrown = true;
                    var code = ((HttpWebResponse) ex.Response).StatusCode;
                    Assert.That(code, Is.EqualTo(HttpStatusCode.NotFound));
                    if (getStorageItemResponse != null)getStorageItemResponse.Dispose();
                }

                Assert.That(exceptionWasThrown, Is.True);
            }
        }
    }

    [TestFixture]
    public class When_getting_a_storage_item_and_providing_a_if_match_request_header : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        private const string DUMMY_ETAG = "5c66108b7543c6f16145e25df9849f7f";

        protected override void SetUp()
        {
            requestHeaderFields = new Dictionary<RequestHeaderFields, string> 
            {{RequestHeaderFields.IfMatch, DUMMY_ETAG}};
        }

        [Test]
        public void Should_return_ok_if_the_item_exists()
        {
            
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }
    }

    [TestFixture]
    public class When_getting_a_storage_item_and_providing_a_if_none_match_request_header : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        private const string DUMMY_ETAG = "5c66108b7543c6f16145e25df9849f7fTest";

        protected override void SetUp()
        {
            requestHeaderFields = new Dictionary<RequestHeaderFields, string> 
            {{RequestHeaderFields.IfNoneMatch, DUMMY_ETAG}};
        }

        [Test]
        public void Should_return_ok_if_the_item_exists()
        {
            
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }
    }

    [TestFixture]
    public class When_getting_a_storage_item_and_providing_a_if_modified_since_request_header : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        private readonly DateTime pastDateTime = DateTime.Now.AddDays(-6);
        private readonly DateTime futureDateTime = DateTime.Now.AddDays(6);

        [Test]
        public void Should_return_not_modified_if_the_item_exists_and_it_hasnt_been_modified()
        {
            

            requestHeaderFields = new Dictionary<RequestHeaderFields, string> 
            {{RequestHeaderFields.IfModifiedSince, futureDateTime.ToString()}};

            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                var exceptionWasThrown = false;
                try
                {
                    testHelper.PutItemInContainer();
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);
                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                } 
                catch(WebException ex)
                {
                    var response = (HttpWebResponse)ex.Response;
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));
                    exceptionWasThrown = true;
                }
                finally
                {
                    if(getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }

                Assert.That(exceptionWasThrown, Is.True);
            }

        }
        [Test]
        public void should_return_item_with_last_modified_date_within_a_minute_of_object_creation()
        {


            requestHeaderFields = new Dictionary<RequestHeaderFields, string> 
            {{RequestHeaderFields.IfModifiedSince, pastDateTime.ToString()}};

            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(getStorageItemResponse.LastModified, Is.AtLeast(DateTime.Now.AddMinutes(-1)));
                    Assert.That(getStorageItemResponse.LastModified, Is.AtMost(DateTime.Now.AddMinutes(2)));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }

        [Test]
        public void Should_return_item_if_the_item_exists_and_has_been_modified_since_designated_time()
        {
            

            requestHeaderFields = new Dictionary<RequestHeaderFields, string> 
            {{RequestHeaderFields.IfModifiedSince, pastDateTime.ToString()}};

            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }
    }

    [TestFixture]
    public class When_getting_a_storage_item_and_providing_a_if_unmodified_since_request_header : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        private readonly DateTime pastDateTime = DateTime.Now.AddDays(-6);
        private readonly DateTime futureDateTime = DateTime.Now.AddDays(6);

        [Test]
        public void Should_return_item_if_the_item_exists_and_it_hasnt_been_modified()
        {
            

            requestHeaderFields = new Dictionary<RequestHeaderFields, string> 
            {{RequestHeaderFields.IfUnmodifiedSince, futureDateTime.ToString()}};

            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);
                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }

        [Test]
        public void Should_return_412_precondition_failed_if_the_item_exists_and_has_been_modified_since_designated_time()
        {
            

            requestHeaderFields = new Dictionary<RequestHeaderFields, string> 
            {{RequestHeaderFields.IfUnmodifiedSince, pastDateTime.ToString()}};

            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                var exceptionWasThrown = false;
                try
                {
                    testHelper.PutItemInContainer();
                    testHelper.PutItemInContainer();
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                catch (WebException ex)
                {
                    var response = (HttpWebResponse)ex.Response;
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.PreconditionFailed));
                    exceptionWasThrown = true;
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }

                Assert.That(exceptionWasThrown, Is.True);
            }
        }
    }

    [TestFixture]
    public class When_including_a_range_header_when_retrieving_a_storage_item : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        

        protected override void SetUp()
        {
            requestHeaderFields = new Dictionary<RequestHeaderFields, string>();
        }

        [Test]
        public void Should_return_partial_content_if_the_item_exists_and_both_range_fields_are_set()
        {
            
            requestHeaderFields.Add(RequestHeaderFields.Range, "0-5");
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
         
                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.PartialContent));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }

        [Test]
        public void Should_return_partial_content_if_the_item_exists_and_only_range_from_is_set()
        {
            
            requestHeaderFields.Add(RequestHeaderFields.Range, "10-");
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();

                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.PartialContent));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }

        [Test]
        public void Should_return_partial_content_if_the_item_exists_and_only_range_to_is_set()
        {
            
            requestHeaderFields.Add(RequestHeaderFields.Range, "-8");
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                ICloudFilesResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();

                    var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, requestHeaderFields);

                    getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.PartialContent));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }
    }
}

