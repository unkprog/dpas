declare namespace dpas {

    export interface IApplication {
        Controller: IController;
    }

    export interface IController {
        Inititialize(): void;
        Dispose(): void;
        App: IApplication;
    }


}
