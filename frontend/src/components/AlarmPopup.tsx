import { useEffect, useState } from "react";
import signalRAlarmService from "../services/signalRAlarmService";
import {AlarmData} from "../models/DTOs.ts";
import "./AlarmPopup.css"
import toast from "react-hot-toast";

export default function AlarmPopup() {
    const [, setAlarmsData] = useState<AlarmData[]>([]);

    useEffect(() => {
        signalRAlarmService.startConnection();

        signalRAlarmService.receiveAlarmData((newAlarmData : AlarmData) => {
            let lastData;
            setAlarmsData((prevAlarmsData) => {             
                lastData = prevAlarmsData[prevAlarmsData.length -1];
                return [...prevAlarmsData, newAlarmData]});
            if(newAlarmData.value == lastData.value) {
                return;
            }
            let duration = 5000;
            let backgroundColor = "#ffe900";
            console.log(newAlarmData.alarm.priority)
            if (newAlarmData.alarm.priority == 1){
                duration = 10000;
                backgroundColor = "#ff9c00"
            }
            else if (newAlarmData.alarm.priority == 2){
                duration = Infinity;
                backgroundColor = "#ff2a00"
            }
            toast.error((t) => (
                <div className="alarm">
                    <button className="delete-button" onClick={() => toast.dismiss(t.id)}>
                      X
                    </button>
                    <span>
                        <b>Device:</b> {newAlarmData.alarm.analogInput.IOAddress}
                    </span>
                    <span>
                        <b>TagId:</b> {newAlarmData.alarm.analogInput.id}
                    </span>
                    <span>
                        <b>Type:</b> {newAlarmData.alarm.type == 0 ? "Low" : "High"}
                    </span>
                    <span>
                        <b>Edge value:</b> {newAlarmData.alarm.edgeValue}
                    </span>
                    <span>
                        <b>Current value:</b> {newAlarmData.value.toFixed(3)}
                    </span>
                    <span>
                        <b>Time:</b> {new Date(newAlarmData.timestamp).toLocaleString()}
                    </span>
                </div>
            ), {duration: duration, style:{backgroundColor:backgroundColor}})
        });
    }, []);

    return <></>
}
