declare module dpas {
    export namespace View {

        export interface IController {
            Init(): void;
        }

        export class Controller implements IController {
            public Init(): void;
        }
    }
}