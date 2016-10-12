
export module View {
    export class Index {

        public ShotAuth() {
            // the "href" attribute of .modal-trigger must specify the modal ID that wants to be triggered
            $('#modal1').openModal();
        }
    }

}

function Init() {
    (new View.Index()).ShotAuth();
}
Init();