
declare namespace dpas {

    export interface IApplication {
        navigateSetController(controller: IController): void;
        navigateRemoveController(controller: IController): void;
        navigate(options: any): void;
        postJson(options: any): void;
        navigateSetContent(path: string, content: JQuery): void;
        showError(msg: string): void;

        showLoading(): void;
        hideLoading(): void;
    }

   

    export let app: IApplication;
    export let template: any;
}
