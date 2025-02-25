import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7229/chathub")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

const startConnection = async () => {
    try {
        await connection.start();
        console.log("Connected to hub");
    } catch (error) {
        console.error("Error connecting to SIGNALR, retrying soon", error);
        setTimeout(startConnection, 5000);
    }
};

startConnection();

export { connection };