declare let $: any;

declare namespace dpas {

    export interface IApplication {
        navigateSetController(controller: IController): void;
        navigate(options: any);
        postJson(options: any);
        navigateSetContent(path: any, content: any);

    }

    export interface IController {
        Initialize(): void;
        Dispose(): void;
    }

    export let app: IApplication;
    export let template: any;
}
