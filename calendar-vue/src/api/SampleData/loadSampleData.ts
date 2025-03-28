import { API_SAMPLE_DATA_ROUTE, JSON_HEADERS } from "../../constants/api";

export async function apiLoadSampleData() {
    await fetch(API_SAMPLE_DATA_ROUTE, {
        method: "POST",
        headers: JSON_HEADERS
    })
        .catch(err => {
            console.error(
                "Could not reach server to load sample data: " + err
            );
        });
}
