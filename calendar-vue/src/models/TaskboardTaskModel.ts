
export class TaskboardTaskModel {
    private _serverId: string;
    private _name: string;
    private _importance: number | null;
    private _intensity: number | null;

    constructor(name: string) {
        this._serverId = "";
        this._name = name;
        this._importance = null;
        this._intensity = null;
    }
    
    get serverId(): string {
        return this._serverId;
    }

    get name(): string {
        return this._name;
    }

    get importance(): number | null {
        return this._importance;
    }

    get intensity(): number | null {
        return this._intensity;
    }
}
