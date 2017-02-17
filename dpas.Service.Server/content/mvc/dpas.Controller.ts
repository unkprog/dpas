/// <reference path="dpas.d.ts" />

namespace dpas {
    export class Controller implements IController {

        constructor() {
            dpas.app.navigateSetController(this);
        }

        public Initialize(): void {
            
        }

        public ApplyLayout(): void {

        }

        public Dispose(): void {
            dpas.app.navigateRemoveController(this);
        }

    }
}