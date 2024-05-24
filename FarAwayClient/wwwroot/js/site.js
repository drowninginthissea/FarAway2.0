document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('sign-in-button').addEventListener('click', () => {
        window.location.href = '/SignIn';
    });

    document.getElementById('sign-up-button').addEventListener('click', () => {
        window.location.href = '/SignUp';
    });

    (() => {
        'use strict'
        const forms = document.querySelectorAll('.needs-validation')

        Array.from(forms).forEach(form => {
            form.addEventListener('submit', event => {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })

    })()
});

