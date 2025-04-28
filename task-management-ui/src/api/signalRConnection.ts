import * as signalR from "@microsoft/signalr";
import { LogLevel } from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7124/hub") // Your backend Hub URL
  .configureLogging(LogLevel.Information)
  .withAutomaticReconnect()
  .build();

export default connection;
