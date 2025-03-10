
export function timeSpanStringToMinutes(timeSpanString: string): number {
    const timeSpanSections = timeSpanString.split(':').reverse();

    if (timeSpanSections.length < 2) {
        return 0;
    }

    let totalMinutes = 0;

    if (timeSpanSections.length >= 1) {
        const secondsStr = timeSpanSections[0];
        totalMinutes += Number(secondsStr) / 60;
    }

    if (timeSpanSections.length >= 2) {
        const minutesStr = timeSpanSections[1];
        totalMinutes += Number(minutesStr);
    }

    if (timeSpanSections.length >= 3) {
        const hoursStr = timeSpanSections[2];
        totalMinutes += Number(hoursStr) * 60;
    }

    if (timeSpanSections.length >= 4) {
        const daysStr = timeSpanSections[3];
        totalMinutes += Number(daysStr) * 60 * 24;
    }

    return totalMinutes;
}
