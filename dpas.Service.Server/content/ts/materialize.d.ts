﻿interface JQuery {
    modal(options): JQuery;
    // Используем для модальных окон
    // 0 - Ok
    // 1 - Cancel
    modalResult: number; 
    tabs(): JQuery;
    material_select(): JQuery;
}

interface Materialize {
    updateTextFields(): JQuery;
}

declare let Materialize: Materialize;
declare let hljs: any;