using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace MyApp.Tests
{
    [TestClass]
    public class SongHubTest
    {
        [TestMethod]
        public void SendNotification()
        {
            var mockClients = new Mock<IHubCallerClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(clients => clients.All)
                       .Returns(mockClientProxy.Object)
                       .Verifiable();

            var songHub = new SongHub
            {
                Clients = mockClients.Object
            };

            var songName = "Test Song";

            await songHub.SendSongNotification(songName);

            mockClients.Verify(
                clients => clients.All.SendAsync("ReceiveSongNotification", songName, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}