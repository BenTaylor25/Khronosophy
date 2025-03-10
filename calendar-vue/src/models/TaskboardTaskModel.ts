
export class TaskboardTaskModel {
    private _serverId: string;
    private _name: string;
    private _expectedDurationMinutes: number;
    private _importance: number | null;
    private _intensity: number | null;

    constructor(name: string, expectedDurationMinutes: number) {
        this._serverId = "";
        this._name = name;
        this._expectedDurationMinutes = expectedDurationMinutes
        this._importance = null;
        this._intensity = null;
    }

    get serverId(): string {
        return this._serverId;
    }

    get name(): string {
        return this._name;
    }
    set name(newName: string) {
        this._name = newName;
    }

    get expectedDurationMinutes(): number {
        return this._expectedDurationMinutes;
    }
    set expectedDurationMinutes(newExpectedDurationMinutes: number) {
        this._expectedDurationMinutes = newExpectedDurationMinutes;
    }

    get importance(): number | null {
        return this._importance;
    }
    set importance(newImportance: number | null) {
        if (newImportance == 0) {
            newImportance = null;
        }

        this._importance = newImportance;
    }

    get intensity(): number | null {
        return this._intensity;
    }
    set intensity(newIntensity: number | null) {
        if (newIntensity == 0) {
            newIntensity = null;
        }

        this._intensity = newIntensity;
    }
}
