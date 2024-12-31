document.addEventListener('DOMContentLoaded', function () {
    const dateInput = document.querySelector('input[type="date"]');
    dateInput.onfocus = function () {
        this.placeholder = ''; 
    };
    dateInput.onblur = function () {
        this.placeholder = 'Select a date';
    };
});