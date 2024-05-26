document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('sign-in-button').addEventListener('click', () => {
        window.location.href = '/SignIn';
    });

    document.getElementById('sign-up-button').addEventListener('click', () => {
        window.location.href = '/SignUp';
    });
});
