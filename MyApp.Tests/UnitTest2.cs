using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

[TestClass]
public class SongsControllerTests
{
    [TestMethod]
    public async Task AddSong_ShouldSendNotification_WhenSongIsValid()
    {
        var mockHubContext = new Mock<IHubContext<SongHub>>();
        var mockClients = new Mock<IHubCallerClients>();
        var mockClientProxy = new Mock<IClientProxy>();

        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
        mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

        var controller = new SongsController(mockHubContext.Object);

        var song = new SongModel { Title = "New Song" };

        var result = await controller.AddSong(song);

        mockClients.Verify(
            clients => clients.All.SendAsync("ReceiveSongNotification", song.Title, It.IsAny<CancellationToken>()),
            Times.Once);

        var redirectResult = Assert.IsInstanceOfType(result, typeof(RedirectToActionResult)) as RedirectToActionResult;
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [TestMethod]
    public async Task AddSong_ShouldReturnView_WhenModelStateIsInvalid()
    {
        var mockHubContext = new Mock<IHubContext<SongHub>>();
        var controller = new SongsController(mockHubContext.Object);
        controller.ModelState.AddModelError("Title", "Required");

        var song = new SongModel();

        var result = await controller.AddSong(song);

        var viewResult = Assert.IsInstanceOfType(result, typeof(ViewResult)) as ViewResult;
        Assert.AreEqual(song, viewResult.Model);
    }
}