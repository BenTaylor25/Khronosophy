import {
    API_CLEAR_SAMPLE_DATA_ROUTE,
    JSON_HEADERS
} from "../../constants/api";

export async function apiClearSampleData() {
    await fetch(API_CLEAR_SAMPLE_DATA_ROUTE, {
        method: "DELETE",
        headers: JSON_HEADERS
    })
        .catch(err => {
            console.error(
                "Could not reach server to clear sample data: " + err
            );
        });
}
