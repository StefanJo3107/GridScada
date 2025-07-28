import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { AlarmData } from "../models/DTOs.ts";

interface SignalRAlarmService {
  connection: HubConnection | null;
  startConnection: () => void;
  receiveAlarmData: (callback: (data: AlarmData) => void) => void;
}

const SignalRAlarmService: SignalRAlarmService = {
  connection: null,

  startConnection: () => {
    const connection = new HubConnectionBuilder()
      .withUrl("http://localhost:5284/hub/alarm", { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    connection.start().catch((error: Error) => {
      console.error("SignalR connection error: ", error);
    });

    SignalRAlarmService.connection = connection;
  },

  receiveAlarmData: (callback: (data: AlarmData) => void) => {
    SignalRAlarmService.connection?.on(
      "receiveAlarmData",
      (data: AlarmData) => {
        callback(data);
      }
    );
  },
};

export default SignalRAlarmService;
