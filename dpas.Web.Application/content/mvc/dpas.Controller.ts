/// <reference path="../ts/materialize.d.ts" />

namespace dpas {

    export interface IController {
        // Метод вызывается после загрузки представления
        Initialize(): void;
        // Метод вызывается после перед отображением представления и при изменении размеров окна
        ApplyLayout(): void;
        // Метод вызывается перед удалением представления
        Dispose(): void;

        Navigate(target: HTMLElement): void;
    }

    export class Controller implements IController {

        constructor() {
            dpas.app.navigateSetController(this);
        }

        public Initialize(): void {
            Materialize.updateTextFields();
        }

        public ApplyLayout(): void {

        }

        public Dispose(): void {
            dpas.app.navigateRemoveController(this);
        }

        public Navigate(target: HTMLElement): void {
        }

    }
}