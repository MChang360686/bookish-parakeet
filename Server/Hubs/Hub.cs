using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
	public class SongHub : Hub
	{
		public async Task SendNewSongNotification(string SongName)
		{
			await Clients.All.SendAsync("NewSongNotification", SongName);
		}
	}
}