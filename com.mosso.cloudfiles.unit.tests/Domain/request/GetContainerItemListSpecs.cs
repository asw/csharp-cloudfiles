using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.GetContainerItemListSpecs
{
    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerItemList(null, "containername");
        }
    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerItemList(null, "containername");
        }
    }

   
    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_and_container_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerItemList("http://storageurl", null);
        }
    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_and_container_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerItemList("http://storageurl", "");
        }
    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_with_query_parameters
    {
        private GetContainerItemList getContainerItemList;

        [SetUp]
        public void setup()
        {
            getContainerItemList = new GetContainerItemList("http://storageurl", "containername");
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getContainerItemList.CreateUri().ToString(), Is.EqualTo("http://storageurl/containername"));
        }

        [Test]
        public void should_have_a_http_put_method()
        {
            Asserts.AssertMethod(getContainerItemList, "GET");
          
        }

     
    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_with_limit_query_parameter
    {
        private GetContainerItemList getContainerItemList;

        [SetUp]
        public void setup()
        {
            var parameters = new Dictionary<GetItemListParameters, string> {{GetItemListParameters.Limit, "2"}};
            getContainerItemList = new GetContainerItemList("http://storageurl", "containername", parameters);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getContainerItemList.CreateUri().ToString(), Is.EqualTo("http://storageurl/containername?limit=2"));
        }

        [Test]
        public void should_have_a_http_put_method()
        {
            //why are all these named like this?
            Asserts.AssertMethod(getContainerItemList, "GET");
        
        }

       
    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_with_marker_query_parameter
    {
        private GetContainerItemList getContainerItemList;

        [SetUp]
        public void setup()
        {
            var parameters = new Dictionary<GetItemListParameters, string> { { GetItemListParameters.Marker, "abc" } };
            getContainerItemList = new GetContainerItemList("http://storageurl", "containername", parameters);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getContainerItemList.CreateUri().ToString(), Is.EqualTo("http://storageurl/containername?marker=abc"));
        }

        [Test]
        public void should_have_a_http_put_method()
        {
            Asserts.AssertMethod(getContainerItemList, "GET");
          
        }

      
    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_with_prefix_query_parameter
    {
        private GetContainerItemList getContainerItemList;

        [SetUp]
        public void setup()
        {
            var parameters = new Dictionary<GetItemListParameters, string> { { GetItemListParameters.Prefix, "a" } };
            getContainerItemList = new GetContainerItemList("http://storageurl", "containername", parameters);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getContainerItemList.CreateUri().ToString(), Is.EqualTo("http://storageurl/containername?prefix=a"));
        }

        [Test]
        public void should_have_a_http_put_method()
        {
            Asserts.AssertMethod(getContainerItemList, "GET");
         
        }


    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_with_path_query_parameter
    {
        private GetContainerItemList getContainerItemList;

        [SetUp]
        public void setup()
        {
            var parameters = new Dictionary<GetItemListParameters, string> { { GetItemListParameters.Path, "dir1/subdir2/" } };
            getContainerItemList = new GetContainerItemList("http://storageurl", "containername", parameters);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getContainerItemList.CreateUri().ToString(), Is.EqualTo("http://storageurl/containername?path=dir1/subdir2/"));
        }

        [Test]
        public void should_have_a_http_put_method()
        {
            Asserts.AssertMethod(getContainerItemList, "GET");
        }

   
    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_with_more_than_one_query_parameter
    {
        private GetContainerItemList getContainerItemList;

        [SetUp]
        public void setup()
        {
            var parameters = new Dictionary<GetItemListParameters, string>
                                 {
                                     { GetItemListParameters.Limit, "2" },
                                     { GetItemListParameters.Marker, "abc" },
                                     { GetItemListParameters.Prefix, "a" },
                                     { GetItemListParameters.Path, "dir1/subdir2/" }
                                 };
            getContainerItemList = new GetContainerItemList("http://storageurl", "containername", parameters);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getContainerItemList.CreateUri().ToString(), Is.EqualTo("http://storageurl/containername?limit=2&marker=abc&prefix=a&path=dir1/subdir2/"));
        }

        [Test]
        public void should_have_a_http_put_method()
        {
            Asserts.AssertMethod(getContainerItemList, "GET");
            
        }


    }
}