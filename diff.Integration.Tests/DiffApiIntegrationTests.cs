using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace diff.Integration.Tests
{
    [TestClass]
    public class DiffApiIntegrationTests
    {
        public const string ApplicationJsonMediaType = "application/json";

        public const string GetDiffUrl = "http://localhost/v1/diff/{0}";
        public const string PutLeftUrl = "http://localhost/v1/diff/{0}/left";
        public const string PutRightUrl = "http://localhost/v1/diff/{0}/right";

        public static HttpClient HttpClient;
        public static HttpServer HttpServer;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            HttpConfiguration config = new HttpConfiguration();
            //configure web api
            WebApiConfig.Register(config);

            HttpServer = new HttpServer(config);
            HttpClient = new HttpClient(HttpServer);
        }

        [TestMethod]
        public  async Task DiffRequestBeforeLeftAndRightWereSetReturnsNotFoundTest()
        {
            //reset storage
            Container.DiffObjectRepository.Clear();

            int id = 0;

            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(String.Format(GetDiffUrl, id));
            Assert.AreEqual(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        /// <summary>
        /// Diff request with only left property set returns NotFound test.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DiffRequestOnlyLeftSetReturnsNotFoundTest()
        {
            //reset storage
            Container.DiffObjectRepository.Clear();

            int id = 0;
            HttpResponseMessage httpResponseMessage;
            
            //set left property only
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutLeftUrl, id), new ObjectData {data = "AAAAAA=="}).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            using (httpResponseMessage = await HttpClient.GetAsync(String.Format(GetDiffUrl, id)))
            {
                Assert.AreEqual(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
            }
        }

        [TestMethod]
        public async Task DiffRequestOnlyRightSetReturnsNotFoundTest()
        {
            //reset storage
            Container.DiffObjectRepository.Clear();

            int id = 0;

            HttpResponseMessage httpResponseMessage;

            //set right property only
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutRightUrl, id), new ObjectData {data = "AAAAAA=="}).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            using (httpResponseMessage = await HttpClient.GetAsync(String.Format(GetDiffUrl, id)))
            {
                Assert.AreEqual(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
            }
        }


        [TestMethod]
        public async Task PutLeftReturnsCreatedTest()
        {
            //set left property
            int id = 1;

            using (HttpResponseMessage httpResponseMessage = await HttpClient.PutAsJsonAsync(String.Format(PutLeftUrl, id), new ObjectData {data = "AAAAAA=="}))
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }
        }

        [TestMethod]
        public async Task PutRigthReturnsCreatedTest()
        {
            int id = 1;

            //set right property
            using (HttpResponseMessage httpResponseMessage = await HttpClient.PutAsJsonAsync(String.Format(PutRightUrl, id), new ObjectData { data = "AQABAQ==" }))
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }
        }

        [TestMethod]
        public async Task PutNullToLeftReturnsBadRequestTest()
        {
            int id = 1;

            //set left property to null
            using (HttpResponseMessage httpResponseMessage = await HttpClient.PutAsJsonAsync(String.Format(PutLeftUrl, id), new ObjectData { data = null }))
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
            }
        }

        [TestMethod]
        public async Task PutNullToRightReturnsBadRequestTest()
        {
            int id = 1;
            //set right property to null
            using (HttpResponseMessage httpResponseMessage = await HttpClient.PutAsJsonAsync(String.Format(PutRightUrl, id), new ObjectData {data = null}))
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
            }
        }

        [TestMethod]
        public async Task PutNonBase64StringToLeftReturnsBadRequestTest()
        {
            int id = 1;

            using (HttpResponseMessage httpResponseMessage = await HttpClient.PutAsJsonAsync(String.Format(PutLeftUrl, id), new ObjectData {data = "A"}))
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
            }
        }

        [TestMethod]
        public async Task PutNonBase64StringToRightReturnsBadRequestTest()
        {
            int id = 1;

            using (HttpResponseMessage httpResponseMessage = await HttpClient.PutAsJsonAsync(String.Format(PutRightUrl, id), new ObjectData {data = "Q"}))
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
            }
        }

        [TestMethod]
        public async Task PutRightWithIdOutsideIntegerRangeTest()
        {
            //diffcontroller is designed to accept interger id, put with id exceeding integer range
            long id = Int64.MaxValue;

            using (HttpResponseMessage httpResponseMessage = await HttpClient.PutAsJsonAsync(String.Format(PutRightUrl, id), new ObjectData { data = "" }))
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
            }
        }


        [TestMethod]
        public async Task PutLeftWithStringExcedingMaxrequestLengthTest()
        {
            //diffcontroller is designed to accept interger id, put with id exceeding integer range
            long id = 2;

            string data = Enumerable.Repeat("AAAAAA==", 100000).Aggregate(String.Empty, (current, next) => next + current);

            using (HttpResponseMessage httpResponseMessage = await HttpClient.PutAsJsonAsync(String.Format(PutLeftUrl, id), new ObjectData { data = data }))
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
            }
        }


        [TestMethod]
        public async Task DiffEqualTest()
        {
            int id = 2;

            HttpResponseMessage httpResponseMessage;

            //set left property
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutLeftUrl, id), new ObjectData { data = "AAAAAA==" }).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            //set right property
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutRightUrl, id), new ObjectData { data = "AAAAAA==" }).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            //get diff
            using (httpResponseMessage = HttpClient.GetAsync(String.Format(GetDiffUrl, id)).Result)
            {
                Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

                ObjectDiff objectDiff = await httpResponseMessage.Content.ReadAsAsync<ObjectDiff>();

                Assert.AreEqual(objectDiff.diffResultType, DiffResultType.Equals);
            }
        }

        [TestMethod]
        public async Task DiffSizeDoNotMatchLeftLongerTest()
        {
            int id = 2;

            HttpResponseMessage httpResponseMessage;

            //set left property
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutLeftUrl, id), new ObjectData { data = "AAAAAA==" }).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            //set right property
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutRightUrl, id), new ObjectData { data = "AAA=" }).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            //get diff
            using (httpResponseMessage = HttpClient.GetAsync(String.Format(GetDiffUrl, id)).Result)
            {
                Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

                ObjectDiff objectDiff = await httpResponseMessage.Content.ReadAsAsync<ObjectDiff>();

                Assert.AreEqual(objectDiff.diffResultType, DiffResultType.SizeDoNotMatch);
            }
        }

        [TestMethod]
        public async Task DiffSizeDoNotMatchRightLongerTest()
        {
            int id = 2;

            HttpResponseMessage httpResponseMessage;

            //set left property
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutLeftUrl, id), new ObjectData { data = "AQA=" }).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            //set right property
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutRightUrl, id), new ObjectData { data = "AQABAQ==" }).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            //get diff
            using (httpResponseMessage = HttpClient.GetAsync(String.Format(GetDiffUrl, id)).Result)
            {
                Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

                ObjectDiff objectDiff = await httpResponseMessage.Content.ReadAsAsync<ObjectDiff>();

                Assert.AreEqual(objectDiff.diffResultType, DiffResultType.SizeDoNotMatch);
            }
        }

        [TestMethod]
        public async Task DiffContentDoNotMatchTest()
        {
            int id = 2;

            HttpResponseMessage httpResponseMessage;

            //set left property
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutLeftUrl, id), new ObjectData { data = "AAAAAA==" }).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            //set right property
            using (httpResponseMessage = HttpClient.PutAsJsonAsync(String.Format(PutRightUrl, id), new ObjectData { data = "AQABAQ==" }).Result)
            {
                Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            }

            //get diff
            using (httpResponseMessage = HttpClient.GetAsync(String.Format(GetDiffUrl, id)).Result)
            {
                Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

                ObjectDiff objectDiff = await httpResponseMessage.Content.ReadAsAsync<ObjectDiff>();

                Assert.AreEqual(objectDiff.diffResultType, DiffResultType.ContentDoNotMatch);
                Assert.AreEqual(objectDiff.diffs.Count, 2); //expect 2 contents diffs

                Assert.AreEqual(objectDiff.diffs[0].offset, 0); //1 diff
                Assert.AreEqual(objectDiff.diffs[0].length, 1);

                Assert.AreEqual(objectDiff.diffs[1].offset, 2); //2 diff
                Assert.AreEqual(objectDiff.diffs[1].length, 2);
            }
        }


        [ClassCleanup]
        public static void ClassCleanup()
        {
            HttpServer?.Dispose();
            HttpClient?.Dispose();
        }
    }
}