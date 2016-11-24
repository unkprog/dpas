declare var $: any;
export module View {
    export class Auth {

        
        public Init() {
           
            $('#modal-auth').modal({
                dismissible: false
            });

            $('#modal-auth').modal('open');
        }
       
    }

}

  
(new View.Auth()).Init();
