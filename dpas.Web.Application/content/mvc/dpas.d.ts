
declare namespace dpas {

    export interface IApplication {
        navigateSetController(controller: IController): void;
        navigateRemoveController(controller: IController): void;
        navigate(options: any);
        postJson(options: any);
        navigateSetContent(path: string, content: JQuery);
        showError(msg: string);

    }

   

    export let app: IApplication;
    export let template: any;
}
