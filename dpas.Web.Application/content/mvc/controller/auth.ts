declare var $: any;
export module View {
    export class Auth {

        public ShowAuthWindow() {
            // the "href" attribute of .modal-trigger must specify the modal ID that wants to be triggered
            $('#modal1').modal('open');
        }
    }

}

function Init() {
    //alert('Init()');
    $('.modal').modal({
        dismissible: false
    });;
    (new View.Auth()).ShowAuthWindow();
}
Init();