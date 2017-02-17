declare let $: any;

declare namespace dpas {

    export interface IApplication {
        navigateSetController(controller: IController): void;
        navigateRemoveController(controller: IController): void;
        navigate(options: any);
        postJson(options: any);
        navigateSetContent(path: any, content: any);

    }

    export interface IController {
        // Метод вызывается после загрузки представления
        Initialize(): void;
        // Метод вызывается после перед отображением представления и при изменении размеров окна
        ApplyLayout(): void;
        // Метод вызывается перед удалением представления
        Dispose(): void;
    }

    export let app: IApplication;
    export let template: any;
}
