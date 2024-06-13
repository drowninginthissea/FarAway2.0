document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.show-qr-code').forEach(button => {
        button.addEventListener('click', () => {
            const rentId = button.getAttribute('data-rent-id');
            fetch(`/Account/RentalsView?handler=QRCode&rentId=${rentId}`)
                .then(response => response.json())
                .then(data => {
                    if (data && data.qrCodeImage) {
                        document.getElementById('qrCodeImage').src = data.qrCodeImage;
                        new bootstrap.Modal(document.getElementById('qrCodeModal')).show();
                    }
                })
                .catch(error => console.log('Error fetching QR core:', error));
        });
    });
});