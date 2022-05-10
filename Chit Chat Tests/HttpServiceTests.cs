using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Contrib.HttpClient;
using Autofac.Extras.Moq;
using ChitChat.Services;
using ChitChat.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Xunit;
using System.Net.Http.Json;
using ChitChat.Helper.Exceptions;

namespace Chit_Chat_Tests
{
    public class HttpServiceTests
    {
        [Fact]
        public async Task PostLoginCredentialsAsync_ShouldReturnUserModel_IfCredentialsAreFound()
        {
            var mockedUserModel = new UserModel("Jack", "ProfilePicture", "ConnectionID");
            var mockedUserCredentials = new UserCredentials("foo", "bar");
            var userResponseModel = new UserResponseModel(mockedUserModel, "Works");
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<HttpMessageHandler>().SetupRequest(HttpMethod.Post, "https://localhost:5001/api/chat/Login")
                    .ReturnsResponse(JsonConvert.SerializeObject(userResponseModel), "application/json");
                var cls = mock.Create<HttpClient>();
                IHttpService httpService = new HttpService(cls);

                var expected = mockedUserModel;

                var actual = await httpService.PostLoginCredentialsAsync(mockedUserCredentials);

                Assert.NotNull(actual);
                Assert.Equal(expected.DisplayName, actual.DisplayName);
                Assert.Equal(expected.ProfilePicture, actual.ProfilePicture);
                Assert.Equal(expected.ConnectionID, actual.ConnectionID);
            }
        }

        [Fact]
        public async Task PostLoginCredentialsAsync_ShouldThrow_IfCredentialsAreNotFound()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mockedUserModel = new UserModel();
                var mockedUserCredentials = new UserCredentials("foo", "bar");
                var userResponseModel = new UserResponseModel(mockedUserModel, "Not Found");
                mock.Mock<HttpMessageHandler>().SetupRequest(HttpMethod.Post, "https://localhost:5001/api/chat/Login")
                  .ReturnsResponse(HttpStatusCode.NotFound, JsonConvert.SerializeObject(userResponseModel), "application/json");
                var cls = mock.Create<HttpClient>();
                IHttpService httpService = new HttpService(cls);



                await Assert.ThrowsAsync<LoginException>(() => httpService.PostLoginCredentialsAsync(mockedUserCredentials));
            }
        }

        [Fact]
        public async Task PostRegisterCredentialsAsync_ShouldBeCalled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mockedUserCredentials = new UserCredentials("foo", "bar");
                mock.Mock<HttpMessageHandler>().SetupRequest(HttpMethod.Post, "https://localhost:5001/api/chat/PostUser").
                ReturnsResponse(HttpStatusCode.OK);


                var cls = mock.Create<HttpClient>();
                IHttpService httpService = new HttpService(cls);
                await httpService.PostRegisterCredentialsAsync(mockedUserCredentials);

                mock.Mock<HttpMessageHandler>().VerifyRequest("https://localhost:5001/api/chat/PostUser", Times.Once());
            }
        }

        [Fact]
        public async Task PostRegisterCredentialsAsync_ShouldThrow_IfResponseNotOk()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mockedUserCredentials = new UserCredentials("foo", "bar");
                mock.Mock<HttpMessageHandler>().SetupRequest(HttpMethod.Post, "https://localhost:5001/api/chat/PostUser").
                ReturnsResponse(HttpStatusCode.BadRequest);


                var cls = mock.Create<HttpClient>();
                IHttpService httpService = new HttpService(cls);

                await Assert.ThrowsAsync<RegistrationException>(() => httpService.PostRegisterCredentialsAsync(mockedUserCredentials));
            }
        }
        
        [Fact]
        public async Task PostRecoveryDataAsync_ShouldBeCalled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<HttpMessageHandler>().SetupRequest(HttpMethod.Post, "https://localhost:5001/api/chat/PostEmail").
                ReturnsResponse(HttpStatusCode.OK);
                mock.Mock<HttpMessageHandler>().SetupRequest(HttpMethod.Post, "https://localhost:5001/api/chat/PostPassword").
                ReturnsResponse(HttpStatusCode.OK);


                var cls = mock.Create<HttpClient>();
                IHttpService httpService = new HttpService(cls);
                await httpService.PostRecoveryDataAsync("PostEmail", null);
                await httpService.PostRecoveryDataAsync("PostPassword", null);

                mock.Mock<HttpMessageHandler>().VerifyRequest("https://localhost:5001/api/chat/PostEmail",  Times.Once());
                mock.Mock<HttpMessageHandler>().VerifyRequest("https://localhost:5001/api/chat/PostPassword", Times.Once());
            }
        }

        [Fact]
        public async Task PostRecoveryDataAsync_ShouldThrow_IfResponseNotOK()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<HttpMessageHandler>().SetupRequest(HttpMethod.Post, "https://localhost:5001/api/chat/PostEmail").
                ReturnsResponse(HttpStatusCode.BadRequest);
                mock.Mock<HttpMessageHandler>().SetupRequest(HttpMethod.Post, "https://localhost:5001/api/chat/PostPassword").
                ReturnsResponse(HttpStatusCode.BadRequest);


                var cls = mock.Create<HttpClient>();
                IHttpService httpService = new HttpService(cls);

                await Assert.ThrowsAsync<RecoveryException>(() => httpService.PostRecoveryDataAsync("PostEmail", null));
                await Assert.ThrowsAsync<RecoveryException>(() => httpService.PostRecoveryDataAsync("PostPassword", null));
            }
        }

    }
}
