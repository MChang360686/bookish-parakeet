const connection = new signalR.HubConnectionBuilder()
    .withUrl("/SongHub")
    .build();

// Start connection
connection.start().catch(err => console.error(err.toString()));

// Listen for event
connection.on("NewSongNotification", function (SongName) {
    alert("New song added: " + SongName);
});