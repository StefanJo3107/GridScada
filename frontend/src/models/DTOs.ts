export interface AnalogInput {
  id: string;
  description: string;
  IOAddress: string;
  scanTime: boolean;
  scanOn: boolean;
  lowLimit: number;
  highLimit: number;
  unit: string;
  value: number;
}

export interface DigitalInput {
  id: string;
  description: string;
  IOAddress: string;
  scanTime: boolean;
  scanOn: boolean;
  value: number;
}

export interface Alarm {
  id: string;
  type: number;
  priority: number;
  edgeValue: number;
  unit: string;
  analogInput: AnalogInput;
}

export interface AlarmData {
  alarm: Alarm;
  timestamp: string;
  value: number;
}

export interface AnalogData {
  id: string;
  analogInput: AnalogInput;
  value: number;
  timestamp: string;
  tagId: string;
}

export interface DigitalData {
  id: string;
  digitalInput: DigitalInput;
  value: number;
  timestamp: string;
  tagId: string;
}

export interface TagsList {
  analogInputs: AnalogInput[];
  digitalInputs: DigitalInput[];
}
