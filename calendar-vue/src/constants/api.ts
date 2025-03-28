
export const SERVER_URL = "http://localhost:5231";

export const API_CALENDAR_EVENT_ROUTE = `${SERVER_URL}/event`;
export const API_CALENDAR_EVENTS_ROUTE = `${SERVER_URL}/events`
export const API_TASK_ROUTE = `${SERVER_URL}/taskboard`
export const API_USER_ROUTE = `${SERVER_URL}/user`

export const API_ETF_SCHEDULE_ROUTE = `${SERVER_URL}/schedule/etf`;
export const API_UTMTK_SCHEDULE_ROUTE = `${SERVER_URL}/schedule/utmtk`;

export const API_SAMPLE_DATA_ROUTE =
    `${SERVER_URL}/sampleData/loadWithTasksAndIntensities`;

export const NOT_SYNCED = "NOT SYNCED WITH SERVER";

export const JSON_HEADERS = {
    'Accept': 'application/json',
    'Content-Type': 'application/json'
}
